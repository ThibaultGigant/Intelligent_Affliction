using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Smiley : MonoBehaviour
{

	/**
	 * Liste des smileys disponibles
	 */
	private GameObject[] smileys;
	//private List<GameObject> smileys;

	// Use this for initialization
	void Start ()
	{
		smileys = new GameObject[transform.childCount];
		int i;
		foreach (Transform smiley in transform) {
			i =  int.Parse( smiley.name.Split(new char[] {' '})[0]);
			smileys[i] = smiley.gameObject;
		}
	}

	/**
	 * Affiche le bon smiley en fonction de la valeur fournie
	 * Change la coloration par la même occasion
	 * @param valeur Valeur à fournir, entre 0 (mauvais) et 100 (bon)
	 */
	public void setSmiley(float valeur) {
		Debug.Log ("Smiley " + valeur);
		int smileyIndice = (int)(valeur * smileys.Length / 100f);

		smileyIndice = Mathf.Min (smileyIndice, smileys.Length - 1);

		for  ( int i = 0 ; i < smileys.Length ; i++ ) {
			smileys [i].SetActive (false);
		}
		if (smileyIndice < smileys.Length / 2) {
			Color color = Color.red;
			color.g = color.b = (105f + (valeur * 3f)) / 255f;
			smileys [smileyIndice].GetComponent<RawImage> ().color = color;
		}
		smileys [smileyIndice].SetActive (true);
	}
}

