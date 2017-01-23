using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject prefabCargo;

	public RawImage menuPrincipal;
	public GameObject earth;
	public GameObject miniEarth;
	public Text nombrePopulationText;
	private PaysManager paysManager;
	private Pays lastPaysSelected = null;

	private float largeurMenuPrincipal = Screen.width * 1.0f;

	// Use this for initialization
	void Start () {
		resizeMenuPrincipal ();
		Parametres.earth = earth;
		Parametres.miniEarth = miniEarth;
		Parametres.prefabCargo = prefabCargo;
		earth.SetActive (true);
		paysManager = Parametres.earth.GetComponentInChildren<PaysManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		updateMenuPricipal ();
		if (lastPaysSelected != Parametres.paysSelected)
			Parametres.switchPays = true;
		else
			Parametres.switchPays = false;
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
			

		nombrePopulationText.text = Utils.toStringFormat((int)nb);//nb.ToString();

	}

	private void resizeMenuPrincipal() {
		menuPrincipal.rectTransform.sizeDelta = new Vector2(largeurMenuPrincipal, Parametres.hauteurMenuPrincipal);
	}
}
