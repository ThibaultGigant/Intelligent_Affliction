using UnityEngine;
using System.Collections;

public class Loisirs : APopulationCategory
{
	/**
	 * La chaleure idéale du pays pour une bonne efficacité au travail
	 */
	private int CHALEUR_IDEALE = 65;
	/**
	 * L'humidité idéale du pays pour une bonne efficacité au travail
	 */
	private int HUMIDITE_IDEALE = 45;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Loisirs(Population population, uint nb) : base(population, nb) {
		nom = "Loisirs";
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce ()
	{
		float newLsr = production ();

		productions.Enqueue((int)newLsr);

		population.country.resources ["Loisirs"].addRessource ((int)newLsr);
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
		Pays country = population.country;
		uint quantityTransport = country.resources ["Transports"].quantity;
		uint quantityNourriture = country.resources ["Nourriture"].quantity;
		float superficie = country.superficie;
		Climat clim = country.climat;

		/**
		 * On compare la production journalière moyenne à la production idéale
		 * 
		 * Facteur discriminant pour le signe de la valeur finale
		 */

		int sum = 0;
		foreach (int i in productions)
			sum += i;
		float moyenne = 1f * sum / productions.Count;

		// Nombre de visite égale à 45% de la population totale
		// En 50 ans
		float ideal = 45f * population.totalPopulation / 100f / (50f * 365f);

		float ecart = ideal / moyenne;
		if (ecart >= 1f)
			ecart = Mathf.Atan (ecart - 1) / (2f * Mathf.PI);
		else
			ecart = Mathf.Atan ((ecart - 1) * 10) / (2f * Mathf.PI);

		float resultat = Utils.indicesNormalises(new float[,] {	{ population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE ), 0f,1f,0.6f,1f, 0f },
																	{ population.country.indiceHI (), 0f,1f,0.75f,1f, 1f }
																});

		return ecart * resultat;
	}

	public override int production()
	{
		/**
		 * Formule
		 * 
		 * Avec un climat non idéal, il y a un ralentissement de la production
		 * 
		 * En France, sur les 10 attractions les plus visitées en 2016 (ou 2015 selon les souces),
		 * il y a eu environ 25 500 000 de visites, soit près de 38% de la population.
		 * Nous montons à 45 % (pour compter le reste des loisirs).
		 * Dans l'idéal, il faudrait qu'avec 10% de la population consacrés au développement des loisirs
		 * ce nombre de visites soit atteind en 50 ans.
		 * 
		 * [population totale] / 10 * 50 * 365 * x = 0.45 * [population totale],
		 * où x désigne la production journalière par "ouvrier"
		 * On obtient x = 2.4657 / 10 000
		 */
		float prod = assignedPopulation * 2.4657f / 10000f;

		float resultat = Utils.indicesNormalises(new float[,] {	{ population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE ), 0f,1f,0.6f,1f, 0f },
																	{ population.country.indiceTransports(), 0f,1f,0.75f,1f, 0f },
																	{ population.country.indiceHI (), 0f,1f,0.75f,1f, 1f }
																});

		return (int) (prod * resultat);
	}

	/**
	 * Evalue l'offre que le pays proposerait à un autre, en fonction du montant demandé
	 * et du pourcentage donné, calculé auparavant  en fonction de l'apport que pourrait
	 * rapporter l'échange avec lui
	 * @param montant Montant de connaissances dans le domaine des loisirs demandé par mois
	 * @param pourcentage Pourcentage de connaissances que l'on est prêt à offrir
	 * @return Montant de connaissance proposé par mois
	 */
	public override int offre(int montant, float pourcentage) {
		return Mathf.Min( montant, (int) (pourcentage * population.country.resources["Loisirs"].quantity ));
	}

	/**
	 * @return la production de nourriture idéale
	 */
	public override int ideal () {
		/**
		 * Avec une telle valeur, on obtiendrait en 50 ans
		 * le taux de visiteur journalière visé
		 */
		return (int) (population.totalPopulation / 10f * 2.4657f / 10000f);
	}

	public override Ressource demande () {
		RessourceLoisirs lsr = new RessourceLoisirs (population.country);

		// Pas la peine de préciser des choses, la carte de visite dit tout
		// Ici on va seulement dire qu'on a besoin de plus de savoir faire
		return (Ressource) lsr;
	}
}