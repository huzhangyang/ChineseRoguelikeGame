using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int timelinePosition
	{
		set{
			timelineAvatar.rectTransform.anchoredPosition.Set(value, timelineAvatar.transform.localPosition.y);
		}
		get{
			return (int)timelineAvatar.rectTransform.anchoredPosition.x;
		}
	}//max:500

	PlayerData data = new PlayerData();
	Image timelineAvatar;
	
	public PlayerData GetData()
	{
		return data;
	}

	public void SetAvatar(GameObject avatar)
	{
		timelineAvatar = avatar.GetComponent<Image>();
		timelineAvatar.sprite = this.GetComponent<Image> ().sprite;
		timelineAvatar.rectTransform.anchoredPosition = Vector2.zero;
	}
}
