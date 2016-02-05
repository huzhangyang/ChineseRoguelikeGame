using UnityEngine;
using System.Collections;

public class BuffBleed:Buff
{	
	protected override void Execute()
	{
		if(source.GetBattleType() != BattleType.Magical)
		{		
			int previousHP = source.currentHP;
			source.currentHP -= Mathf.RoundToInt(source.maxHP * 0.05f);
			MessageEventArgs args = new MessageEventArgs ();
			args.AddMessage("Message",string.Format("{0}因为流血失去了{1}点生命！", source.GetName(), previousHP - source.currentHP));
			EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
		}
	}
	
	protected override void Revert()
	{
	}
}
