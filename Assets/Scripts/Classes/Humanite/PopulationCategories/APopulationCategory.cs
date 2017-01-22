using UnityEngine;

public abstract class APopulationCategory
{
	/**
	 * Nom de la catégorie
	 */
	public string nom;

	/**
	 * La population à laquelle est ratachée
	 * cette catégorie
	 */
	public Population population;

	/**
	 * Taille de la population assignée à cette catégorie, en nombre d'habitants
	 */
	public uint assignedPopulation;

	/**
	 *
	 */
	public LimitedQueue<int> productions;

	/**
	 * Pourcentage de population que le joueur souhaiterait voir affectée à cette catégorie
	 */
	public float wantedPercentage = -1f;

	/**
	 * Nombre de tour que l'on réajuste les catégories depuis que le joueur l'a demandé
	 */
	public int stepReajusteForPlayer = 0;

	/**
	 * 
	 */
	protected GameObject panelItem;

	/**
	 * Valeur maximale que l'offre peut atteindre
	 * (cf. la méthode offre)
	 */
	private static float MAX_OFFRE = 100;

	/**
	 * Valeur minimale que l'offre peut atteindre
	 * (cf. la méthode offre)
	 */
	private static float MIN_OFFRE = -100;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public APopulationCategory(Population pop, uint nb) {
		population = pop;
		assignedPopulation = nb;
		productions = new LimitedQueue<int> (100);
	}

	/**
	 * Ajoute des personnes à cette catégorie
	 * @param nb Nombre de personnes à ajouter à cette cotégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement ajoutées à la catégorie
	 */
	public uint addAssigned(uint nb) {
		if (nb < 0)
			return 0;
		assignedPopulation += nb;
		return nb;
	}

	/**
	 * Ajoute des personnes à cette catégorie
	 * @param nb Nombre de personnes à ajouter à cette cotégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement ajoutées à la catégorie
	 */
	public uint addAssigned(int nb) {
		if (nb < 0)
			return (uint) 0;
		assignedPopulation += (uint) nb;
		return (uint) nb;
	}

	/**
	 * Enlève des personnes de cette catégorie
	 * @param nb Nombre de personnes à retirer de cette catégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement retirées de la catégorie
	 */
	public uint removeAssigned(uint nb) {
		if (nb < 0)
			return 0;
		if (nb > assignedPopulation) {
			uint temp = assignedPopulation;
			assignedPopulation = 0;
			return temp;
		}
		assignedPopulation -= nb;
		return nb;
	}

	/**
	 * Enlève des personnes de cette catégorie
	 * @param nb Nombre de personnes à retirer de cette catégorie. Doit être positif, sinon rien n'est fait
	 * @return Le nombre de personnes réellement retirées de la catégorie
	 */
	public uint removeAssigned(int nb_int) {
		uint nb = (uint)nb_int;
		if (nb < 0)
			return 0;
		if (nb > assignedPopulation) {
			uint temp = assignedPopulation;
			assignedPopulation = 0;
			return temp;
		}
		assignedPopulation -= nb;
		return nb;
	}

	/**
	 * Production de "ressources", en fonction de ce qu'apporte la catégorie
	 */
	public abstract void produce ();

	/**
	 * Estimation de la production de "ressources"
	 */
	public abstract int production ();

	/**
	 * Production idéale
	 */
	public abstract int ideal ();

	/**
	 * besoins
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public abstract float besoins();

	/**
	 * Evalue l'offre que le pays proposerait à un autre
	 * @param montant Montant de ressources par mois demandé
	 * @param pourcentage Pourcentage d'excédant de production que l'on est prêt à offrir
	 * @return Indique combien l'on est prêt à fournir par mois à un autre pays
	 */
	public abstract int offre(int montant, float pourcentage);

	public bool modeAuto() {
		if (wantedPercentage == -1f ||
		 	population.country.indiceHI () < 0.3 ||
		   	(stepReajusteForPlayer > 10 && besoins () > 0.6))
		{
			stepReajusteForPlayer = -1;
			wantedPercentage = -1f;
			return true;
		}
		return false;
	}

	public void createGraphiqueProduction (Texture2D texture) {
		Utils.createGraphique (texture, productions);
		/*UnityEngine.Color backgroundColor = new UnityEngine.Color (30f/255f,30f/255f,30f/255f,200f/255f);
		int i = 0;
		int max = getMaxProduction ();
		foreach (int nb in productions) {
			for (int j = 0; j < texture.height; j++) {
				texture.SetPixel (i, j, backgroundColor);
			}

			float nb_norma = (float) nb;
			nb_norma /= max;
			nb_norma *= texture.height;

			Debug.Log (nb_norma + " " + nb);

				
			//texture.SetPixel (i, Mathf.Max((int)nb_norma - 1, 0), Color.white);
			texture.SetPixel (i, (int)nb_norma, Color.white);

			i++;
		}
		for ( i = productions.Count ; i < texture.width ; i++ ) {
			for ( int j = 0 ; j < texture.height ; j ++) {
				texture.SetPixel (i, j, backgroundColor);
			}
		}

		texture.Apply ();*/
	}

	public void createGraphiqueConsommation(Texture2D texture) {
		if (nom == "Agriculture")
			Utils.createGraphique (texture, population.country.resources ["Nourriture"].consommation);
		else if (nom == "Loisirs")
			Utils.createGraphique (texture, population.country.resources ["RessourceLoisirs"].consommation);
		else if (nom == "Transports")
			Utils.createGraphique (texture, population.country.resources ["Transports"].consommation);
	}
		
	/**
	 * @return La production journalière maximale
	 */
	public int getMaxProduction() {
		int max = productions.Peek();
		foreach (int t in productions) {
			if (t > max)
				max = t;
		}
		return max;
	}
		
	/**
	 * @return La production journalière minimale
	 */
	public int getMinProduction() {
		int min = productions.Peek();
		foreach (int t in productions) {
			if (t < min)
				min = t;
		}
		return min;
	}

}

