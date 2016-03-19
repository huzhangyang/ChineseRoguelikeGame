using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class Buff {

	public int id{get{return data.id;}}
	public bool isBornBuff{get{return data.bornBuff;}}
	public int effectTurns;
	protected BuffData data;
	protected BattleObject source;
	protected GameObject buffIcon;

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
		buff.AddBuffIcon();

		return buff;
	}
	
	//check on every turn, see if buff still valid
	public void Tick()
	{
		if(effectTurns > 0)
		{
			effectTurns--;
			TickBuffIcon();
		}

		if(effectTurns == 0)
		{
			Remove();
		}
		else
		{
			Check(BuffTrigger.Ready);
		}
	}

	public void Remove()
	{
		Revert();	
		RemoveBuffIcon();
	}

	/*-----BUFF CALLBACK-----*/
	public void Check(BuffTrigger trigger)
	{
		if(data.trigger == trigger)
			this.Execute();
	}

	private void AddBuffIcon()
	{
		if(data.bornBuff) return;
		buffIcon = GameObject.Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "BuffIcon")) as GameObject;
		buffIcon.transform.SetParent(source.GetComponent<BattleObjectUIEvent>().buffTransform, false);
		buffIcon.GetComponent<BuffIconUIEvent>().Init(this.data, this.effectTurns);
	}

	private void TickBuffIcon()
	{
		if(buffIcon != null)
			buffIcon.GetComponent<BuffIconUIEvent>().SetEffectTurns(this.effectTurns);
	}

	private void RemoveBuffIcon()
	{
		if(buffIcon != null)
			GameObject.Destroy(buffIcon.gameObject);
	}
}
