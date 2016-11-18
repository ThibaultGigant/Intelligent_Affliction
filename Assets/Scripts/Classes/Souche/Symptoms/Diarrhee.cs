using System;

public class Diarrhee : ISymptom
{
	/**
	 * Coût de l'évolution du symptom
	 */
	public int coutEvolution {
		get { return coutEvolution; }
		set { coutEvolution = value; }
	}

	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Diarrhee (int cout)
	{
		coutEvolution = cout;
	}

	/**
	 * Altère les skills de la souche
	 */
	public void affectSkills() {

	}
}

