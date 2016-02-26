using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class Buff {

	public int id{get{return data.id;}}
	public int effectTurns;
	protected BuffData data;
	protected BattleObject source;

	protected abstract void Init();
	protected abstract void Execute();
	protected abstract void Revert();	

	public static Buff CreateBuff(BattleObject bo, BuffData data, int effectTurns)
	{
		Buff buff = (Buff)Assembly.GetExecutingAssembly().CreateInstance("Buff" + data.buffEffect, true); 
		buff.source = bo;
		buff.data = data;
		buff.effectTurns = effectTurns;
		buff.Init();

		return buff;
	}
	
	//check on every turn, see if buff still valid
	public void Tick()
	{
		if(effectTurns > 0)
		{
			effectTurns--;
		}

		if(effectTurns == 0)
		{
			this.Revert();			
		}
		else
		{
			Check(BuffTrigger.Ready);
		}
	}

	/*-----BUFF CALLBACK-----*/
	public void Check(BuffTrigger trigger)
	{
		if(data.trigger == trigger)
			this.Execute();
	}
}
