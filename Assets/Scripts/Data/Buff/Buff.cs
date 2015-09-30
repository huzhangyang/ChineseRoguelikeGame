using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Buff {

	BuffData data;
	BuffEffect effect;
	int effectTurns;

	public Buff(BuffData data)
	{
		this.data = data;
		this.effectTurns = data.lastTurns;
		ParseBuffEffect(data.buffEffect);
		OnAddBuff();
	}

	private void ParseBuffEffect(string effectString)
	{
		string name;
		string[] args;

		string[] splitedString = effectString.Split(':');
		name = splitedString[0].Trim();
		args = splitedString[1].Split(',');


		effect = (BuffEffect)Assembly.GetExecutingAssembly().CreateInstance("BuffEffect" + name);

		if(effect == null)
		{
			effect = new BuffEffectNone();
		}
	}
	//check on every turn, see if buff still valid
	public int Tick()
	{
		effectTurns--;
		if(effectTurns <= 0)
		{
			effect.Revert();			
		}
		return effectTurns;
	}

	/*-----BUFF CALLBACK-----*/
	public void OnAddBuff()
	{
		if(data.trigger == BuffTrigger.Always)
			effect.Execute();
	}

	public void OnHit()
	{
		if(data.trigger == BuffTrigger.Hit)
			effect.Execute();
	}

	public void OnBeHit()
	{
		if(data.trigger == BuffTrigger.Behit)
			effect.Execute();
	}

	public void OnReady()
	{
		if(data.trigger == BuffTrigger.Ready)
			effect.Execute();
	}

	public void OnAction()
	{
		if(data.trigger == BuffTrigger.Action)
			effect.Execute();
	}
}
