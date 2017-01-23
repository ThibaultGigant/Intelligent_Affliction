﻿using System;

public class Sueurs : AbstactSymptom
{
	/**
	 * Constructeur
	 * @param cout Coût de l'évolution du symptom
	 */
	public Sueurs ()
	{
		this.name = "Sueurs";
		this.lethalityIndex = DonneeSouche.lethalitySymptomes[this.name];
		this.detectableIndex = DonneeSouche.detectabilitySymptomes[this.name];
	}

	/**
	 * Renvoie un facteur influençant la puissance du symptôme
	 * Dépend des connaissances accumulées sur le symptôme et du pourcentage de la population assignée à la médecine
	 * @return facteur d'atténuation du symptôme
	 */
	public override float getAffectedByHumans()
	{
		return computeHumanEffectOnSymptom ("KnowledgeSueurs");
	}
}

