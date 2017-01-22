using UnityEngine;
using System;

public abstract class AbstactSymptom
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
	 * Coût de l'évolution du symptome
	 */
	public int coutEvolution {
		get { return coutEvolution; }
		set { coutEvolution = value; }
	}

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
		return this.lethalityIndex;
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
	public abstract float affectSkillResistanceCold();

	/**
	 * Altère la résistance à la chaleur de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public abstract float affectSkillResistanceHeat();

	/**
	 * Altère la transmission par l'eau de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public abstract float affectSkillWaterSpreading();

	/**
	 * Altère la transmission par l'air de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public abstract float affectSkillAirSpreading();

	/**
	 * Altère la transmission par le contact de la souche.
	 * @return Facteur multiplicateur qui sera appliqué à cette compétence
	 */
	public abstract float affectSkillContactSpreading();

}

