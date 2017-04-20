using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreenScript : MonoBehaviour {

	public bool state = false;
	public GameObject StartButton;
	public GameObject labelContainer; 
	public GameObject coinsAchivements;
	public GameObject timeAchivements;

	void Start(){
		Debug.Log ("survive: "+ PlayerPrefs.GetInt ("survive") );
		Debug.Log ("money: "+ PlayerPrefs.GetInt ("money") );
	}

	public void GoToGame(){
		SceneManager.LoadScene ("GameScene",LoadSceneMode.Single);
	}

	public void DisplayAchivements(){
		if (state) {
			if (PlayerPrefs.GetInt ("survive") == 1) {
				timeAchivements.SetActive (false);
			}
			if (PlayerPrefs.GetInt ("money") == 1) {
				coinsAchivements.SetActive (false);
			}
			StartButton.SetActive (true);
			labelContainer.SetActive (true);
			state = false;
		} else {

			if (PlayerPrefs.GetInt ("survive") == 1) {
				timeAchivements.SetActive (true);
			}
			if (PlayerPrefs.GetInt ("money") == 1) {
				coinsAchivements.SetActive (true);
			}
			StartButton.SetActive (false);
			labelContainer.SetActive (false);

			state = true;
		}
	}
}
