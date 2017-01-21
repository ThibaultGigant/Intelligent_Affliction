using UnityEngine;
using System.Collections;

public class Loisirs : APopulationCategory
{
	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Loisirs(Population population, uint nb) : base(population, nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce ()
	{

	}

	/**
	 * offre
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float besoins () {
		/**
		 * Formule
		 * 
		 * Avec des transports bien déservis, il est plus simple d'aller travailler
		 * 
		 * Moins la population est grande, moins il y a besoin d'attraction et de loisirs
		 * Peut rendre le signe négatif
		 */
		return 0f;
	}

	public override int production()
	{
		/**
		 * Formule
		 * 
		 * Avec un climat non idéal, il y a un ralentissement de la production
		 * 
		 * En France, sur les 10 attractions les plus visitées en 2016 (ou 2015 selon les souces),
		 * il y a eu environ 25 500 000 de visites prêt de 38% de la population.
		 * Nous montons à 45 % (pour compter le reste des loisirs).
		 * Dans l'idéal, il faudrait qu'avec 10% de la population consacrés au développement des loisirs
		 * ce nombre de visites soit atteind en 50 ans.
		 * 
		 * [population totale] / 10 * 0.4
		 */
		return 0;
	}

	public override int offre(int montant, float pourcentage) {
		return 0;
	}
}