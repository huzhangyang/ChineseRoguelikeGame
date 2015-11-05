using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour {

	Text message;

	void Awake()
	{
		message = this.GetComponentInChildren<Text>();
	}

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.RegisterEvent(BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent(BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleWin, OnBattleWin);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleLose, OnBattleLose);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleWin, OnBattleWin);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleLose, OnBattleLose);
	}

	void OnBattleEnter(MessageEventArgs args)
	{
		ClearMessage();
	}

	void OnEnemySpawn(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("EnemyName");
		AddMessage(name + " 出现了！");
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		string message = args.GetMessage<string>("Message");
		AddMessage(message);
	}

	void OnBattleObjectMiss(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		AddMessage("被" + name + "躲开了！");
	}

	void OnBattleObjectCritical(MessageEventArgs args)
	{
		AddMessage("会心一击！");
	}

	void OnBattleObjectHurt(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		int damage = args.GetMessage<int>("Damage");
		AddMessage(name + " 受到" + damage + "点伤害！");
	}

	void OnBattleObjectHeal(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		int amount = args.GetMessage<int>("Amount");
		int curHP = args.GetMessage<int>("CurrentHP");
		AddMessage(name + " 回复了 " + amount + "点生命，现在生命值为 " + curHP + "!");
	}

	void OnBattleObjectCounter(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		AddMessage(name + " 看穿了攻击，并做出反击！");
	}

	void OnBattleObjectDied(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		AddMessage(name + " 不敌！");
	}

	void OnBattleWin(MessageEventArgs args)
	{
		AddMessage("战斗胜利！");
	}

	void OnBattleLose(MessageEventArgs args)
	{
		AddMessage("战斗失败。");
	}

	void ClearMessage()
	{
		message.text = "";
	}

	void AddMessage(string msg)
	{
		message.text += msg +"\n";
	}
}
