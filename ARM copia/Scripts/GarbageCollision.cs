using UnityEngine;
using System.Collections;

public class GarbageCollision : MonoBehaviour {
	CollisionCommunication colCom; //Interface for collisions
	private bool move;
	private Vector3 gMove;
	// Use this for initialization
	void Start () {
		move = false;
		colCom=(CollisionCommunication)GameObject.Find("ScriptObject").GetComponent<Main>();
	}
	
	// Update is called once per frame
	void Update () {
		if (move)
			this.transform.Translate (gMove);;
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Aster")
			gMove = colCom.hasCollideGarbage(this.gameObject);
			move = true;
	}
	public interface CollisionCommunication{
		Vector3 hasCollideGarbage(GameObject garbage);
	}
	
}
