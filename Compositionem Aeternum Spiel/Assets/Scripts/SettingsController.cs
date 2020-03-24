using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
	private SettingsData settings;
	private DataController dataController;
	private Dropdown dropdownGender;
	private Dropdown dropdownGermanLanguageLevel;
	private Dropdown dropdownEnglishLanguageLevel;
	private Dropdown dropdownHighestSchoolFinish;
	private Dropdown dropdownSocialStandPlayer;
	private Dropdown dropdownSocialStandParents;
	private Dropdown dropdownReligion;
	private Dropdown dropdownCountryOfOrigin;
	private Slider sliderAge;
	public Text ageDisplay;


	void Start()
	{
		sliderAge = GameObject.Find("sliderAge").GetComponent<Slider>();
		sliderAge.value = PlayerPrefs.GetFloat("Age");
		dropdownGender = GameObject.Find("dropdownGender").GetComponent<Dropdown>();
		dropdownGender.value = PlayerPrefs.GetInt("Gender");
		dropdownGermanLanguageLevel = GameObject.Find("dropdownGermanLanguageLevel").GetComponent<Dropdown>();
		dropdownGermanLanguageLevel.value = PlayerPrefs.GetInt("GermanLanguageLevel");
		dropdownEnglishLanguageLevel = GameObject.Find("dropdownEnglishLanguageLevel").GetComponent<Dropdown>();
		dropdownEnglishLanguageLevel.value  = PlayerPrefs.GetInt("EnglishLanguageLevel");
		dropdownHighestSchoolFinish = GameObject.Find("dropdownHighestSchoolFinish").GetComponent<Dropdown>();
		dropdownHighestSchoolFinish.value = PlayerPrefs.GetInt("HighestSchoolFinish");
		dropdownSocialStandPlayer = GameObject.Find("dropdownSocialStandPlayer").GetComponent<Dropdown>();
		dropdownSocialStandPlayer.value = PlayerPrefs.GetInt("SocialStandPlayer");
		dropdownSocialStandParents = GameObject.Find("dropdownSocialStandParents").GetComponent<Dropdown>();
		dropdownSocialStandParents.value = PlayerPrefs.GetInt("SocialStandParents");
		dropdownReligion = GameObject.Find("dropdownReligion").GetComponent<Dropdown>();
		dropdownReligion.value = PlayerPrefs.GetInt("Religion");
		dropdownCountryOfOrigin = GameObject.Find("dropdownCountryOfOrigin").GetComponent<Dropdown>();
		dropdownCountryOfOrigin.value = PlayerPrefs.GetInt("CountyOfOrigin");
	}

	public void StartMenu()
	{
		SaveSettings();
		PlayerPrefs.Save();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Menuscreen");
	}

	public void SaveSettings()
	{
		//get Setting Components and set PlayerPrefs, Int in the Dropdown is required to get the Index
		sliderAge = GameObject.Find("sliderAge").GetComponent<Slider>();
		PlayerPrefs.SetFloat("Age", sliderAge.value);

		dropdownGender = GameObject.Find("dropdownGender").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("Gender", dropdownGender.value);

		dropdownGermanLanguageLevel = GameObject.Find("dropdownGermanLanguageLevel").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("GermanLanguageLevel", dropdownGermanLanguageLevel.value);

		dropdownEnglishLanguageLevel = GameObject.Find("dropdownEnglishLanguageLevel").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("EnglishLanguageLevel", dropdownEnglishLanguageLevel.value);	
		
		dropdownHighestSchoolFinish = GameObject.Find("dropdownHighestSchoolFinish").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("HighestSchoolFinish", dropdownHighestSchoolFinish.value);
		
		dropdownSocialStandPlayer = GameObject.Find("dropdownSocialStandPlayer").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("SocialStandPlayer", dropdownSocialStandPlayer.value);
		
		dropdownSocialStandParents = GameObject.Find("dropdownSocialStandParents").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("SocialStandParents", dropdownSocialStandParents.value);
		

		dropdownReligion = GameObject.Find("dropdownReligion").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("Religion", dropdownReligion.value);
		

		dropdownCountryOfOrigin = GameObject.Find("dropdownCountryOfOrigin").GetComponent<Dropdown>();
		PlayerPrefs.SetInt("CountyOfOrigin", dropdownCountryOfOrigin.value);
		
		PlayerPrefs.Save();

		//Send Current Settings to Datacontroller
		DataController.data.age = sliderAge.value;
		DataController.data.gender = dropdownGender.value;
		DataController.data.germanlanguagelevel = dropdownGermanLanguageLevel.value;
		DataController.data.englishlanguagelevel  = dropdownEnglishLanguageLevel.value;
		DataController.data.highestschoolfinish = dropdownHighestSchoolFinish.value;
		DataController.data.socialstandplayer = dropdownSocialStandPlayer.value;
		DataController.data.socialstandparents = dropdownSocialStandParents.value;
		DataController.data.religion = dropdownReligion.value;
		DataController.data.countryoforigin = dropdownCountryOfOrigin.value;

		PlayerPrefs.Save();		

	}

}
