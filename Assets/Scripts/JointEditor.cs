using UnityEngine;
using System.Collections;

public class JointEditor : MonoBehaviour {
	public enum TranslatePlane {XY, YZ};
	public TranslatePlane translatePlane;
	public Transform joint;
	public Camera displayCam;
	public Transform[] directChildren;

	void Start()
	{
	}

	void Update()
	{
		//set initial button position
		if (joint != null)
			transform.position = displayCam.WorldToScreenPoint(joint.position);
		
	}

	void DrawLine()
	{
	}

	public void SwitchCam(Camera newCam)
	{
		gameObject.SetActive (newCam == displayCam);
	}

	public void OnDrag() {
		//move editor button
		transform.position = Input.mousePosition;
		Vector3 newPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//use this to keep children joints locked
		Vector3[] prevChildPos = new Vector3[joint.childCount];
		//move the joint to button's position
		if (translatePlane == TranslatePlane.XY) {
			for (int i = 0; i < joint.childCount; i++)
				prevChildPos [i] = joint.GetChild(i).position;
			joint.position = new Vector3 (newPosition.x, newPosition.y, joint.position.z);
		} 
		else {
			for (int i = 0; i < joint.childCount; i++)
				prevChildPos [i] = joint.GetChild(i).position;
			joint.position = new Vector3 (joint.position.x, newPosition.y, newPosition.z);
		}
		for (int i = 0; i < joint.childCount; i++)
			joint.GetChild(i).position = prevChildPos[i];
	}
}