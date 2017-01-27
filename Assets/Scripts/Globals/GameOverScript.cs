using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {
	public GameObject worldMap;
	public Canvas gameOverCanvas;
	public Text gameOverText;

	Animator anim;


	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
	}

	void Update () {
		if (isWon ()) {
			gameOverCanvas.enabled = true;
			gameOverText.text = "Vous avez gagné !";
			anim.SetTrigger ("GameOver");
		}
		if (isLost ()) {
			gameOverCanvas.enabled = true;
			gameOverText.text = "Vous avez gagné !";
			anim.SetTrigger ("GameOver");
		}
	}

	/**
	 * Vérifie si le jeu est gagné
	 */
	private bool isWon()
	{
		bool res = true;
		foreach (Pays p in worldMap.GetComponentsInChildren<Pays>()) {
			if (p.souche != null && p.souche.getNbInfected () > 0) {
				res = false;
				break;
			}
		}
		return res;
	}

	/**
	 * Vérifie si le jeu est perdu
	 */
	private bool isLost()
	{
		bool res = true;
		foreach (Pays p in worldMap.GetComponentsInChildren<Pays>()) {
			if (p.getNbPopulation() > 0) {
				res = false;
				break;
			}
		}
		return res;
	}
}
