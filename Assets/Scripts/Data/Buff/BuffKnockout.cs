using UnityEngine;
using System;
using System.Collections;

public class BuffKnockout:Buff
{	
	protected override void Init()
	{	
		source.timelinePosition = 0;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}被击倒了！", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Execute()
	{
		source.damageTaken.isCrit = true;
	}
	
	protected override void Revert()
	{	
	}
}
