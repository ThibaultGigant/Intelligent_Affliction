using UnityEngine;
using System.Collections;

public abstract class APopulationCategory
{
	/**
	 * Taille de la population assigné à cette catégorie, en nombre d'habitant
	 */
	public int assignedPopulation;


	/**
	 * Constructeur
	 * @param nb Taille de la population initialement assigné à cette catégorie, en nombre d'habitant
	 */
	public APopulationCategory(int nb) {
		assignedPopulation = nb;
	}

	/**
	 * Ajoute des personnes à cette catégorie
	 * @param nb Nombre de personnes à ajouter à cette cotégorie. Doit être positif, sinon rien n'est fait
	 * ---
	 * Ajouter une valeur de retour qui donnerai le nombre effectif de personne ajoutées, en cas de rébellion sociale ?
	 */
	public void addAssigned(int nb) {
		assignedPopulation += nb;
	}

	/**
	 * Enlève des personnes de cette catégorie
	 * @param nb Nombre de personnes à retirer de cette cotégorie. Doit être positif, sinon rien n'est fait
	 * ---
	 * Ajouter une valeur de retour qui donnerai le nombre effectif de personne retirées, en cas de rébellion sociale ?
	 */
	public void removeAssigned(int nb) {
		assignedPopulation -= nb;
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public abstract void produce ();
}

