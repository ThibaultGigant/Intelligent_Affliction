using System;
using System.Collections.Generic;

/**
 * Couple
 */
public class Couple<T,U>
{
	public T first;

	public U second;

	public Couple (T t, U u) {
		first = t;
		second = u;
	}
}

