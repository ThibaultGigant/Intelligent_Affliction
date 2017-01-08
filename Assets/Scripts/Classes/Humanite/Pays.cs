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
	private int pointsRecherche;
	/**
	 * Ensemble des Ressource du pays, accessibles par leur nom
	 */
	public IDictionary<string, Ressource> resources;
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

		SetupRessources ();
	}

	/**
	 * Initialisation des ressources du pays
	 */
	private void SetupRessources() {
		resources = new Dictionary<string, Ressource> ();
		resources.Add ("Nourriture", new Nourriture ());
		resources.Add ("KnowledgeToux", new Knowledge ("Toux", 1));
		resources.Add ("KnowledgeEternuements", new Knowledge ("Eternuements", 1));
		resources.Add ("KnowledgeFievre", new Knowledge ("Fievre", 1));
		resources.Add ("KnowledgeDiarrhees", new Knowledge ("Diarrhees", 1));
		resources.Add ("KnowledgeSueurs", new Knowledge ("Sueurs", 1));
		resources.Add ("KnowledgeArretOrganes", new Knowledge ("ArretOrganes", 1));
		resources.Add ("KnowledgeResistance", new Knowledge ("Resistance", 1));
		resources.Add ("KnowledgeSpreading", new Knowledge ("Spreading", 1));
		resources.Add ("Transports", new RessourceTransports ());
		resources.Add ("Loisirs", new RessourceLoisirs ());
	}

	/**
	 * Fonction appelée à chaque frame
	 * Fait vivre la population
	 */
	public void Update() {
		//TestFunctions ();

		SelectionPays ();
		checkSelection ();

		if (ClockManager.newDay) {
			population.categoriesPop.produce ();
			//population.categories ["Transports"].produce ();
			//population.categories ["Agriculture"].produce ();
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
			GameObject.Find ("GameManager/Menus/Menu Gauche/Principale/Nom Pays").GetComponent<Text> ().text = gameObject.name;
			Debug.Log (name);
			isSelected = true;
		}
		// Déselection du pays, par un simple clique en dehors de la zone
		else if (isSelected && MouseManager.simpleLeftClick && !MouseManager.doesHit (gameObject)) {
			Parametres.SetPaysSelected(null);
			GameObject.Find ("GameManager/Menus/MenuGauche/Principale/Nom Pays").GetComponent<Text> ().text = "Monde";
			isSelected = false;
		}
		// Déselection du pays, si un autre à été sélectionné
		else if (Parametres.paysSelected != null && Parametres.paysSelected.name != name) {
			isSelected = false;
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
	 * @return Une valeur entre 0.75 et 1. 0.75 si les conditions sont catastrophiques,
	 * un s'ils sont parfaits
	 // L'idéal étant un pays avec une chaleur de 65
	 // et une humidité de 45 (valeure arbitraire, à revoir)
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
		 * On ramène le domaine de l'image de [0,1] à [0.75, 1]
		 */
		Climat clim = population.country.climat;
		float ind = (float)(Mathf.Abs (CHALEUR_IDEALE - clim.chaleur) +
			Mathf.Abs (HUMIDITE_IDEALE - clim.humidite)) / 100f;
		ind = Mathf.Pow (ind, 2f) / 4f - ind + 1;
		return (1f - ind) / 4f + 0.75f;
	}

	/**
	 * indiceTransportSuperficie
	 * @return Une valeur entre 0,75 et 1. Zéro s'il y a aucun transport, un s'il y en a beaucoup
	 */
	public float indiceTransports()
	{
		/**
		 * Formule
		 * • La "quantité" de transport par rapport à la superficie totale.
		 * (100 * [quantité de Transport]) / (5 * [Superficie])
		 * En France, la longueur du réseau férroviaire correspond à 5% de la
		 * superficie du pays (en comparant seulement les valeurs, sans se soucier
		 * de leurs unités de mesure). On monte la valeur à 8% arbitrairement
		 * pour prendre en compte les bus et taxis, etc.
		 * 
		 * Le résultat est bornée supérieurement par 1.
		 * On ramène l'image de la fonction de [0,1] sur [0.75,1]
		 */
		int quantityTransport = population.country.resources ["Transports"].quantity;
		float ind =  (100f * (float) quantityTransport) / (12f * 160f * Mathf.Sqrt(population.country.superficie));

		return (Mathf.Min (1f, ind) / 4f) + 0.75f;
	}

	/**
	 * indiceHI
	 * Renvoie un facteur prenant en considération le HappinessIndex du pays
	 * Formule : la même que pour indiceClimat
	 * HI² / 4 - HI + 1, où HI = HappinessIndex / 50
	 * Ainsi, pour la valeur maximale 100, on obtient 0
	 * pour la moitié 50, on obtient 1/4
	 * et pour la valeur minimale 0, on obtient 1
	 * Ainsi, pour HI = 100, la valeur vaudra moins de 0.1 pourcent
	 * On ramène le domaine de l'image de [0,1] à [0.25, 1]
	 * @return Un facteur, entre 0.75 et 1, prenant en considération le HappinessIndex du pays, utile pour certains calculs (rebellions, besoins, ...)
	 */
	public float indiceHI() {
		float HI_normalize = (float)population.getHappinessIndex () / 50f;
		float ind = Mathf.Pow (HI_normalize, 2f) / 4f - HI_normalize + 1;
		return (3f * ind / 4f) + 0.25f;
 	}

	/**
	 * Crée un cargo (avion, camion ou bateau) avec les ressources à envoyer au pays voulu et la population migrante
	 */
	public void exchangeResources()
	{
		
	}

	public void applyPlayerOrders()
	{
		this.population.reorganizePopulationCategories ();
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
}

