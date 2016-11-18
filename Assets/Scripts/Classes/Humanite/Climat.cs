using System;
using UnityEngine;

public class Climat
{
	/**
	 * "Indice de chaleur" du pays
	 * Sera un entier entre 0 et 100, 0 représentant le pays le plus froid et 100 le pays le plus chaud
	 */
	public int chaleur { get; set;}
	/**
	 * Humidité du pays
	 * Sera un entier entre 0 et 100, 0 représentant le pays le plus sec et 100 le pays le plus humide
	 */
	public int humidite { get; set;}

	/**
	 * Constructeur
	 * @param c La chaleur du pays
	 * @param h L'humidité du pays
	 */
	public Climat (int c, int h)
	{
		// Vérification des bornes
		chaleur = Mathf.Clamp(c, 0, 100);
		humidite = Mathf.Clamp(h, 0, 100);
	}
}

