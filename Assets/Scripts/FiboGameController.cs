using UnityEngine;
using System.Collections;

public class FiboGameController : MonoBehaviour {

	public TextMesh ScoreText;
	public TextMesh	TimerText;
	public TextMesh	GameOverText;
	public int timer;
	private int prevNum;
	private int currentNum;
	private int targetNum;
	private int goalTime;
	private int[] boxValues;
	private bool gameOver;
	private bool restart;
	private int boxes;
	private int highscore;
	// Use this for initialization
	void Start () 
	{
		GameOverText.text = "";
		gameOver = false;
		restart = false;
		TimerText.text = "35";
		prevNum = 1;
		currentNum = 1;
		targetNum = 2;
		boxes = 6; // number of boxes we have
		ScoreText.text = "Fibonacci Sequence:\n" + prevNum + ", " + currentNum;
		goalTime = (int) Time.time + timer;
		boxValues = new int[boxes];
		highscore = PlayerPrefs.GetInt("Highscore", highscore);
		GameObject bestScore = GameObject.Find("BestBox/BestScore");
		TextMesh bestTm = bestScore.GetComponent<TextMesh> ();
		if(highscore >= 100000 && highscore < 1000000) // 100,000 or more font size is 40
			bestTm.fontSize = 40;
		if(highscore >= 1000000) // 1M or more font size 37
			bestTm.fontSize = 35;
		bestTm.text = highscore.ToString();
		NextSequence (0);
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameOver)
		{
			float time = goalTime - Time.time;
			TimerText.text = time.ToString ("0.00") + " seconds";
			if(time <= 0)
			{
				TimerText.text = "0.00";
				GameOver();
			}
		}

		if (restart && Input.GetButton("Fire1"))
		{
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	public void NextSequence(int clicked)
	{
		if (gameOver)
			return;

		if(clicked != 0 && clicked != targetNum)
		{
			GameOver ();
		}

		if(clicked == targetNum)
		{
			prevNum = currentNum;
			currentNum = targetNum;
			targetNum = prevNum + targetNum;

			if(currentNum < 8)
				ScoreText.text = string.Concat(ScoreText.text,", " + currentNum);
			else
			{
				ScoreText.text = "Fibonacci Sequence:\n";
				string seqText = prevNum + ", " + currentNum;
				int tempPrev = prevNum;
				int tempCurr = currentNum;
				int newCalc = 0;
				for(int i = 0; i < 3; i++)
				{
					newCalc = tempCurr - tempPrev;
					seqText = string.Concat(newCalc + ", ",seqText);
					tempCurr = tempPrev;
					tempPrev = newCalc;
				}
				ScoreText.text = string.Concat(ScoreText.text,seqText);
			}
			goalTime = (int) Time.time + timer;
		}

		int targetBox = (int) Random.Range (0.0f,5.0f);
		boxValues [targetBox] = targetNum; // pick a random box number for the correct answer
		for(int i = 0; i < boxes; i++) // then pick random number for the rest of the boxes
		{
			if(i == targetBox)
				continue;

			int boxVal = 0;
			if(targetNum < 100)
				boxVal = (int) Random.Range(0.0F,100.0F);
			if(targetNum > 100)
				boxVal = (int) Random.Range(targetNum - 100,targetNum + 100); // random a value +-100 of the target value
			boxValues [i] = boxVal;
		}

		for(int i = 0; i < boxes; i++) // then assign  the number values into the boxes
		{
			string boxname = string.Concat("Box",i.ToString());
			GameObject box = GameObject.Find(boxname);
			TextMesh tm = box.GetComponentInChildren<TextMesh> ();

			if(boxValues [i] >= 100000 && boxValues [i] < 1000000) // 100,000 or more font size is 40
				tm.fontSize = 45;
			if(boxValues [i] >= 1000000) // 1M or more font size 37
				tm.fontSize = 40;

			tm.text = boxValues [i].ToString();
		}
	}

	public void GameOver ()
	{
		GameOverText.text = "Game Over!";
		gameOver = true;
		if(currentNum > highscore)
		{
			PlayerPrefs.SetInt("Highscore", currentNum);
			GameObject bestScore = GameObject.Find("BestBox/BestScore");
			TextMesh bestTm = bestScore.GetComponent<TextMesh> ();
			if(currentNum >= 100000 && currentNum < 1000000) // 100,000 or more font size is 40
				bestTm.fontSize = 40;
			if(currentNum >= 1000000) // 1M or more font size 37
				bestTm.fontSize = 35;
			bestTm.text = currentNum.ToString();
		}
			
		AudioSource aSource = GetComponent<AudioSource> ();
		aSource.Play ();
		StartCoroutine (RestartRoutine ());
	}

	public bool getGameOver()
	{
		return gameOver;
	}

	IEnumerator RestartRoutine ()
	{
		yield return new WaitForSeconds (5);
		GameOverText.text = "Game Over!\nTap to retry!";
		restart = true;
	}
}
