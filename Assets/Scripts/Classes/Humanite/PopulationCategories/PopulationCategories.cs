using System;
using System.Collections.Generic;
using UnityEngine;

public class PopulationCategories
{

	/**
	 * 
	 */
	Population population;

	/**
	 * Ensemble des Catégories de population, accessibles par leur nom
	 */
	public IDictionary<string, APopulationCategory> categories;

	public PopulationCategories (Population pop, uint totalPopulation)
	{
		population = pop;
		SetupCategories (totalPopulation);
	}

	/**
	 * Répartit la population entre les différentes catégories, lors du premier lancement du jeu
	 */
	private void SetupCategories (uint totalPopulation) {
		if (totalPopulation < 0) {
			Debug.LogError (population.country.nomPays + ", Repartition : Le nombre de personnes doit être positif");
			totalPopulation = 0;
		}

		// Repartition uniforme, parmis les 6 catégories. (TODO : A revoir...)
		uint nbReparti = totalPopulation / 6;

		categories = new Dictionary<string, APopulationCategory> ();
		categories.Add ("Agriculture", new Agriculture (population, nbReparti));
		categories.Add ("Inactifs", new Inactifs (population, nbReparti));
		categories.Add ("Loisirs", new Loisirs (population, nbReparti));
		categories.Add ("Medecine", new Medecine (population, nbReparti));
		categories.Add ("Recherche", new Recherche (population, nbReparti));
		categories.Add ("Transports", new Transports (population, totalPopulation - 5 * nbReparti)); // Pour eviter les imprécisions dues à la discrétisation des pourcentages
	}

	public void produce() {
		foreach (string key in categories.Keys) {
			categories [key].produce ();
		}
	}

	public void reorganizePopulationCategories()
	{
		
	}
}

