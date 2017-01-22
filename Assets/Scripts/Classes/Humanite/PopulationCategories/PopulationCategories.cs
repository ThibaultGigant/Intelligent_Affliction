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

	/**
	 * Réorganisation de la population dans les catégories sans la présence du joueur
	 * @return Le nombre de personne ayant changé de catégorie
	 */
	public int reorganizePopulationCategoriesAuto() {
		/**
		 * • minBesoins		: Valeur minimale parmi les besoins de chaque catégorie
		 * • maxBesoins		: Valeur maximale parmi les besoins de chaque catégorie
		 * 
		 * • besoinsPositif		: Liste des besoins positifs
		 * • besoinsNegatif	: Liste des besoins négatifs
		 * 
		 * • ordrePositif		: Liste des catégories dont les besoins sont positifs,
		 * 						  de façon à ce que l'ordre corresponde à celui de besoinsPositif
		 * • ordreNegatif		: Liste des catégories dont les besoins sont négatifs,
		 * 						  de façon à ce que l'ordre corresponde à celui de besoinsNegatif
		 * 
		 * • besoin			: Variable temporaire, recevant les besoins d'une catégorie
		 */
		float minBesoins = Mathf.Infinity;
		float maxBesoins = -Mathf.Infinity;

		List<float> besoinsPositif = new List<float> ();
		List<float> besoinsNegatif = new List<float> ();

		List<APopulationCategory> ordrePositif = new List<APopulationCategory> ();
		List<APopulationCategory> ordreNegatif = new List<APopulationCategory> ();

		float besoin;

		/**
		 * Calcul des besoins de chaque catégorie
		 * Séparation des catégories dont l'effectif est en excès, et ceux en sous-nombre
		 */
		foreach (APopulationCategory cate in categories.Values) {
			// On ne considère que les catégorie autonomes (qui ne suivent pas, ou plus, les règles du joueur)
			if (!cate.modeAuto ())
				continue;
			
			// Récupération des besoin de la catégorie
			besoin = cate.besoins ();

			// Garde en mémoire les besoins et la catégorie associée,
			// selon le signe de la valeur
			if (besoin > 0) {
				besoinsPositif.Add (besoin);
				ordrePositif.Add (cate);
			}
			else {
				besoinsNegatif.Add (-besoin);
				ordreNegatif.Add (cate);
			}

			// Garde en mémoire les valeurs extrêmes
			if (minBesoins > besoin)
				minBesoins = besoin;
			if (maxBesoins < besoin)
				maxBesoins = besoin;
		}

		/**
		 * Nécessité de normaliser pour utiliser un tirage aléatoire selon les besoins de chaque catégorie
		 * • besoinsPositif_int	: Liste des valeurs correspondants aux besoins des catégories qui gonfleront leurs effectifs, après normalisation
		 * • besoinsNegatif_int	: Liste des valeurs correspondants aux besoins des catégories qui diminueront leurs effectifs, après normalisation
		 */
		List<int> besoinsPositif_int = new List<int> ();
		List<int> besoinsNegatif_int = new List<int> ();

		// Si aucune catégorie n'a besoin de remaniement, on s'arrête
		if (besoinsPositif.Count + besoinsNegatif.Count == 0)
			return 0;

		/**
		 * Si toutes les catégories veulent la même chose, à savoir gonfler ses rangs, ou les diminuer,
		 * il faut le repérer
		 * • preneurPositif	: Vrai si la catégorie qui va gonfler ses effectifs, souhaitait lui-même le faire
		 * • doneurNegatif	: Vrai si la catégorie qui va diminuer ses effectifs, souhaitait lui-même le faire
		 */
		bool preneurPositif = besoinsPositif.Count > 0;
		bool doneurNegatif = besoinsNegatif.Count > 0;

		/**
		 * S'il n'y a pas de demandeur, on devra prendre chez ceux qui offrent le moins
		 */
		if (!preneurPositif) {
			for (int i = 0; i < besoinsNegatif.Count; i++)
				besoinsPositif_int.Add ((int)((1f - besoinsNegatif [i]) * 100f / maxBesoins));
		}
		else {
			for (int i = 0; i < besoinsPositif.Count; i++)
				besoinsPositif_int.Add ((int)((besoinsPositif [i]) * 100f / maxBesoins));
		}

		/**
		 * S'il n'y a pas d'offreur, on devra prendre chez ceux qui demandent le moins
		 */
		if (!doneurNegatif) {
			for (int i = 0; i < besoinsPositif.Count; i++)
				besoinsNegatif_int.Add ((int)((1f - besoinsPositif [i]) * 100f / minBesoins));
		}
		else {
			for (int i = 0; i < besoinsNegatif.Count; i++)
				besoinsNegatif_int.Add ((int)((besoinsNegatif [i]) * 100f / minBesoins));
		}

		int indicePositif = Utils.tirageAlatoireList (besoinsPositif_int);
		int indiceNegatif = Utils.tirageAlatoireList (besoinsPositif_int);

		/**
		 * Si toutes les catégories veulent la même chose, il est possible les
		 * indices choisies désignent la même catégorie, dans quel cas, il est inutile de
		 * faire un transfert
		 */
		if ((!doneurNegatif || !preneurPositif) && indiceNegatif == indicePositif)
			return 0;

		// Maximum 10% de la population qui peux changer par jour par catégorie
		int transfert = 0;
		if (preneurPositif) {
			transfert = (int)Mathf.Min (0.1f * population.totalPopulation, population.country.indiceHI () * besoinsPositif [indicePositif] * ordrePositif [indicePositif].assignedPopulation);
			APopulationCategory doneur;
			if (doneurNegatif)
				doneur = ordreNegatif [indiceNegatif];
			else
				doneur = ordrePositif [indiceNegatif];
			transfert = Mathf.Min (transfert, Mathf.Min((int) ordrePositif [indicePositif].assignedPopulation, (int) doneur.assignedPopulation));

			ordrePositif [indicePositif].addAssigned (transfert);
			doneur.removeAssigned (transfert);
		}
		else {
			transfert = (int)Mathf.Min (0.1f * population.totalPopulation, population.country.indiceHI () * besoinsPositif [indicePositif] * ordreNegatif [indicePositif].assignedPopulation);
			APopulationCategory doneur;
			if (doneurNegatif)
				doneur = ordrePositif [indiceNegatif];
			else
				doneur = ordreNegatif [indiceNegatif];
			transfert = Mathf.Min( transfert ,Mathf.Min((int) ordreNegatif [indicePositif].assignedPopulation, (int) doneur.assignedPopulation));

			ordreNegatif [indicePositif].addAssigned (transfert);
			doneur.removeAssigned (transfert);
		}

		return transfert;
	}

	public void reorganizePopulationCategoriesForPlayer()
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
			if (cate.wantedPercentage == -1f)
				continue;
			
			change = cate.wantedPercentage * population.totalPopulation - cate.assignedPopulation;
			change *= population.country.indiceHI ();
			// Si l'on approche du pourcentage voulu, ou que la populatino n'est pas assez satisfaite, on s'arrête
			if (change < 0.01f * population.totalPopulation) {
				cate.wantedPercentage = -1f;
				cate.stepReajusteForPlayer = -1;
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

			cate.stepReajusteForPlayer++;

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

