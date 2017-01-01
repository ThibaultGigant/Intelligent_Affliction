using System;
using System.Collections.Generic;

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
		int rnd = new Random().Next(0,100);
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
}

