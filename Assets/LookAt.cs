using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	[SerializeField] Transform target;

	// Update is called once per frame
	void Update () {
		transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
	}
}
