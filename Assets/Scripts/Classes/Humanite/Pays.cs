﻿using System;
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
	 * Messages reçus
	 */
	private List<CarteDeVisite> messages;

	/**
	 * Liste de tous les pays
	 */
	public List<Pays> pays;

	/**
	 * Liste des pays non liés
	 */
	public List<Pays> paysNonLies;

	/**
	 * Liste des pays que l'on a appelé, et dont la réponse est en attente
	 */
	public List<Pays> paysEnAttente;

	/**
	 * Fonction appelée lorsque le pays s'active pour la première fois
	 * Initialisation du pays
	 */
	public void Awake ()
	{
		nomPays = gameObject.name;
		population = new Population(this);
		links = new Links (this);
		climat = new Climat (DonneePays.getChaleur(nomPays), DonneePays.getHumidite(nomPays));
		superficie = DonneePays.getSuperficie(nomPays);
		resources = new PaysRessources (this);
		souche = null;
		messages = new List<CarteDeVisite> ();
		paysNonLies = new List<Pays> ();
		paysEnAttente = new List<Pays> ();
		echangeSet = new EchangeSet ();
	}

	public void setPaysNonLies() {
		foreach (Pays p in pays) {
			paysNonLies.Add (p);
		}
	}

	/**
	 * Fonction appelée à chaque frame
	 * Fait vivre la population
	 */
	public void Update() {
		if (!Parametres.chooseMaritime)
			SelectionPays ();

		/**
		 * Chaque jour
		 * • Production des ressources
		 * • Consomation des ressources
		 */
		if (ClockManager.newDay) {
			population.categories.produce ();
			resources.consome ();
			population.reorganizePopulationCategories ();
			population.updateHappiness ();
			appelPays ();
			traiteMessages ();
			effectueEchange ();
		}
		if (ClockManager.newMonth) {
			appelPays ();
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
		}
		else if (Parametres.paysSelected && Parametres.paysSelected.name != name && mat.GetFloat ("_Metallic") == 0f) {
			mat.SetFloat ("_Metallic", 0.5f);
		}
		else if (!Parametres.paysSelected && mat.GetFloat ("_Metallic") != 0f) {
			mat.SetFloat ("_Metallic", 0f);
		}
			
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
		return indiceProduction ("Loisirs");
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
		float ratioInfecteSain = 1f - (float) population.nbInfectedDetected / (float) population.totalPopulation;
		Medecine medecineCategorie = ((Medecine)population.categories.categories ["Medecine"]);

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
		foreach ( int i in population.categories.categories[nomCate].productions ) {
			somme += i;
		}
		float moyenne = somme / population.categories.categories[nomCate].productions.Count;
		float ideal = (float) population.categories.categories [nomCate].ideal ();
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
		Medecine medecine = (Medecine) (population.categories.categories ["Medecine"]);
		return medecine.moyenneSoignes() / (1f + medecine.ideal ());
	}

	/**
	 * Indice de la recherche
	 * Renvoie une valeur entre 0 (mauvais) et 1 (bien)
	 */
	public float indiceRecherche() {
		Recherche recherche = (Recherche) (population.categories.categories ["Recherche"]);
		return Mathf.Min(1f, recherche.moyennePoints() / ( 1f + (float) recherche.ideal()));
	}

	/**
	 * Crée une nouvelle souche à associer au pays.
	 * 
	 * Cette méthode ne devrait être appelée que lors du démarrage de l'application,
	 * sur le pays qui démarrera avec la maladie
	 * @param nbInfected Nombre d'infectés arrivant avec cette souche
	 */
	public void createSouche(uint nbInfected)
	{
		this.souche = gameObject.AddComponent<Souche> ();
		this.souche.country = this;
		this.souche.setNbInfected(nbInfected);
	}

	/**
	 * Crée une nouvelle souche à associer au pays
	 * 
	 * Cette méthode est appelée lorsque le pays est sain et reçoit une population de malades
	 * @param souche Souche qui arrive avec les malades
	 * @param nbInfected Nombre d'infectés arrivant avec cette souche
	 */
	public void createSouche(Souche souche, uint nbInfected)
	{
		if (this.souche == null) {
			this.souche = gameObject.AddComponent<Souche> ();
			this.souche.country = this;

			// Récupération des capacités
			this.souche.skills.setWaterSpreading (souche.skills.getWaterSpreading ());
			this.souche.skills.setAirSpreading (souche.skills.getAirSpreading ());
			this.souche.skills.setContactSpreading (souche.skills.getContactSpreading ());
			this.souche.skills.setResistanceCold (souche.skills.getResistanceCold ());
			this.souche.skills.setResistanceHeat (souche.skills.getResistanceHeat ());

			// Copie des symptômes
			foreach (KeyValuePair<string, AbstractSymptom> pair in souche.symptoms) {
				this.souche.symptoms.Add (pair.Key, DonneeSouche.getSymptomFromName (pair.Key));
			}

			// Mise en place du nombre d'infectés
			this.souche.setNbInfected (nbInfected);
		}
		else {
			this.souche.fusion (souche, nbInfected);
		}
	}



	/**
	 * Crée un cargo (avion, camion ou bateau) avec les ressources à envoyer au pays voulu et la population migrante
	 */
	public void exchangeResources()
	{
		//foreach (Echa)
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
		return this.population.categories.categories [category].assignedPopulation / (float)this.population.totalPopulation;
	}

	/**
	 * Fournie une carte de visite, indiquant les ressources qu'il est prêt à échanger aux autres pays
	 */
	public CarteDeVisite getCarteDeVisite() {
		CarteDeVisite carte = new CarteDeVisite (this);
		Ressource offre;
		foreach (Ressource resource in resources.resources.Values) {
			offre = resource.offre ();
			if (offre == null)
				continue;
			carte.addRessource (offre);
		}

		Medecine medecine = (Medecine) (population.categories.categories ["Medecine"]);
		float ratio = medecine.moyenneSoignes() / medecine.moyenneNouveauxInfectes();
		// On ne communique notre efficacité que si le ratio est supérieur à 0.7
		if (ratio > 0.7f)
			carte.ratioSoigneInfecteDetecte = ratio;

		return carte;
	}

	/**
	 * Appel aux autres pays, pour commencer des échanges
	 */
	public void appelPays() {
		if (paysNonLies.Count == 0)
			return;
		
		
		float max = Mathf.NegativeInfinity;
		float tmp;
		APopulationCategory cateMax = null;
		foreach (APopulationCategory cate in population.categories.categories.Values) {
			tmp = cate.besoins ();
			if (max < tmp && cate.demande() != null) {
				max = tmp;
				cateMax = cate;
			}
			else if (max == tmp && cate.demande() != null && UnityEngine.Random.value < 0.3f ) {
				max = tmp;
				cateMax = cate;
			}
			else {
			}
		}

		if (max > Parametres.seuilDAppelALAide) {
			CarteDeVisite carte = getCarteDeVisite ();
			Ressource res = cateMax.demande ();
			//res.quantity = (uint)((float)res.quantity / paysNonLies.Count);
			if (res != null) {
				carte.addRessourceDemandee (res);
			}
			else {
				return;
			}

			sendMessagePaysNonLies (carte);
		}
	}

	/**
	 * Envoie un message à tous les pays susceptible de pouvoir apporter de l'aide
	 */
	public void sendMessagePaysNonLies (CarteDeVisite carte) {
		foreach (Pays pays in paysNonLies) {
			pays.addMessage (carte);
		}
	}

	/**
	 * Ajoute un message à ce pays (appellé depuis un autre pays)
	 */
	public void addMessage (CarteDeVisite carte) {
		messages.Add (carte);
	}

	/**
	 * Traite les messages des autres pays, voulant échanger des ressources
	 */
	public void traiteMessages () {
		if (messages.Count == 0)
			return;
		
		CarteDeVisite msg = messages [0];
		messages.RemoveAt (0);

		if (!paysNonLies.Contains (msg.pays))
			return;

		float besoin;

		APopulationCategory cate = null;
		Ressource finalRessource = null;
		Ressource finalRessourceSend = null;

		bool flag = false;

		foreach (Ressource res in msg.ressources.resources.Values) {
			besoin = Mathf.Infinity;
			cate = null;

			if (res.nom == "Nourriture")
				cate = population.categories ["Agriculture"];

			if (besoin == Mathf.Infinity && res.nom.Contains ("Knowledge"))
				cate = population.categories ["Recherche"];

			if (cate == null) {
				cate = population.categories [res.nom];
			}

			if (cate == null)
				continue;

			besoin = cate.besoins ();

			if (besoin == Mathf.Infinity)
				continue;
			
			if (besoin > 0.3f && res.quantity > 0.2f * (cate.ideal () - cate.moyenneProduction ())) {
				flag = true;
				finalRessource = res;
				break;
			}
		}

		foreach (Ressource res in msg.ressourcesDemandees.resources.Values) {
			if (res.nom == "Nourriture" && resources [res.nom].offre () != null && res.quantity <= 0.3f * resources [res.nom].offre ().quantity) {
				finalRessourceSend = resources [res.nom];
				break;
			}

			if (res.nom.Contains ("Knowledge")) {
				finalRessourceSend = resources [res.nom];
				if (UnityEngine.Random.value < 0.2f)
					break;
			}

			if (finalRessourceSend != null)
				break;
		}

		if (flag && finalRessourceSend != null) {
			string type;
			Link newLink;
			if (UnityEngine.Random.value < 0.5f) {
				newLink = links.addMaritime (msg.pays);
				msg.pays.links.addMaritime (this);
			}
			else {
				newLink = links.addAerien (msg.pays);
				msg.pays.links.addAerien (this);
			}

			if (newLink == null)
				return;

			Echange echange = new Echange ( finalRessourceSend.nom, finalRessourceSend.offre().quantity, finalRessource.nom, finalRessource.quantity, newLink);
			createLink (this, msg.pays, echange, true);
		}
	}

	/**
	 * Création de lien entre deux pays
	 * @param paysOne Ce pays ci
	 * @param paysTwo Le pays avec qui on crée le lien
	 * @param echange Le contract d'échange entre les deux pays
	 * @param flag Permet de faire faire la même opération à l'autre pays. Mettre à true
	 */
	public void createLink(Pays paysOne, Pays paysTwo, Echange echange, bool flag) {
		if (!echangeSet.echanges.Keys.Contains (paysTwo))
			echangeSet.echanges [paysTwo] = new Dictionary<string, Echange> ();
		echangeSet.echanges[paysTwo][echange.ressourceRecu] = echange;
		paysNonLies.Remove (paysTwo);
		if (flag)
			paysTwo.createLink (paysTwo, paysOne, echange, false);
	}

	/**
	 * Effectue les échanges définis par les contracts qu'à ce pays
	 */
	public void effectueEchange() {
		echangeSet.effectueEchange ();
	}
}