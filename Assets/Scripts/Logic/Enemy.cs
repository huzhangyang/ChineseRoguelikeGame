using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int enemyID;
	public int timelinePosition
	{
		set{
			timelineAvatar.rectTransform.anchoredPosition.Set(value, timelineAvatar.transform.localPosition.y);
			}
		get{
			return (int)timelineAvatar.rectTransform.anchoredPosition.x;
			}
	}//max:500

	EnemyData data = new EnemyData();
	Image timelineAvatar;

	public EnemyData GetData()
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
