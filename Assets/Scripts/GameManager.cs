using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager singleton {get; private set;}
	[SerializeField] Material gridMat;
	public static bool characterPlaced;
	public static bool raceStarted;
	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetGridVisibility(bool isOn)
	{
		if (isOn)
			gridMat.SetColor("_Color", new Color(1,1,1,1));
		else
			gridMat.SetColor("_Color", new Color(1,1,1,0));
	}
}
