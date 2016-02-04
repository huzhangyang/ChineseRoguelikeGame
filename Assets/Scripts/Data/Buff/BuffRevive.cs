using UnityEngine;
using System.Collections;

public class BuffRevive:Buff
{	
	protected override void Execute()
	{
		source.currentHP = 1;
		source.timelinePosition = -10000;
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",source.GetName() + "的‘不死’使她复活了！");
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Revert()
	{
	}
}
