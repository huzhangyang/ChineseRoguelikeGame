using UnityEngine;
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

	// 消息处理函数代理
	public delegate void EventHandler(MessageEventArgs arg);
		
	// 消息对应参数容器
	private class InvokeParam
	{
		public InvokeParam(int evt, MessageEventArgs arg)
		{
			this.evt = evt;
			this.args = arg;
		}
			
		public int evt;
		public MessageEventArgs args;
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
	private void PostEvent(int eventID, MessageEventArgs args)
	{
		waitEvent.Add(new InvokeParam(eventID, args));
	}

	public void PostEvent(BattleEvent evt, MessageEventArgs args)
	{
		PostEvent((int)evt, args);
	}

	public void PostEvent(BattleEvent evt)
	{
		PostEvent((int)evt, new MessageEventArgs());
	}

	public void PostEvent(UIEvent evt, MessageEventArgs args = null)
	{
		PostEvent((int)evt, args);
	}

	public void PostEvent(UIEvent evt)
	{
		PostEvent((int)evt, new MessageEventArgs());
	}

	// 立即执行事件
	public void InvokeEvent(int eventID, MessageEventArgs args)
	{
		List<EventHandler> handlerList;
		if (events.TryGetValue(eventID, out handlerList) == true)
		{
			for (int i = 0; i < handlerList.Count; ++i)
			{
				handlerList[i](args);
			}
		}
	}
		
	// 注册事件		
	private void RegisterEvent(int eventID, EventHandler handler)
	{
		if (events.ContainsKey(eventID) == false)
		{
			events.Add(eventID, new List<EventHandler>());
		}
		events[eventID].Add(handler);
	}

	public void RegisterEvent(BattleEvent evt, EventHandler handler)
	{
		RegisterEvent((int)evt, handler);
	}
	
	public void RegisterEvent(UIEvent evt, EventHandler handler)
	{
		RegisterEvent((int)evt, handler);
	}
		
	// 取消注册事件
	private void UnRegisterEvent(int eventID, EventHandler handler)
	{
		if (events.ContainsKey(eventID) == false)
		{
			return;
		}
		events[eventID].Remove(handler);
	}

	public void UnRegisterEvent(BattleEvent evt, EventHandler handler)
	{
		UnRegisterEvent((int)evt, handler);
	}
	
	public void UnRegisterEvent(UIEvent evt, EventHandler handler)
	{
		UnRegisterEvent((int)evt, handler);
	}
}

public class MessageEventArgs : EventArgs
{
	public Dictionary<string, object> messages;

	public MessageEventArgs()
	{
		messages = new Dictionary<string, object>();
	}
	
	public void AddMessage(string _key, object _value)
	{
		if ( messages.ContainsKey( _key ))
			messages[_key]= _value;
		else
			messages.Add( _key , _value );
	}
	
	public bool ContainsMessage(string _key)
	{
		if( messages.ContainsKey(_key) )
		{
			return true;
		}
		
		return false;		
	}
	
	public T GetMessage<T>(string _key)
	{
		T ret = default(T);
		if(messages.ContainsKey(_key) && messages[_key] is T)
		{
			ret = (T)messages[_key];
		}
		
		return ret;
	}
}