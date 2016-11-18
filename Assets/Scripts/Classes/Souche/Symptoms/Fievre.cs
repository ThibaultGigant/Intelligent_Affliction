using System;

public class Fievre : ISymptom
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
	public Fievre (int cout)
	{
		coutEvolution = cout;
	}

	/**
	 * Altère les skills de la souche
	 */
	public void affectSkills() {

	}
}

