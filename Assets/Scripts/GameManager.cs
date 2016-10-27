using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager singleton {get; private set;}
	[SerializeField] GameObject curCharacter;
	// Use this for initialization
	void Awake () {
		if (singleton == null)
		{
			singleton = this;
		}
		else
		{
			Destroy(curCharacter);
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
		DontDestroyOnLoad(curCharacter);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
