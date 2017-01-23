using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphiquesCategories : MonoBehaviour
{
	public GameObject[] morePanel;

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
	public GameObject graphique;

	// Use this for initialization
	void Awake ()
	{
		Debug.Log ("Graph Cate Awake");
		transform.FindChild("Production").GetComponent<RawImage>().texture = new Texture2D(100,100);
		transform.FindChild("Consommation").GetComponent<RawImage>().texture = new Texture2D(100,100);
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
				categories = Parametres.paysSelected.GetComponent<Pays> ().population.categories;
				setGraphique ();
			}
		}
		
	}

	public void setGraphique() {
		if (transform.FindChild("Production") != null)
			categories[gameObject.name].createGraphiqueProduction ( (transform.FindChild("Production").gameObject));
		if (transform.FindChild("Consommation") != null)
			categories[gameObject.name].createGraphiqueConsommation ( (transform.FindChild("Consommation").gameObject));
		//graphique.transform.FindChild ("Consommation/Data").GetComponentInChildren<LineChart> ().UpdateData( categories.categories[gameObject.name].createGraphiqueConsommation () );
	}

	/**
	 * Change la valeur de isVisible à chaque appel
	 */
	public void toggleVisible() {
		isVisible = !isVisible;
		if (isVisible && Parametres.paysSelected != null) {
			gameObject.SetActive (true);
			graphique.SetActive(true);
			categories = Parametres.paysSelected.GetComponent<Pays> ().population.categories;
			setGraphique ();
			foreach( GameObject g in morePanel) {
				if (g.activeSelf)
					g.SetActive (false);
			}
			foreach (Transform t in graphique.transform.FindChild("Catégories")) {
				if (t != transform)
					t.gameObject.SetActive (false);
			}
		}
		else {
			GameObject graph = transform.Find ("/GameManager/Menus/Menu Gauche/More Infos/Graphiques").gameObject;
			foreach (Transform t in graph.transform.FindChild("Catégories")) {
				t.gameObject.SetActive (false);
			}
			graph.SetActive(false);
			gameObject.SetActive (false);
		}
	}
}

