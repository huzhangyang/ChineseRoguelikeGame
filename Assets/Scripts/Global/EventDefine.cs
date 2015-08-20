using UnityEngine;
using System.Collections;

public enum EventDefine
{
	EnterBattle,//进入战斗
	EnemySpawn,//敌人出现
	StartBattle,//开始战斗
	UpdateTimeline,//更新时间轴
	PlayerReady,//玩家就绪
	ShowAvailableCommands,//显示全部可用指令
	ClickCommand,//玩家点击了一个指令
	SelectCommand,//玩家或敌方选定了一个指令
	ExecuteCommand,//玩家或敌方执行了一个指令
}