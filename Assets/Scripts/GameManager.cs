using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager singleton {get; private set;}
	[SerializeField] Material gridMat;
	[SerializeField] ControlEngine controller;
	[SerializeField] Tutorial tutorial;
	[SerializeField] Transform destination;
	AudioSource[] sounds;
	public static bool characterPlaced;
	public static bool raceStarted;
	// Use this for initialization
	void Awake () {
		singleton = this;
		sounds = GetComponents<AudioSource>();
		int gongSound = Random.Range(2, sounds.Length);
	}
	
	// Update is called once per frame
	void Update () {
		// check if race is done
		if (GameManager.raceStarted)
		{
			bool raceOver = false;
			Vector3 charPos2D = new Vector3 (controller.root.transform.position.x, 0, controller.root.transform.position.z);
			Vector3 destPos2D = new Vector3 (destination.position.x, 0, destination.position.z);
				if (Vector3.Distance(charPos2D, destPos2D) < 0.05f)
				raceOver = true;
			if (raceOver)
			{
				controller.run = false;
				int gongSound = Random.Range(2, sounds.Length);
				PlaySound(gongSound);
				raceStarted = false;
				characterPlaced = false;
				tutorial.GetComponent<Text>().text += " A WORLD RECORD!!!1! (probably)";
			}
		}	
	}

	public void SetGridVisibility(bool isOn)
	{
		if (isOn)
			gridMat.SetColor("_Color", new Color(1,1,1,1));
		else
			gridMat.SetColor("_Color", new Color(1,1,1,0));
	}

	public void PlaySound (int soundID)
	{
		sounds[soundID].Play();
	}

	public void StopSounds ()
	{
		foreach (AudioSource sound in sounds)
			sound.Stop();
	}

	public void SetDestination(Vector3 dest)
	{
		destination.position = dest;
		controller.SetDesiredPosition(destination.position);
		controller.run = true;
		StopSounds();
		PlaySound(1);
		raceStarted = true;
		tutorial.UpdateText(3);
	}

	public void PlaceCharacter()
	{
		StopSounds();
		PlaySound(0);
		characterPlaced = true;
		raceStarted = false;
		destination.GetChild(0).gameObject.SetActive(false);
	}
}
