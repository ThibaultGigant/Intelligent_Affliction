using UnityEngine;
using System.Collections;

public class Inactifs : APopulationCategory
{
	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assigné à cette catégorie, en nombre d'habitant
	 */
	public Inactifs(int nb) : base(nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {}
}