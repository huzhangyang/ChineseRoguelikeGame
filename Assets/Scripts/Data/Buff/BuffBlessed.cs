using UnityEngine;
using System.Collections;

public class BuffBlessed:Buff
{	
	protected override void Init()
	{
	}
	
	protected override void Execute()
	{
		source.damageTaken.dmg = 0;
		source.damageTaken.interrupt = 0;
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",string.Format("{0}因为祝福完全抵御了此次伤害！", source.GetName()));
		EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
		source.buffList.Remove(this);
	}
	
	protected override void Revert()
	{
	}
}
