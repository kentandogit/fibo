using UnityEngine;
using System.Collections;

public class BoxScript : TouchLogic {

	public GameObject explosion;
	private FiboGameController gameController;
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <FiboGameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTouchEnded()
	{
		TextMesh tm = this.GetComponentInChildren<TextMesh> ();
		if(!gameController.getGameOver())
			Instantiate(explosion, transform.position, transform.rotation);
		gameController.NextSequence (int.Parse(tm.text));
	}
}
