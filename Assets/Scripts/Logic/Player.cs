using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : BattleObject {
/*
 * 角色在战斗中的数据实体与逻辑。
 * */	
	protected PlayerData playerData;

	public void Init(int playerID)
	{
		data = DataManager.Instance.GetPlayerDataSet().GetPlayerData(playerID).Clone();
		playerData = data as PlayerData;

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(playerID);
		UIEvent.InitHPBar(maxHP, data.battleType);
		currentHP = maxHP;

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnPlayerSpawn, args);

		for(int i = 0; i < data.bornBuffs.Count; i++)
		{
			AddBuff(data.bornBuffs[i], -1);//添加固有BUFF
		}
		SkillHelper.CheckWeaponBuff(this);
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
