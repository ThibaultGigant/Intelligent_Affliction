using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RepartionCategories : MonoBehaviour {

	/**
	 * Pays sélectionné
	 */
	private Pays pays = null;

	/**
	 * Ensemble des catégories
	 */
	private PopulationCategories categories;

	/**
	 * Indique si la fenêtre est visible
	 */
	private bool isVisible = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isVisible)
		{
			if (Parametres.paysSelected == null) {
				isVisible = false;
				gameObject.SetActive(false);
			}
			else if (ClockManager.newDay) {
				if (pays == null || pays.nomPays != Parametres.paysSelected.name) {
					pays = Parametres.paysSelected.GetComponent<Pays> ();
					categories = pays.population.categoriesPop;
				}
				setRepartition ();
			}
			setWantedRepartitions (false);
		}
	}

	/**
	 * Affiche les répartition des catégories
	 */
	private void setRepartition () {
		// foreach (APopulationCategory cate in categories.categories) {
		Slider cylindre;
		Slider aimedCylinder;
		foreach (Transform cate in transform.FindChild("Cylindres")) {
			cylindre = cate.FindChild ("Cylindre").GetComponentInChildren<Slider> ();
			cylindre.value = (float) categories.categories [cate.name].assignedPopulation / 
				(float) pays.population.totalPopulation;
			aimedCylinder = cate.FindChild ("Aimed Cylindre").GetComponentInChildren<Slider> ();
				
			aimedCylinder.value = categories.categories [cate.name].wantedPercentage;
		}
	}

	/**
	 * Change la valeur de isVisible à chaque appel
	 */
	public void toggleVisible() {
		isVisible = !isVisible;
		if (Parametres.paysSelected == null) {
			isVisible = false;
			gameObject.SetActive(false);
		}
		if (isVisible && Parametres.paysSelected != null) {
			gameObject.SetActive(true);
			pays = Parametres.paysSelected.GetComponent<Pays> ();
			categories = pays.population.categoriesPop;
			setRepartition ();
		}
	}

	public void setWantedRepartitions (bool flag) {
		Slider barre;
		Slider aimedCylindre;
		float somme = 0f;
		foreach (Transform cate in transform.FindChild("Cylindres")) {
			barre = cate.FindChild ("Barre").GetComponentInChildren<Slider> ();
			somme += barre.value;
		}
		foreach (Transform cate in transform.FindChild("Cylindres")) {
			barre = cate.FindChild ("Barre").GetComponentInChildren<Slider> ();
			if (flag) {
				categories.categories [cate.name].wantedPercentage = barre.value / somme;
				barre.value /= somme;
			}
			else {
				aimedCylindre = cate.FindChild ("Aimed Cylindre").GetComponentInChildren<Slider> ();
				aimedCylindre.value = barre.value / somme;
			}
		}
	}
}
