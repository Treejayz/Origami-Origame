﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSystem : MonoBehaviour {
	int activepage1 = -1;
	int activepage2 = 0;
	public GameObject[] pages;
	bool[] pageshave;// change this to grab from collectables CollectableManager.storybookpages CollectableManager.spellbookpages
	public Selectable prev;
	public Selectable next;
	public Selectable back;
	public Selectable swapbutton;
	public Selectable bookbutton1;
	public Selectable bookbutton2;
	public GameObject otherBook;
	bool active;
	// Use this for initialization
	void Start () {
		active = false;	

	}
	
	// Update is called once per frame
	void Update () {
		//int startIndex = pages [i].transform.GetSiblingIndex()
		for (int i = -1; i < activepage2; i++){
			pages [i+1].GetComponent<RectTransform> ().anchorMin = new Vector2 (.0f, .5f);
			pages [i+1].GetComponent<RectTransform> ().anchorMax = new Vector2 (.0f, .5f);
			pages [i+1].transform.SetSiblingIndex(pages.Length);
			pages [i+1].GetComponentInChildren<Text> ().enabled = false;
		}
		for (int i = activepage2; i < pages.Length; i++) {
			pages [i].GetComponent<RectTransform> ().anchorMin = new Vector2 (1f, .5f);
			pages [i].GetComponent<RectTransform> ().anchorMax = new Vector2 (1f, .5f);
			pages [i].transform.SetSiblingIndex(0);
			if (this.name == "Book 1") {
				if (i == activepage2 && !CollectableManager.storybookpages [i] && active) {
					pages [i].GetComponentInChildren<Text> ().enabled = true;
				} else {
					pages [i].GetComponentInChildren<Text> ().enabled = false;
				}
			} else {
				if (i == activepage2 && !CollectableManager.spellbookpages [i] && active) {
					pages [i].GetComponentInChildren<Text> ().enabled = true;
				} else {
					pages [i].GetComponentInChildren<Text> ().enabled = false;
				}
			}
		}
		if (active) {
			next.gameObject.GetComponent<Button> ().enabled = true;
			next.gameObject.GetComponent<Image> ().enabled = true;
			prev.gameObject.GetComponent<Button> ().enabled = true;
			prev.gameObject.GetComponent<Image> ().enabled = true;
			Navigation nav = back.GetComponentInChildren<Button> ().navigation;
			nav.selectOnUp = prev;
			nav.selectOnRight = swapbutton;
			nav.selectOnLeft = swapbutton;
			back.GetComponentInChildren<Button> ().navigation = nav;

			nav = swapbutton.GetComponentInChildren<Button> ().navigation;nav.selectOnUp = next;
			swapbutton.GetComponentInChildren<Button> ().navigation = nav;

			swapbutton.gameObject.GetComponent<Button> ().enabled = true;
			swapbutton.gameObject.GetComponent<Image> ().enabled = true;
			if (activepage1 <= -1) {
				prev.gameObject.GetComponent<Button> ().interactable = false;
			} else {
				prev.gameObject.GetComponent<Button> ().interactable = true;
			}
			if (activepage2 >= pages.Length -1) {
				next.gameObject.GetComponent<Button> ().interactable = false;
			} else {
				next.gameObject.GetComponent<Button> ().interactable = true;
			}
		} else if(!otherBook.GetComponent<BookSystem>().isactive()){
			Navigation nav = back.GetComponentInChildren<Button> ().navigation;
			nav.selectOnUp = bookbutton1;
			nav.selectOnRight = bookbutton2;
			nav.selectOnLeft = back;
			back.GetComponentInChildren<Button> ().navigation = nav;
			next.gameObject.GetComponent<Button> ().enabled = false;
			next.gameObject.GetComponent<Image> ().enabled = false;
			prev.gameObject.GetComponent<Button> ().enabled = false;
			prev.gameObject.GetComponent<Image> ().enabled = false;
			swapbutton.gameObject.GetComponent<Button> ().enabled = false;
			swapbutton.gameObject.GetComponent<Image> ().enabled = false;
		}
	}

	public void next_page(){
		if (activepage2 < pages.Length-1) {
			activepage1++;
			activepage2++;
			if (active && activepage2 == pages.Length-1){
				GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (GameObject.Find ("Previous"));
				next.gameObject.GetComponent<Button> ().interactable = false;
			}
		}
	}
	public void prev_page(){
		if (activepage1 > -1) {
			activepage1--;
			activepage2--;
			if (active && activepage1 == -1){
				GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (GameObject.Find ("Next"));
				prev.gameObject.GetComponent<Button> ().interactable = false;
			}
		}
	}
	public void swap(){
		if (active) {
			deactivate ();
		} else {
			activate ();
		}
	}
	public void activate(){
		bookbutton1.gameObject.GetComponent<Button> ().enabled = false;
		bookbutton1.gameObject.GetComponent<Image> ().enabled = false;
		bookbutton2.gameObject.GetComponent<Button> ().enabled = false;
		bookbutton2.gameObject.GetComponent<Image> ().enabled = false;
		for (int i = 0; i < pages.Length; i++){
			if (this.name == "Book 1") {
				if (CollectableManager.storybookpages [i]) {
					pages [i].GetComponent<PageDisplay> ().page_change ();
				}
			} else {
				if (CollectableManager.spellbookpages [i]) {
					pages [i].GetComponent<PageDisplay> ().page_change ();
				}
			}
		}
		active = true;
		activepage1=-1;
		activepage2=0;
		foreach (Image display in this.GetComponentsInChildren<Image>()){
			display.enabled = true; 
		}
	}
	public void deactivate(){
		if (!otherBook.GetComponent<BookSystem> ().isactive ()) {
			bookbutton1.gameObject.GetComponent<Button> ().enabled = true;
			bookbutton1.gameObject.GetComponent<Image> ().enabled = true;
			bookbutton2.gameObject.GetComponent<Button> ().enabled = true;
			bookbutton2.gameObject.GetComponent<Image> ().enabled = true;
		}
		activepage1=-1;
		activepage2=0;
		active = false;
		foreach (Image display in this.GetComponentsInChildren<Image>()){
			display.enabled = false; 
		}
	}
	public bool isactive(){
		return active;
	}
}
