using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleMessage : MonoBehaviour {

	Text message;

	void Start()
	{
		message = this.GetComponent<Text>();
		ClearMessage();
	}

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
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
		AddMessage(name + " 做出行动！");
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
