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
		data = DataManager.Instance.GetPlayerDataSet().GetPlayerData(playerID).Clone();
		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);
	}
	
	public PlayerData GetData()
	{
		return (PlayerData)data;
	}

	protected override void SelectCommand()
	{
		base.SelectCommand();
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("PlayerName", data.name);
		EventManager.Instance.PostEvent(EventDefine.PlayerReady, args);
	}

}
