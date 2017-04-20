using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private GameObject playerObject;
	private PlayerMotor playerMotor;
	private bool isDead = false;
	private bool survival = false;
	private bool money = false;
	private int countSurvival = 0;
	private int countMoney = 0;

	[Header ("Tile Manager")]
	public float spawnZ = -3.0f;
	public float tileLength = 6.0f;
	public float safeZone = 12.0f;
	public int tilesOnScreen = 4;
	private int lastPrefabIndex = 0;
	private List<GameObject> activeTiles;


	[Header ("Score Manager") ]
	public GameObject ScoreUI;
	public float totalScore = 0;
	public Text scoreText;
	public int totalCoins = 0;
	public Text coinsText;

	[Header ("Difficulty Level")]
	public int currentLevel = 1;
	public int scoreNextLevel = 10;

	[Header ("Coins & Power Ups")]
	public float itemSpawnInterval = 5.0f;

	[Header("Game Over UI")]
	public GameObject GameOverObject;
	public GameOverManager goManager;

	[Header("Achivements UI")]
	public GameObject survivalAchivement;
	public GameObject moneyAchivement;

	#region Basic Functions
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		playerMotor = playerObject.GetComponent<PlayerMotor> ();
		activeTiles = new List<GameObject>();
		for (int i = 0; i < tilesOnScreen; i++) {
			SpawnTile (lastPrefabIndex);
		}
		StartCoroutine ("ItemSpawn");
	}
	
	// Update is called once per frame
	void Update () {

		if(playerObject.transform.position.z-safeZone > (spawnZ - tilesOnScreen * tileLength) ){
			SpawnTile (1);
			DeleteTile ();
		}


		if(totalScore >= scoreNextLevel){
			LevelUp ();
		}
			

		if(!isDead){

			totalScore += Time.deltaTime;
			scoreText.text = "Seconds: " + ((int)totalScore).ToString ();

			if ((int)totalScore >= 30) {
				survival = true;
				countSurvival++;
			}

			if(survival && (countSurvival==1) ){
				survivalAchivement.SetActive (true);
				survivalAchivement.AddComponent<DestroyAchivement> ();
				return;
			}

			if (money && (countMoney==1) ) {
				moneyAchivement.SetActive (true);
				moneyAchivement.AddComponent<DestroyAchivement> ();
				return;
			}

		}




		
	}
	#endregion


	#region Tile Management Functions

	private void SpawnTile(int prefabIndex ){
		GameObject tile;
		int rand = 0; 

		if (prefabIndex != 0){
			rand = RandomPrefabIndex ();
		}else{
			rand = 0;
		}
		tile = Instantiate (Resources.Load ("Track_" + rand)) as GameObject;	
		tile.transform.SetParent (transform);
		tile.transform.position = Vector3.forward * spawnZ;
		spawnZ += tileLength;
		activeTiles.Add (tile);

	}

	private int RandomPrefabIndex(){
		int randomIndex = lastPrefabIndex;
		while (randomIndex == lastPrefabIndex){
			randomIndex = Random.Range (0, 5);
		}
		lastPrefabIndex = randomIndex;
		return randomIndex;
	}

	private void DeleteTile(){
		Destroy (activeTiles [0]);
		activeTiles.RemoveAt (0);
	}

	#endregion

	#region Item Spawn Functions
	/*
	public void StartItems(){
		Debug.Log ("inicio corrutinas");
		StartCoroutine ("ItemSpawn");
	}*/

	IEnumerator ItemSpawn( ){
		if (!isDead) {
			GameObject item;
			int rand = 0; 
			rand = RandomItembIndex ();
			//rand = Random.Range(0,4);
			//Debug.Log("item creado: " + rand);
			item = Instantiate (Resources.Load ("Item_" + rand)) as GameObject;
			item.transform.position = new Vector3 (RandomXPos (), 1.0f, spawnZ);

			yield return new WaitForSeconds (itemSpawnInterval);

			StartCoroutine ("ItemSpawn");

		} else {
			yield return new WaitForSeconds(0.0001f);
		}

	}

	private float RandomXPos(){
		int randomIndex = 0;
		float result = 0.0f;
		randomIndex = Random.Range (0, 4);

		if (randomIndex == 1) {
			result = -1.96f;
		} else if (randomIndex == 2) {
			result = 0.0f;
		} else if (randomIndex == 3) {
			result = -1.96f;
		}

		return result;
	}

	private int RandomItembIndex(){
		int randomIndex = 1;

		randomIndex = (int) Random.Range (0, 4);
		if(randomIndex ==  0){
			randomIndex = 1;
		}else if(randomIndex == 4){
			randomIndex = 3;
		}

		return randomIndex;
	}



	#endregion

	#region Difficulty Level

	private void LevelUp(){
		scoreNextLevel *= 2;
		currentLevel++;

		if (currentLevel < 10) {
			playerMotor.characterSpeed+=currentLevel;	
		}
	}

	#endregion

	#region PlayerColisions

	public void DeathPlayer(){
		isDead = true;
		ScoreUI.SetActive (false);
		GameOverObject.SetActive (true);
		goManager.loadData ((int)totalScore,totalCoins);
	}

	public void AddCoinCount(){
		totalCoins++;
		coinsText.text = "Coins: " + totalCoins.ToString ();
		if (totalCoins>=40){
			money = true;
			countMoney++;
		}
	}

	#endregion



}
