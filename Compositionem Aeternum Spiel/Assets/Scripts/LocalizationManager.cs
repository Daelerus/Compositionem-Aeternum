using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;                                                  // The System.IO namespace contains functions related to loading and saving files
using UnityEngine.Experimental.Networking ;
public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";
    private string dataAsJson;

    // Use this for initialization
    void Awake () 
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string> ();
        string filePath;

        filePath = Path.Combine (Application.streamingAssetsPath + "/", fileName);
        Debug.Log("filePath " + filePath);

        //dataAsJson = File.ReadAllText(filePath);
        if(filePath.Contains("://") || filePath.Contains(":///"))
        {
            filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/"+ fileName);
            //filePath = System.IO.Path.Combine(Application.persistentDataPath , fileName);
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            StartCoroutine(getWebRequest(filePath));
                    
        }                                         
        else
        {
            dataAsJson = File.ReadAllText(filePath);
        }
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        //Debug.Log ("Data loaded    : " + loadedData);
        for (int i = 0; i < loadedData.items.Length; i++) 
        {
            localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);
            //Debug.Log("Added " + loadedData.items [i].key + "And   " + loadedData.items [i].value);    
        }
        Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        DataController.data.language = fileName;
        isReady = true;

    }

    IEnumerator getWebRequest(string filePath)
    {
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
            Debug.LogError(www.error);
        else
            Debug.Log(www.downloadHandler.text);
            dataAsJson = www.downloadHandler.text;                   
    }

    

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey (key)) 
        {
            result = localizedText [key];
        }

        return result;

    }

    public bool GetIsReady()
    {
        return isReady;
    }

}