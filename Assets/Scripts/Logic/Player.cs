using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : BattleObject {
/*
 * 角色在战斗中的数据实体与逻辑。
 * */	
	public void Init(int playerID)
	{
		data = DataManager.Instance.GetPlayerDataSet().GetPlayerData(playerID).Clone();
		currentHP = ((PlayerData)data).currentHP;
		BattleLogic.players.Add(this);

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(playerID);
		UIEvent.InitHPBar(currentHP, data.maxHP, true);

		//temp
		AcquireItem(1,3);
	}

	protected override void SelectCommand()
	{
		base.SelectCommand();
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("PlayerName", data.name);
		EventManager.Instance.PostEvent(EventDefine.PlayerReady, args);
	}

}
