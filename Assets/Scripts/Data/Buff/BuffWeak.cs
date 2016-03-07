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
			args.AddMessage("Message",string.Format("{0}因为<气虚>,灵气上限下降!", source.GetName()));
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
			args.AddMessage("Message",string.Format("{0}不再<气虚>了!", source.GetName()));
			EventManager.Instance.PostEvent (BattleEvent.OnBuffDeactivated, args);
		}
	}
}
