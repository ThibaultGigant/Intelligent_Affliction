using System;
using UnityEngine;

public class Climat
{
	/**
	 * Chauleur du pays (un autre terme ?)
	 */
	public int chaleur;
	/**
	 * Humidité du pays
	 */
	public int humidite;

	/**
	 * Constructeur
	 * @param c La chaleur du pays (donner les bornes)
	 * @param h L'humidité du pays (donner les bornes)
	 */
	public Climat (int c, int h)
	{
		// Vérifier les bornes
		chaleur = c;
		humidite = h;
	}
}

