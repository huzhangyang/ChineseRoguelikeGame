using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : BattleObject {
/*
 * 角色在战斗中的数据实体与逻辑。
 * */	
	PlayerData data = new PlayerData();
	
	public PlayerData GetData()
	{
		return data;
	}

	protected override void OnUpdateTimeline(MessageEventArgs args)
	{
		base.OnUpdateTimeline(args);
		if(timelinePosition >= 400)
		{
			EventManager.Instance.PostEvent(EventDefine.PlayerReady);
		}	
	}
}
