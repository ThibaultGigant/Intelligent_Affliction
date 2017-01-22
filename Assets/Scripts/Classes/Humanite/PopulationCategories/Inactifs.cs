using UnityEngine;
using System.Collections;

public class Inactifs : APopulationCategory
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
	public Inactifs(Population population, uint nb) : base(population, nb) {
		nom = "Inactif";
	}

	/**
	 * Peut casser des infrastructures, liées aux transports ou aux loisirs
	 */
	public override void produce () {
		int newIna = production ();

		productions.Enqueue((int)newIna);

		/**
		 * Dégrade aléatoirement des infrastructures de loisirs ou de transports
		 * Pour un pourcentage totale de 10%
		 */
		int degradeTransports = (int) (newIna * Random.value);

		population.country.resources ["Transports"].removeRessource ((float) degradeTransports / 100f);
		population.country.resources ["Loisirs"].removeRessource ((float) (newIna - degradeTransports) / 100f);
	}

	/**
	 * besoins
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float besoins () {

		float indiceLoisirs = population.categoriesPop.categories ["Loisirs"].besoins ();
		if (indiceLoisirs >= 0f)
			indiceLoisirs = Mathf.Atan (indiceLoisirs) / (2f * Mathf.PI);
		else
			indiceLoisirs = Mathf.Atan ((indiceLoisirs) * 10) / (2f * Mathf.PI);

		float resultat = Utils.indicesNormalises(new float[,] {	{ population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE ), 0f,1f,0.6f,1f, 0f },
																	{ population.country.indiceHI (), 0f,1f,0f,0.3f, 1f } // Jusqu'a 30% d'inactifs
																});

		return indiceLoisirs * resultat;
	}

	/**
	 * Renvoie le pourcentage de dégradation des infrastructures
	 * entre 0 et 10%
	 * @return Le pourcentage de dégradation des infrastructures, entre 0 et 100
	 */
	public override int production()
	{
		/**
		 * Formule
		 * 
		 * Disons que seule 5% des inactifs dégradent les infrastructures
		 * Plus le contentement global de la population est gfaible, plus il y a de la casse
		 * Idem s'il n'y a pas assez de loisirs
		 * Un climat non clément incite les délinquant à rester chez eux (=> parti pris)
		 */

		float indiceLoisirs = population.categoriesPop.categories ["Loisirs"].besoins ();
		if (indiceLoisirs >= 0f)
			indiceLoisirs = Mathf.Atan (indiceLoisirs) / (2f * Mathf.PI);
		else
			indiceLoisirs = 0f;//Mathf.Atan ((indiceLoisirs) * 10) / (2f * Mathf.PI);

		float resultat = Utils.indicesNormalises(new float[,] {	{ population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE ), 0f,1f,0.6f,1f, 0f },
																	{ indiceLoisirs, 0f,1f,0.5f,1f, 0f },
																	{ 1f - population.country.indiceHI (), 0f,1f,0f,0.1f, 0f } // Jusqu'a 10% de dégradation totale
																});

		return (int)(resultat * 100f);
	}

	public override int offre(int montant, float pourcentage) {
		return 0;
	}

	public override int ideal () {
		return 0;
	}
}