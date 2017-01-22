using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Exclamation : MonoBehaviour
{

	/**
	 * Liste des smileys disponibles
	 */
	private GameObject[] exclamations;

	// Use this for initialization
	void Awake ()
	{
		exclamations = new GameObject[transform.childCount];
		int i;
		foreach (Transform exclamation in transform) {
			i =  int.Parse( exclamation.name.Split(new char[] {' '})[0]);
			exclamations[i] = exclamation.gameObject;
		}
	}

	/**
	 * Affiche le bon smiley en fonction de la valeur fournie
	 * Change la coloration par la même occasion
	 * @param valeur Valeur à fournir, entre 0 (mauvais) et 100 (bon)
	 */
	public void setExclamation(float valeur) {
		int exclamationIndice = (int)((valeur + 1f) * exclamations.Length / 2f);

		for  ( int i = 0 ; i < exclamations.Length ; i++ ) {
			exclamations [i].SetActive (false);
		}
		if (Mathf.Abs(exclamationIndice - exclamations.Length / 2 ) > 1) {
			Color color = Color.red;
			color.g = color.b = 1f - Mathf.Abs(valeur);
			exclamations [exclamationIndice].GetComponent<RawImage> ().color = color;
		}
		exclamations [exclamationIndice].SetActive (true);
	}
}

