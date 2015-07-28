using UnityEngine;
using System.Collections;
[System.Serializable]
public class PlayerData : ObjectData {
/*
 * 玩家数据。除了通用属性外，还有一些玩家独有的属性。
*/
	//attribute exp
	public int expMaxHP;//最大生命值
	public int expMaxMP;//最大灵力值
	public int expPower;//力量
	public int expSkill;//技术
	public int expAgility;//敏捷
	public int expToughness;//韧性
	public int expLuck;//运气
	//for adventure

	//Equipment
	public Ring ring1;
	public Ring ring2;

	public PlayerData()
	{
	}
}
