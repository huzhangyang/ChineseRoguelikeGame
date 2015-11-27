using UnityEngine;
using System.Collections;

public enum BattleEvent
{
	OnBattleEnter,//进入战斗
	OnBattleStart,//开始战斗
	OnBattleWin,//战斗胜利
	OnBattleLose,//战斗失败
	OnBattleFinish,//离开战斗

	OnPlayerSpawn,//敌人出现
	OnEnemySpawn,//敌人出现
	OnTimelineUpdate,//更新时间轴
	OnMessageUpdate,//更新战斗信息

	OnBuffActivated,//有BUFF被激活
	OnHPAutoRecover,//HP自动恢复
	OnMPAutoRecover,//MP自动恢复

	OnPlayerReady,//玩家就绪
	OnCommandShowUp,//显示指令面板
	OnBasicCommandSelected,//玩家点击了一个指令类别
	OnCommandClicked,//玩家点击了一个指令
	OnCommandSelected,//玩家或敌方选定了一个指令
	OnCommandExecute,//玩家或敌方执行了一个指令

	BattleObjectMiss,//有玩家或敌人未击中
	BattleObjectCritical,//有玩家或敌人造成暴击
	BattleObjectHurt,//有玩家或敌人受伤
	BattleObjectHeal,//有玩家或敌人恢复生命
	BattleObjectCounter,//有玩家或敌人反击
	BattleObjectDied,//有玩家或敌人死亡
	BattleObjectEscape,//有玩家或敌人逃跑
}