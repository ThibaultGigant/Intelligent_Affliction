using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

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

		return liste.Count - 1;
	}

	public static int tirageNormale( float mu, float sigma ) {
		return (int)(1f / (sigma * Mathf.Sqrt (2f * Mathf.PI)) * Mathf.Exp (-((UnityEngine.Random.value-0.5f) * 6 * sigma - mu) / (2f * Mathf.Pow (sigma, 2f))));
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
		if (nb / 1/*000*/ < 1)
			return nb.ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE")) + " K";
		if (nb / 1000000 < 1)
			return ((int)(nb / 1000)).ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE")) + " M";
		else
			return (((float) nb / 1000000)).ToString ("N1", CultureInfo.CreateSpecificCulture ("sv-SE")) + " Mrd";
	}

	/**
	 * Renvoie le produit des indices, chacun normalisées comme décris dans la structure donnée en paramètre
	 * @param indices Liste des indices à multiplier. Il s'agit d'une liste à deux dimensions, où chaque ligne correspond, pour un indice à
	 * { valeur de l'indice, valeur minimale, valeur maximale, valeur minimal après normalisation, valeur maximale après normalisation, [0 si on veut multiplier | 1 si on veut diviser] }
	 * @return Le résultat
	 */
	public static float indicesNormalises(float[,] indices) {
		float resultat = 1f;
		for ( int i = 0 ; i < indices.GetLength(0) ; i++ ) {
			if (indices [i, 5] == 0f) {
				resultat *= ((indices [i, 0] * (indices [i, 4] - indices [i, 3]) / (indices [i, 2] - indices [i, 1])) + indices [i, 3]);
			}
			else
				resultat /= ((indices [i, 0] * (indices [i, 4] - indices [i, 3]) / (indices [i, 2] - indices [i, 1])) + indices [i, 3]);
		}
		return resultat;
	}

	public static void createGraphique(GameObject graphique, LimitedQueue<int> liste) {
		Texture2D texture = (Texture2D) (graphique.GetComponent<RawImage>().texture);
		UnityEngine.Color backgroundColor = new UnityEngine.Color (30f/255f,30f/255f,30f/255f,200f/255f);
		int i = 0;
		if (liste.Count != 0) {
			int max = liste.Peek ();
			foreach (int nb in liste)
				if (max < nb)
					max = nb;
				
			foreach (int nb in liste) {
				for (int j = 0; j < texture.height; j++) {
					texture.SetPixel (i, j, backgroundColor);
				}

				float nb_norma = (float)nb;
				nb_norma /= max;
				nb_norma *= texture.height;


				//texture.SetPixel (i, Mathf.Max((int)nb_norma - 1, 0), Color.white);
				texture.SetPixel (i, (int)nb_norma, Color.white);

				i++;
			}
			graphique.transform.FindChild ("Max").GetComponent<Text> ().text = "" + max;
		}
		for ( i = liste.Count ; i < texture.width ; i++ ) {
			for ( int j = 0 ; j < texture.height ; j ++) {
				texture.SetPixel (i, j, backgroundColor);
			}
		}


		texture.Apply ();
	}
}

