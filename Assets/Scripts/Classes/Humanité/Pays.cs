using System;
using UnityEngine;

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
	 * Intérêt que le pays à au yeux de tous.
	 * Monuments, avancées, qualité de vie en sont quelques facteurs
	 */
	public float intérêt;
	/**
	 * Richesse du pays
	 */
	public float richesse;
	/**
	 * Taille initiale de la population, en nombre d'habitants
	 */
	public int initialNumberPopulation;
	/**
	 * La population assicié à ce pays
	 */
	private Population population;
	/**
	 * Liens que ce pays entretient avec d'autres
	 */
	private Links links;
	/**
	 * Ressources : Nourriture
	 */
	private Ressource nourriture;
	/**
	 * Ressources : KnowledgeToux
	 */
	private Ressource knowledgeToux;
	/**
	 * Ressources : KnowledgeEternuements
	 */
	private Ressource knowledgeEternuements;
	/**
	 * Ressources : KnowledgeFievre
	 */
	private Ressource knowledgeFievre;
	/**
	 * Ressources : KnowledgeDiarrhees
	 */
	private Ressource knowledgeDiarrhees;
	/**
	 * Ressources : KnowledgeSueurs
	 */
	private Ressource knowledgeSueurs;
	/**
	 * Ressources : KnowledgeArretOrganes
	 */
	private Ressource knowledgeArretOrganes;
	/**
	 * Ressources : KnowledgeResistance
	 */
	private Ressource knowledgeResistance;
	/**
	 * Ressources : KnowledgeSpreading
	 */
	private Ressource knowledgeSpreading;
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
	 * Light
	 */
	private GameObject light = null;

	/**
	 * Fonction appelée lorsque le pays s'active pour la première fois
	 * Initialisation du pays
	 */
	public void Start ()
	{
		nomPays = gameObject.name;
		population = new Population(this, initialNumberPopulation);
		links = new Links ();

		SetupRessources ();

		foreach (Transform t in transform) {
			if (t.gameObject.layer == LayerMask.NameToLayer("Light") )
				light = t.gameObject;
		}
	}

	/**
	 * Initialisation des ressources du pays
	 */
	private void SetupRessources() {
		nourriture = new Nourriture ();
		knowledgeToux = new KnowledgeToux ();
		knowledgeEternuements = new KnowledgeEternuments ();
		knowledgeFievre = new KnowledgeFievre ();
		knowledgeDiarrhees = new KnowledgeEternuments ();
		knowledgeSueurs = new KnowledgeSueurs ();
		knowledgeArretOrganes = new KnowledgeArretOrganes ();
		knowledgeResistance = new KnowledgeResistance ();
		knowledgeSpreading = new KnowledgeSpreading ();
	}

	/**
	 * Fonction appelée à chaque frame
	 * Fait vivre la population
	 */
	public void Update() {
		TestFunctions ();

		SelectionPays ();
		checkSelection ();

	}

	private void SelectionPays() {
		RaycastHit hit = MouseManager.getHit ();
		// Sélection du pays
		if (!isSelected && MouseManager.doubleLeftClick && MouseManager.doesHit(gameObject)) {
			Parametres.SetPaysSelected(gameObject);
			Debug.Log (name);
			isSelected = true;
		}
		// Déselection du pays, par un simple clique en dehors de la zone
		else if (isSelected && MouseManager.simpleLeftClick && !MouseManager.doesHit (gameObject)) {
			Parametres.SetPaysSelected(null);
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
			if (light)
				light.SetActive (true);
		}
		else if (Parametres.paysSelected && Parametres.paysSelected.name != name && mat.GetFloat ("_Metallic") == 0f) {
			mat.SetFloat ("_Metallic", 0.5f);
			if (light)
				light.SetActive (false);
		}
		else if (!Parametres.paysSelected && mat.GetFloat ("_Metallic") != 0f) {
			mat.SetFloat ("_Metallic", 0f);
			if (light)
				light.SetActive (false);
		}
		else if (Parametres.paysSelected && Parametres.paysSelected.name == name && light && !light.activeSelf) {
			light.SetActive (true);
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
	}

	/**
	 * Ajout d'un lien maritime
	 * @param pays Le pays avec lequel le lien doit être créé
	 */
	public void addMaritimeLink(Pays pays) {
	}

	/**
	 * Ajout d'un lien aérien
	 * @param pays Le pays avec lequel le lien doit être créé
	 */
	public void addAirLink(Pays pays) {
	}

	/**
	 * Suppression d'un lien terrestre
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeTerrestrialLink(Pays pays) {
	}

	/**
	 * Suppression d'un lien maritime
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeMaritimeLink(Pays pays) {
	}

	/**
	 * Suppression d'un lien aérien
	 * @param pays Le pays avec lequel le lien doit être supprimé
	 */
	public void removeAirLink(Pays pays) {
	}

	/**
	 * Echange de ressources entre pays
	 * @param pays Le pays avec lequel l'échange se fait
	 * @param ressourceOut Les ressources données à l'autre pays
	 * @param ressourceIn Les ressources reçues par l'autre pays
	 */
	public void exchangeRessource(Pays pays, Ressource ressourceOut, Ressource ressourceIn) {
	}
}

