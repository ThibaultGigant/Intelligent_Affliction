using UnityEngine;
using System.Collections;

public class Transports : APopulationCategory
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
	public Transports(Population population, uint nb) : base(population, nb) {
		nom = "Transports";
	}

	/**
	 * Production de Transport
	 * Cette ressource se congrège deux informations : la longueur des lignes
	 * et leurs vitesses moyennes. Le montant de ressources en Transport est repésenté
	 * par la longueur du réseau de transport du pays, multiplié par la vitesse
	 * moyenne (la grandeur des nombres est assez grande)
	 */
	public override void produce () {
		
		float newTrs = production ();

		productions.Enqueue((int)newTrs);

		population.country.resources ["Transports"].addRessource ((int)newTrs);
	}

	/**
	 * Estimation de la production de Transport chaque jour
	 */
	public override int production() {
		/**
		 * Formule
		 * • Considérons que seul le quart des personnes assignées à cette
		 * catégorie s'occupent de créer des lignes. Le reste conduit, maintient,
		 * etc
		 * • Considérons qu'une équipe de x personnes puisse ajouter un mètre
		 * à une ligne de transport par mois, de façopn à atteindre un réseau férovière
		 * de 8% de la superficie avec une vitesse moyenne de 160 km/h en un peu
		 * plus de 30 ans. Les valeurs de référence ont été calculées par rapport aux chiffres
		 * obtenues consernant la France.
		 * [ Trace du calcul, à reporter dans le rapport si on le garde]
		 * En France, 66 030 000 habitants ont été recencés en 2013, pour une superficie de 643 801 km²
		 * Avec de telles valeurs, considérons que cette catégorie possède 1/6e de la population. Le quart
		 * de ceux-ci représente 2 751 250 (arrondissons à 2 500 000).
		 * Ainsi, pour qu'en 30 ans, il y ai 8% de la superficie avec une vitesse moyenne de 160 km/h,
		 * il faudrait que :
		 * [nombre d'équipe] * 12 * 30 = 8% * [Superficie] * 160
		 * où [nombre d'équipe] = 2 500 000 / [taille d'une équipe]
		 * d'où la formule générale suivante :
		 * x = (225 * 2 500 000) / ( 8 * [Superficie])
		 * • Moins le climat est clément, moins la production sera efficace
		 * • 
		 * • On evite de dépasser un certain cap : [Superficie] * 8%
		 * 
		 * 1/4 * [nombre d'employés] / [taille d'une équipe]
		 * ([nombre d'employés] * [Superficie]) / 281 250 000
		 * ([nombre d'employés] / 1000 * [superficie]) / 281 250
		 * 
		 * Une dernière division par 30 est possible, pour pouvoir faire l'opération jour par jour
		 */
		float newTrs = (float) (assignedPopulation / 30f) * population.country.superficie / 281250000f;

		/*if (population.country.name == "Afrique") {
			Debug.Log ("Timon Transport production");
			Debug.Log (assignedPopulation / 4f);
			Debug.Log ("taille équipe " + 281250000f/(8f*population.country.superficie));
			Debug.Log (newTrs);
		}*/

		float resultat = Utils.indicesNormalises(new float[,] {	{ population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE ), 0f,1f,0.6f,1f, 0f },
																	{ population.country.indiceHI (), 0f,1f,0.75f,1f, 1f }
																});

		return (int) (newTrs * resultat);
	}

	/**
	 * offre
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float besoins () {
		Pays country = population.country;
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

		// 12% de la superficie * 160km/h
		// En 30 ans
		float ideal = 12f * 160f * Mathf.Sqrt(population.country.superficie) / 100f / (30f * 365f);

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

	/**
	 * Evalue l'offre que le pays proposerait à un autre, en fonction du montant demandé
	 * et du pourcentage donné, calculé auparavant  en fonction de l'apport que pourrait
	 * rapporter l'échange avec lui
	 * @param montant Montant de connaissances dans le domaine du transport demandé par mois
	 * @param pourcentage Pourcentage de connaissances que l'on est prêt à offrir
	 * @return Montant de connaissance proposé par mois
	 */
	public override int offre(int montant, float pourcentage) {
		return Mathf.Min( montant, (int) (pourcentage * population.country.resources["Transports"].quantity ));
	}

	/**
	 * @return la production de nourriture idéale
	 */
	public override int ideal () {
		/**
		 * Avec une telle valeur, on obtiendrait en 30 ans
		 * le taux de visiteur journalière visé
		 */
		return (int) ((population.totalPopulation / 6f / 30f) * population.country.superficie / 281250000f);
	}

	public override Ressource demande () {
		RessourceTransports trs = new RessourceTransports (population.country);

		// Pas la peine de préciser des choses, la carte de visite dit tout
		// Ici on va seulement dire qu'on a besoin de plus de savoir faire
		return (Ressource) trs;
	}
}