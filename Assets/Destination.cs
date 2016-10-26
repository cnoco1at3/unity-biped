using UnityEngine;
using System.Collections;

public class Destination : MonoBehaviour {
	[SerializeField] Camera tangoCam;
	private bool isDesignated {public get; set;}
	Animation anim;
	RaycastHit hitInfo;

	void Awake()
	{
	}

	// Update is called once per frame
	void Update () {
		for (var i = 0; i < Input.touchCount; ++i)
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began && Physics.Raycast(tangoCam.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo))
			{
				transform.GetChild(0).gameObject.SetActive(true);
				anim = GetComponentInChildren<Animation>();
				anim.Stop();
				anim.Play("ARMarkerShow", PlayMode.StopAll);
				transform.position = hitInfo.point;
				isDesignated = true;

			}
		}
		if(Input.GetMouseButtonDown(0) && Physics.Raycast(tangoCam.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			transform.GetChild(0).gameObject.SetActive(true);
			anim = GetComponentInChildren<Animation>();
			anim.Stop();
			anim.Play("ARMarkerShow", PlayMode.StopAll);
			transform.position = hitInfo.point;
			isDesignated = true;
		}
	}
}
