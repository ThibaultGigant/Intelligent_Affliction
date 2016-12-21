using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Population {
	/**
	 * Pays associé
	 */
	public Pays country;
	/**
	 * "Contentement" de la population
	 * TODO : Quelle est la valeur par défaut ? Comment on l'utilise ?
	 */
	private int happinessIndex;
	/**
	 * Taille initiale de la population, en nombre d'habitants
	 */
	public uint initialNumberPopulation;
	/**
	 * Population totale du pays
	 */
	public uint totalPopulation;
	/**
	 * Nombre de personnes détectées comme infectées (peut être différent du nombre réel d'infectés)
	 */
	public uint nbInfectedDetected;
	/**
	 * Ensemble des Catégories de population, accessibles par leur nom
	 */
	public IDictionary<string, APopulationCategory> categories;

	/**
	 * Constructeur
	 * @param p Pays associé
	 * @param nb Taille de la population, en nombre d'habitant
	 */
	public Population(Pays p) {
		country = p;
		uint nb = (uint) Random.Range (100000, 20000000);
		initialNumberPopulation = nb;
		totalPopulation = nb;

		SetupCategories ();
	}

	/**
	 * Répartit la population entre les différentes catégories, lors du premier lancement du jeu
	 */
	private void SetupCategories () {
		if (totalPopulation < 0) {
			Debug.LogError (country.nomPays + ", Repartition : Le nombre de personnes doit être positif");
			totalPopulation = 0;
		}

		// Repartition uniforme, parmis les 6 catégories. (TODO : A revoir...)
		uint nbReparti = (uint) totalPopulation / 6;

		categories = new Dictionary<string, APopulationCategory> ();
		categories.Add ("Agriculture", new Agriculture (this, nbReparti));
		categories.Add ("Inactifs", new Inactifs (this, nbReparti));
		categories.Add ("Loisirs", new Loisirs (this, nbReparti));
		categories.Add ("Medecine", new Medecine (this, nbReparti));
		categories.Add ("Recherche", new Recherche (this, nbReparti));
		categories.Add ("Transports", new Transports (this, totalPopulation - 5 * nbReparti)); // Pour eviter les imprécisions dues à la discrétisation des pourcentages
	}

	/**
	 * Définit la valeur du happinessIndex
	 * @param happy Valeur de happiness que l'on veut affecter
	 */
	public void setHappinessIndex(int happy) {
		happinessIndex = happy;
	}

	/**
	 * Revoi la valeur du happinessIndex
	 * @return La valeur de happiness de la population du pays
	 */
	public int getHappinessIndex() {
		return happinessIndex;
	}

	/**
	 * Ajoute des habitants à la population
	 * TODO : Comment définir la catégorie d'affection ?
	 * @param nb Nombre d'habitants que l'on veut ajouter
	 */
	public void addPeople(uint nb) {
		totalPopulation += nb;
		// Pour l'instant, les nouveaux seront des inactifs
		categories["Inactifs"].addAssigned (nb);
	}

	/**
	 * Retire des habitants de la population
	 * TODO : De quelle catégorie les enlever ?
	 * @param nb Nombre d'habitants que l'on veut retirer
	 */
	public void removePeople(uint nb) {
		uint temp = nb;

		foreach (string cat in categories.Keys) {
			if (temp == 0)
				return;
			temp -= categories [cat].removeAssigned (temp);
		}

		totalPopulation -= nb - temp;
	}

	/**
	 * Transfert de personnes entre deux catégories de population
	 * quantity personnes sont transférées de la catégorie source à la catégorie destination
	 * @param source Catégorie où on récupère les personnes à transférer
	 * @param destination Catégorie où on rajoute les personnes à transférer
	 * @return Nombre de personnes réellement transférées entre les catégories
	 */
	public uint transfertBetweenCategories(string source, string destination, uint quantity) {
		return categories [destination].addAssigned (categories [source].removeAssigned (quantity));
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
	public uint naissances() {
		uint nbNaissances = (uint) Mathf.Max(0, Mathf.FloorToInt(0.0012f * totalPopulation / 12f)); // TODO : Max ? On n'est jamais < 0, si ?
		addPeople (nbNaissances);
		return nbNaissances;
	}

	/**
	 * Nombre de décès "fixe" en fonction de la taille de la population. 0.9% de leur nombre par année
	 * Appel de la fonction tous les mois (TODO : ?) (d'où la division par douze)
	 */
	public uint deces() {
		uint nbDeces = (uint) Mathf.Max(0, Mathf.FloorToInt(0.009f * totalPopulation / 12f));
		removePeople (nbDeces);
		return nbDeces;
	}
}
