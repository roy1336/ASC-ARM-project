using UnityEngine;
using System.Collections;

public class DeathCollision : MonoBehaviour {
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
			colCom.hasCollideDeath(other.gameObject);
	}
	public interface CollisionCommunication{
		void hasCollideDeath(GameObject asteroide);
	}

}
