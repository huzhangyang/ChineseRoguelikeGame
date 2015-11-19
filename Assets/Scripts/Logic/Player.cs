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

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(playerID);
		currentHP = maxHP;
		UIEvent.InitHPBar(currentHP, maxHP, data.battleType);

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnPlayerSpawn, args);
	}

	protected override void SelectCommand()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("PlayerName", data.name);
		EventManager.Instance.PostEvent(BattleEvent.OnPlayerReady, args);
	}

}
