using UnityEngine;
using System.Collections.Generic;

/**
 * Classe permettant de faire des files de type FIFO de taille limitée dès la création
 */
public class LimitedList<T> : List<T>
{

	/**
	 * Taille de la file
	 */
	public int size { get ; set ; }

	/**
	 * Constructeur
	 */
	public LimitedList (int size)
	{
		this.size = size;
	}

	/**
	 * Permet d'ajouter un objet tout en gardant la file de la taille voulue
	 */
	public void Enqueue (T obj)
	{
		base.Add (obj);
		while (Count > size) {
			base.RemoveAt (0);
		}
	}

	/**
	 * Affichage de la file avec le type stocké et la taille de cette fille
	 */
	public override string ToString ()
	{
		string str = "Limited List of type <" + typeof(T).Name + "> size : " + Count + "\n";
		int i = 0;
		foreach (T t in this) {
			str += i + " : " + t.ToString () + "\n";
			i++;
		}
		str += "\n";
		return str;
	}
}