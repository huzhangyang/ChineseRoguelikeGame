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
		UIEvent.InitHPBar(maxHP, data.battleType);
		currentHP = maxHP;

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnPlayerSpawn, args);


		if(playerID == 1)
		{
			AddBuff(1, -1);//为妹子添加不死buff
		}
	}

	protected override void SelectCommand()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Player", this);
		EventManager.Instance.PostEvent(BattleEvent.OnPlayerReady, args);
	}

	public override bool IsBoss()
	{
		return false;
	}

}
