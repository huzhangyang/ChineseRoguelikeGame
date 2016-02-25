using UnityEngine;
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
	public int stamina;//体力
	public int power;//力量
	public int agility;//敏捷
	public int skill;//技巧
	public int toughness;//韧性
	public int insight;//洞察
	public int eloquence;//口才
	public int luck;//运气
	//extra attribute
	public float slashMulti = 1;
	public float thrustMulti = 1;
	public float bluntMulti = 1;
	public float yinMulti = 1;	
	public float yangMulti = 1;
	public float neutralMulti = 1;
	//Equipment
	public int weaponID;
	public int ring1ID;
	public int ring2ID;
	public List<int> magicIDs = new List<int>();
	public List<int> itemKeys = new List<int>();
	public List<int> itemValues = new List<int>();	
	protected Dictionary<int, int> items = new Dictionary<int, int>();//id, amount

	public void OnSerialize()
	{
		itemKeys = new List<int>(items.Keys);
		itemValues = new List<int>(items.Values);
	}

	public void OnDeSerialize()
	{
		items.Clear();
		for (int i = 0; i != System.Math.Min(itemKeys.Count, itemValues.Count); i++)
		{
			items.Add(itemKeys[i], itemValues[i]);
		}
	}

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
