using UnityEngine;
using System.Collections;

public class BuffBurnt:Buff
{	
	protected override void Init()
	{
		source.attackMulti -= 0.5f;
		source.defenceMulti -= 0.5f;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为<烧伤>,能力下降!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Execute()
	{
		
	}
	
	protected override void Revert()
	{
		source.attackMulti += 0.5f;
		source.defenceMulti += 0.5f;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}不再<烧伤>了!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
	}
}
