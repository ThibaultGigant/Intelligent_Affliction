using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public RawImage menuPrincipal;
	public GameObject earth;
	public Text nombrePopulationText;

	private float largeurMenuPrincipal = Screen.width * 1.0f;

	// Use this for initialization
	void Start () {
		resizeMenuPrincipal ();
		Parametres.earth = earth;
		earth.SetActive (true);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateMenuPricipal ();
	}

	private void updateMenuPricipal () {
		if (largeurMenuPrincipal != Screen.width * 1.0f) {
			largeurMenuPrincipal = Screen.width * 1.0f;
			resizeMenuPrincipal ();
		}

		//unsigned long nb;
		//if (Parametres.paysSelected != null)
			//nb = Parametres.paysSelected.GetComponent<Pays>().getNumberPopulation();
		//else
			//nb = Parametres.earth.getNumberPopulation();
			

		//nombrePopulationText.text = nb;

	}

	private void resizeMenuPrincipal() {
		menuPrincipal.rectTransform.sizeDelta = new Vector2(largeurMenuPrincipal, Parametres.hauteurMenuPrincipal);
	}
}
