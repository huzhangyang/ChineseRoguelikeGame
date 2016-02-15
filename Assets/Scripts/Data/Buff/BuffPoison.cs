using UnityEngine;
using System.Collections;

public class BuffPoison:Buff
{	
	int poisonTurns;

	protected override void Init()
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}中毒了！", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Execute()
	{
		poisonTurns++;
		int previousHP = source.currentHP;
		source.currentHP -= Mathf.RoundToInt(source.maxHP * poisonTurns * 0.05f);

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为中毒失去了{1}点生命！", source.GetName(), previousHP - source.currentHP));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
	}
	
	protected override void Revert()
	{

	}
}
