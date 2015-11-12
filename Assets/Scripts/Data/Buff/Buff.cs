using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Buff {

	BuffData data;
	BuffEffect effect;
	BattleObject source;
	int effectTurns;

	public Buff(BattleObject source, BuffData data)
	{
		this.data = data;
		this.effectTurns = data.lastTurns;
		this.source = source;
		ParseBuffEffect(data.buffEffect);
		OnAddBuff();
	}

	private void ParseBuffEffect(string effectString)
	{
		string name = "";
		string args = "";

		string[] splitedString = effectString.Split(':');
		name = splitedString[0].Trim();		
		if(splitedString.Length > 1)
			args = splitedString[1];

		effect = (BuffEffect)Assembly.GetExecutingAssembly().CreateInstance("BuffEffect" + name, true);

		if(effect == null)
		{
			effect = new BuffEffectNone();
		}
		effect.Init(source, args);
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
