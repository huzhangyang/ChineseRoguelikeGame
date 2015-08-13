using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : BattleObject {
/*
 * 角色在战斗中的数据实体与逻辑。
 * */	
	public int playerID;

	void Start()
	{
		data = new PlayerData(playerID);
		SetHPBar ();
	}
	
	public PlayerData GetData()
	{
		return (PlayerData)data;
	}

	protected override void OnUpdateTimeline(MessageEventArgs args)
	{
		base.OnUpdateTimeline(args);
		if(timelinePosition >= 400)
		{
			MessageEventArgs _args = new MessageEventArgs();
			_args.AddMessage("PlayerName",data.name);
			EventManager.Instance.PostEvent(EventDefine.PlayerReady,_args);
		}	
	}
}
