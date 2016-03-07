using UnityEngine;
using System.Collections;

public class BuffRevive:Buff
{	
	protected override void Init()
	{
	}

	protected override void Execute()
	{
		source.currentHP = 1;
		source.timelinePosition = -10000;
		source.ClearBuff();
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",source.GetName() + "因为<灵气体质>,复活了!");
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Revert()
	{
	}
}
