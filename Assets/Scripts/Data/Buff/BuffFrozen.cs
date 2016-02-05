using UnityEngine;
using System;
using System.Collections;

public class BuffFrozen:Buff
{	
	protected override void Execute()
	{
		GlobalManager.Instance.StartCoroutine(RemoveBuffFrozen());
		source.isBuffFrozen = true;
	}
	
	protected override void Revert()
	{	
		GlobalManager.Instance.StopCoroutine(RemoveBuffFrozen());
		source.isBuffFrozen = false;
	}

	IEnumerator RemoveBuffFrozen()
	{
		yield return new WaitForSeconds(5);
		Revert();
	}
}
