using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class Buff {

	protected BuffData data;
	protected BattleObject source;
	protected int effectTurns;

	protected abstract void Execute();
	protected abstract void Revert();	

	public static Buff CreateBuff(BattleObject bo, BuffData data, int effectTurns)
	{
		Buff buff = (Buff)Assembly.GetExecutingAssembly().CreateInstance("Buff" + data.buffEffect, true); 
		buff.source = bo;
		buff.data = data;
		buff.effectTurns = effectTurns;
		buff.OnAddBuff();

		return buff;
	}
	
	//check on every turn, see if buff still valid
	public int Tick()
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

		return effectTurns;
	}

	/*-----BUFF CALLBACK-----*/
	public void Check(BuffTrigger trigger)
	{
		if(data.trigger == trigger)
			this.Execute();
	}

	private void OnAddBuff()
	{
		Check(BuffTrigger.Always);
	}


	public void OnReady()
	{
		if(data.trigger == BuffTrigger.Ready)
			this.Execute();
	}

	public void OnAction()
	{
		if(data.trigger == BuffTrigger.Ready)
			this.Execute();
	}
}
