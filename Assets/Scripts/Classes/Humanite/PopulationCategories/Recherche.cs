using UnityEngine;
using System.Collections.Generic;

public class Recherche : APopulationCategory
{
	/**
	 * Liste des dernières améliorations
	 */
	public LimitedQueue<float> ameliorations;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Recherche(Population population, uint nb) : base(population, nb) {
		nom = "Recherche";
		ameliorations = new LimitedQueue<float> (Parametres.tailleMemoire);
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {
		// Obligé de tout faire d'un coup, puisqu'il y a deux types de "productions"
		production();
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
		 * Si la recherche n'avance pas assez (est loin de sa production idéale), il faut plus d'effectif
		 * 
		 * Si le nombre de soigné est assez élevé, peut considérer qu'on a fait notre travail
		 */

		float indiceRecherche = population.country.indiceRecherche ();

		return (1f - indiceRecherche);
	}

	public override int production()
	{
		/**
		 * Formule
		 * 
		 * Nombre de points de recherche produit
		 * Un par groupe de 1000 chercheurs, chaque semaine
		 * Plus il y a d'infectes, plus il est simple de faire des recherches sur la maladie
		 * Plus on a déjà de points, moins il est simple de avoir de nouveaux
		 * 
		 * Choix du symptome à traiter
		 * Plus le symptome est développé, moins il est urgent de s'en occuper
		 * Plus la léthalité d'un tel symptome est grande, plus il faut se presser
		 * Tirage aléatoire, selon le ratio
		 * 
		 * Développement des connaissaces
		 * Plus il y a de chercheurs, mieux c'est
		 * Plus il est simple de détecter le symptom, mieux c'est
		 * Pour "normaliser", on ne prend pas en compte le nombre de chercheur, mais le pourcentage de la population
		 * qui s'y consacre (le nombre de chercheur en lui même est considéré dans l'apport en points de recherche, donc un grans pays sera quand même favorisé)
		 */

		float ratioInfectes = (float) population.nbInfectedDetected / (float) population.totalPopulation;
		int points = (int)((float) assignedPopulation * 0.003f *  Mathf.Sqrt( population.country.indiceHI () )* (0.5f + ratioInfectes / 2f ));

		population.country.pointsRecherche += points;

		float[] choixDeveloppement = new float[DonneeSouche.listSymptoms.Count];
		float somme = 0f;
		for ( int i = 0 ; i < DonneeSouche.listSymptoms.Count ; i++ ) {
			if (population.country.souche != null && population.country.souche.symptoms.ContainsKey (DonneeSouche.listSymptoms [i])) {
				choixDeveloppement [i] = population.country.souche.symptoms [DonneeSouche.listSymptoms [i]].getDetectableIndex (); // (1f + ((Knowledge)population.country.resources ["Knowledge" + DonneeSouche.listSymptoms [i]]).developpement);
				somme += choixDeveloppement [i];
			}
		}

		int[] choixDeveloppementNorma = new int[DonneeSouche.listSymptoms.Count];
		for ( int i = 0 ; i < DonneeSouche.listSymptoms.Count ; i++ ) {
			choixDeveloppementNorma[i] = ( int ) (choixDeveloppement [i] / somme * 100f);
		}
			
		int choix = Utils.tirageAlatoireList (new List<int> (choixDeveloppementNorma));

		string symptomChoisi = DonneeSouche.listSymptoms [choix];

		float amelioration = 0f;
		if (DonneeSouche.coutsSymptomes [symptomChoisi] <= population.country.pointsRecherche) {
			if (population.country.souche != null && population.country.souche.symptoms.ContainsKey (symptomChoisi)) {
				amelioration = assignedPopulation / population.totalPopulation * population.country.souche.symptoms [symptomChoisi].getDetectableIndex ();
				((Knowledge)population.country.resources ["Knowledge" + symptomChoisi]).developpement += amelioration;
				population.country.pointsRecherche -= DonneeSouche.coutsSymptomes [symptomChoisi];
			}
		}

		productions.Enqueue (points);
		ameliorations.Enqueue (amelioration);

		return points;
	}

	public override int offre(int montant, float pourcentage) {
		return 0;
	}

	/**
	 * Indique le nombre de points qui devraient être produits par jour idéalement
	 */
	public override int ideal() {
		/**
		 * Formule
		 * 
		 * Arbitrairement, il faudrait qu'il y ai 1 points par groupe de x personnes (x = 10% de la population assignée à la recherche)
		 * Plus il y a eu d'avancement, moins on a besoin de se développer (?)
		 */
		float somme = 0f;
		for ( int i = 0 ; i < DonneeSouche.listSymptoms.Count ; i++ ) {
			somme +=  DonneeSouche.lethalitySymptomes[ DonneeSouche.listSymptoms[i]  ] / ( 1f +((Knowledge) population.country.resources["Knowledge" + DonneeSouche.listSymptoms[i] ]).developpement);
		}

		float ratioInfectes = population.nbInfectedDetected / population.totalPopulation;

		return (int) ( somme * ratioInfectes) + 1;
	}

	/**
	 * Revoie la moyenne des points produits par jour
	 */
	public float moyennePoints() {
		float moyenne = 0f;
		foreach (int nb in productions) {
			moyenne += (float) nb;
		}
		moyenne /= (float) productions.Count;
		return moyenne;
	}

	public override Ressource demande () {

		if (besoins () < Parametres.seuilDAppelALAide)
			return null;

		Knowledge knowledge;
		Knowledge minKnowledge = null;
		float min = Mathf.Infinity;
		foreach ( Ressource res in population.country.resources.resources.Values ) {
			if (res.GetType () != typeof(Knowledge))
				continue;

			knowledge = (Knowledge) res;
			if (knowledge.developpement < min) {
				min = knowledge.developpement / DonneeSouche.lethalitySymptomes[ knowledge.sujetKnowledge ];
				minKnowledge = knowledge;
			}
		}

		Knowledge know = new Knowledge (population.country, minKnowledge.sujetKnowledge, minKnowledge.coutDeRecherche);
		know.quantity = minKnowledge.quantity;
		know.developpement = minKnowledge.developpement;
		return (Ressource) know;
	}
}