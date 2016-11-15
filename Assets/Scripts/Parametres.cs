using UnityEngine;
using System.Collections;

public static class Parametres {
	/**
	 * simuSpeed	: Vitesse à laquelle s'écoule le temps
	 * ---
	 * • 1			: Première vitesse		| 2 secondes par jour
	 * • 2			: Deuxième vitesse		| 2 secondes pour trois jours
	 * • 3			: Troisième vitesse	| 2 secondes pour 6 jours (10 secondes par mois) 
	 */
	public static int simuSpeed = 1;

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

	private static float theta1 = 23f;
	private static float theta2 = 180f;

	/**
	 * earthAxis : Axe de rotation de la terre
	 */
	public static Quaternion earthAxis = Quaternion.Euler(new Vector3(Mathf.Sin(theta1 * 2 * Mathf.PI / 360), Mathf.Cos(theta1 * 2 * Mathf.PI / 360), Mathf.Cos((90 - theta1) * 2 * Mathf.PI / 360)));

	public static Date date = new Date(1, 1, 2016);

	public static float hauteurMenuPrincipal = 85f;

	/**
	 * earth : La Terre
	 */
	public static GameObject earth;

	/**
	 * Référence sur le pays sélectionné
	 */
	public static GameObject paysSelected = null;

	public static void SetPaysSelected(GameObject pays) {
		paysSelected = pays;
	}
}
