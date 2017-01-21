using UnityEngine;
using System.Collections;

public class PaysManager : MonoBehaviour {

	// Use this for initialization
	/*
	 * Ajoute un composant Pays à chaque Zone de la carte
	 */
	void Start () {
		foreach ( Transform t in transform )
			t.gameObject.AddComponent <Pays>();
	}

	/**
	 * Retourne la population mondiale actuelle
	 * @return la population mondiale actuelle
	 */
	public uint getTotalPopulation()
	{
		uint total = 0;
		foreach (Transform t in transform) {
			Pays temp = t.GetComponent<Pays> ();
			if (temp == null)
				return 0;
			total += temp.getNbPopulation ();
		}

		return total;
	}

	/**
	 * Retourne la population mondiale initiale
	 * @return la population mondiale actuelle
	 */
	public uint getInitialPopulation()
	{
		uint total = 0;
		foreach (Transform t in transform) {
			total += t.GetComponent<Pays> ().getInitialNbPopulation ();
		}

		return total;
	}
}
