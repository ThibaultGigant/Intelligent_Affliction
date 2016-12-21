using UnityEngine;
using System.Collections;

public class Medecine : APopulationCategory
{
	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Medecine(Population population, uint nb) : base(population, nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {}
}