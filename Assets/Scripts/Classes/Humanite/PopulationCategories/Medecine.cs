using UnityEngine;
using System.Collections;

public class Medecine : APopulationCategory
{

	/**
	 * Liste des montants de nouveaux infectés par jour
	 */
	public LimitedQueue<int> infectesDecouverts;

	/**
	 * Liste des montants des soignés par jour
	 */
	public LimitedQueue<int> soignes;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Medecine(Population population, uint nb) : base(population, nb) {
		nom = "Medecine";
		infectesDecouverts = new LimitedQueue<int> (Parametres.tailleMemoire);
		soignes = new LimitedQueue<int> (Parametres.tailleMemoire);
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {
		infectesDecouverts.Enqueue (0);
		soignes.Enqueue (0);
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

	/**
	 * @return La production de nourriture idéale
	 */
	public override int ideal () {
		/**
		 * Dans l'idéal
		 * • Il faudrait soigner autant de nouveaux infectés que l'on découvre par jour
		 * • Pondéré par le ratio entre le nombre d'infectés et la population totale
		 * • • 1 + [nombre d'infectés détectés] / [population totale] * 3 (=> pour presser les médecins)
		 */
		return (int) (moyenneNouveauxInfectes () * ( 1f + (population.nbInfectedDetected / population.totalPopulation) * 3f ));
	}

	public float moyenneSoignes() {
		float moyenne = 0f;
		foreach (float nb in soignes) {
			moyenne += nb;
		}
		moyenne /= soignes.Count;
		return moyenne;
	}

	public float moyenneNouveauxInfectes() {
		float moyenne = 0f;
		foreach (float nb in infectesDecouverts) {
			moyenne += nb;
		}
		moyenne /= infectesDecouverts.Count;
		return moyenne;
	}

}