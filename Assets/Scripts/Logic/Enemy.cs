using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	protected EnemyAI AI;
	protected EnemyData enemyData;

	public void Init(int enemyID)
	{
		data = DataManager.Instance.GetEnemyDataSet ().GetEnemyData (enemyID).Clone();
		enemyData = data as EnemyData;
		isEnemy = true;

		UIEvent = this.GetComponent<BattleObjectUIEvent>();
		UIEvent.Init(enemyData.imageID);
		UIEvent.InitEnemyHPBar(maxHP, data.battleType);
		currentHP = maxHP;

		AI = this.GetComponent<EnemyAI>();
		AI.InitAI(enemyData.aiID);

		for(int i = 0; i < data.bornBuffs.Count; i++)
		{
			AddBuff(data.bornBuffs[i], -1);//添加固有BUFF
		}
		SkillHelper.CheckWeaponBuff(this);

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Object", this);
		EventManager.Instance.PostEvent(BattleEvent.OnEnemySpawn, args);
	}

	protected override void SelectCommand()
	{
		AI.AutoSelectCommand();
		battleStatus = BattleStatus.Action;
	}

	public override bool IsBoss()
	{
		return enemyData.isBoss;
	}
}
