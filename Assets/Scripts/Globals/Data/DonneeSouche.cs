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
}

