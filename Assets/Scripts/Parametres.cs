using UnityEngine;
using System.Collections;

public static class Parametres {
	/**
	 * simuSpeed : Vitesse à laquelle s'écoule le temps
	 */
	public static float simuSpeed = 1f;

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
}
