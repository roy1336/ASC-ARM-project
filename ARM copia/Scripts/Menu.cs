using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Menu : MonoBehaviour {
	public Button explosive;
	public Button rocket;

	// Use this for initialization
	void Start () {
		explosive.onClick.AddListener(()=> Application.LoadLevel("ARM_E"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
