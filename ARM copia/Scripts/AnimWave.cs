using UnityEngine;
using System.Collections;

public class AnimWave : MonoBehaviour {
	public Sprite[] wave;
	private int waveCount;
	private float waitTime;
	private SpriteRenderer currentSprite;
	// Use this for initialization
	void Start () {
		currentSprite = gameObject.GetComponent <SpriteRenderer>();
		waveCount = 4;
		waitTime = 0.08f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (waitTime > 0) {
			waitTime -= Time.fixedDeltaTime;
		} else {
			waitTime = 0.08f;
			waveCount--;
			if(waveCount == -1)
				waveCount=4;
			currentSprite.sprite= wave [waveCount];
		}
	}
}
