using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Main : TouchLogicalOneTouch, SafeCollision.CollisionCommunication,DeathCollision.CollisionCommunication, GarbageCollision.CollisionCommunication {

	public Button pause;
	public Text textBomb;
	public Text textSpeed;
	public GameObject explosive;
	private GameObject expl;
	private Ray ray1;
	private RaycastHit hit1;
	private List<GameObject> bombs;
	private GameObject cameraMain;
	private int currentBomb;
	private Vector3 speed;
	private GameObject aster;
	private bool isPaused;
	private int state;
	public Image messageBackground;
	public Image messageContent;

	private const float MIN_SPEED = 8;
	private const int PLAYING_STATE = 0;
	private const int PAUSED_STATE = 1;
	private const int WIN_STATE = 2;
	private const int LOSE_STATE = 3;
	private const int INIT_STATE = 4;

	public Sprite pauseText;
	public Sprite resumeText;
	public Sprite winText;
	public Sprite winBack;
	public Sprite loseText;
	public Sprite loseBack;
	public Sprite highSpeed;
	public Sprite normalSpeed;

	private bool wait;
	private float waitTime;
	private float radioLuna ;
	private Vector3 posicionLuna,vectA,vectB;
	//private int bombCount;
	// Use this for initialization
	void Start () {
		isPaused = false;
		radioLuna = 1000;
		posicionLuna = GameObject.Find("Luna").transform.position;
		state = INIT_STATE;

		pause.image.overrideSprite = pauseText;
		pause.onClick.AddListener(()=> pauseGame());
		wait = true;
		waitTime = 1.5f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (state) {
		case PLAYING_STATE:
			CheckTouches ();
			cameraMain.transform.Translate (speed);
			textSpeed.text = speed.magnitude.ToString();
			if(speed.magnitude > MIN_SPEED){
				aster.GetComponent<SpriteRenderer>().sprite = highSpeed;
			}
			else{
				aster.GetComponent<SpriteRenderer>().sprite = normalSpeed;
			}
			if(aster.transform.position.x < posicionLuna.x ||aster.transform.position.y < posicionLuna.y){
				lose ();
			}
			break;
		case WIN_STATE:
			if (wait) {
				if(waitTime > 0){
					waitTime -= Time.fixedDeltaTime;
					Debug.Log(waitTime.ToString());
					cameraMain.transform.RotateAround(posicionLuna,new Vector3(0,0,1), speed.magnitude);
				}
				else{
					wait = false;
					//Application.LoadLevel("FeedBack");
				}
			}
			break;
		case LOSE_STATE:
			if (wait) {
				if(waitTime > 0){
					waitTime -= Time.fixedDeltaTime;
					cameraMain.transform.Rotate (new Vector3(0,0,1));
					Debug.Log(waitTime.ToString());
				}
				else{
					wait = false;
					//Application.LoadLevel("FeedBack");
				}
			}
			break;
		case INIT_STATE:
			if (wait) {
				if(waitTime > 0){
					waitTime -= Time.fixedDeltaTime;
				}
				else{
					wait = false;
					state = PLAYING_STATE;
					messageBackground.enabled = false;
					messageContent.enabled = false;
				}
			}
			break;
		case PAUSED_STATE:
			break;
		}
	}

	private void pauseGame(){
		if (isPaused) {
			pause.image.overrideSprite = pauseText;
			//messageBackground.enabled = false;
			isPaused = false;
			state = PLAYING_STATE;
		} 
		else {
			//messageBackground.enabled = true;
			pause.image.overrideSprite = resumeText;
			isPaused=true;
			state = PAUSED_STATE;
		}
	}

	void OnEnable(){
		Init script;
		script = GetComponent<Init> ();
		bombs = script.getBombs();
		script.enabled = false;
		speed = new Vector3 (-0.5f, -0.02f, 0f);
		aster = GameObject.Find ("Main Camera/Aster");
		cameraMain = GameObject.Find ("Main Camera");
		isPaused = false;
		messageBackground.enabled = true;
		messageContent.enabled = true;

	}

	public override void OnTouchEndedAnywhere(){
		ray1 = cameraMain.camera.ScreenPointToRay (touch.position);
		if (Physics.Raycast (ray1, out hit1) && hit1.transform.name == "Bomb(Clone)") {
			currentBomb = bombs.FindIndex(x => x.transform.position.Equals(hit1.transform.position));
			explotarBomba ();

		}
	}

	private void explotarBomba(){
		GameObject bombToDelete;
		bombToDelete = bombs[currentBomb];
		if (expl != null) {
			GameObject.Destroy(expl);
		}
		speed = speed + aster.transform.position - bombToDelete.transform.position;
		speed.z = 0f;
		speed *= 0.1f;
		bombs.RemoveAt(currentBomb);
		expl= (GameObject)Instantiate (explosive, bombToDelete.transform.position, Quaternion.identity);
		expl.transform.SetParent(cameraMain.transform);
		Object.DestroyObject(bombToDelete);

		textBomb.text = "0"+bombs.Count.ToString();

		if (bombs.Count == 0) {
			if(wrongWay()){
				lose ();
			}
		}
	}

	void SafeCollision.CollisionCommunication.hasCollideSafe(GameObject asteroide){
		win ();

	}

	void DeathCollision.CollisionCommunication.hasCollideDeath(GameObject asteroide){
		lose ();
	}
	Vector3 GarbageCollision.CollisionCommunication.hasCollideGarbage(GameObject garbage){
		Vector3 speedChange;
		speedChange = aster.transform.position - garbage.transform.position;
		speedChange.z = 0;
		speed = speed + speedChange.normalized;
		return -speedChange.normalized*2;
	}

	void win(){
		if (speed.magnitude <= MIN_SPEED) {
			state=WIN_STATE;
			messageBackground.overrideSprite = winBack;
			messageContent.overrideSprite = winText;
			messageBackground.enabled = true;
			messageContent.enabled = true;
			wait=true;
			waitTime = 2f;
		}
	}

	void lose(){
		state = LOSE_STATE;
		messageBackground.overrideSprite = loseBack;
		messageContent.overrideSprite = loseText;
		messageBackground.enabled = true;
		messageContent.enabled = true;
		wait=true;
		waitTime = 2f;
	}
	bool wrongWay(){
		float alfa, beta, gamma;
		Vector3 nSpeed = speed.normalized;
		vectA = new Vector3 (posicionLuna.x - aster.transform.position.x,
		                   radioLuna + posicionLuna.y - aster.transform.position.y,
		                  0);
		vectB = new Vector3 (radioLuna + posicionLuna.x - aster.transform.position.x,
			                   posicionLuna.y - aster.transform.position.y,
		                   0);
		vectA = vectA.normalized;
		vectB = vectB.normalized;
		alfa = (vectA.x * vectB.x) + (vectA.y * vectB.y) ;
		beta = (vectA.x * nSpeed.x) + (vectA.y * nSpeed.y) ;
		gamma = (vectB.x * nSpeed.x) + (vectB.y * nSpeed.y);
		//Debug.Log (alfa.ToString()+" "+beta.ToString()+" "+gamma.ToString() );
		if (beta < alfa || gamma < alfa) {
			return true;
		}
		return false;

	}


}
