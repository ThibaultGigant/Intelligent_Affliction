using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Agriculture : APopulationCategory
{
	/**
	 * La chaleure idéale du pays pour une bonne agriculture
	 */
	private int CHALEUR_IDEALE = 65;
	/**
	 * L'humidité idéale du pays pour une bonne agriculture
	 */
	private int HUMIDITE_IDEALE = 45;

	/**
	 * Constructeur
	 * @param population La population à laquelle est ratachée
	 * @param nb Taille de la population initialement assignée à cette catégorie, en nombre d'habitants
	 */
	public Agriculture(Population pop, uint nb) : base(pop, nb) {
		//panelItem = GameObject.Find("Menu Gauche/Catégories/Agriculture");
		//panelItem.GetComponent<Image> ().color = new Color32 (255, 133, 133, 255);
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

		float newAgr = production ();

		productions.Enqueue((int)newAgr);

		population.country.resources ["Nourriture"].addRessource (Mathf.FloorToInt(newAgr));
	}

	/**
	 * Estimation de la production de Nourriture chaque jour
	 */
	public override int production() {
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
		float newAgr = 10f * assignedPopulation * population.country.indiceClimat (CHALEUR_IDEALE, HUMIDITE_IDEALE) * population.country.indiceTransports();

		//Debug.Log (population);

		if (population.country.nomPays == "Afrique") {
			/*Debug.Log ("Agriculture");
			Debug.Log ("ap " + assignedPopulation);
			Debug.Log ("clim " + population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE));
			Debug.Log ("sup " + population.country.indiceTransports());*/
			//Debug.Log (newAgr);
		}

		return Mathf.FloorToInt (newAgr);
	}

	/**
	 * besoins
	 * Indique les besoins de la catégorie
	 * La valeur est d'autant plus élevée que la catégorie
	 * à besoin d'effectif supplémentaire, et inversement
	 * (des normalisations sont nécessaires)
	 * @return Une valeur indiquant ses besoins en effectif, entre MIN_OFFRE et MAX_OFFRE
	 */
	public override float besoins()
	{
		// TODO prendre en compte les stocks

		//assignedPopulation
		// e^-HI
		// Transport
		Pays country = population.country;
		int HI = population.getHappinessIndex();
		int quantityTransport = country.resources ["Transports"].quantity;
		int quantityNourriture = country.resources ["Nourriture"].quantity;
		float superficie = country.superficie;
		Climat clim = country.climat;

		//Debug.Log (quantityNourriture);

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
			indiceNourriture = Mathf.Atan (indiceNourriture - 1) / (2f * Mathf.PI);
		else
			indiceNourriture = Mathf.Atan ((indiceNourriture - 1) * 10) / (2f * Mathf.PI);

		/*Debug.Log ("HI = " + indiceHI);
		Debug.Log ("Climat = " + population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE));
		Debug.Log ("Nourriture = " + indiceNourriture);
		Debug.Log ("Transport = " + population.country.indiceTransports());*/

		return population.country.indiceHI() * indiceNourriture * population.country.indiceTransports() * population.country.indiceClimat(CHALEUR_IDEALE, HUMIDITE_IDEALE);
	}

	/**
	 * Evalue l'offre que le pays proposerait à un autre, en fonction du montant demandé
	 * et du pourcentage donné, calculé auparavant  en fonction de l'apport que pourrait
	 * rapporter l'échange avec lui
	 * @param montant Montant de Nourriture par mois demandé
	 * @param pourcentage Pourcentage d'excédant de production que l'on est prêt à offrir
	 * @return Montant de Nourriture par mois proposé
	 */
	public override int offre(int montant, float pourcentage) {

		// Calcul de l'excedent moyen de nourriture produite par jour
		int sum = 0;
		int[] prod = productions.ToArray ();
		for ( int i = 0 ; i < prod.Length ; i++ ) {
				sum += prod[i];
		}
		float moyenne = sum / (1.0f * prod.Length);

		float excedant = moyenne - population.country.resources["Nourriture"].consome(false);

		/**
		 * Formule
		 * 
		 * Si l'excédant est supérieur
		 * à 10% de la population de ce pays
		 * 
		 * Propose le minimum entre le pourcentage prêt à donner de l'excédant moyen
		 * et le montant demandé par l'autre pays
		 * 
		 * min ( pourcentage * [excédant moyen] , montant )
		 */
		if (sum >= 0.1 * population.totalPopulation) {
			int excedantMoyen = (int) (excedant * pourcentage);
			return excedantMoyen < montant ? excedantMoyen : montant ;
		}

		return 0;
	}
}