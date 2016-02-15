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
		isEnemy = true;

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(enemyID);
		UIEvent.InitEnemyHPBar(maxHP, data.battleType);
		currentHP = maxHP;

		AI = this.GetComponent<EnemyAI>();
		AI.InitAI();

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnEnemySpawn, args);
	}

	protected override void SelectCommand()
	{
		AI.AISelectCommand();
		battleStatus = BattleStatus.Action;
	}
}
