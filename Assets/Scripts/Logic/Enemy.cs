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

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(enemyID);
		UIEvent.InitEnemyHPBar(maxHP, data.battleType);
		currentHP = maxHP;

		AI = this.GetComponent<EnemyAI>();
		AI.InitAI();

		//temp
		data.weaponID = 1000 + Random.Range(1,9) * 100;
		data.magicIDs.Add(2001);
		data.magicIDs.Add(2002);
		data.magicIDs.Add(2003);
		AcquireItem(1,1);

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnEnemySpawn, args);
	}

	protected override void SelectCommand()
	{
		commandToExecute = AI.AISelectCommand();
		battleStatus = BattleStatus.Action;
	}
}
