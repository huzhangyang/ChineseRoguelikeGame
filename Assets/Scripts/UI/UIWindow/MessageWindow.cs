using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour {
	
	Text message;
	List<string> msgList = new List<string>();
	const int MAX_MSG_COUNT = 20;
	
	void Awake()
	{
		message = this.GetComponentInChildren<Text>();
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.RegisterEvent(BattleEvent.OnMessageUpdate, OnMessageUpdate);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnMessageUpdate, OnMessageUpdate);
	}
	
	void OnBattleEnter(MessageEventArgs args)
	{
		ClearMessage();
	}

	void OnMessageUpdate(MessageEventArgs args)
	{
		string msg = args.GetMessage<string>("Message");
		msgList.Add(msg);
		while(msgList.Count > MAX_MSG_COUNT)
		{
			msgList.RemoveAt(0);
		}
		message.text = "";
		for(int i = 0; i < msgList.Count - 1; i++)
		{
			message.text += msgList[i] + "\n";
		}
		message.DOKill();
		message.DOText(message.text + "<color=yellow>" + msgList[msgList.Count - 1] + "</color>", 1, true).SetEase(Ease.Linear);
	}
		
	void ClearMessage()
	{
		message.text = "";
		msgList.Clear();	
	}

}
