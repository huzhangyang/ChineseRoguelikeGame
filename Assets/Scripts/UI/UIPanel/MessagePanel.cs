using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {
	
	Text message;
	List<string> msgList = new List<string>();
	const int MAX_MSG_COUNT = 25;
	const int ALIGNMENT_MSG_COUNT = 15;
	
	void Awake()
	{
		message = this.GetComponentInChildren<Text>();
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleEnter, OnMessageClear);
		EventManager.Instance.RegisterEvent(BattleEvent.OnBattleFinish, OnMessageClear);
		EventManager.Instance.RegisterEvent(BattleEvent.OnMessageUpdate, OnMessageUpdate);
		EventManager.Instance.RegisterEvent(UIEvent.OnMessageShow, OnMessageShow);
		EventManager.Instance.RegisterEvent(UIEvent.OnMessageClear, OnMessageClear);

	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleEnter, OnMessageClear);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnBattleFinish, OnMessageClear);
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnMessageUpdate, OnMessageUpdate);
		EventManager.Instance.UnRegisterEvent(UIEvent.OnMessageShow, OnMessageShow);
		EventManager.Instance.UnRegisterEvent(UIEvent.OnMessageClear, OnMessageClear);
	}
	
	void OnMessageClear(MessageEventArgs args)
	{
		ClearMessage();
	}

	void OnMessageShow(MessageEventArgs args)
	{
		string msg = args.GetMessage<string>("Message");
		message.text = msg;
	}

	void OnMessageUpdate(MessageEventArgs args)
	{
		string msg = args.GetMessage<string>("Message");
		//handle msgList
		msgList.Add(msg);
		while(msgList.Count > MAX_MSG_COUNT)
		{
			msgList.RemoveAt(0);
		}
		//handle alignment
		if (msgList.Count >= ALIGNMENT_MSG_COUNT)
		{
			message.alignment = TextAnchor.LowerLeft;
		} 
		else
		{
			message.alignment = TextAnchor.UpperLeft;
		}
		//reassamble message
		message.text = "";
		for(int i = 0; i < msgList.Count - 1; i++)
		{
			message.text += msgList[i] + "\n";
		}
		//highlight latest message with animation
		message.DOKill();
		message.DOText(message.text + "<color=yellow>" + msgList[msgList.Count - 1] + "</color>", 0.5f).SetEase(Ease.Linear);
	}
		
	void ClearMessage()
	{
		message.text = "";
		msgList.Clear();	
	}

}
