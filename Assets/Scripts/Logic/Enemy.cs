using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	public int enemyID;

	void Start()
	{
		data = DataManager.Instance.GetEnemyDataSet ().GetEnemyData (enemyID).Clone();
		data.currentHP = data.maxHP;
		data.battleType = BattleType.Both;
		data.weaponID = 1000 + Random.Range(1,6) * 100;
		data.magicIDs.Add(2001);
		data.magicIDs.Add(2002);
		data.magicIDs.Add(2003);
		data.AcquireItem(1,3);
		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);
		GetComponent<EnemyAI>().InitAI();

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("EnemyName", data.name);
		EventManager.Instance.PostEvent(EventDefine.EnemySpawn,args);
	}

	public EnemyData GetData()
	{
		return (EnemyData)data;
	}

	protected override void SelectCommand()
	{
		base.SelectCommand();
		commandToExecute = GetComponent<EnemyAI>().AISelectCommand();
		battleStatus = BattleStatus.Action;
	}
}
