using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */	
	public BattleStatus battleStatus = BattleStatus.BattleNotStart;

	public int timelinePosition
	{
		set{
			timelineAvatar.rectTransform.anchoredPosition = new Vector2(value, 0);
		}
		get{
			return (int)timelineAvatar.rectTransform.anchoredPosition.x;
		}
	}//max:500

	Image timelineAvatar;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, OnStartBattle);
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
		
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnStartBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
	}

	void OnStartBattle(MessageEventArgs args)
	{
		this.battleStatus = BattleObject.BattleStatus.Prepare;
	}

	protected virtual void OnUpdateTimeline(MessageEventArgs args)
	{
		if(this.battleStatus == BattleObject.BattleStatus.Prepare)
			this.timelinePosition += 10;
		if(timelinePosition >= 400)
		{
			battleStatus = BattleObject.BattleStatus.Ready;
		}
	}

	public void SetAvatar(GameObject avatar)
	{
		timelineAvatar = avatar.GetComponent<Image>();
		timelineAvatar.sprite = this.GetComponent<Image> ().sprite;
		timelineAvatar.rectTransform.anchoredPosition = Vector2.zero;
	}

	public enum BattleStatus
	{
		BattleNotStart,//战斗尚未开始
		Prepare,//等待选择行动
		Ready,//选择行动中
		Action,//即将行动
		Pause,//暂时无法行动
	}
}

