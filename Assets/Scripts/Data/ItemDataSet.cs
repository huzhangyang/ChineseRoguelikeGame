using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataSet : ScriptableObject 
{
	public List<ItemData> itemDataSet = new List<ItemData> ();
	public List<WeaponData> weaponDataSet = new List<WeaponData>();
	public List<MagicData> magicDataSet = new List<MagicData>();
	
	public ItemData GetItemData(int itemID)
	{
		foreach (ItemData data in itemDataSet)
		{
			if (data.id == itemID)
				return data;
		}
		Debug.LogError("Asking for an non-exist item:" + itemID);
		return null;
	}

	public WeaponData GetWeaponData(int weaponID)
	{
		foreach (WeaponData data in weaponDataSet)
		{
			if (data.id == weaponID && data.type == ItemType.Weapon)
				return data;
		}
		Debug.LogError("Asking for an non-exist weapon:" + weaponID);
		return null;
	}

	public MagicData GetMagicData(int magicID)
	{
		foreach (MagicData data in magicDataSet)
		{
			if (data.id == magicID && data.type == ItemType.Magic)
				return data;
		}
		Debug.LogError("Asking for an non-exist magic:" + magicID);
		return null;
	}
}

[System.Serializable]
public class ItemData{
/*
 * 道具类。包括武器、饰品等。
*/
	public int id;
	public string name;
	public ItemType type;
	public string description;
}

[System.Serializable]
public class WeaponData:ItemData{
/*
 * 武器类。
*/
	public int basicATK;//基准攻击力
	public int basicSPD;//基准攻击速度
	public int basicACC;//基准命中率
	public int basicCRT;//基准暴击率
	public int skill1ID;
	public int skill2ID;
	public int skill3ID;
}

[System.Serializable]
public class MagicData:ItemData{
/*
 * 魔法类。
*/
	public int basicATK;//基准攻击力
	public int basicSPD;//基准攻击速度
	public int basicACC;//基准命中率
	public int basicCRT;//基准暴击率
	public int skillID;
}

public enum ItemType{NormalItem, Weapon, Magic, Ring}
