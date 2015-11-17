using UnityEngine;
using System.Collections;

public class TouchLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		for (var i = 0; i < Input.touchCount; i++) 
		{
			GUITexture guiTexture = GetComponent<GUITexture> ();
			if (guiTexture.HitTest(Input.GetTouch(i).position))
			{
				if(Input.GetTouch(i).phase == TouchPhase.Began)
				{
					this.SendMessage("OnTouchBegan");
				}

				if(Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					this.SendMessage("OnTouchEnded");
				}
			}
		}
	}

	void OnMouseDown()
	{
		this.SendMessage("OnTouchEnded");
	}
}
