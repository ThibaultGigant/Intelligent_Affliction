using UnityEngine;
using System.Collections;

public class Loisirs : APopulationCategory
{
	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assigné à cette catégorie, en nombre d'habitant
	 */
	public Loisirs(int nb) : base(nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {}
}