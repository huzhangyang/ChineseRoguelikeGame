﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataSet
{
	public List<PlayerData> dataSet = new List<PlayerData>();
	
	public PlayerData GetPlayerData(int playerID)
	{
		foreach (PlayerData data in dataSet)
		{
			if (data.id == playerID)
				return data;
		}
		Debug.LogError("Asking for an non-exist player:" + playerID);
		return null;
	}
}

[System.Serializable]
public class PlayerData : ObjectData {
/*
 * 玩家数据。除了通用属性外，还有一些玩家独有的属性。
*/
	//attribute exp
	public int expMaxHP;//生命成长值
	public int expPower;//力量成长值
	public int expAgility;//敏捷成长值
	public int expToughness;//韧性成长值
	public int expInsight;//洞察成长值
	public int expSkill;//技术成长值
	public int expLuck;//运气成长值
	public int expEloquence;//口才成长值

	public void InitWithID(int playerID)
	{
		switch(playerID)
		{
		case 0:	name = "Man";
			maxHP = 1500;
			power = 1500;
			agility = 100;
			toughness = 1500;
			insight = 1000; 
			skill = 150;
			luck = 50;
			eloquence = 500;
			
			battleType = BattleType.Physical;
			weaponID = 1000 + Random.Range(1,6) * 100 + 1;
			
			break;
		case 1: name = "Girl";
			maxHP = 1000;
			power = 1000;
			agility = 150;
			toughness = 1000;
			insight = 1500; 
			skill = 50;
			luck = 150;
			eloquence = 1000;
			
			battleType = BattleType.Magical;
			weaponID = 1000;
			magicIDs.Add(2001);
			magicIDs.Add(2002);
			magicIDs.Add(2003);
			break;
		}
		AcquireItem(1,3);
		this.id = playerID;
		currentHP = maxHP;
		expMaxHP = 0;
		expPower = 0;
		expAgility = 0;
		expToughness = 0;
		expInsight = 0;
		expSkill = 0;
		expLuck = 0;
		expEloquence = 0;
	}

	public PlayerData Clone()
	{
		return(PlayerData)this.MemberwiseClone();
	}
}
