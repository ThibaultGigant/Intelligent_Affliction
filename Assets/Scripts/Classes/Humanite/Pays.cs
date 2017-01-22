using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pays : MonoBehaviour
{
	/**
	 * Nom du pays
	 */
	public string nomPays;
	/**
	 * Superficie du pays, en km²
	 */
	public float superficie;
	/**
	 * Intérêt que le pays à aux yeux de tous.
	 * Monuments, avancées, qualité de vie en sont quelques facteurs
	 */
	public float interet;
	/**
	 * Richesse du pays
	 */
	public float richesse;
	/**
	 * Nombre de points de recherche accumulés jusqu'à présent par les chercheurs
	 */
	public int pointsRecherche;
	/**
	 * Ensemble des Ressource du pays, accessibles par leur nom
	 */
	//public IDictionary<string, Ressource> resources;
	public PaysRessources resources;
	/**
	 * La population associée à ce pays
	 */
	public Population population;
	/**
	 * Liens que ce pays entretient avec d'autres
	 */
	private Links links;
	/**
	 * Climat du pays
	 */
	public Climat climat;
	/**
	 * Souche infectant le pays
	 */
	public Souche souche;

	/**
	 * Indique si le pays est sélectionné
	 */
	public bool isSelected = false;

	/**
	 * Ensemble des échanges qu'entretient ce pays
	 */
	public EchangeSet echangeSet;

	/**
	 * Fonction appelée lorsque le pays s'active pour la première fois
	 * Initialisation du pays
	 */
	public void Start ()
	{
		nomPays = gameObject.name;
		population = new Population(this);
		links = new Links ();
		climat = new Climat (DonneePays.getChaleur(nomPays), DonneePays.getHumidite(nomPays));
		superficie = DonneePays.getSuperficie(nomPays);
		resources = new PaysRessources (this);
	}

	/**
	 * Fonction appelée à chaque frame
	 * Fait vivre la population
	 */
	public void Update() {
		SelectionPays ();

		/**
		 * Chaque jour
		 * • Production des ressources
		 * • Consomation des ressources
		 */
		if (ClockManager.newDay) {
			population.categoriesPop.produce ();
			resources.consome ();
			population.reorganizePopulationCategories ();
			population.updateHappiness ();
		}

		/*
		exchangeResources ();
		applyPlayerOrders ();
		// TODO : modification des liens
		//
		*/

	}

	private void SelectionPays() {
		// Sélection du pays
		if (!isSelected && MouseManager.doubleLeftClick && MouseManager.doesHit(gameObject)) {
			Parametres.SetPaysSelected(gameObject);
			GameObject.Find ("/GameManager/Menus/Menu Gauche/Principale/Nom Pays").GetComponent<Text> ().text = gameObject.name;
			isSelected = true;
			checkSelection ();
		}
		// Déselection du pays, par un simple clique sur le mini world
		else if (isSelected && MouseManager.simpleLeftClick && !MouseManager.doesHit (gameObject) && MouseManager.doesHit(Parametres.miniEarth)) {
			Parametres.SetPaysSelected(null);
			GameObject.Find ("/GameManager/Menus/Menu Gauche/Principale/Nom Pays").GetComponent<Text> ().text = "Monde";
			isSelected = false;
			checkSelection();
		}
		// Déselection du pays, si un autre à été sélectionné
		else if (Parametres.paysSelected != null && Parametres.paysSelected.name != name) {
			isSelected = false;
			checkSelection();
		}
	}

	private void checkSelection() {
		Material mat = GetComponent<Renderer> ().material;

		if (Parametres.paysSelected && Parametres.paysSelected.name == name && mat.GetFloat ("_Metallic") != 0f) {
			mat.SetFloat ("_Metallic", 0f);
//			if (light)
//				light.SetActive (true);
		}
		else if (Parametres.paysSelected && Parametres.paysSelected.name != name && mat.GetFloat ("_Metallic") == 0f) {
			mat.SetFloat ("_Metallic", 0.5f);
//			if (light)
//				light.SetActive (false);
		}
		else if (!Parametres.paysSelected && mat.GetFloat ("_Metallic") != 0f) {
			mat.SetFloat ("_Metallic", 0f);
//			if (light)
//				light.SetActive (false);
		}
//		else if (Parametres.paysSelected && Parametres.paysSelected.name == name && light && !light.activeSelf) {
//			light.SetActive (true);
//		}
			
	}

	/**
	 * Fonction temporaire, qui nous servira de test
	 */
	private void TestFunctions() {
		if (ClockManager.newDay) {
			population.naissances ();
			population.deces ();
		}
	}

	/**
	 * Met à jour le happinessIndex du pays
	 */
	public void updateHappinessIndex() {
	}

	/**
	 * Gère les migrations de la population
	 */
	public void makeMigrations() {
	}

	/**
	 * Ajout d'un lien terrestre
	 * @param pays Le pays avec lequel le lien doit être créé
	 */
	public void addTerrestrialLink(Pays pays) {
		links.addTerrestre (pays);
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Le pays avec lequel le lien doit être créé
	 */
	public void addMaritimeLink(Pays pays) {
		links.addMaritime (pays);
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Le pays avec lequel le lien doit être créé
	 */
	public void addAirLink(Pays pays) {
		links.addAerien (pays);
	}

	/**
	 * Suppression d'un lien terrestre
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeTerrestrialLink(Pays pays) {
		links.removeTerrestre (pays);
	}

	/**
	 * Suppression d'un lien maritime
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeMaritimeLink(Pays pays) {
		links.removeMaritime (pays);
	}

	/**
	 * Suppression d'un lien aérien
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeAirLink(Pays pays) {
		links.removeAerien (pays);
	}

	/**
	 * Echange de ressources entre pays
	 * @param pays Le pays avec lequel l'échange se fait
	 * @param ressourceOut Les ressources données à l'autre pays
	 * @param ressourceIn Les ressources reçues par l'autre pays
	 */
	public void exchangeRessource(Pays pays, Ressource ressourceOut, Ressource ressourceIn) {
	}

	/**
	 * Retourne le nombre d'habitants actuel du pays
	 * @return le nombre d'habitants actuel du pays
	 */
	public uint getNbPopulation()
	{
		if (population == null)
			return 0;
		return population.totalPopulation;
	}

	/**
	 * Retourne le nombre d'habitants initial du pays
	 * @return le nombre d'habitants initial du pays
	 */
	public uint getInitialNbPopulation()
	{
		return population.initialNumberPopulation;
	}

	/**
	 * indiceClimat
	 * Donne une indication quant à la condition climatique du pays
	 * Plus la chaleur et l'humidité d'un pays est proche des valeurs
	 * idéale, plus la valeur retournée sera grande
	 * @return Une valeur entre 0 et 1. 0 si les conditions sont catastrophiques,
	 * 1 s'ils sont parfaits
	 */
	public float indiceClimat(int CHALEUR_IDEALE, int HUMIDITE_IDEALE)
	{
		/**
		 * Formule :
		 * x² / 4 - x + 1,
		 * où x = (|CHAULEUR_IDEALE - chaleur| + |HUMIDITE_IDEALE - humidite|) / 100
		 * et x appartient à [0, 2]
		 * Ainsi, en 0, le facteur atteind son maximum, 1
		 * en 1, le facteur atteind 1/4
		 * en 2, le facteur atteind 0
		 * On "inverse" le résultat x -> 1 - x
		 */
		Climat clim = population.country.climat;
		float ind = (float)(Mathf.Abs (CHALEUR_IDEALE - clim.chaleur) +
			Mathf.Abs (HUMIDITE_IDEALE - clim.humidite)) / 100f;
		ind = Mathf.Pow (ind, 2f) / 4f - ind + 1;
		return (1f - ind);
	}

	/**
	 * indiceTransportSuperficie
	 * @return Une valeur entre 0 et 1. Zéro s'il y a aucun transport, un s'il y en a beaucoup
	 */
	public float indiceTransports()
	{
		return indiceProduction ("Transports");
	}

	/**
	 * indiceTransportSuperficie
	 * @return Une valeur entre 0 et 1. Zéro s'il y a aucun transport, un s'il y en a beaucoup
	 */
	public float indiceLoisirs()
	{
		/**
		 * Formule
		 * • La "quantité" de loisir
		 */
		uint quantityTransport = resources ["Transports"].quantity;
		float ind =  (100f * (float) quantityTransport) / (12f * 160f * Mathf.Sqrt(population.country.superficie));

		return (Mathf.Min (1f, ind));
	}

	/**
	 * indiceTransportSuperficie
	 * @return Une valeur entre 0 et 1. Zéro s'il y a aucun transport, un s'il y en a beaucoup
	 */
	public float indiceNourriture()
	{
		return indiceProduction ("Agriculture");
	}

	public float indiceInfection () {
		/**
		 * Formule
		 * • Ratio non infecté, sain
		 * • Ratio entre la moyenne de soignés, et le nombre de nouveaux infectés par jour
		 * • On borne le produit des deux ratio à 1
		 */
		float ratioInfecteSain = 1f - population.nbInfectedDetected / population.totalPopulation;
		Medecine medecineCategorie = ((Medecine)population.categoriesPop.categories ["Medecine"]);

		float moy1 = medecineCategorie.moyenneSoignes ();
		float moy2 = medecineCategorie.moyenneNouveauxInfectes ();

		float ratioNouveauxSoignes;
		if (moy2 != 0)
			ratioNouveauxSoignes = moy1 / moy2;
		else
			ratioNouveauxSoignes = 1f;

		return Mathf.Min(ratioNouveauxSoignes * ratioInfecteSain, 1f);
	}

	public float indiceProduction(string nomCate) {
		/**
		 * Formule
		 * • On calcul la moyenne de production de [nom], que l'on compare à
		 * la consommation journalière de la population
		 * • On borne le ratio par 2
		 * • On ramène le domaine de définition à [0,1]
		 */
		float somme = 0;
		//for ( int i = 0 ; i < population.categoriesPop.categories[nomCate].productions.Count ; i++ ) {
		foreach ( int i in population.categoriesPop.categories[nomCate].productions ) {
			somme += i;
		}
		float moyenne = somme / population.categoriesPop.categories[nomCate].productions.Count;
		float ideal = (float) population.categoriesPop.categories [nomCate].ideal ();
		float indice = moyenne / ( ideal != 0 ? ideal : 1f);

		indice = Mathf.Min (indice, 2f);

		return indice / 2f;
	}

	/**
	 * indiceHI
	 * Renvoie un facteur prenant en considération le HappinessIndex du pays
	 * Formule : la même que pour indiceClimat
	 * HI² / 4 - HI + 1, où HI = HappinessIndex / 50
	 * Ainsi, pour la valeur maximale 100, on obtient 0
	 * pour la moitié 50, on obtient 1/4
	 * et pour la valeur minimale 0, on obtient 1
	 * On inverse le resultat, de façon à obtenir 1 quand HI est a 100
	 * @return Un facteur, entre 0 et 1, prenant en considération le HappinessIndex du pays, utile pour certains calculs (rebellions, besoins, ...)
	 */
	public float indiceHI() {
		float HI_normalize = (float)population.getHappinessIndex () / 50f;
		float ind = Mathf.Pow (HI_normalize, 2f) / 4f - HI_normalize + 1;
		return (1f - ind);
	}

	/**
	 * Indice de la médecine
	 * Renvoie une valeur entre 0 (mauvais) et 1 (bien)
	 */
	public float indiceMedecine() {
		Medecine medecine = (Medecine) (population.categoriesPop.categories ["Medecine"]);
		return medecine.moyenneSoignes() / medecine.ideal ();
	}

	/**
	 * Indice de la recherche
	 * Renvoie une valeur entre 0 (mauvais) et 1 (bien)
	 */
	public float indiceRecherche() {
		Recherche recherche = (Recherche) (population.categoriesPop.categories ["Recherche"]);
		return recherche.moyennePoints() / recherche.ideal();
	}



	/**
	 * Crée un cargo (avion, camion ou bateau) avec les ressources à envoyer au pays voulu et la population migrante
	 */
	public void exchangeResources()
	{
		
	}

	public void applyPlayerOrders()
	{
		// TODO : modification des liens
	}

	public void addPeople(uint nbPeople)
	{
		this.population.addPeople (nbPeople);
	}

	public void removePeople(uint nbPeople)
	{
		this.population.removePeople (nbPeople);
	}

	public void addInfectedPeople(uint nbPeople)
	{
		this.souche.addInfectedPeople (nbPeople);
	}

	public void removeInfectedPeople(uint nbPeople)
	{
		this.souche.removeInfectedPeople (nbPeople);
	}

	/**
	 * Retourne le pourcentage de personnes assignée à la catégorie passée en paramètre
	 * @param category Catégorie dont on veut le pourcentage d'assignés
	 * @return Pourcentage voulu
	 */
	public float getPourcentageCategory(string category)
	{
		return this.population.categoriesPop.categories [category].assignedPopulation / (float)this.population.totalPopulation;
	}
}

