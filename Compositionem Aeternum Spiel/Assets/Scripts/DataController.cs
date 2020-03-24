using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;      
using UnityEngine.Networking;                                                  // The System.IO namespace contains functions related to loading and saving files
using UnityEngine.Experimental.Networking ;

public class DataController : MonoBehaviour 
{
    public static DataController data;
    private RoundData[] allRoundData;
    private PlayerProgress playerProgress;
    private SettingsData settingsData;

    private SettingsData settings;
	public float age;
	public int gender;
	public int germanlanguagelevel;
	public int englishlanguagelevel;
	public int highestschoolfinish;
	public int socialstandplayer;
	public int socialstandparents;
	public int religion;
	public int countryoforigin;
    public float totaltime;
    public int playerScore;
    public float averageCorrect;
    public string language;

    private string gameDataFileName = "data.json";

    void Start()
    {
        if( data== null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if( data != this)
        {
            Destroy(gameObject);
        }
        InitiateTotalTime();

        InitiatePlayerScore();

        LoadSettings();

        //LoadGameData();

        LoadPlayerProgress();  


        SceneManager.LoadScene("LoadingScreen");

        //SceneManager.LoadScene("MenuScreen");
    }

    public RoundData GetCurrentRoundData()
    {
        // If we wanted to return different rounds, we could do that here
        // We could store an int representing the current round index in PlayerProgress

        return allRoundData[0];
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if(newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    public void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath;
        #if UNITY_EDITOR
        if(language =="localizedText_de.json")
            filePath = Path.Combine (Application.streamingAssetsPath, "data_de.json");
        else if (language == "localizedText_en.json")
            filePath = Path.Combine (Application.streamingAssetsPath, "data_en.json");
        else
        {
            filePath = null;
            Debug.LogError("Fehler bei Pfad");
        }    

        #elif UNITY_ANDROID
        filePath = System.IO.Path.Combine(Application.persistentDataPath , gameDataFileName);
        if(language =="localizedText_de.json")
        {
            filePath = System.IO.Path.Combine(Application.persistentDataPath , "data_de.json");
            TextAsset file = Resources.Load<TextAsset>("StreamingAssets/data_de") as TextAsset;
            string content = file.ToString();
            allRoundData = JsonUtility.FromJson<GameData>(content).allRoundData;
        }
        else if(language =="localizedText_en.json")
        {
            filePath = System.IO.Path.Combine(Application.persistentDataPath , "data_en.json");
            TextAsset file = Resources.Load<TextAsset>("StreamingAssets/data_en") as TextAsset;
            string content = file.ToString(); 
            allRoundData = JsonUtility.FromJson<GameData>(content).allRoundData;
        }
        else
        {
            filePath = null;
            Debug.LogError("Fehler bei Pfad");
        } 


        #endif

        #if UNITY_EDITOR
        if(File.Exists(filePath) ==true)
        {
            
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath); 
            // Pass the json to JsonUtility, and tell it to create a GameData object from it   
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            //Debug.Log("Content "+ content);         
            // Retrieve the allRoundData property of loadedData
            allRoundData = loadedData.allRoundData;

            
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
        #endif
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if(PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

    public void LoadSettings()
	{
			age = PlayerPrefs.GetInt("Age");
			gender = PlayerPrefs.GetInt("Gender");
			germanlanguagelevel = PlayerPrefs.GetInt("GermanLanguageLevel");
			englishlanguagelevel  = PlayerPrefs.GetInt("EnglishLanguageLevel");
			highestschoolfinish = PlayerPrefs.GetInt("HighestSchoolFinish");
			socialstandplayer = PlayerPrefs.GetInt("SocialStandPlayer");
			socialstandparents = PlayerPrefs.GetInt("SocialStandParents");
			religion = PlayerPrefs.GetInt("Religion");
			countryoforigin = PlayerPrefs.GetInt("CountyOfOrigin");
	}
    
    public void InitiateTotalTime()
    {
            totaltime = 0;
    }

    public void InitiatePlayerScore()
    {
            playerScore = 0;
            averageCorrect = 0F;
    }
}