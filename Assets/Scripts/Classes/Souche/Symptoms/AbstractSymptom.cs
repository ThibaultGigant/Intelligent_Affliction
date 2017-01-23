using UnityEngine;
using System;

public abstract class AbstractSymptom
{
	/**
	 * Souche qui possède ce symptôme
	 */
	public Souche souche;

	/**
	 * Nom du symptôme
	 */
	protected String name;

	/**
	 * Facteur influençant la létalité du symptôme
	 */
	protected float lethalityIndex;

	/**
	 * Facteur influençant la détection de la souche à cause du symptôme
	 */
	protected float detectableIndex;

	/**
	 * Retourne le nom du symptôme
	 * @return Nom du symptôme
	 */
	public String getName()
	{
		return this.name;
	}

	/**
	 * Retourne l'indice de létalité
	 * @return Indice voulu
	 */
	public float getLethalityIndex()
	{
		return this.lethalityIndex * this.getAffectedByHumans();
	}

	/**
	 * Renvoie un indice signifiant l'augmentation de la détection de la souche par les médecins
	 * @return Indice voulu
	 */
	public float getDetectableIndex()
	{
		return this.detectableIndex;
	}

	/**
	 * Calcul de l'effet qu'ont les connaissances des humains et la population assignée à la médecine sur le symptome dont la connaissance liée est passée en paramètre
	 * @param symptomKnowledge Connaissance associée au symptome qu'on étudie
	 * @return Facteur d'atténuation dû aux humains
	 */
	public float computeHumanEffectOnSymptom(string symptomKnowledge)
	{
		return Mathf.Clamp((1f - (1f + this.souche.country.resources [symptomKnowledge].getQuantity ()) * this.souche.country.getPourcentageCategory ("Medecine") / 10f ), 0f, 1f);
	}

	/**
	 * Renvoie un facteur influençant la puissance du symptôme
	 * Dépend des connaissances accumulées sur le symptôme et du pourcentage de la population assignée à la médecine
	 * @return facteur d'atténuation du symptôme
	 */
	public abstract float getAffectedByHumans();

	/**
	 * Altère la résistance au froid de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public float affectSkillResistanceCold()
	{
		return DonneeSouche.influenceSymptomes[this.name][0] * getAffectedByHumans();
	}

	/**
	 * Altère la résistance à la chaleur de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public float affectSkillResistanceHeat()
	{
		return DonneeSouche.influenceSymptomes[this.name][1] * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par l'eau de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public float affectSkillWaterSpreading()
	{
		return DonneeSouche.influenceSymptomes[this.name][2] * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par l'air de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public float affectSkillAirSpreading()
	{
		return DonneeSouche.influenceSymptomes[this.name][3] * getAffectedByHumans();
	}

	/**
	 * Altère la transmission par le contact de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public float affectSkillContactSpreading()
	{
		return DonneeSouche.influenceSymptomes[this.name][4] * getAffectedByHumans();
	}

}

