using UnityEngine;
using System.Collections;

public abstract class APopulationCategory
{
	/**
	 * Taille de la population assignée à cette catégorie, en nombre d'habitants
	 */
	public uint assignedPopulation;


	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public APopulationCategory(uint nb) {
		assignedPopulation = nb;
	}

	/**
	 * Ajoute des personnes à cette catégorie
	 * @param nb Nombre de personnes à ajouter à cette cotégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement ajoutées à la catégorie
	 */
	public uint addAssigned(uint nb) {
		if (nb < 0)
			return 0;
		assignedPopulation += nb;
		return nb;
	}

	/**
	 * Enlève des personnes de cette catégorie
	 * @param nb Nombre de personnes à retirer de cette catégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement retirées de la catégorie
	 */
	public uint removeAssigned(uint nb) {
		if (nb < 0)
			return 0;
		if (nb > assignedPopulation) {
			uint temp = assignedPopulation;
			assignedPopulation = 0;
			return temp;
		}
		assignedPopulation -= nb;
		return nb;
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public abstract void produce ();
}

