using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {
	float timer;
	int curText;
	public void UpdateText (int textID)
	{
		curText = textID;
		switch(textID)
		{
		case 1:
			GetComponent<Text>().text = "Set character's starting position.";
			break;
		case 2:
			GetComponent<Text>().text = "Set character's destination to start!";
			break;
		case 3:
			timer = 0;
			break;
		}
	}

	void Update ()
	{
		if (curText == 3)
		{
			if (GameManager.raceStarted)
				timer += Time.deltaTime;
			GetComponent<Text>().text = "" + timer; 
		}
	}
}
