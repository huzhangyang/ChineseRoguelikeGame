using UnityEngine;
using System.Collections;

public class BuffEndure:Buff
{	
	protected override void Init()
	{
	}
	
	protected override void Execute()
	{
		source.damageTaken.interrupt = 0;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为<霸体>,没有被击退!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Revert()
	{
	}
}
