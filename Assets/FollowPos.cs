using UnityEngine;
using System.Collections;

public class FollowPos : MonoBehaviour {
	[SerializeField] Transform target;
	[SerializeField] float offsetY;

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(target.position.x, target.position.y - offsetY, target.position.z);
	}
}
