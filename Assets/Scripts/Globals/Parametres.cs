using UnityEngine;
using System.Collections;
using System;

public class Parametres {
	/**
	 * simuSpeed	: Vitesse à laquelle s'écoule le temps
	 * ---
	 * • 1			: Première vitesse		| 2 secondes par jour
	 * • 2			: Deuxième vitesse		| 2 secondes pour trois jours
	 * • 3			: Troisième vitesse	| 2 secondes pour 6 jours (10 secondes par mois) 
	 */
	public static int simuSpeed = 1;

	public static bool isReady = false;

	private static int[] timeSpeed =
							{
								1,			// 100 frames = 1 jour, soit 2 secondes par jour
								3,			// 100 frames = 3 jour, soit 2 secondes pour trois jours
								6,			// 100 frames = 6 jour, soit 2 secondes pour 6 jours (10 secondes par mois) 
							};

	public static int timeOfADay = 300;

	public static void setSimuSpeed(int speed) {
		simuSpeed = speed;
	}

	public static int getTimeSpeed() {
		return timeSpeed[simuSpeed];
	}

	/**
	 * earthCentre : Centre de la terre
	 */
	public static Vector3 earthCenter = new Vector3(0f, -15.77f, -5.79f);

	public static DateTime date = new DateTime(2016, 1, 1);

	public static float hauteurMenuPrincipal = 85f;

	/**
	 * earth : La Terre
	 */
	public static GameObject earth;

	/**
	 * Référence sur le pays sélectionné
	 */
	public static GameObject paysSelected = null;
	/*
	public static var coutSympotoms =
	{
		/**
		 * Coût de l'évolution du symptom Toux
		 *
			public static int coutToux = 1;
			/**
		 * Coût de l'évolution du symptom Diarrhee
		 *
			public static int coutDiarrhee = 1;
			/**
		 * Coût de l'évolution du symptom Eternuements
		 *
			public static int coutEternuements = 1;
			/**
		 * Coût de l'évolution du symptom Sueurs
		 *
			public static int coutSueurs = 1;
			/**
		 * Coût de l'évolution du symptom ArretDesOrganes
		 *
			public static int coutArretDesOrganes = 1;
			/**
		 * Coût de l'évolution du symptom Fievre
		 *
			public static int coutFievre = 1;
	}
		*/

	/**
	 * Coût de l'évolution du symptom Toux
	 */
	public static int coutToux = 1;
	/**
	 * Coût de l'évolution du symptom Diarrhee
	 */
	public static int coutDiarrhee = 1;
	/**
	 * Coût de l'évolution du symptom Eternuements
	 */
	public static int coutEternuements = 1;
	/**
	 * Coût de l'évolution du symptom Sueurs
	 */
	public static int coutSueurs = 1;
	/**
	 * Coût de l'évolution du symptom ArretDesOrganes
	 */
	public static int coutArretDesOrganes = 1;
	/**
	 * Coût de l'évolution du symptom Fievre
	 */
	public static int coutFievre = 1;

	public static void SetPaysSelected(GameObject pays) {
		paysSelected = pays;
	}
}
