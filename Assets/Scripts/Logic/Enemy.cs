using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	protected EnemyAI AI;

	public void Init(int enemyID)
	{
		data = DataManager.Instance.GetEnemyDataSet ().GetEnemyData (enemyID).Clone();
		BattleLogic.enemys.Add(this);

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(enemyID);
		currentHP = maxHP;
		UIEvent.InitHPBar(currentHP, maxHP, ((EnemyData)data).isBoss);

		AI = this.GetComponent<EnemyAI>();
		AI.InitAI();

		//temp
		data.weaponID = 1000 + Random.Range(1,6) * 100;
		data.magicIDs.Add(2001);
		data.magicIDs.Add(2002);
		data.magicIDs.Add(2003);
		AcquireItem(1,1);

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("EnemyName", data.name);
		EventManager.Instance.PostEvent(EventDefine.EnemySpawn,args);
	}

	protected override void SelectCommand()
	{
		base.SelectCommand();
		commandToExecute = AI.AISelectCommand();
		battleStatus = BattleStatus.Action;
	}
}
