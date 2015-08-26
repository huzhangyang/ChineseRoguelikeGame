using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour {

	Text message;
	Scrollbar messageBar;

	void Awake()
	{
		message = this.GetComponentInChildren<Text>();
		messageBar = this.GetComponentInChildren<Scrollbar>();
	}

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(EventDefine.EnterBattle, OnEnterBattle);
		EventManager.Instance.RegisterEvent(EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent(EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent(EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent(EventDefine.BattleWin, OnBattleWin);
		EventManager.Instance.RegisterEvent(EventDefine.BattleLose, OnBattleLose);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(EventDefine.EnterBattle, OnEnterBattle);
		EventManager.Instance.UnRegisterEvent(EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent(EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent(EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectMiss, OnBattleObjectMiss);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectCritical, OnBattleObjectCritical);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectHurt, OnBattleObjectHurt);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectHeal, OnBattleObjectHeal);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectCounter, OnBattleObjectCounter);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleWin, OnBattleWin);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleLose, OnBattleLose);
	}

	void OnEnterBattle(MessageEventArgs args)
	{
		ClearMessage();
	}

	void OnEnemySpawn(MessageEventArgs args)
	{
		string name = args.GetMessage("EnemyName");
		AddMessage(name + " 出现了！");
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		string name = args.GetMessage("PlayerName");
		AddMessage(name + " 将作何选择？");
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		CommandType commandType = (CommandType)Convert.ToInt32(args.GetMessage("CommandType"));
		string commandName = args.GetMessage("CommandName");
		int skillOrItemID = Convert.ToInt32(args.GetMessage("SkillOrItemID"));
		switch(commandType)
		{//very temp handle!!
		case CommandType.UseSkill:
			AddMessage(name + " 使用了 " + DataManager.Instance.GetSkillDataSet().GetSkillData(skillOrItemID).name + "!");
			break;
		case CommandType.Defence:
			AddMessage(name + " 正在 " + commandName + "!");
			break;
		case CommandType.UseItem:
			if(DataManager.Instance.GetItemDataSet().IsWeapon(skillOrItemID))
				AddMessage(name + " 将武器更换为 " + DataManager.Instance.GetItemDataSet().GetWeaponData(skillOrItemID).name + "!");
			else
				AddMessage(name + " 使用了 " + DataManager.Instance.GetItemDataSet().GetItemData(skillOrItemID).name + "!");				
			break;
		case CommandType.Strategy:
			break;
		case CommandType.None:
			AddMessage(name + " 什么也没做!");
			break;
		}
	}

	void OnBattleObjectMiss(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		AddMessage("被" + name + "躲开了！");
	}

	void OnBattleObjectCritical(MessageEventArgs args)
	{
		AddMessage("会心一击！");
	}

	void OnBattleObjectHurt(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		int damage = Convert.ToInt32(args.GetMessage("Damage"));
		AddMessage(name + " 受到" + damage + "点伤害！");
	}

	void OnBattleObjectHeal(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		int amount = Convert.ToInt32(args.GetMessage("Amount"));
		int curHP = Convert.ToInt32(args.GetMessage("CurrentHP"));
		AddMessage(name + " 回复了 " + amount + "点生命，现在生命值为 " + curHP + "!");
	}

	void OnBattleObjectCounter(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		AddMessage(name + " 看穿了攻击，并做出反击！");
	}

	void OnBattleObjectDied(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
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
		messageBar.value = 0;
	}
}
