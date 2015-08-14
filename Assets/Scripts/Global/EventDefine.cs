using UnityEngine;
using System.Collections;

public enum EventDefine
{
	EnterBattle,//进入战斗
	StartBattle,//开始战斗
	UpdateTimeline,//更新时间轴
	PlayerReady,//玩家就绪
	SelectCommand,//玩家或敌方选定了一个指令
	ExecuteCommand,//玩家或敌方执行了一个指令
}