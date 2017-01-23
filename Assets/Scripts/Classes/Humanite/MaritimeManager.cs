using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MaritimeManager : MonoBehaviour
{

	/**
	 * Liste des ports
	 */
	public GameObject[] ports;

	/**
	 * Liste des liens
	 */
	public GameObject[] liens;

	/**
	 * Bouton "Mariitme"
	 */
	public GameObject maritime;

	/**
	 * Icons liés au mode maritime
	 */
	public GameObject[] icons;

	/**
	 * Liste des liens actifs
	 */
	private bool[] liensActif;
	 /**
	  * Liste des ports sélectionnés
	  */
	private GameObject[] portsSelected;

	// Use this for initialization
	void Start ()
	{
		liensActif = new bool[ liens.Length ];
		portsSelected = new GameObject[2];
		portsSelected [0] = null;
		portsSelected [1] = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		clickOnPort ();
	}

	public void maritimeOnClick() {
		Parametres.chooseMaritime = !Parametres.chooseMaritime;
		if (Parametres.chooseMaritime) {
			for ( int i = 0 ; i < ports.Length ; i++ ) {
				ports [i].SetActive (true);
			}
			for ( int i = 0 ; i < liens.Length ; i++ ) {
				if (liensActif[i])
					liens [i].SetActive (true);
			}
			maritime.GetComponentInChildren<Image> ().color = new Color(210f /255f, 184f / 255f, 99f / 255f, 186f /255f);
			for ( int i = 0 ; i < icons.Length ; i++ ) {
				icons [i].SetActive (true);
			}
		}
		else {
			for ( int i = 0 ; i < ports.Length ; i++ ) {
				ports [i].SetActive (false);
			}
			for ( int i = 0 ; i < liens.Length ; i++ ) {
				if (liensActif[i])
					liens [i].SetActive (false);
			}
			maritime.GetComponentInChildren<Image> ().color = new Color(84f / 225f, 84f / 255f , 84f / 255f , 186f / 255f);
			for ( int i = 0 ; i < icons.Length ; i++ ) {
				icons [i].SetActive (false);
			}
		}
	}

	public void clickOnPort() {
		foreach ( GameObject port in ports ) {
			if (MouseManager.doubleLeftClick && MouseManager.doesHitMaritime (port)) {
				if (portsSelected [0] == null && portsSelected [1] != port) {
					portsSelected [0] = port;
					foreach (Transform t in port.transform) {
						t.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", new Color(1f, 153f / 255f , 0f , 1f)) ;
						foreach (Transform t2 in t) {
							t2.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", new Color(1f, 153f / 255f , 0f , 1f)) ;
						}
					}
				}
				else if (portsSelected [1] == null && portsSelected [0] != port) {
					portsSelected [1] = port;
					foreach (Transform t in port.transform) {
						t.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", new Color(1f, 153f / 255f , 0f , 1f)) ;
						foreach (Transform t2 in t) {
							t2.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", new Color(1f, 153f / 255f , 0f , 1f)) ;
						}
					}
				}
				else if (portsSelected [0] == port) {
					portsSelected [0] = null;
					foreach (Transform t in port.transform) {
						t.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", Color.grey) ;
						foreach (Transform t2 in t) {
							t2.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", Color.grey) ;
						}
					}
				}
				else if (portsSelected [1] == port) {
					portsSelected [1] = null;
					foreach (Transform t in port.transform) {
						t.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", Color.grey) ;
						foreach (Transform t2 in t) {
							t2.gameObject.GetComponentInChildren<Renderer> ().material.SetColor("_Color", Color.grey) ;
						}
					}
				}
			}
		}
	}

	public void addLien() {
		int indice = getLien ();
		liensActif [indice] = true;
		liens [indice].SetActive (true);
	}

	public void removeLien() {
		int indice = getLien ();
		liensActif [indice] = false;
		liens [indice].SetActive (false);
	}

	/**
	 * Renvoie l'indice du lien qui est sélectionner, s'il y en a un
	 */
	private int getLien() {
		if (portsSelected [0] != null && portsSelected [1] != null) {
			string name1 = portsSelected [0].name + " " + portsSelected [1].name;
			string name2 = portsSelected [1].name + " " + portsSelected [0].name;
			for ( int i = 0 ; i < liens.Length ; i++ ) {
				if (liens [i].name == name1 || liens [i].name == name2) {
					return i;
				}
			}
		}
		return -1;
	}


}

