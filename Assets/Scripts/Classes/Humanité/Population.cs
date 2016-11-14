using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Population {
	/**
	 * Pays associé
	 */
	public Pays pays;
	/**
	 * "Contentement" de la population
	 * ---
	 * TODO :
	 * Quelle est la valeur par défaut ?
	 */
	private int happinessIndex;
	/**
	 * Population totale du pays
	 */
	public int totalPopulation;
	/**
	 * Catégorie de population : Agriculture
	 */
	private APopulationCategory agriculturePopulationCategory;
	/**
	 * Catégorie de population : Inactifs
	 */
	private APopulationCategory inactifsPopulationCategory;
	/**
	 * Catégorie de population : Loisirs
	 */
	private APopulationCategory loisirsPopulationCategory;
	/**
	 * Catégorie de population : Medecine
	 */
	private APopulationCategory medecinePopulationCategory;
	/**
	 * Catégorie de population : Recherche
	 */
	private APopulationCategory recherchePopulationCategory;
	/**
	 * Catégorie de population : Transports
	 */
	private APopulationCategory transportsPopulationCategory;

	/**
	 * Constructeur
	 * @param p Pays associé
	 * @param nb Taille de la population, en nombre d'habitant
	 */
	public Population(Pays p, int nb) {
		pays = p;
		totalPopulation = nb;

		SetupCategories ();
	}

	/**
	 * Répartit la population entre les différentes catégories, lors du premier lancement du jeu
	 */
	private void SetupCategories () {
		if (totalPopulation < 0) {
			Debug.LogError (pays.nomPays + ", Repartition : Le nombre de personnes doit être positif");
			totalPopulation = 0;
		}

		// Repartition uniforme, parmis les 6 catégories. (TODO : A revoir...)
		int nbReparti = totalPopulation / 6;

		agriculturePopulationCategory = new Agriculture (nbReparti);
		inactifsPopulationCategory =new Inactifs (nbReparti);
		loisirsPopulationCategory = new Loisirs (nbReparti);
		medecinePopulationCategory = new Medecine (nbReparti);
		recherchePopulationCategory = new Recherche (nbReparti);
		transportsPopulationCategory = new Transports (totalPopulation - 5 * nbReparti); // Pour eviter les imprécisions dues à la discrétisation des pourcentages
	}

	/**
	 * Définit la valeur du happinessIndex
	 * @param happy Valeur de happiness que l'on veut affecter
	 */
	public void setHappinessIndex(int happy) {
		happinessIndex = happy;
	}

	/**
	 * Ajoute des habitants à la population
	 * @param nb Nombre d'habitants que l'on veut ajouter
	 * ---
	 * TODO : Comment définir la catégorie d'affection ?
	 */
	public void addPeople(int nb) {
		totalPopulation += nb;
		// Pour l'instant, les nouveaux seront des inactifs
		inactifsPopulationCategory.addAssigned (nb);
	}

	/**
	 * Retire des habitants de la population
	 * @param nb Nombre d'habitants que l'on veut retirer
	 * ---
	 * TODO : De quelle catégorie les enlever ?
	 */
	public void removePeople(int nb) {
		totalPopulation -= nb;
		// Pour l'instant, ils seront retirés selon un ordre arbitraire
		// Inactifs
		if (inactifsPopulationCategory.assignedPopulation >= nb) {
			inactifsPopulationCategory.removeAssigned (nb);
			return;
		}
		inactifsPopulationCategory.removeAssigned (inactifsPopulationCategory.assignedPopulation);
		nb -= inactifsPopulationCategory.assignedPopulation;

		// Loisirs
		if (loisirsPopulationCategory.assignedPopulation >= nb) {
			loisirsPopulationCategory.removeAssigned (nb);
			return;
		}
		loisirsPopulationCategory.removeAssigned (loisirsPopulationCategory.assignedPopulation);
		nb -= loisirsPopulationCategory.assignedPopulation;

		// Transports
		if (transportsPopulationCategory.assignedPopulation >= nb) {
			transportsPopulationCategory.removeAssigned (nb);
			return;
		}
		transportsPopulationCategory.removeAssigned (transportsPopulationCategory.assignedPopulation);
		nb -= transportsPopulationCategory.assignedPopulation;

		// Recherche
		if (recherchePopulationCategory.assignedPopulation >= nb) {
			recherchePopulationCategory.removeAssigned (nb);
			return;
		}
		recherchePopulationCategory.removeAssigned (recherchePopulationCategory.assignedPopulation);
		nb -= recherchePopulationCategory.assignedPopulation;

		// Medecine
		if (medecinePopulationCategory.assignedPopulation >= nb) {
			medecinePopulationCategory.removeAssigned (nb);
			return;
		}
		medecinePopulationCategory.removeAssigned (medecinePopulationCategory.assignedPopulation);
		nb -= medecinePopulationCategory.assignedPopulation;

		// Agriculture
		if (agriculturePopulationCategory.assignedPopulation >= nb) {
			agriculturePopulationCategory.removeAssigned (nb);
			return;
		}
		agriculturePopulationCategory.removeAssigned (agriculturePopulationCategory.assignedPopulation);
	}

	/**
	 * A l'appel de cette fonction, la population n'écoute plus le joueur et décide de se rebeller
	 */
	public void goRogue() {
	}

	/**
	 * Nombre de naissances "fixe" en fonction de la taille de la population. 1.2% de leur nombre par année
	 * Appel de la fonction tous les mois (TODO : ?) (d'où la division par douze)
	 */
	public int naissances() {
		int nbNaissances = Mathf.Max(0, Mathf.FloorToInt(0.0012f * totalPopulation / 12f)); // TODO : Max ? On n'est jamais < 0, si ?
		addPeople (nbNaissances);
		return nbNaissances;
	}

	/**
	 * Nombre de décès "fixe" en fonction de la taille de la population. 0.9% de leur nombre par année
	 * Appel de la fonction tous les mois (TODO : ?) (d'où la division par douze)
	 */
	public int deces() {
		int nbDeces = Mathf.Max(0, Mathf.FloorToInt(0.009f * totalPopulation / 12f));
		removePeople (nbDeces);
		return nbDeces;
	}
}
