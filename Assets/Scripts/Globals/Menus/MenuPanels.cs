using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuPanels : MonoBehaviour
{

	/**
	 * Boutton du Happiness
	 */
	public GameObject Happiness;

	/**
	 * Panel des catégories
	 */
	public GameObject CategoriesPanel;

	/**
	 * Panel des graphiques
	 */
	public GameObject graphiquesPanel;

	/**
	 * Sprite plus
	 */
	public Sprite plusButton;

	/**
	 * Sprite moins
	 */
	public Sprite moinsButton;

	/**
	 * Smiley correspondant au Happiness Index
	 */
	private Smiley smileyHappiness;

	private Dictionary<string, GameObject> categories;

	void Start () {
		smileyHappiness = Happiness.transform.FindChild ("Smiley").GetComponentInChildren<Smiley> ();
		categories = new Dictionary<string, GameObject> ();
		foreach ( Transform t in transform.FindChild("More Infos/Catégories")) {
			categories.Add ( t.name, t.gameObject );
		}
	}

	void Update () {
		if (Parametres.paysSelected != null)
			smileyHappiness.setSmiley (Parametres.paysSelected.GetComponentInChildren<Pays> ().population.getHappinessIndex ());
		else {
			CategoriesPanel.SetActive (false);
			Image image = Happiness.transform.FindChild("More").GetComponent<Image> ();
			image.sprite = plusButton;
		}
			
	}

	/**
	 * Clique sur le boutton Happinnes
	 */
	public void HappinessOnClick() {
		Image image = Happiness.transform.FindChild("More").GetComponent<Image> ();

		if (Parametres.paysSelected == null) {
			if (CategoriesPanel.activeSelf)
				CategoriesPanel.GetComponent<CategoriesObserver> ().toggleVisible();

			CategoriesPanel.SetActive (false);
			image.sprite = plusButton;
			return;
		}

		if (image.sprite == plusButton) {
			CategoriesPanel.SetActive (true);
			image.sprite = moinsButton;
		}
		else {
			CategoriesPanel.SetActive (false);
			image.sprite = plusButton;
		}

		CategoriesPanel.GetComponent<CategoriesObserver> ().toggleVisible();

	}

	public void GraphiqueAgrOnClick () {
		if (graphiquesPanel.activeSelf)
			graphiquesPanel.SetActive (false);
		else
			graphiquesPanel.SetActive (true);
	}

	/**
	 * Clique sur le boutton Happinnes
	 */
	public void GraphiqueOnClick(string cate) {
		Image image = categories[cate].transform.FindChild("More").GetComponent<Image> ();

		if (Parametres.paysSelected == null) {
			if (CategoriesPanel.activeSelf)
				CategoriesPanel.GetComponent<CategoriesObserver> ().toggleVisible();

			CategoriesPanel.SetActive (false);
			image.sprite = plusButton;
			return;
		}

		if (image.sprite == plusButton) {
			//CategoriesPanel.SetActive (true);
			image.sprite = moinsButton;
		}
		else {
			//CategoriesPanel.SetActive (false);
			image.sprite = plusButton;
		}

		graphiquesPanel.transform.FindChild("Catégories/" + cate).GetComponent<GraphiquesCategories> ().toggleVisible();

	}


}

