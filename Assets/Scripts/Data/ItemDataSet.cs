using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataSet : ScriptableObject 
{
	public List<ItemData> dataSet = new List<ItemData>();
	
	public ItemData GetItemData(int itemID)
	{
		foreach (ItemData data in dataSet)
		{
			if (data.id == itemID)
				return data;
		}
		Debug.LogError("Asking for an non-exist item:" + itemID);
		return null;
	}

	public WeaponData GetWeaponData(int weaponID)
	{
		foreach (ItemData data in dataSet)
		{
			if (data.id == weaponID)
				return (WeaponData)data;
		}
		Debug.LogError("Asking for an non-exist weapon:" + weaponID);
		return null;
	}
}

public enum ItemType{NormalItem, Weapon, Ring}

[System.Serializable]
public class ItemData{
/*
 * 道具类。包括武器、饰品等。
*/
	public int id;
	public string name;
	public ItemType type;
}

[System.Serializable]
public class WeaponData:ItemData{
/*
 * 道具类。包括装备。
*/
	public int basicATK;//基准攻击力
	public int basicSPD;//基准攻击速度
	public int basicACC;//基准命中率
	public int basicCRT;//基准暴击率
	public int skill1ID;
	public int skill2ID;
	public int skill3ID;
}
