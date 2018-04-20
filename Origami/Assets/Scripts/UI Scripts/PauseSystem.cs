﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour {
	public bool pause = false; 
	// Use this for initialization
	void Start () {
		CollectableManager.Startup ();
	}
	
	// Update is called once per frame
	void Update () {
		Component[] canvases;
		canvases = GetComponentsInChildren<Canvas>();
		pause = false; 
		foreach (Canvas can in canvases){
			if (can.enabled && can.name != "Fade") {
				pause = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
			if (!pause) {
				this.transform.Find ("Pause Menu").GetComponent<Canvas> ().enabled = true;
			} else {
				foreach (Canvas can in canvases) {
					can.enabled = false;
				}
			}
		}
		if (pause) {
			Cursor.visible = true;
			Time.timeScale = 0.0F;
			AkSoundEngine.SetRTPCValue("MenuUp", 1.0f, null);
		} else {
			Time.timeScale = 1.0F;
			Cursor.visible = false;
			AkSoundEngine.SetRTPCValue("MenuUp", 0.0f, null);
		}
		if (pause) {
			this.transform.Find ("Fade").GetComponent<Canvas> ().enabled = true;
		} else {
			this.transform.Find ("Fade").GetComponent<Canvas> ().enabled = false;
		}
	}
}
