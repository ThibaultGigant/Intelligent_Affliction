using System;
using System.Collections.Generic;

public static class DonneeSouche
{
	/*************************************************/
	/*************** Listes de valeurs ***************/
	/*************************************************/

	public static List<string> listSymptoms = new List<string> (
		new string[] {
			"ArretDesOrganes",
			"Diarrhee",
			"Eternuements",
			"Fievre",
			"Sueurs",
			"Toux"
		}
	);

	public static List<string> listTransmissionSkills = new List<string> (
		new string[] {
			"Water",
			"Air",
			"Contact"
		}
	);

	public static List<string> listResistanceSkills = new List<string> (
		new string[] {
			"Cold",
			"Heat"
		}
	);


	/*************************************************/
	/******************** Coûts **********************/
	/*************************************************/

	public static int coutSkill = 5;

	public static IDictionary<string, int> coutsSkills = new Dictionary<string,int> ()
	{
		{"Water", coutSkill},
		{"Air", coutSkill},
		{"Contact",coutSkill},
		{"Cold", coutSkill},
		{"Heat",coutSkill}
	};

	public static IDictionary<string, int> coutsSymptomes = new Dictionary<string,int> ()
	{
		{"ArretDesOrganes", 500},
		{"Diarrhee", 100},
		{"Eternuements",30},
		{"Fievre", 80},
		{"Sueurs",60},
		{"Toux", 40}
	};

	public static int coutMaxSymptom = 500;

	public static int coutEvolutionSpeedUp = 10;


	/*************************************************/
	/************** Facteurs d'influence *************/
	/*************************************************/

	public static IDictionary<string, float> lethalitySymptomes = new Dictionary<string,float> ()
	{
		{"ArretDesOrganes", 15f},
		{"Diarrhee", 5f},
		{"Eternuements",1.1f},
		{"Fievre", 3f},
		{"Sueurs",2.5f},
		{"Toux", 2f}
	};

	public static IDictionary<string, float> detectabilitySymptomes = new Dictionary<string,float> ()
	{
		{"ArretDesOrganes", 3f},
		{"Diarrhee", 2.2f},
		{"Eternuements",1.1f},
		{"Fievre", 1.8f},
		{"Sueurs",2f},
		{"Toux", 1.5f}
	};

	public static IDictionary<string, List<float>> influenceSymptomes = new Dictionary<string, List<float>> ()
	{
		// Dans l'ordre pour les skill affectés : Cold, Heat, Water, Air, Contact
		{"ArretDesOrganes", new List<float> {1f, 1f, 1f, 1f, 1f}},
		{"Diarrhee", new List<float> {1f, 1f, 3f, 1f, 1.5f}},
		{"Eternuements", new List<float> {1f, 1f, 1.1f, 3.5f, 2f}},
		{"Fievre", new List<float> {1f, 1.3f, 1f, 1f, 1.5f}},
		{"Sueurs", new List<float> {1f, 1f, 1.2f, 1f, 2.5f}},
		{"Toux", new List<float> {1f, 1f, 1.1f, 3f, 2f}}
	};

	/**
	 * Facteur de randomisation : Si un random est plus bas que ce chiffre, on fait autre chose
	 */
	public static float epsilonGreedyFactor = 0.05f;
}

