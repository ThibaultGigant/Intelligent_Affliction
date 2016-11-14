﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockManager : MonoBehaviour
{
	/**
	 * Text devant contenir la date à afficher
	 */
	public Text textDate;

	/**
	 * Booléen informant si durant la frame courrante, une nouvelle journée démarre
	 * true si c'est le cas, false sinon
	 */
	public static bool newDay = true;

	/**
	 * Horloge. Compte le temps écoulé durant la journée
	 * Dès quelle dépasse Parametres.timeOfADay,
	 * une nouvelle journée démarre et on soustrait cette valeur à countTime
	 */
	private int countTime = 0;

	// Use this for initialization
	void Start ()
	{
		displayDate ();
	}
	
	/**
	 * Fonction appelée à chaque frame.
	 * ---
	 * Se charge de compter le temps, et de mettre à jour la date.
	 */
	void Update ()
	{
		// newDay doit rester à true durant seulement une frame pour chaque journée
		if (newDay)
			newDay = false;
		
		countTime += Parametres.getTimeSpeed ();
		if (countTime >= Parametres.timeOfADay) {
			Parametres.date.Tomorow ();
			displayDate ();
			countTime -= Parametres.timeOfADay;
			newDay = true;
		}
	}

	/**
	 * Mise à jour de l'affichage de la date
	 */
	private void displayDate() {
		textDate.text = Parametres.date.getJour() + "/" + Parametres.date.getMois() + "/" + Parametres.date.getAnnee();
	}
}

