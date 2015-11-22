using UnityEngine;
using System.Collections;

public class BuffEffectRevive:BuffEffect 
{
	public override void Init(BattleObject source, string Argument)
	{
		buffSource = source;
	}
	
	public override void Execute()
	{
		buffSource.currentHP = 1;
		buffSource.timelinePosition = -10000;
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",buffSource.GetName() + "的‘不死’使她复活了！");
		EventManager.Instance.PostEvent (BattleEvent.OnMessageUpdate, args);
	}
	
	public override void Revert()
	{
	}
}
