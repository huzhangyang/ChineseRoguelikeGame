using UnityEngine;
using System;
using System.Collections;

public class BuffFrozen:Buff
{	
	protected override void Execute()
	{
		GlobalManager.Instance.StartCoroutine(RemoveBuffFrozen());
		source.isBuffFrozen = true;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}被冰冻了！", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Revert()
	{	
		GlobalManager.Instance.StopCoroutine(RemoveBuffFrozen());
		source.isBuffFrozen = false;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}解除了冰冻！", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
	}

	IEnumerator RemoveBuffFrozen()
	{
		yield return new WaitForSeconds(5);
		Revert();
	}
}
