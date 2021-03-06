﻿using UnityEngine;
using System.Collections;

public enum BattleEvent
{
	OnBattleEnter = 0,//进入战斗
	OnBattleStart,//开始战斗
	OnBattleWin,//战斗胜利
	OnBattleLose,//战斗失败
	OnBattleFinish,//离开战斗

	OnPlayerSpawn,//敌人出现
	OnEnemySpawn,//敌人出现
	OnTimelineUpdate,//更新时间轴

	OnBuffActivated,//有BUFF被激活
	OnBuffDeactivated,//有BUFF被激活
	OnEffectExecuted,//有特效被激活
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
	BattleObjectInterrupted,//有玩家或敌人被打断
	BattleObjectDied,//有玩家或敌人死亡
	BattleObjectEscape,//有玩家或敌人逃跑
}

public enum UIEvent
{
	OpenUIWindow = 100,
	OnItemClicked,
	OnSwitchLeader,
	OnMessageClear,//清除信息
	OnMessageSet,//直接显示某信息
	OnMessageUpdate,//更新某信息
	OnMessageHide,//隐藏信息
	OnMessageShow,//显示信息
}

public enum GameEvent
{
	PlayDialogue = 200,
}

public enum UIWindowID
{
	IntroWindow,
	LoadingWindow,
	MapWindow,
	BattleWindow,
	ChatWindow

}