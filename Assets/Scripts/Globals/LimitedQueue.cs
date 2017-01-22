using UnityEngine;
using System.Collections.Generic;

/**
 * Classe permettant de faire des files de type FIFO de taille limitée dès la création
 */
public class LimitedQueue<T> : Queue<T>
{

	/**
	 * Taille de la file
	 */
	public int size { get ; set ; }

	/**
	 * Constructeur
	 */
	public LimitedQueue (int size)
	{
		this.size = size;
	}

	/**
	 * Permet d'ajouter un objet tout en gardant la file de la taille voulue
	 */
	public void Enqueue (T obj)
	{
		base.Enqueue (obj);
		while (Count > size) {
			base.Dequeue ();
		}
	}

	/**
	 * Affichage de la file avec le type stocké et la taille de cette fille
	 */
	public override string ToString ()
	{
		string str = "Limited Queue of type <" + typeof(T).Name + "> size : " + Count + "\n";
		int i = 0;
		foreach (T t in this) {
			str += i + " : " + t.ToString () + "\n";
			i++;
		}
		str += "\n";
		return str;
	}
}