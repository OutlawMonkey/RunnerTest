using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAchivement : MonoBehaviour {

	public float destroyTime = 2.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine ("StartCountdown");
	}

	// Update is called once per frame
	void Update () {

	}

	/**
	 * metodo para cambiar los colores de las balas mandar llamar cada que se piensa cambiar el color de la bala
	 */
	public IEnumerator StartCountdown( ){

		yield return new WaitForSeconds(destroyTime);// max life duration of particle
		executeDestroy ();//caling destrcution of particle
	}

	public void executeDestroy(){
		Destroy (gameObject); 
	}
}
