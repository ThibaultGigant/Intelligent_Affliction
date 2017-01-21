using System;
using System.Collections.Generic;

public static class DonneePays
{
	public static IDictionary<string, IDictionary<string, ulong>> donneePays = new Dictionary<string, IDictionary<string, ulong>> ()
	{ 	{"Afrique" 			, new Dictionary<string, ulong>()
									{	{"superficie"		, 30370000},
										{"population"		, 1216000/*000*/},
										{"chaleur"		, 78},
										{"humidite"		, 23}
									}},
		{"Europe"			, new Dictionary<string, ulong>()
									{	{"superficie"		, 10180000},
										{"population"	, 743100/*000*/},
										{"chaleur"		, 53},
										{"humidite"		, 62}
									}},
		{"Asie"				, new Dictionary<string, ulong>()
									{
										{"superficie"		, 44580000},
										{"population"	, 4436000/*000*/},
										{"chaleur"		, 44},
										{"humidite"		, 50}
									}},
		{"Eurasie"    		, new Dictionary<string, ulong>()
									{	{"superficie"		, 54760000},
										{"population"	, 5179100/*000*/},
										{"chaleur"		, 47},
										{"humidite"		, 56}
									}},
		{"Amérique du Nord" , new Dictionary<string, ulong>()
									{	{"superficie"		, 24710000},
										{"population"	, 579000/*000*/},
										{"chaleur"		, 39},
										{"humidite"		, 52}
									}},
		{"Océanie" , new Dictionary<string, ulong>()
									{	{"superficie"		, 8525989},
										{"population"	, 38000/*000*/},
										{"chaleur"		, 81},
										{"humidite"		, 71}
									}},
		{"Amérique du Sud"	, new Dictionary<string, ulong>()
									{	{"superficie"		, 17840000},
										{"population"	, 422500/*000*/},
										{"chaleur"		, 68},
										{"humidite"		, 68}
									}}
	};

	/**
	 * Renvoie la superficie du pays souhaité
	 * @param nomPays		Nom du pays
	 * @return				La superficie du pays
	 */
	public static float getSuperficie(string nomPays)
	{
		return (float)donneePays [nomPays] ["superficie"];
	}

	/**
	 * Renvoie la taille de la population du pays souhaité
	 * @param nomPays		Nom du pays
	 * @return				La taille de la population du pays
	 */
	public static uint getPopulation(string nomPays)
	{
		return (uint) donneePays [nomPays] ["population"];
	}

	/**
	 * Renvoie la chaleur du pays souhaité
	 * @param nomPays		Nom du pays
	 * @return				La chaleur du pays
	 */
	public static int getChaleur(string nomPays)
	{
		return (int) donneePays [nomPays] ["chaleur"];
	}

	/**
	 * Renvoie l'humidité du pays souhaité
	 * @param nomPays		Nom du pays
	 * @return				L'humidité du pays
	 */
	public static int getHumidite(string nomPays)
	{
		return (int) donneePays [nomPays] ["humidite"];
	}
}

