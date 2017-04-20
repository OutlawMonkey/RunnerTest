using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour {


	[Header ("Score Manager") ]
	public Text scoreText;
	public Text coinsText;

	#region Load Data

	public void loadData(int totalScore, int totalCoins){
	
		scoreText.text = "Seconds: " + totalScore.ToString ();
		coinsText.text = "Coins: " + totalCoins.ToString ();

		if (totalScore >= 30){
			PlayerPrefs.SetInt ("survive",1);
		}
		if (totalCoins >= 40){
			PlayerPrefs.SetInt ("money", 1);
		}
	}

	#endregion


	#region Scene Change

	public void GoToMenu(){
		SceneManager.LoadScene ("MenuScene",LoadSceneMode.Single);
	}

	#endregion




}
