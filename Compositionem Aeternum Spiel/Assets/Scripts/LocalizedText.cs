﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    public string key;

    // Use this for initialization
    void Start () 
    {
        //Text text = GetComponent<Text>();
        Text text = GetComponent<Text>();
        if ( text == null)
            Debug.LogError("No Text Component found");
        text.text = LocalizationManager.instance.GetLocalizedValue (key);        
    }

}