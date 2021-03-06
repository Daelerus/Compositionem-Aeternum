using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.Experimental.Networking ;

public class GameController : MonoBehaviour
{
    private static Dictionary<string, object> dict = new Dictionary<string, object>();
    public SimpleObjectPool answerButtonObjectPool;
    public Text questionText;
    public Text scoreDisplay;
    public Text timeRemainingDisplay;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive = false;
    private float timeRemaining;
    private int playerScore;
    public int questionIndex;
    private float timeUsed;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    private AnswerData[] answers;

    private int answerID;

    private string finalString;

    void Start()
    {
        questionIndex = 0;
        
        dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this round

        currentRoundData = dataController.GetCurrentRoundData();                            // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
        questionPool = currentRoundData.questions;                                            // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object
        questionPool.Shuffle();
        timeRemaining = currentRoundData.timeLimitInSeconds;                                // Set the time limit for this round based on the RoundData object
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        timeUsed= 0;
        

        ShowQuestion();
        isRoundActive = true;
        dict.Clear();
    }

    void Update()
    {
        if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;                                                // If the round is active, subtract the time since Update() was last called from timeRemaining
            UpdateTimeRemainingDisplay();
            timeUsed= currentRoundData.timeLimitInSeconds-timeRemaining;

            if (timeRemaining <= 0f)                                                        // If timeRemaining is 0 or less, the round ends
            {
                timeUsed = currentRoundData.timeLimitInSeconds;
                DataController.data.totaltime+=timeUsed;
                if(DataController.data.playerScore!=0)
                {
                    DataController.data.averageCorrect = ((float)DataController.data.playerScore/(float)questionPool.Length);
                }
                else
                {
                    DataController.data.averageCorrect = 0;
                }

                EndRound();
            }
        }
        dict.Clear();
    }

    void ShowQuestion()
    {
        RemoveAnswerButtons();

        QuestionData questionData = questionPool[questionIndex];                            // Get the QuestionData for the current question
        questionText.text = questionData.questionText;                                        // Update questionText with the correct text
        
        System.Random rnd=new System.Random();
        var answersInRandomOrder = questionData.answers.OrderBy(x => rnd.Next()).ToArray();  

        for (int i = 0; i < answersInRandomOrder.Length; i ++)                                // For every AnswerData in the current QuestionData...
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();            // Spawn an AnswerButton from the object pool
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            answerButtonGameObject.transform.localScale = Vector3.one;

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.SetUp(answersInRandomOrder[i]);                                    // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer
        }

    }

    void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)                                            // Return all spawned AnswerButtons to the object pool
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;                    // If the AnswerButton that was clicked was the correct answer, add points
            scoreDisplay.text = playerScore.ToString();
            DataController.data.playerScore +=1;
        }

        if(questionPool.Length > questionIndex + 1)                                            // If there are more questions, show the next question
        {
            questionIndex++;
            ShowQuestion();
        }
        else                                                                                // If there are no more questions, the round ends
        {
            timeUsed= currentRoundData.timeLimitInSeconds - timeRemaining;
            DataController.data.totaltime+=timeUsed;
            DataController.data.averageCorrect = ((float)DataController.data.playerScore/(float)questionPool.Length);
            EndRound();
        }
        DataController.data.averageCorrect = ((float)DataController.data.playerScore/(float)questionPool.Length);
        dict.Add("userid",AnalyticsSessionInfo.userId);
		dict.Add("language",DataController.data.language);
		dict.Add("age",DataController.data.age);
        dict.Add("gender",DataController.data.gender);
        dict.Add("germanlanguagelevel",DataController.data.germanlanguagelevel);
        dict.Add("englishlanguagelevel",DataController.data.englishlanguagelevel);
        dict.Add("highestschoolfinish",DataController.data.highestschoolfinish);
        dict.Add("socialstandplayer",DataController.data.socialstandplayer);
        dict.Add("socialstandparents",DataController.data.socialstandparents);
        dict.Add("religion",DataController.data.religion);
        dict.Add("countryoforigin",DataController.data.countryoforigin);
		dict.Add("question", questionIndex+1);
        dict.Add("timeremaining",timeUsed);
        dict.Add("totaltime",DataController.data.totaltime);
        dict.Add("playerscore",DataController.data.playerScore);
        dict.Add("averagecorrect",DataController.data.averageCorrect.ToString());
        dict.Add("questionid",questionPool[questionIndex].questionID);
        dict.Add("questioncorrect",isCorrect);
        foreach(KeyValuePair<string,object> key in dict)
        {
            finalString+=key.Key+"="+key.Value+"&"; 
        }
        finalString+="\n";
        dict.Clear();
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
    }

    public void EndRound()
    {
        isRoundActive = false;

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
        StartCoroutine(makePost());
    }

    public void ReturnToMenu()
    {
    
        SceneManager.LoadScene("MenuScreen");
        
        dict.Clear();
    }

    private string authenticate (string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }

    IEnumerator makePost()
    {
        WWWForm form = new WWWForm();
        form.AddField("data",finalString);
        string authorization = authenticate("****", "*****");
        //UnityWebRequest www = UnityWebRequest.Get("https://nw9.asuscomm.com/QuizReceiver/receive.php");
        //www.SetRequestHeader("AUTHORIZATION",authorization);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://nw9.asuscomm.com/QuizReceiver/receive.php", finalString))
        {
            www.SetRequestHeader("AUTHORIZATION",authorization);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

}