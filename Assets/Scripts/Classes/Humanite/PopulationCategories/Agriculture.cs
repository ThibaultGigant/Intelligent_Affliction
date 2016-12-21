using UnityEngine;
using System.Collections;

public class Agriculture : APopulationCategory
{
	/**
	 * La chaleure idéale du pays pour une bonne agriculture
	 * (cf. la méthode offre)
	 */
	int CHALEUR_IDEALE;
	/**
	 * L'humidité idéale du pays pour une bonne agriculture
	 * (cf. la méthode offre)
	 */
	int HUMIDITE_IDEALE;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Agriculture(Population population, uint nb) : base(population, nb) {
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public override void produce () {}

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
		 * Prend en compte le climat du pays
		 * L'idéal étant un pays avec une chaleur de 65
		 * et une humidité de 45 (valeure arbitraire, à revoir)
		 */
		float indiceClimat = Mathf.Abs (CHALEUR_IDEALE - clim.chaleur) +
		                     Mathf.Abs (HUMIDITE_IDEALE - clim.humidite);

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

		/**
		 * Prend en compte les transports, et la superficie du pays
		 * Si la superficie est trop grande, qu'il y a peu de transport,
		 * mais que la quantité de nourriture est grande, c'est qu'il
		 * y a trop de labeur pour rien
		 * 
		 * (pour l'instant, le nombre de personnes n'est pas pris en compte)
		 * 
		 * Racine carrée de la superficie car elle se mesure en m², tandis que les lignes de
		 * Transport (TGV, bus, ...) se mesurent en m
		 */
		float indiceTransport = ( Mathf.Sqrt(superficie) / (float) quantityTransport );

		return indiceHI * indiceClimat * indiceNourriture * indiceTransport;
	}
}