using System;
using System.Collections.Generic;
using UnityEngine;

public class PopulationCategories
{

	/**
	 * Population répartie dans ces catégories
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

	public void produce() {
		foreach (string key in categories.Keys) {
			categories [key].produce ();
		}
	}

	public void reorganizePopulationCategories()
	{
		Dictionary<APopulationCategory, int> echangesToleresPositif = new Dictionary<APopulationCategory, int> ();
		Dictionary<APopulationCategory, int> echangesToleresNegatif = new Dictionary<APopulationCategory, int> ();
		float change;
		int tolerance;
		int positif = 0;
		int negatif = 0;
		APopulationCategory cate;
		foreach (string nom in categories.Keys) {
			cate = categories [nom];
			change = cate.wantedPercentage * population.totalPopulation - cate.assignedPopulation;
			change *= population.country.indiceHI ();
			// Si l'on approche du pourcentage voulu, ou que la populatino n'est pas assez satisfaite, on s'arrête
			if (change < 0.01f * population.totalPopulation) {
				cate.wantedPercentage = -1f;
				continue;
			}

			// On ne fait migrer que 1% de la population totale du pays par catégorie et par jour
			if (change > 0) {
				tolerance = (int) Mathf.Min (change, 0.01f * population.totalPopulation);
				positif += tolerance;
				echangesToleresPositif.Add (cate, tolerance);
			}
			else {
				tolerance = (int) Mathf.Max (change, -0.01f * population.totalPopulation);
				negatif -= tolerance;
				echangesToleresNegatif.Add (cate, -tolerance);
			}

		}
		int echangeEffectif;
		if (positif > negatif) {
			foreach (APopulationCategory donnant in echangesToleresNegatif.Keys) {
				foreach (APopulationCategory prenant in echangesToleresPositif.Keys) {
					if (echangesToleresPositif [prenant] > 0) {
						echangeEffectif = Mathf.Min (echangesToleresPositif [prenant], echangesToleresNegatif [donnant]);
						prenant.addAssigned (echangeEffectif);
						donnant.removeAssigned (echangeEffectif);
						echangesToleresPositif [prenant] -= echangeEffectif;
						echangesToleresNegatif [donnant] -= echangeEffectif;
					}

					if (echangesToleresNegatif [donnant] <= 0)
						break;
				}
			}
		}
		else {
			foreach (APopulationCategory prenant in echangesToleresPositif.Keys ) {
				foreach ( APopulationCategory donnant in echangesToleresNegatif.Keys ) {
					if (echangesToleresNegatif [donnant] > 0) {
						echangeEffectif = Mathf.Min (echangesToleresNegatif [prenant], echangesToleresPositif [donnant]);
						prenant.addAssigned (echangeEffectif);
						donnant.removeAssigned (echangeEffectif);
						echangesToleresNegatif [prenant] -= echangeEffectif;
						echangesToleresPositif [donnant] -= echangeEffectif;
					}

					if (echangesToleresPositif [prenant] <= 0)
						break;
				}
			}
		}

	}
}

