using System;
using UnityEngine;
using System.Collections.Generic;

public class Pays : MonoBehaviour
{
	/**
	 * Nom du pays
	 */
	public string nomPays { get; set;}
	/**
	 * Superficie du pays, en km²
	 */
	public float superficie { get; set;}
	/**
	 * Intérêt que le pays à aux yeux de tous.
	 * Monuments, avancées, qualité de vie en sont quelques facteurs
	 */
	public float interet { get; set;}
	/**
	 * Richesse du pays
	 */
	public float richesse { get; set;}
	/**
	 * Nombre de points de recherche accumulés jusqu'à présent par les chercheurs
	 */
	private int pointsRecherche { get; set;}
	/**
	 * Ensemble des Ressource du pays, accessibles par leur nom
	 */
	private IDictionary<string, Ressource> resources { get; set;}
	/**
	 * La population associée à ce pays
	 */
	private Population population { get; set;}
	/**
	 * Liens que ce pays entretient avec d'autres
	 */
	private Links links { get; set;}
	/**
	 * Climat du pays
	 */
	public Climat climat { get; set;}
	/**
	 * Souche infectant le pays
	 */
	public Souche souche { get; set;}

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
}

