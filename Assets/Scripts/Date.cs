using System;

public class Date
{
	private int jour;
	private int mois;
	private int annee;

	private int[] finDeMois = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

	public Date (int j, int m, int a)
	{
		jour = j;
		mois = m;
		annee = a;
	}

	public void Tomorow() {
		if (jour >= finDeMois [mois]) {
			if (mois == 2 && jour == 28 && ((annee % 4 == 0 && annee % 100 != 0) || annee % 400 == 0) ) {
				jour++;
				return;
			}

			jour = 1;
			if (mois == 12) {
				mois = 1;
				annee++;
			}
			else {
				mois++;
			}
		}
		else
			jour++;
	}

	public int getJour() { return jour; }
	public int getMois() { return mois; }
	public int getAnnee() { return annee; }

	public Date copy() {
		return new Date (jour, mois, annee);
	}

	public bool Equals(Date date) {
		return jour == date.getJour () && mois == date.getMois () && annee == date.getAnnee ();
	}

}

