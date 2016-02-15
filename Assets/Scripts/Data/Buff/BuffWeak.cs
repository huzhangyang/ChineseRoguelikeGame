using UnityEngine;
using System.Collections;

public class BuffWeak:Buff
{	
	protected override void Init()
	{
		if(source.GetBattleType() != BattleType.Physical)
		{		
			source.maxHPMulti -= 0.5f;
			MessageEventArgs args = new MessageEventArgs ();
			args.AddMessage("Message",string.Format("{0}因为气虚失去了一半生命上限！", source.GetName()));
			EventManager.Instance.PostEvent (BattleEvent.OnBuffActivated, args);
		}
	}

	protected override void Execute()
	{

	}
	
	protected override void Revert()
	{
		if(source.GetBattleType() != BattleType.Physical)
		{		
			source.maxHPMulti += 0.5f;
			MessageEventArgs args = new MessageEventArgs ();
			args.AddMessage("Message",string.Format("{0}不再气虚了！", source.GetName()));
			EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
		}
	}
}
