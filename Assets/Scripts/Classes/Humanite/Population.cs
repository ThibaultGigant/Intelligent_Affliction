﻿using UnityEngine;
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

	public PopulationCategories categories;

	/**
	 * Constructeur
	 * @param p Pays associé
	 * @param nb Taille de la population, en nombre d'habitant
	 */
	public Population(Pays p) {
		country = p;
		uint nb = DonneePays.getPopulation(country.nomPays);
		initialNumberPopulation = nb;
		totalPopulation = nb;

		happinessIndex = 50;

		nbInfectedDetected = (uint) 0;

		categories = new PopulationCategories (this, totalPopulation);
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

		float ratio;

		foreach (string cat in categories.Keys) {
			ratio = (float) (categories [cat].assignedPopulation) / (float) totalPopulation;
			if (temp == 0)
				return;
			temp -= categories [cat].removeAssigned ((int)(nb * ratio));

			if (cat == "Recherche" && Random.value < 0.05) {
				country.pointsRecherche -= Mathf.Min (country.pointsRecherche, (int)((float)nb * ratio * 0.001));
			}
		}

		totalPopulation -= nb - temp;
	}

	/**
	 * A l'appel de cette fonction, la population n'écoute plus le joueur et décide de se rebeller
	 */
	public void goRogue() {
	}

	/**
	 * Nombre de naissances "fixe" en fonction de la taille de la population. 1.2% de leur nombre par année
	 */
	public uint naissances() {
		uint nbNaissances = (uint) Mathf.Max(0, Mathf.FloorToInt(0.0012f * totalPopulation / 12f));
		addPeople (nbNaissances);
		return nbNaissances;
	}

	/**
	 * Nombre de décès "fixe" en fonction de la taille de la population. 0.9% de leur nombre par année
	 */
	public uint deces() {
		uint nbDeces = (uint) Mathf.Max(0, Mathf.FloorToInt(0.009f * totalPopulation / 12f));
		removePeople (nbDeces);
		return nbDeces;
	}

	public void reorganizePopulationCategories()
	{
		categories.reorganizePopulationCategoriesForPlayer ();
		if ( Mathf.Pow( Random.value, 2f) < country.indiceHI())
			categories.reorganizePopulationCategoriesAuto ();
	}

	public void consome() {

	}

	public void updateHappiness () {
		float resultat = Utils.indicesNormalises(new float[,] {
			{ country.indiceTransports(), 0f,1f,0.8f,1f, 0f },
			{ country.indiceLoisirs(), 0f,1f,0.8f,1f, 0f },
			{ country.indiceInfection(), 0f, 1f, 0.5f, 1f, 0f},
			{ country.indiceNourriture (), 0f,1f,0.2f,1f, 0f }
		});
		happinessIndex = (int) (resultat * 100f);
	}
}