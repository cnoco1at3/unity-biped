using UnityEngine;
using System.Collections;

public class UIFollowPlayer : MonoBehaviour {
	[SerializeField] float distanceFromPlayer = 0.05f;
	[SerializeField] Transform player;
	[SerializeField] Transform lookTarget;

	// Update is called once per frame
	void Update () {
		transform.position = lookTarget.position + Vector3.Normalize(player.position - lookTarget.position) * distanceFromPlayer;
		transform.LookAt (player);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
	}
}
