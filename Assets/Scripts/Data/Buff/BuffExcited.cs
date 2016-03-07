using UnityEngine;
using System.Collections;

public class BuffExcited:Buff
{	
	protected override void Init()
	{
		source.attackMulti += 0.2f;
		source.defenceMulti += 0.2f;
		source.speedMulti += 0.2f;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为<激昂>,能力上升!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Execute()
	{
		
	}
	
	protected override void Revert()
	{
		source.attackMulti -= 0.2f;
		source.defenceMulti -= 0.2f;
		source.speedMulti -= 0.2f;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}不再<激昂>了!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
	}
}
