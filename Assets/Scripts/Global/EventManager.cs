﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class EventManager : MonoBehaviour{

/*
事件管理器
*/
	
	private static EventManager s_Instance;
	public EventManager() { s_Instance = this; }
	public static EventManager Instance { get { return s_Instance; } }
		
	// 消息参数,所以参数将继承于EventArgs
	public class EventStringArgs : EventArgs
	{
		public Dictionary<string, string> msg = new Dictionary<string, string>();
	}

	// 消息处理函数代理
	public delegate void EventHandler(EventArgs arg);
		
	// 消息对应参数容器
	private class InvokeParam
	{
		public InvokeParam(int evt, EventArgs arg)
		{
			this.evt = evt;
			this.args = arg;
		}
			
		public int evt;
		public EventArgs args;
	}

	private Dictionary<int, List<EventHandler>> events = new Dictionary<int, List<EventHandler>>(); // 保存所有消息处理的字典
	private List<InvokeParam> waitEvent = new List<InvokeParam>(); // 消息队列

	public void Update()
	{
		// 遍历等待处理消息
		List<EventHandler> handlerList;
		InvokeParam invoke;
		for (int i = 0; i < waitEvent.Count; ++i)
		{
			invoke = waitEvent[i];
			if (events.TryGetValue(invoke.evt, out handlerList) == true)
			{
				for (int j = 0; j < handlerList.Count; ++j)
				{
					handlerList[j](invoke.args);
				}
			}
		}
		waitEvent.Clear();
	}
		
	public void Release()
	{
		events.Clear();
		waitEvent.Clear();
	}
		
	// 将事件加入消息队列
	public void PostEvent(EventDefine evt, EventArgs args)
	{
		waitEvent.Add(new InvokeParam((int)evt, args));
	}

	// 立即执行事件
	public void InvokeEvent(EventDefine evt, EventArgs args)
	{
		int _event = (int)evt;
		List<EventHandler> handlerList;
		if (events.TryGetValue(_event, out handlerList) == true)
		{
			for (int i = 0; i < handlerList.Count; ++i)
			{
				handlerList[i](args);
			}
		}
	}
		
	// 注册事件		
	public void RegisterEvent(EventDefine evt, EventHandler handler)
	{
		int _event = (int)evt;
		if (events.ContainsKey(_event) == false)
		{
			events.Add(_event, new List<EventHandler>());
		}
		events[_event].Add(handler);
	}
		
	// 取消注册事件
	public void UnRegisterEvent(EventDefine evt, EventHandler handler)
	{
		int _event = (int)evt;
		if (events.ContainsKey(_event) == false)
		{
			return;
		}
		events[_event].Remove(handler);
	}
}
