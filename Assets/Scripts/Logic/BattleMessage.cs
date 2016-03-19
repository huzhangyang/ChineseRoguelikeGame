using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour {

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent(BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.RegisterEvent(BattleEvent.OnHPAutoRecover, OnHPAutoRecover);
		EventManager.Instance.RegisterEvent(BattleEvent.OnMPAutoRecover, OnMPAutoRecover);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBuffActivated, OnBuffActivated);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBuffDeactivated, OnBuffDeactivated);
		EventManager.Instance.RegisterEvent(BattleEvent.OnEffectExecuted, OnEffectExecuted);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectInterrupted, OnBattleObjectInterrupted);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleWin, OnBattleWin);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleLose, OnBattleLose);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnHPAutoRecover, OnHPAutoRecover);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnMPAutoRecover, OnMPAutoRecover);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBuffActivated, OnBuffActivated);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBuffDeactivated, OnBuffDeactivated);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnEffectExecuted, OnEffectExecuted);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectInterrupted, OnBattleObjectInterrupted);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleWin, OnBattleWin);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleLose, OnBattleLose);
	}

	void OnEnemySpawn(MessageEventArgs args)
	{
		BattleObject bo = args.GetMessage<BattleObject>("Object");
		AddMessage(bo.GetName() + " 出现了！");
	}

	void OnHPAutoRecover(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		int amount = args.GetMessage<int>("Amount");
		AddMessage(name + "的伤口正在痊愈， 恢复了" + amount + "点生命！");
	}

	void OnMPAutoRecover(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		int amount = args.GetMessage<int>("Amount");
		AddMessage(name + "的灵力自动恢复了" + amount + "点！");
	}

	void OnBuffActivated(MessageEventArgs args)
	{
		string message = args.GetMessage<string>("Message");
		AddMessage(message);
	}

	void OnBuffDeactivated(MessageEventArgs args)
	{
		string message = args.GetMessage<string>("Message");
		AddMessage(message);
	}

	void OnEffectExecuted(MessageEventArgs args)
	{
		string message = args.GetMessage<string>("Message");
		AddMessage(message);
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
		AddMessage(name + " 回复了 " + amount + "点生命!");
	}

	void OnBattleObjectCounter(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		AddMessage(name + " 看穿了攻击，并做出反击！");
	}

	void OnBattleObjectInterrupted(MessageEventArgs args)
	{
		string name = args.GetMessage<string>("Name");
		AddMessage(name + " 的行动被打断了！");
	}

	void OnBattleObjectDied(MessageEventArgs args)
	{
		BattleObject bo = args.GetMessage<BattleObject>("Object");
		AddMessage(bo.GetName() + " 不敌！");
	}

	void OnBattleWin(MessageEventArgs args)
	{
		AddMessage("战斗胜利！");
	}

	void OnBattleLose(MessageEventArgs args)
	{
		AddMessage("战斗失败。");
	}

	void AddMessage(string msg)
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Message",msg);
		EventManager.Instance.PostEvent (UIEvent.OnMessageUpdate, args);
	}
}
