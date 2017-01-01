using UnityEngine;
using System.Collections;

public class Agriculture : APopulationCategory
{
	/**
	 * La chaleure idéale du pays pour une bonne agriculture
	 * (cf. la méthode offre)
	 */
	int CHALEUR_IDEALE = 65;
	/**
	 * L'humidité idéale du pays pour une bonne agriculture
	 * (cf. la méthode offre)
	 */
	int HUMIDITE_IDEALE = 45;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Agriculture(Population population, uint nb) : base(population, nb) {
	}

	/**
	 * Production de Nourriture
	 * Plus la population assignée à cette catégorie est grande, plus la production sera
	 * importante.
	 * Si le climat du pays s'éloigne trop du climat idéal, la récolte en souffrira
	 * Si la superficie du pays est grand, il y aura davantage de terres cultivable.
	 * Si les transports sont bien développé, les agriculteurs auront moins de mal à
	 * travailler
	 */
	public override void produce () {
		/**
		 * Formule
		 * • Dans le meilleur des mondes, considérons que 10% d'agriculteurs est une portion
		 * raisonnable. Il faut pour cela que chaque agriculteur puisse nourrire dix personnes.
		 * • Plus le climat est éloigné de l'idéal, moins il y a de récoltes. cf. la méthode
		 * indiceClimat
		 * • Plus il y a de transport, mieux c'est. Plus la surface est grande, mieux c'est.
		 * cf. la méthode indiceTransportSuperficie.
		 * 
		 * 10 * [nombre d'agriculteurs] * [indice Climat] * ([indice Transport])
		 */
		float newAgr = 10f * assignedPopulation * indiceClimat () * indiceTransportSuperficie();

		if (population.country.name == "Afrique") {
			Debug.Log ("ap " + assignedPopulation);
			Debug.Log ("clim " + indiceClimat());
			Debug.Log ("sup " + indiceTransportSuperficie());
			Debug.Log (newAgr);
		}

		//population.country.resources ["Agriculture"].addRessource (newAgr);
	}

	/**
	 * offre
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * (des normalisations sont nécessaires)
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float offre()
	{
		// TODO prendre en compte les stocks

		//assignedPopulation
		// e^-HI
		// Transport
		Pays country = population.country;
		int HI = population.getHappinessIndex();
		int quantityTransport = country.resources ["Transport"].quantity;
		int quantityNourriture = country.resources ["Nourriture"].quantity;
		float superficie = country.superficie;
		Climat clim = country.climat;

		/**
		* Prend en compte le HappinessIndex
		* Plus cette valeur est élevé, moins les besoins seront
		* prononcés
		*/
		float indiceHI = Mathf.Exp ((float)-HI);

		/**
		 * Prise en compte du ratio entre la taille de la population, et la nourriture
		 * du pays
		 * 
		 * Facteur discriminant pour le signe de la valeur finale
		 */
		float indiceNourriture = (float) population.totalPopulation / (float) quantityNourriture;
		if (indiceNourriture >= 1f)
			indiceNourriture = Mathf.Atan (indiceNourriture - 1);
		else
			indiceNourriture = Mathf.Atan ((indiceNourriture - 1) * 10);



		Debug.Log ("HI = " + indiceHI);
		Debug.Log ("Climat = " + indiceClimat());
		Debug.Log ("Nourriture = " + indiceNourriture);
		Debug.Log ("Transport = " + indiceTransportSuperficie());

		return indiceHI * indiceNourriture * indiceTransportSuperficie() / indiceClimat();
	}

	/**
	 * indiceClimat
	 * Donne une indication quant à la condition climatique du pays
	 * Plus la chaleur et l'humidité d'un pays est proche des valeurs
	 * idéale, plus la valeur retournée sera grande
	 * @return Une valeur entre 0 et 1. Zéro si les conditions sont catastrophiques,
	 * un s'ils sont parfaits
	 // L'idéal étant un pays avec une chaleur de 65
	 // et une humidité de 45 (valeure arbitraire, à revoir)
	 */
	private float indiceClimat()
	{
		/**
		 * Formule :
		 * x² / 4 - x + 1,
		 * où x = (|CHAULEUR_IDEALE - chaleur| + |HUMIDITE_IDEALE - humidite|) / 100
		 * et x appartient à [0, 2]
		 * Ainsi, en 0, le facteur atteind son maximum, 1
		 * en 1, le facteur atteind 1/4
		 * en 2, le facteur atteind 0
		 */
		Climat clim = population.country.climat;
		float ind = (float)(Mathf.Abs (CHALEUR_IDEALE - clim.chaleur) +
			Mathf.Abs (HUMIDITE_IDEALE - clim.humidite)) / 100f;
		return Mathf.Pow (ind, 2f) / 4f - ind + 1;
	}

	/**
	 * indiceTransportSuperficie
	 * @return Une valeur entre 0,75 et 1. Zéro s'il y a aucun transport, un s'il y en a beaucoup
	 */
	private float indiceTransportSuperficie()
	{
		/**
		 * Formule
		 * • La "quantité" de transport par rapport à la superficie totale.
		 * [quantité de Transport] / sqrt([Superficie])
		 * Racine carrée de la superficie car elle se mesure en m², tandis que les lignes de
		 * Transport (TGV, bus, ...) se mesurent en m
		 * • Plus la superficie est grande, mieux c'est.
		 * 
		 * ( [quantité de Transport] / sqrt([Superficie]) ) * [Superficie]
		 * Cela donne [quantité de Transport] * sqrt([Superficie])
		 * 
		 * Le résultat est bornée supérieurement par 1.
		 * On ramène l'image de la fonction de [0,1] sur [0.75,1]
		 */
		int quantityTransport = population.country.resources ["Transport"].quantity;
		float ind =  (float) quantityTransport * Mathf.Sqrt(population.country.superficie);

		return (Mathf.Min (1f, ind) / 4f) + 0.75f;
	}
}