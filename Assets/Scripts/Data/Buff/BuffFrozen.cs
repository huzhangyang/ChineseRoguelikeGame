using UnityEngine;
using System;
using System.Collections;

public class BuffFrozen:Buff
{	
	protected override void Init()
	{	
		source.buffFrozenTime = 5;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为<冰冻>,暂时不能行动!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}

	protected override void Execute()
	{

	}
	
	protected override void Revert()
	{	
		source.buffFrozenTime = 0;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}不再<冰冻>了!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
	}
}
