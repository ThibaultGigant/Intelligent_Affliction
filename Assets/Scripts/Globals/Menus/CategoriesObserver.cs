using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CategoriesObserver : MonoBehaviour {

	/**
	 * Indique si le panel des catégories est affiché ou non
	 */
	private bool isVisible = true;

	/**
	 * Ensemble des catégories
	 */
	private PopulationCategories categories;

	/**
	 * Dictionnaire des menus de correspondant à chaque catégorie
	 */
	private IDictionary<string, Transform> menuCate;

	/**
	 * Dictionnaire des images du menu de correspondant à chaque catégorie
	 */
	private IDictionary<string, Image> imageCate;

	/**
	 * Dictionnaire des champs Text du menu de correspondant à la valeur de chaque catégorie
	 */
	private IDictionary<string, Text> valueCate;

	/**
	 * Liste des noms des catégories
	 */
	private string[] nomCate = { "Agriculture", "Loisirs", "Medecine", "Recherche", "Transports", "Inactifs"};

	// Use this for initialization
	void Start () {
		menuCate = new Dictionary<string, Transform> ();
		imageCate = new Dictionary<string, Image> ();
		valueCate = new Dictionary<string, Text> ();
		foreach (string cate in nomCate)
		{
			menuCate [cate] = transform.Find (cate);
			imageCate [cate] = menuCate [cate].GetComponent<Image> ();
			valueCate [cate] = menuCate [cate].Find ("Valeur").GetComponent<Text>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isVisible)
		{
			if (Parametres.paysSelected == null) {
				//isVisible = false;
			}
			else if (ClockManager.newDay) {
				categories = Parametres.paysSelected.GetComponent<Pays> ().population.categoriesPop;
				setValues ();
			}
		}
	}

	/**
	 * Met à jour les couleurs des menus de chaque catégorie
	 */
	private void setValues() {
		foreach (string cate in nomCate)
		{
			float besoin = categories.categories [cate].besoins ();
			valueCate [cate].text = besoin.ToString("R");
			byte besoinColor = (byte) ((besoin + 1) * 127);
			byte red;
			if (besoinColor <= 127)
				red = besoinColor;
			else
				red = (byte) ((255 - besoinColor) * 133 / 255);
			byte green = (byte) (255 - besoinColor);
			byte blue = (byte)(besoinColor * 133 / 255);
			imageCate [cate].color = new Color32 (red, green, blue, 255);

		}
	}

	/**
	 * Change la valeur de isVisible à chaque appel
	 */
	public void toggleVisible() {
		isVisible = !isVisible;
		if (isVisible) {
			setValues ();
			categories = Parametres.paysSelected.GetComponent<Pays> ().population.categoriesPop;
		}
	}
}
