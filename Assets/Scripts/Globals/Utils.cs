using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

/**
 * Utils
 * ---
 * Ressence les fonctions utilitaires
 */
public static class Utils
{
	/**
	 * tirageAlatoireList
	 * Tire aléatoirement un nombre, suivant la distribution
	 * donnée en paramètre, sous forme de liste
	 * @param liste	La distribution de probabilité, sous frome de liste (System.Collections.Generic.List<int>)
	 * 					Les valeures doivent êtres positives et leur somme doit être égale à 100
	 * @return			Revoie un entier tiré aléatoirement selon la distribution
	 */
	public static int tirageAlatoireList(List<int> liste)
	{
		// La liste doit exister
		if (liste == null || liste.Count == 0)
			return -1;

		// Tirage aléatoire
		int rnd = new System.Random().Next(0,100);
		int sommeCumule = 0;
		// Calcul de la somme cumulée
		for ( int i = 0 ; i < liste.Count ; i++ )
		{
			sommeCumule += liste[i];
			if (rnd < sommeCumule)
				return i;
		}

		/**
		 * Si un problème à été rencontré,
		 * par exemple, la liste ne somme pas à 100
		 */
		return -1;
	}

	public static Couple<float, float> moyenneVariance(uint[] liste) {
		float moy = 0, var = 0;

		for ( int i = 0 ; i < liste.Length ; i++ ) {
			moy += liste[i];
		}
		moy /= liste.Length;

		for ( int i = 0 ; i < liste.Length ; i++ ) {
			var += Mathf.Pow( liste[i] - moy ,2f);
		}
		var /= liste.Length;

		return new Couple<float, float> (moy, var);
	}

	public static Couple<float, float> moyenneVariance(List<uint> liste) {
		float moy = 0, var = 0;

		for ( int i = 0 ; i < liste.Count ; i++ ) {
			moy += liste[i];
		}
		moy /= liste.Count;

		for ( int i = 0 ; i < liste.Count ; i++ ) {
			var += Mathf.Pow( liste[i] - moy ,2f);
		}
		var /= liste.Count;

		return new Couple<float, float> (moy, var);
	}

	public static string toStringFormat(int nb) {
		if (nb / 1000 < 1)
			return nb.ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE"));
		if (nb / 1000000 < 1)
			return ((int)(nb / 1000)).ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE")) + " K";
		else
			return (((float) nb / 1000000)).ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE")) + " M";
	}
}

