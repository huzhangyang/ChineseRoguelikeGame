using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */	
	public BattleStatus battleStatus = BattleObject.BattleStatus.Prepare;
	public bool isPaused = true;

	public int timelinePosition
	{
		set
		{
			timelineAvatar.rectTransform.anchoredPosition = new Vector2(value, 0);
		}
		get
		{
			return (int)timelineAvatar.rectTransform.anchoredPosition.x;
		}
	}//max:500

	protected ObjectData data;
	protected Image timelineAvatar;
	protected Slider HPBar;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);		
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
	}

	protected void OnUpdateTimeline(MessageEventArgs args)
	{
		if(!this.isPaused)
			this.timelinePosition += data.agility;
		if(timelinePosition >= 400 && battleStatus == BattleStatus.Prepare)
		{
			SelectCommand();
		}
		if(timelinePosition >= 500 && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected virtual void SelectCommand()
	{
		timelinePosition = 400;
		battleStatus = BattleStatus.Ready;
	}

	protected virtual void ExecuteCommand()
	{
		EventManager.Instance.PostEvent(EventDefine.ExecuteCommand);
		timelinePosition = 0;
		battleStatus = BattleStatus.Prepare;
	}

	protected void SetHPBar()
	{
		HPBar = this.gameObject.GetComponentInChildren<Slider> ();
		HPBar.maxValue = data.maxHP;
		HPBar.value = data.currentHP;
	}

	public void SetAvatar(GameObject avatar)
	{
		timelineAvatar = avatar.GetComponent<Image>();
		timelineAvatar.sprite = this.GetComponent<Image> ().sprite;
		timelineAvatar.rectTransform.anchoredPosition = Vector2.zero;
	}

	public enum BattleStatus
	{
		Prepare,//等待选择行动(0~400)
		Ready,//选择行动中(400)
		Action,//即将行动(400~500)
	}
}

