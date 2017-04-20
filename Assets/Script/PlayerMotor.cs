using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private CharacterController characterController;
	private Vector3 moveVector;
	private Vector3 tempVector;
	private float animationDuration = 1.5f;
	private GameManager gameManager;
	private float turboTime = 10.0f;
	private float starTime = 0.0f;
	private bool isDead = false;
	private Animation playerAnim;
	private Color OriginalColor;
	public Renderer shader;
	public Material material;

	[Header ("Player Physics")]
	public float characterSpeed = 5.0f;
	public float jumpSpeed = 5.0f;

	public float verticalVelocity = 0.0f;
	public float gravity = 12.0f;


	[Header ("Power Up States")]
	public bool speedX = false;
	public float speedFactor = 0.0f;
	public float speedXduration = 0.0f;
	public bool noDamage = false;
	public float noDamageDuration = 0.0f;
	public Color powerColor;

	[Header ("Swipe Controls")]
	public float maxTime = 0.5f;
	public float minSwipeDist = 50.0f;
	private float startTime;
	private float endTime;

	private Vector3 startPos;
	private Vector3 endPos;
	private float swipeDistance;
	private float swipeTime;


	void Start () {
		characterController = gameObject.GetComponent<CharacterController> ();
		playerAnim = gameObject.GetComponent<Animation> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		ColorUtility.TryParseHtmlString("#919085FF",out OriginalColor) ;

		shader = transform.GetChild (1).gameObject.GetComponent<Renderer>();
		material = shader.material;


	}

	void Update () {

		if (isDead){
			return;
		}

		if(noDamage){
			if(Time.time > starTime){
				noDamage = false;
				material.SetColor ("_Color",OriginalColor);
			}	
		}


		if (Time.time < animationDuration) {
			characterController.Move (Vector3.forward * characterSpeed * Time.deltaTime);
			playerAnim.Play ("soldierRun");
			//gameManager.StartItems ();
			return;
		}

		moveVector = Vector3.zero;
		tempVector = Vector3.zero;

		if (characterController.isGrounded) {
		
			if ( Input.GetButtonDown ("Jump") ) {
				verticalVelocity = jumpSpeed;

			} else {
				verticalVelocity = -0.5f;
				if (!playerAnim.IsPlaying ("soldierRun")) {
					
					playerAnim.Play ("soldierRun");
				}

			}

					
		}else {
			verticalVelocity -= gravity * Time.deltaTime;
			playerAnim.Play ("soldierLanding");

		}

		// left or right

		if (Input.GetAxis("Horizontal") > 0){
			if(transform.position.x < 1.96f){
				moveVector.x += 1.96f;
			}	
		}else if (Input.GetAxis("Horizontal") < 0){
			if(transform.position.x > -1.96f){
				moveVector.x -= 1.96f;
			}	
		}

		//touch detection

		if (Input.touchCount > 0) {

			Touch touch = Input.GetTouch (0);
			if(touch.phase == TouchPhase.Began){

				startTime = Time.time;
				startPos = touch.position;

			}else if(touch.phase == TouchPhase.Ended){

				endTime = Time.time;
				endPos = touch.position;

				swipeDistance = (endPos - startPos).magnitude;
				swipeTime = endTime - startTime;

				if (swipeTime < maxTime && swipeDistance > minSwipeDist) {

					Vector2 distance = endPos - startPos;

					if (Mathf.Abs (distance.x) > Mathf.Abs (distance.y)) {
						if(distance.x > 0){
							if(transform.position.x < 1.9f){
								moveVector.x += 100.0f;
							}	
						}else if(distance.x < 0){
							if(transform.position.x > -1.9f){
								moveVector.x -= 100.0f;
							}
						} 

					}else if (Mathf.Abs (distance.x) < Mathf.Abs (distance.y)) {
						if(distance.y > 0){
							if (characterController.isGrounded) {
									verticalVelocity = jumpSpeed;
							}

						}

					}


					Debug.Log ( moveVector +" * "+Time.deltaTime+ " = "+(moveVector*Time.deltaTime));

					Debug.Log ( moveVector.x +" * "+Time.deltaTime+ " = "+(moveVector.x*Time.deltaTime));

				}
			}

		}

		// up
		moveVector.y = verticalVelocity;


		// forward
		if(speedX){
			if (Time.time >= turboTime) {
				speedX = false;
				characterSpeed -= speedFactor;
				material.SetColor ("_Color",OriginalColor);
			}
		}

		moveVector.z = characterSpeed;
		//Debug.Log (moveVector * Time.deltaTime);

		tempVector = moveVector * Time.deltaTime;

		if (tempVector.x > 1.9f) {
			tempVector.x = 1.9f;
		}else if(tempVector.x < -1.9f){
			tempVector.x = -1.9f;
		}

		//characterController.Move (moveVector * Time.deltaTime);
		characterController.Move (tempVector);
	}


	private void OnControllerColliderHit (ControllerColliderHit hit){
		if (hit.point.z > transform.position.z + characterController.radius) {
			string tag = hit.collider.tag;
			if (tag == "Obstacle") {
				Debug.Log ("noDamage "+noDamage);
				if (!noDamage) {
					isDead = true;
					gameManager.DeathPlayer ();
				} else {
					Debug.Log ("Time.Time "+ Time.time);
					Debug.Log ("starTime "+ starTime);
					if (Time.time < starTime) {
						Debug.Log ("invencible");
						//hit.gameObject.GetComponent<BoxCollider> ().enabled = false;
						Destroy(hit.gameObject);
					} else {
						Debug.Log ("no invencible");
						noDamage = false;
					}
				}

			} else if (tag == "Coin") {
				gameManager.AddCoinCount ();
				Destroy(hit.gameObject);
			}else if (tag == "SpeedX"){
				turboTime = Time.time + speedXduration;
				speedX = true;
				characterSpeed *= speedFactor;
				//amarillo
				ColorUtility.TryParseHtmlString("#D2CC03FF",out powerColor) ;
				material.SetColor ("_Color",powerColor);

				Destroy(hit.gameObject);
			}else if(tag == "NoDamage"){
				noDamage = true;
				starTime = Time.time + noDamageDuration;
				//azul
				ColorUtility.TryParseHtmlString("#0016F9FF",out powerColor) ;
				material.SetColor ("_Color",powerColor);
				Destroy(hit.gameObject);
			}
				

		}
	}

}




