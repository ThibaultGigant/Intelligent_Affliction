using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Souche {
	/**
	 * Pays que la souche a infecté
	 */
	public Pays country;
	/**
	 * Nombre de personnes infectées par cette souche dans ce pays
	 */
	private uint nbInfected = 0;

	/**
	 * Points qui serviront à évoluer
	 */
	private int evolutionPoints = 0;

	/**
	 * Facteur vitesse d'évolution de la souche
	 * Influe sur le nombre de points gagnés lors de la production
	 */
	private float evolutionSpeed = 1;

	/**
	 * Historique des évolutions de la souche, pour le calcul des motivations
	 */
	private HistoriqueSouche historique;



	/*
		Les attributs suivants représentent en réalité des pourcentages :
		ce seront des entiers de 0 à 100 (pourcentage d'acquisition de la capacité)
	*/
	/**
	* Ensemble des symptomes de la souche
	*/
	public IDictionary<string, AbstractSymptom> symptoms;
	/**
	 * Ensemble des skills de la souche
	 */
	public Skills skills;

	/**
	 * Constructeur
	 */
	public Souche(Pays pays) {
		country = pays;
		symptoms = new Dictionary<string, AbstractSymptom> ();
		skills = new Skills (this);
		historique = new HistoriqueSouche ();
	}

	/**
	 * Retourne le nombre d'infectés du pays
	 * @return Entier donnant ce nombre
	*/
	public uint getNbInfected() 
	{
		return nbInfected;
	}

	/**
	 * Modifie le nombre d'infectés
	 * @param nb Nouveau nombre d'infectés
	 */
	public void setNbInfected(uint nb)
	{
		this.nbInfected = nb;
	}

	/**
	 * Modifie la vitesse d'évolution
	 * @param speed Nouvelle vitesse d'évolution
	 */
	public void setEvolutionSpeed(float speed)
	{
		this.evolutionSpeed = speed;
	}

	/**
	 * Ajout de Personnes infectées
	 * @param toAdd: Nombre de personnes infectées à rajouter. Ce nombre doit être positif et inférieur à la population non-infectée.
	*/
	public void addInfectedPeople(uint toAdd)
	{
		if (toAdd < 0)
			return;
		uint nbNotInfected = country.getNbPopulation () - nbInfected;
		if (nbNotInfected < toAdd)
			nbInfected += nbNotInfected;
		nbInfected += toAdd;
	}

	/**
	 * Suppression de Personnes infectées, probablement soignées ou tuées par la maladie
	 * @param toRemove: Nombre de personnes infectées à rajouter. Ce nombre doit être positif et inférieur au nombre de personnes infectées.
	*/
	public void removeInfectedPeople(uint toRemove)
	{
		if (toRemove < 0)
			return;
		if (toRemove < nbInfected)
			nbInfected -= toRemove;
		else
			nbInfected = 0;
	}

	/**
	 * Effectue une évolution de la souche en fonction des motivations.
	 */
	public void evolve()
	{
		DateTime date = DateTime.Now;
		List<float> motivations = new List<float> ();
		motivations.Add (motivationTransmission (date));
		motivations.Add (motivationResistance (date));
		motivations.Add (motivationEvolutionSpeed (date));
		motivations.Add (motivationLethality (date));

		int index = 0;
		float maxi = motivations [0];

		for (int i = 1; i < motivations.Count; i++) {
			if (motivations [i] > maxi) {
				index = i;
				maxi = motivations [i];
			}
		}

		if (index == 0)
			evolveTransmission (date);
		else if (index == 1)
			evolveResistance (date);
		else if (index == 2)
			evolveEvolutionSpeed (date);
		else if (index == 3)
			evolveLethality(date);

		// Ajout du gradient d'infection
		this.historique.addInfectionGradient (date, this.getNbInfected () - this.historique.getPreviousNbInfected ());

	}

	/**
	 * Production de points d'évolution
	 * En fonction des facteurs suivants :
	 * 	- grad :gradient d'infection
	 * 	- nbInfectes : nombre d'infectés actuels
	 * 	- pourc : ratio des infectés
	 * 	- evolutionSpeed : la vitesse d'évolution
	 * On suit la formule : evolutionSpeed * pourc * 10 *(1 + grad) / nbInfectes
	 */
	public void produce()
	{
		float grad = this.historique.lastInfectionGradient ().Value;
		float pourc = this.getNbInfected () / this.country.getNbPopulation ();
		this.evolutionPoints += Mathf.FloorToInt (evolutionSpeed * pourc * 10 * (1 + grad) / nbInfected);
	}

	/**
	 * Indicateur de la volonté de la souche d'augmenter sa faculté de transmission
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- grad : Augmentation du gradient d'infection après la dernière évolution de ce caratère
	 * 	- val : La valeur actuelle de la compétence de la souche, 
	 * 		Cette dernière prend en compte les effets des symptomes de la souche, ainsi que de la recherche et medecine des humains
	 * On suit la formule : duree/100 * ((1 - pourc)*5 + (1 / grad)*2) * (100 - val)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationTransmission(DateTime date)
	{
		DateTime last = historique.lastBestEvolutionTransmissionDate();
		double duree = date.Subtract(last).TotalSeconds;
		float grad = (float) historique.lastInfectionGradient ().Value;
		// Si le gradient est négatif, on perd des amis, il faut augmenter la volonté de transmission
		if (grad < 0)
			grad = - grad / 100;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();
		float val = this.skills.getSkillValue (this.historique.getCorrespondingProperty (last));
		return (float) (duree / 100) * (5 * (1 - pourc) + 3 * (1 / grad)) * (100 - val);
	}
		
	/**
	 * Indicateur de la volonté de la souche d'augmenter sa faculté de résistance
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- grad : Augmentation du gradient d'infection après la dernière évolution de ce caratère
	 * 	- val : La valeur actuelle de la compétence de la souche, 
	 * 		Cette dernière prend en compte les effets des symptomes de la souche, ainsi que de la recherche et medecine des humains
	 * On suit la formule : duree/100 * ((1 - pourc) + 5*(1/grad)) * (100 - val)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationResistance(DateTime date)
	{
		DateTime last = historique.lastBestEvolutionResistanceDate();
		double duree = date.Subtract(last).TotalSeconds;
		float grad = (float) historique.lastInfectionGradient ().Value;
		// Si le gradient est négatif, on perd des amis, il faut augmenter la volonté de résistance
		if (grad < 0)
			grad = - grad / 5;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();
		float val = this.skills.getSkillValue (this.historique.getCorrespondingProperty (last));
		return (float) (duree / 100) * ((1 - pourc) + 5 * (1 / grad)) * (100 - val);
	}
		
	/**
	 * Indicateur de la volonté de la souche d'augmenter sa vitesse d'évolution
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution de ce caractère
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- speed : la vitesse actuelle
	 * 	- val : le nombre de points d'évolution que l'on a actuellement
	 * On suit la formule : duree/100 * (1-pourc)^3 * (val / speed)
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationEvolutionSpeed(DateTime date)
	{
		DateTime last = historique.lastEvolutionEvolutionSpeed();
		double duree = date.Subtract(last).TotalSeconds;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();

		return (float) (duree / 100) * Mathf.Pow((1 - pourc), 3) * (this.evolutionPoints / this.evolutionSpeed);
	}

	/**
	 * Indicateur de la volonté de la souche d'augmenter la "puissance" de l'un de ses symptômes, et donc sa létalité
	 * Calculé en prenant en compte plusieurs facteurs :
	 * 	- duree : Temps écoulé (en secondes) depuis la dernière évolution d'un symptôme
	 * 	- pourc : Pourcentage d'infectés dans la population
	 * 	- val : Nombre de symptômes restant à déveloper
	 * On suit la formule : duree/100 * (5 * pourc) * val
	 * @param date Date à laquelle la méthode est sensée avoir été appelée
	 * @return L'indicateur en question
	 */
	private float motivationLethality(DateTime date)
	{
		DateTime last = historique.lastEvolutionEvolutionSpeed();
		double duree = date.Subtract(last).TotalSeconds;

		float pourc = this.getNbInfected () / this.country.getNbPopulation();

		return (float) (duree / 100) * 5 * pourc * (DonneeSouche.listSymptoms.Count - this.symptoms.Count);
	}

	/**
	 * Choix du type de transmission à améliorer et application de l'évolution de cette transmission
	 * @param date Date de l'évolution
	 */
	private void evolveTransmission(DateTime date)
	{
		DateTime d = historique.lastBestEvolutionTransmissionDate();
		string s = this.historique.getCorrespondingProperty (d);

		// Ajout de random, pour ne pas toujours faire la même chose
		float r = UnityEngine.Random.value;
		int index;
		if (r <= DonneeSouche.epsilonGreedyFactor) {
			index = UnityEngine.Random.Range (0, DonneeSouche.listTransmissionSkills.Count - 1);
			s = DonneeSouche.listTransmissionSkills [index];
		}

		// On veut faire évoluer la transmission de type s
		if (evolutionPoints >= DonneeSouche.coutsSkills [s]) {
			this.skills.addSkillValue (s, 1);
			this.historique.addTransmissionEvolution (s, date);
			this.evolutionPoints -= DonneeSouche.coutsSkills [s];
		}
	}

	/**
	 * Choix du type de résistance à améliorer et application de l'évolution de cette résistance
	 * @param date Date de l'évolution
	 */
	private void evolveResistance(DateTime date)
	{
		DateTime d = historique.lastBestEvolutionResistanceDate();
		string s = this.historique.getCorrespondingProperty (d);

		// Ajout de random, pour ne pas toujours faire la même chose
		float r = UnityEngine.Random.value;
		int index;
		if (r <= DonneeSouche.epsilonGreedyFactor) {
			index = UnityEngine.Random.Range (0, DonneeSouche.listResistanceSkills.Count - 1);
			s = DonneeSouche.listResistanceSkills [index];
		}

		// On veut faire évoluer la transmission de type s
		if (evolutionPoints >= DonneeSouche.coutsSkills [s]) {
			this.skills.addSkillValue (s, 1);
			this.historique.addResistanceEvolution (s, date);
			this.evolutionPoints -= DonneeSouche.coutsSkills [s];
		}
	}

	/**
	 * Application de l'augmentation de la vitesse d'évolution
	 * @param date Date de l'évolution
	 */
	private void evolveEvolutionSpeed(DateTime date)
	{
		if (evolutionPoints >= DonneeSouche.coutEvolutionSpeedUp) {
			this.evolutionSpeed += 1;
			this.historique.addEvolutionSpeedEvolution (date);
			this.evolutionPoints -= DonneeSouche.coutEvolutionSpeedUp;
		}
	}

	/**
	 * Choix du type de transmission à améliorer et application de l'évolution vers la transmission
	 * @param date Date de l'évolution
	 */
	private void evolveLethality(DateTime date)
	{
		float pourc = this.getNbInfected () / this.country.getNbPopulation();

		// Dans le cas où une grande majorité de la population est infectée et qu'on a assez de points, on achève la population
		if (pourc >= 90
			&& !(this.symptoms.ContainsKey ("ArretDesOrganes"))
			&& this.evolutionPoints >= DonneeSouche.coutsSymptomes ["ArretDesOrganes"]) {
			this.symptoms.Add("ArretDesOrganes", new ArretDesOrganes());
			return;
		}

		// Si le pourcentage est bas, au contraire on peut regarder si on peut ajouter un symptôme qui aiderait à la transmission
		if (pourc < 30) {
			int water = this.skills.getWaterSpreading ();
			int air = this.skills.getAirSpreading ();
			int contact = this.skills.getContactSpreading ();

			// S'il faut augmenter la transmission par l'eau
			if (water <= air && water <= contact) {
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Eternuements"] && !this.symptoms.ContainsKey ("Eternuements")) {
					this.symptoms.Add ("Eternuements", new Eternuements ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Sueurs"] && !this.symptoms.ContainsKey ("Sueurs")) {
					this.symptoms.Add ("Sueurs", new Sueurs ());
					return;
				}
			}

			// S'il faut augmenter la transmission par l'air
			if (air <= water && air <= contact) {
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Eternuements"] && !this.symptoms.ContainsKey ("Eternuements")) {
					this.symptoms.Add ("Eternuements", new Eternuements ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Toux"] && !this.symptoms.ContainsKey ("Toux")) {
					this.symptoms.Add ("Toux", new Toux ());
					return;
				}
			}

			// S'il faut augmenter la transmission par le contact
			if (contact <= water && contact <= air) {
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Sueurs"] && !this.symptoms.ContainsKey ("Sueurs")) {
					this.symptoms.Add ("Sueurs", new Sueurs ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Toux"] && !this.symptoms.ContainsKey ("Toux")) {
					this.symptoms.Add ("Toux", new Toux ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Eternuements"] && !this.symptoms.ContainsKey ("Eternuements")) {
					this.symptoms.Add ("Eternuements", new Eternuements ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Fievre"] && !this.symptoms.ContainsKey ("Fievre")) {
					this.symptoms.Add ("Fievre", new Fievre ());
					return;
				}
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Diarrhee"] && !this.symptoms.ContainsKey ("Diarrhee")) {
					this.symptoms.Add ("Diarrhee", new Diarrhee ());
					return;
				}
			}
		}

		// Si on n'a rien fait jusque là, on choisit un symptôme à développer, du moins cher au plus cher
		string s = "";
		int minVal = DonneeSouche.coutMaxSymptom;
		List<string> remainingSymptoms = new List<string>();
		foreach (string temp in DonneeSouche.listSymptoms) {
			if (!this.symptoms.ContainsKey (temp))
				remainingSymptoms.Add (temp);
		}

		if (remainingSymptoms.Count < 1)
			return;
		foreach (string temp in remainingSymptoms) {
			if (DonneeSouche.coutsSymptomes [temp] < minVal && !this.symptoms.ContainsKey (temp)) {
				s = temp;
				minVal = DonneeSouche.coutsSymptomes [temp];
			}
		}

		if (UnityEngine.Random.value <= DonneeSouche.epsilonGreedyFactor) {
			int index = UnityEngine.Random.Range (0, remainingSymptoms.Count - 1);
			s = remainingSymptoms [index];
		}

		if (!s.Equals ("")) {
			switch (s) {
			case "ArretDesOrganes":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["ArretDesOrganes"] && !this.symptoms.ContainsKey ("ArretDesOrganes")) {
					this.symptoms.Add ("ArretDesOrganes", new ArretDesOrganes ());
					return;
				}
				break;
			case "Diarrhee":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Diarrhee"] && !this.symptoms.ContainsKey ("Diarrhee")) {
					this.symptoms.Add ("Diarrhee", new Diarrhee ());
					return;
				}
				break;
			case "Eternuements":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Eternuements"] && !this.symptoms.ContainsKey ("Eternuements")) {
					this.symptoms.Add ("Eternuements", new Eternuements ());
					return;
				}
				break;
			case "Fievre":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Fievre"] && !this.symptoms.ContainsKey ("Fievre")) {
					this.symptoms.Add ("Fievre", new Fievre ());
					return;
				}
				break;
			case "Sueurs":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Sueurs"] && !this.symptoms.ContainsKey ("Sueurs")) {
					this.symptoms.Add ("Sueurs", new Sueurs ());
					return;
				}
				break;
			case "Toux":
				if (this.evolutionPoints >= DonneeSouche.coutsSymptomes ["Toux"] && !this.symptoms.ContainsKey ("Toux")) {
					this.symptoms.Add ("Toux", new Toux ());
					return;
				}
				break;
			default:
				break;
			}
		}
			
	}

	/**
	 * Application de la fusion entre une souche arrivante, et la souche actuelle.
	 * @param souche Souche qui arrive avec les migrants
	 * @param nbInfectedComing Nombre de migrants qui arrivent avec cette souche : Influencera l'impact des caractéristiques de la souche arrivante sur la souche courante
	 */
	public void fusion(Souche souche, uint nbInfectedComing)
	{
		// Fusion des capacités
		if (souche.skills.getWaterSpreading () > this.skills.getWaterSpreading ()) {
			this.skills.setWaterSpreading (Mathf.FloorToInt (this.skills.getWaterSpreading () + ((souche.skills.getWaterSpreading () - this.skills.getWaterSpreading ()) * nbInfectedComing / 10)));
		}
		if (souche.skills.getAirSpreading () > this.skills.getAirSpreading ()) {
			this.skills.setAirSpreading (Mathf.FloorToInt (this.skills.getAirSpreading () + ((souche.skills.getAirSpreading () - this.skills.getAirSpreading ()) * nbInfectedComing / 10)));
		}
		if (souche.skills.getContactSpreading () > this.skills.getContactSpreading ()) {
			this.skills.setContactSpreading (Mathf.FloorToInt (this.skills.getContactSpreading () + ((souche.skills.getContactSpreading () - this.skills.getContactSpreading ()) * nbInfectedComing / 10)));
		}
		if (souche.skills.getResistanceCold () > this.skills.getResistanceCold ()) {
			this.skills.setResistanceCold (Mathf.FloorToInt (this.skills.getResistanceCold () + ((souche.skills.getResistanceCold () - this.skills.getResistanceCold ()) * nbInfectedComing / 10)));
		}
		if (souche.skills.getResistanceHeat () > this.skills.getResistanceHeat ()) {
			this.skills.setResistanceHeat (Mathf.FloorToInt (this.skills.getResistanceHeat () + ((souche.skills.getResistanceHeat () - this.skills.getResistanceHeat ()) * nbInfectedComing / 10)));
		}

		// Fusion des symptomes
		foreach (string temp in DonneeSouche.listSymptoms) {
			if (!this.symptoms.ContainsKey (temp) && souche.symptoms.ContainsKey (temp)) {
				if (UnityEngine.Random.value * nbInfectedComing < 1 / DonneeSouche.coutsSymptomes [temp]) {
					this.symptoms.Add (temp, souche.symptoms [temp]);
				}
			}
		}

		// Rajout des infectés à la population d'infectés
		this.nbInfected += nbInfectedComing;
		// Ajout de points d'évolution car fusion :
		this.evolutionPoints ++;
	}

	/**
	 * Application de la contamination de nouveaux habitants et de la mort de certains d'entre eux si c'est nécessaire
	 * La Contamination suit la formule suivante : nbInfected * somme des skills * (1 - pourcentage d'infectés)
	 */
	public void contamination()
	{
		/* Contamination */
		///////////////////
		float pourc = this.getNbInfected () / this.country.getNbPopulation ();
		float skillsSum = 0f;
		foreach (string s in DonneeSouche.listTransmissionSkills) {
			skillsSum += this.skills.getSkillValue (s);
		}
		foreach (string s in DonneeSouche.listResistanceSkills) {
			skillsSum += this.skills.getSkillValue (s);
		}
		skillsSum /= 10;

		uint toAdd;
		if (UnityEngine.Random.value > DonneeSouche.epsilonGreedyFactor)
			toAdd = (uint) Mathf.FloorToInt (this.nbInfected * skillsSum * (1 - pourc));
		else
			toAdd = (uint) Mathf.CeilToInt (this.nbInfected * skillsSum * (1 - pourc));

		this.addInfectedPeople (toAdd);


		/* Assassinat */
		////////////////
		float killSum = 0f;
		foreach (KeyValuePair<string, AbstractSymptom> pair in this.symptoms) {
			killSum += Mathf.Max(pair.Value.getLethalityIndex (), 0);
		}
		killSum /= 100;

		uint toKill;
		if (UnityEngine.Random.value > DonneeSouche.epsilonGreedyFactor)
			toKill = (uint) Mathf.FloorToInt (this.nbInfected * skillsSum);
		else
			toKill = (uint) Mathf.CeilToInt (this.nbInfected * skillsSum);
		this.removeInfectedPeople (toKill);
		this.country.removePeople (toKill);

	}


}
