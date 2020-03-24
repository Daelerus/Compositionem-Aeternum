using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScreenController : MonoBehaviour
{
	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
		DataController.data.totaltime = 0;
		DataController.data.playerScore = 0;
		DataController.data.averageCorrect = 0F;
	}
	public void StartSettings()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("settings");
		DataController.data.totaltime = 0;
		DataController.data.playerScore = 0;
		DataController.data.averageCorrect = 0F;
	}
	public void StartLanguageScreen()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");
		DataController.data.totaltime = 0;
		DataController.data.playerScore = 0;
		DataController.data.averageCorrect = 0F;
	}

}