using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	public int enemyID;

	void Awake()
	{
		data = DataManager.Instance.GetEnemyDataSet ().GetEnemyData (enemyID);
		SetHPBar ();
	}

	public EnemyData GetData()
	{
		return (EnemyData)data;
	}

	protected override void SelectCommand()
	{
		base.SelectCommand();
		//TODO AISelectCommand();
		EventManager.Instance.PostEvent(EventDefine.SelectCommand);
	}
}
