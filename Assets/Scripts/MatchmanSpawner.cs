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

/// <summary>
/// Tango floor finding user interface controller. 
/// 
/// Place a marker at the y position of the found floor and allow user to recalculate.
/// </summary>
public class MatchmanSpawner : MonoBehaviour 
{
	[SerializeField] GameObject Matchman;
	GameObject curMatchman;
	RaycastHit hitInfo;
    
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f)), out hitInfo))
			{
				// Limit distance of the marker position from the camera to the camera's far clip plane. This makes sure that the marker
				// is visible on screen when the floor is found.
				if (curMatchman == null)
					curMatchman = Instantiate (Matchman);
				curMatchman.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+1, hitInfo.point.z);
//				Vector3 cameraBase = new Vector3(Camera.main.transform.position.x, hitInfo.point.y, Camera.main.transform.position.z);
//				target = cameraBase + Vector3.ClampMagnitude(hitInfo.point - cameraBase, Camera.main.farClipPlane * 0.9f);
			}
			for (var i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					if (curMatchman == null)
						curMatchman = Instantiate (Matchman);
					curMatchman.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+1, hitInfo.point.z);
				}
			}
		}
    }
}