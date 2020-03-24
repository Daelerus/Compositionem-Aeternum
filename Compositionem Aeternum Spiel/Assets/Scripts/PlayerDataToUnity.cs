using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
public class PlayerDataToUnity : MonoBehaviour
{
    private static Dictionary<string, object> dict = new Dictionary<string, object>();
    // Start is called before the first frame update
    void OnClickQuestion()
    {
        dict["age"] = DataController.data.age;
        dict["gender"] = DataController.data.gender;
        dict["germanlanguagelevel"] = DataController.data.germanlanguagelevel;
        dict["englishlanguagelevel"] = DataController.data.englishlanguagelevel;
        dict["highestschoolfinish"] = DataController.data.highestschoolfinish;
        dict["socialstandplayer"] =DataController.data.socialstandplayer;
        dict["socialstandparents"] =DataController.data.socialstandparents;
        dict["religion"] =DataController.data.religion;
        dict["countryoforigin"] =DataController.data.countryoforigin;
        dict["totaltime"] =DataController.data.totaltime;
        dict["playerscore"] = DataController.data.playerScore;
        dict["averagecorrect"] = DataController.data.averageCorrect;
        Analytics.CustomEvent("QuestionAnswered", dict);

    }

    void OnClickMenu()
    {
        dict["age"] = DataController.data.age;
        dict["gender"] = DataController.data.gender;
        dict["germanlanguagelevel"] = DataController.data.germanlanguagelevel;
        dict["englishlanguagelevel"] = DataController.data.englishlanguagelevel;
        dict["highestschoolfinish"] = DataController.data.highestschoolfinish;
        dict["socialstandplayer"] =DataController.data.socialstandplayer;
        dict["socialstandparents"] =DataController.data.socialstandparents;
        dict["religion"] =DataController.data.religion;
        dict["countryoforigin"] =DataController.data.countryoforigin;
        dict["totaltime"] =DataController.data.totaltime;
        dict["playerscore"] = DataController.data.playerScore;
        dict["averagecorrect"] = DataController.data.averageCorrect;
        Analytics.CustomEvent("GameCompleted", dict);
    }

}
