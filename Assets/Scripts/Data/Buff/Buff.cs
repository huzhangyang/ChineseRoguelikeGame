using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buff {

	BuffData data;
	BuffEffect effect;

	public Buff(BuffData data)
	{
		this.data = data;
		ParseBuffEffect(data.buffEffect);
	}

	private void ParseBuffEffect(string effectString)
	{

	}
}
