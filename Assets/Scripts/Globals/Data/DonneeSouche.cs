using System;
using System.Collections.Generic;

public static class DonneeSouche
{
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

	public static IDictionary<string, int> coutsSymptomes = new Dictionary<string,int> ()
	{
		{"ArretDesOrganes", 500},
		{"Diarrhee", 100},
		{"Eternuements",30},
		{"Fievre", 80},
		{"Sueurs",60},
		{"Toux", 40}
	};

	public static IDictionary<string, float> lethalitySymptomes = new Dictionary<string,int> ()
	{
		{"ArretDesOrganes", 15f},
		{"Diarrhee", 5f},
		{"Eternuements",1.1f},
		{"Fievre", 3f},
		{"Sueurs",2.5f},
		{"Toux", 2f}
	};

	public static IDictionary<string, float> detectabilitySymptomes = new Dictionary<string,int> ()
	{
		{"ArretDesOrganes", 3f},
		{"Diarrhee", 2.2f},
		{"Eternuements",1.3f},
		{"Fievre", 1.8f},
		{"Sueurs",2f},
		{"Toux", 1.5f}
	};
}

