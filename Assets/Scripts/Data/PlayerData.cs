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

	public PlayerData(int playerID)
	{
		switch(playerID)
		{
		case 0:	name = "Man";
				maxHP = 200;
				maxMP = 100;
				power = 15;
				skill = 15;
				agility = 10;
				toughness = 15;
				luck = 5;
				break;
		case 1: name = "Girl";
				maxHP = 100;
				maxMP = 200;
				power = 5;
				skill = 15;
				agility = 20;
				toughness = 10;
				luck = 10;
				break;
		}
		currentHP = maxHP;
		currentMP = maxMP;
		expMaxHP = 0;
		expMaxMP = 0;
		expPower = 0;
		expSkill = 0;
		expAgility = 0;
		expToughness = 0;
		expLuck = 0;
	}
}
