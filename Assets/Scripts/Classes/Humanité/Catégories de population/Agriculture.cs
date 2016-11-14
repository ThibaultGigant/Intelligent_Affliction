using UnityEngine;
using System.Collections;

public class Agriculture : APopulationCategory
{
	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Agriculture(int nb) : base(nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {}
}