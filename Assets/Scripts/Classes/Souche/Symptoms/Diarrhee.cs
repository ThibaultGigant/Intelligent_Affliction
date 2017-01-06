using System;

public class Diarrhee : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Diarrhee (int cout)
	{
		coutEvolution = cout;
		this.name = "Diarrhee";
	}

	/**
	 * Altère les skills de la souche
	 */
	public override void affectSkills() {

	}
}

