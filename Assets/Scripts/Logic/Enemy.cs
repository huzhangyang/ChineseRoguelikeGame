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
		data = DataManager.Instance.GetEnemyDataSet ().GetEnemyData (enemyID);
		data.currentHP = data.maxHP;
		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);

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
		AISelectCommand();
	}

	void AISelectCommand()
	{
		battleStatus = BattleStatus.Action;
	}
}
