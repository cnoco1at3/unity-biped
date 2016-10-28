//-----------------------------------------------------------------------
// <copyright file="TangoFloorFindingUIController.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using System.IO;
using Tango;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tango floor finding user interface controller. 
/// 
/// Place a marker at the y position of the found floor and allow user to recalculate.
/// </summary>
public class MatchmanSpawner : MonoBehaviour 
{
	[SerializeField] GameObject matchman;
	[SerializeField] GameObject marker;
	[SerializeField] GameObject googleArrow;
	[SerializeField] Camera tangoCam;
	[SerializeField] GameObject placeCharacterButton;
	Vector3 o_rootRot;
	Vector3 o_parentRot;
	RaycastHit hitInfo;
	bool spawning;

	void Start()
	{
		o_parentRot = matchman.transform.eulerAngles;
		o_rootRot = matchman.GetComponent<ControlEngine>().root.transform.localEulerAngles;
	}

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
		if (Physics.Raycast(tangoCam.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f)), out hitInfo))
		{
			marker.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
			if (spawning || Input.GetKeyDown(KeyCode.Space))
			{
				// Limit distance of the marker position from the camera to the camera's far clip plane. This makes sure that the marker
				// is visible on screen when the floor is found.
//				Vector3 cameraBase = new Vector3(tangoCam.transform.position.x, hitInfo.point.y+4, tangoCam.transform.position.z);
//				Vector3 target = cameraBase + Vector3.ClampMagnitude(hitInfo.point - cameraBase, tangoCam.farClipPlane * 0.9f);
				googleArrow.SetActive(true);
				googleArrow.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
				Animation a = GetComponentInChildren<Animation>();
				a.Stop();
				a.Play("ARMarkerShow", PlayMode.StopAll);
				matchman.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+0.3f, hitInfo.point.z);
				matchman.transform.eulerAngles = o_parentRot;
				matchman.GetComponent<ControlEngine>().root.transform.localPosition = new Vector3(0,0,0);
				matchman.GetComponent<ControlEngine>().root.transform.localEulerAngles = o_rootRot;
				matchman.GetComponent<ControlEngine>().SetDesiredPosition(matchman.transform.position);
				matchman.SetActive(true);
				placeCharacterButton.SetActive(true);	
				spawning = false;
			}
		}
	}

	public void PlaceCharacter()
	{
		GameManager.singleton.PlaceCharacter();
		placeCharacterButton.SetActive(false);	
		spawning = true;
		matchman.GetComponent<ControlEngine>().run = false;
	}
}