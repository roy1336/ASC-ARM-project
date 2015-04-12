using UnityEngine;
using System.Collections;

public class SafeCollision : MonoBehaviour {
	CollisionCommunication colCom; //Interface for collisions

	// Use this for initialization
	void Start () {
		colCom=(CollisionCommunication)GameObject.Find("ScriptObject").GetComponent<Main>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Aster")
			colCom.hasCollideSafe(other.gameObject);
	}
	void OnTriggerStay(Collider other){
		if(other.gameObject.name == "Aster")
			colCom.hasCollideSafe(other.gameObject);
	}
	public interface CollisionCommunication{
		void hasCollideSafe(GameObject asteroide);
	}

}
