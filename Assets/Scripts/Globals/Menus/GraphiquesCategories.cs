using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphiquesCategories : MonoBehaviour
{

	/**
	 * Indique si le panel des catégories est affiché ou non
	 */
	private bool isVisible = false;

	/**
	 * Ensemble des catégories
	 */
	private PopulationCategories categories;

	/**
	 * Graphique correspondant à la catégorie
	 */
	private GameObject graphique;

	// Use this for initialization
	void Awake ()
	{
		graphique = GameObject.Find ("/GameManager/Menus/Menu Gauche/More Infos/Graphiques/Catégories/" + gameObject.name);
		graphique.transform.FindChild("Production").GetComponent<RawImage>().texture = new Texture2D(100,100);
		graphique.transform.FindChild("Consommation").GetComponent<RawImage>().texture = new Texture2D(100,100);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isVisible)
		{
			if (Parametres.paysSelected == null) {
				isVisible = false;
			}
			else if (ClockManager.newDay) {
				categories = Parametres.paysSelected.GetComponent<Pays> ().population.categoriesPop;
				setGraphique ();
			}
		}
		
	}

	public void setGraphique() {
		categories.categories[gameObject.name].createGraphiqueProduction ((Texture2D) (graphique.transform.FindChild("Production").GetComponent<RawImage>().texture));
		categories.categories[gameObject.name].createGraphiqueConsommation ((Texture2D) (graphique.transform.FindChild("Consommation").GetComponent<RawImage>().texture));
		//graphique.transform.FindChild ("Consommation/Data").GetComponentInChildren<LineChart> ().UpdateData( categories.categories[gameObject.name].createGraphiqueConsommation () );
	}

	/**
	 * Change la valeur de isVisible à chaque appel
	 */
	public void toggleVisible() {
		isVisible = !isVisible;
		if (isVisible && Parametres.paysSelected != null) {
			transform.Find ("/GameManager/Menus/Menu Gauche/More Infos/Graphiques").gameObject.SetActive(true);
			categories = Parametres.paysSelected.GetComponent<Pays> ().population.categoriesPop;
			setGraphique ();
		}
	}
}

