using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {
	float timer;
	int curText;
	public void UpdateText (int textID)
	{
		curText = textID;
		timer = 0;
	}

	void Update ()
	{
		switch(curText)
		{
		case 1:
			GetComponent<Text>().text = "Set character's starting position.";
			break;
		case 2:
			if (GameManager.characterPlaced)
				GetComponent<Text>().text = "Set character's destination to start!";
			else
				GetComponent<Text>().text = "Looking for floor...";
			break;
		case 3:
			if (GameManager.raceStarted)
				timer += Time.deltaTime;
			string curTime = "" + Mathf.Round(timer * 100) / 100;
			GetComponent<Text>().text = curTime;
			break;
		default:
			break;
		}
	}
}
