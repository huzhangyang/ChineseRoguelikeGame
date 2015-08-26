﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class ObjectData{

/*
 * 物体数据基类。怪物和玩家继承此类。
*/

	//basic information
	public string name;//名称
	public int id;//玩家或怪物编号
	public int talentID;//天赋ID
	public int natureID;//主性格ID
	public BattleType battleType;//战斗类型
	//basic attribute
	public int maxHP;//最大生命值
	public int power;//力量or灵力
	public int agility;//敏捷
	public int toughness;//强韧
	public int insight;//洞察
	public int skill;//技术
	public int luck;//运气
	public int eloquence;//口才
	//battle attribute
	public int currentHP;//当前生命值
	//Equipment
	public int weaponID;
	public int ring1ID;
	public int ring2ID;
	public List<int> magicIDs = new List<int>();
	protected Dictionary<int, int> items = new Dictionary<int, int>();//id, amount

	public int GetItemCount(int itemID)
	{
		if( !items.ContainsKey(itemID))
		{
			return 0;
		}
			
		return items[itemID];
	}

	public void ConsumeItem(int itemID)
	{
		if(!items.ContainsKey(itemID) ||items[itemID] <= 0)
		{
			Debug.LogError("Consume an nonexisting item " + itemID + "!");
			return;
		}
		items[itemID]--;
	}

	public void AcquireItem(int itemID, int count)
	{
		if(!items.ContainsKey(itemID))
			items.Add(itemID,count);
		else
			items[itemID] += count;
	}
}

public enum BattleType{Physical, Magical, Both}
