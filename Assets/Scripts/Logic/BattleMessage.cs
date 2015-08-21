using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour {

	Text message;
	Scrollbar messageBar;

	void Start()
	{
		message = this.GetComponentInChildren<Text>();
		messageBar = this.GetComponentInChildren<Scrollbar>();
		ClearMessage();
	}

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.RegisterEvent(EventDefine.BattleObjectDied, OnBattleObjectDied);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent(EventDefine.BattleObjectDied, OnBattleObjectDied);
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
		if(commandType == CommandType.UseSkill)
		{
			AddMessage(name + " 使用了 " + commandName + "!");
		}
		else if(commandType ==  CommandType.None)
		{
			AddMessage(name + " 什么也没做!");
		}
	}
	void OnBattleObjectDied(MessageEventArgs args)
	{
		string name = args.GetMessage("Name");
		AddMessage(name + " 不敌！");
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
