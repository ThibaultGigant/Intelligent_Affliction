using UnityEngine;
using System.Collections;

public class Recherche : APopulationCategory
{
	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Recherche(Population population, uint nb) : base(population, nb) {
		nom = "Recherche";
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {
		/**
		 * Formule
		 * 
		 * Nombre de points de recherche produit
		 * Un par groupe de 10 chercheurs, chaque semaine
		 * Plus il y a d'infectes, plus il est simple de faire des recherches sur la maladie
		 * Plus on a déjà de points, moins il est simple de avoir de nouveaux
		 */

		float ratioInfectes = population.nbInfectedDetected = population.totalPopulation;
		int points = (int)(assignedPopulation / 70f * population.country.indiceHI () * ratioInfectes);

		population.country.pointsRecherche += points;

	}

	/**
	 * offre
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float besoins () {
		return 0f;
	}

	public override int production()
	{
		return 0;
	}

	public override int offre(int montant, float pourcentage) {
		return 0;
	}

	public override int ideal() {
		return 0;
	}
}