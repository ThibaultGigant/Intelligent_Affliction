using UnityEngine;
using System.Collections;

public abstract class APopulationCategory
{
	/**
	 * Taille de la population assignée à cette catégorie, en nombre d'habitants
	 */
	public int assignedPopulation;


	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public APopulationCategory(int nb) {
		assignedPopulation = nb;
	}

	/**
	 * Ajoute des personnes à cette catégorie
	 * @param nb Nombre de personnes à ajouter à cette cotégorie. Doit être positif, sinon rien n'est fait
	 * ---
	 * TODO :
	 * Ajouter une valeur de retour qui donnerait le nombre effectif de personne ajoutées, en cas de rébellion sociale ?
	 */
	public void addAssigned(int nb) {
		if (nb < 0)
			return;
		assignedPopulation += nb;
	}

	/**
	 * Enlève des personnes de cette catégorie
	 * @param nb Nombre de personnes à retirer de cette catégorie. Doit être positif, sinon rien n'est fait
	 * ---
	 * TODO :
	 * Ajouter une valeur de retour qui donnerai le nombre effectif de personne retirées, en cas de rébellion sociale ?
	 */
	public void removeAssigned(int nb) {
		if (nb < 0)
			return;
		if (nb > assignedPopulation)
			assignedPopulation = 0;
		else
			assignedPopulation -= nb;
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public abstract void produce ();
}

