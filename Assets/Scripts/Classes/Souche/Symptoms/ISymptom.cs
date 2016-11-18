using System;

public interface ISymptom
{
	/**
	 * Coût de l'évolution du symptom
	 */
	int coutEvolution { get; set; }

	/**
	 * Altère les skills de la souche
	 */
	void affectSkills();
}

