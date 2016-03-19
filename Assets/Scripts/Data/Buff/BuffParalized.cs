using UnityEngine;
using System;
using System.Collections;

public class BuffParalized:Buff
{	
	protected override void Init()
	{	
		source.accuracyMulti -= 0.25f;
		source.evasionMulti -= 0.25f;
		source.critMulti -= 0.25f;
		source.critResistMulti -= 0.25f;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为<麻痹>,能力下降!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Execute()
	{
		
	}
	
	protected override void Revert()
	{	
		source.accuracyMulti += 0.25f;
		source.evasionMulti += 0.25f;
		source.critMulti += 0.25f;
		source.critResistMulti += 0.25f;
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}不再<麻痹>了!", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
	}
}
