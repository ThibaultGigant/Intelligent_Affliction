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
		/**
		 * Formule
		 * 
		 * • Plus le ratio entre le nombre d'infectés détectés et la population totale
		 * est grand, plus il est probable d'en détecter
		 * • Plus les connaissances à propos des symptomes est grand, plus il y a de chance d'en
		 * détecter de nouveaux, et d'en soigner
		 * 
		 * • Dans l'idéal, avec la connaissance absolue, il faudrait qu'avec 1/6 de la population consacré à la médecine, on puisse
		 * garder en forme l'ensemble de la population si celle-ci se faisait infectée à 5% chaque jour
		 * 
		 * • Facteur de connaissances.
		 * Plus le développement des recherches est poussée, plus le facteur sera grand
		 * Plus le coût d'un symptome est élevé, plus le facteur sera grand (efficacité ?)
		 * Prise en compte de la détectabilité des symptomes.
		 * 
		 * • Pour la détection, on calcul le nombre d'infectés non détectés, puis on fait le ratio par rapport
		 * à la population totale (pour obtenir la densité de ces personnes dans le pays).
		 * On prend en compte le facteur lié aux connaissances, pet le nombre de médecin
		 * [nouveaux détectés] = ( ([infectés non détectés] - [infectés détectés]) / [population totale] )
		 * 							* [Facteur de connaissances] * [nombre de médecins]
		 */

		float ratioInfectedPopulation = population.nbInfectedDetected / population.totalPopulation;

		float knowledgesFacteurDetection = 0f;
		float knowledgesFacteurSoin = 0f;

		Knowledge knowledge;

		foreach ( Ressource resource in population.country.resources.resources.Values ) {
			if (resource.GetType () != typeof(Knowledge))
				return;
			knowledge = (Knowledge) resource;

			knowledgesFacteurDetection 	+= knowledge.developpement * DonneeSouche.detectabilitySymptomes[resource.nom.Substring(9)];
			knowledgesFacteurSoin		+= knowledge.developpement / DonneeSouche.lethalitySymptomes[resource.nom.Substring(9)];
		}

		int soigne = (int) (assignedPopulation * ratioInfectedPopulation * knowledgesFacteurDetection / 20f);

		int detectes = (int)(((float) (population.country.souche.getNbInfected() - population.nbInfectedDetected) * knowledgesFacteurDetection) /
			(float) population.totalPopulation * (float) assignedPopulation);

		population.nbInfectedDetected += (uint) detectes;
		population.nbInfectedDetected -= (uint) soigne;
		population.country.souche.removeInfectedPeople( (uint) soigne );

		infectesDecouverts.Enqueue (detectes);
		soignes.Enqueue (soigne);
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
		 * Si la recherche n'avance pas assez (est loin de sa production idéale), et qu'il n'y a assez de soignés
		 * on peut se permettre de relacher des médecins
		 * 
		 * S'il n'y a pas assez de soignés, ce n'est pas bien
		 */

		float indiceRecherche = population.country.indiceRecherche ();

		float indiceSoin = population.country.indiceMedecine();

		return indiceRecherche / (1f + indiceSoin);
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