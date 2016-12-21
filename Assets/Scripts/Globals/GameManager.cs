﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public RawImage menuPrincipal;
	public GameObject earth;
	public Text nombrePopulationText;
	private PaysManager paysManager;

	private float largeurMenuPrincipal = Screen.width * 1.0f;

	// Use this for initialization
	void Start () {
		resizeMenuPrincipal ();
		Parametres.earth = earth;
		earth.SetActive (true);
		paysManager = Parametres.earth.GetComponentInChildren<PaysManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		updateMenuPricipal ();
	}

	private void updateMenuPricipal () {
		if (largeurMenuPrincipal != Screen.width * 1.0f) {
			largeurMenuPrincipal = Screen.width * 1.0f;
			resizeMenuPrincipal ();
		}

		uint nb = 0;
		if (Parametres.paysSelected != null)
			nb = Parametres.paysSelected.GetComponent<Pays>().getNbPopulation();
		else
			nb = paysManager.getTotalPopulation();
			

		nombrePopulationText.text = nb.ToString();

	}

	private void resizeMenuPrincipal() {
		menuPrincipal.rectTransform.sizeDelta = new Vector2(largeurMenuPrincipal, Parametres.hauteurMenuPrincipal);
	}
}