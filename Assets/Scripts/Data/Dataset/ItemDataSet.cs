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
			if (data.id == weaponID)
				return data;
		}
		Debug.LogError("Asking for an non-exist weapon:" + weaponID);
		return null;
	}

	public MagicData GetMagicData(int magicID)
	{
		foreach (MagicData data in magicDataSet)
		{
			if (data.id == magicID)
				return data;
		}
		Debug.LogError("Asking for an non-exist magic:" + magicID);
		return null;
	}

	public MagicData GetMagicDataBySkillID(int skillID)
	{
		return magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});
	}

	public bool IsWeapon(int itemID)
	{
		if(itemID >= 1000 && itemID < 2000)
			return true;
		else
			return false;
	}

	public bool IsMagic(int itemID)
	{
		if(itemID >= 2000 && itemID < 3000)
			return true;
		else
			return false;
	}

	public bool IsWeaponSkill(int skillID)
	{
		return GetMagicDataBySkillID(skillID) == null;
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
	public int basicATKMin;//攻击下限
	public int basicATKMax;//攻击上限
	public int basicACC;//基准命中率
	public int basicCRT;//基准暴击率
	public int basicSPD;//基准攻击速度
	public int interrupt;//武器打断值
	public int skill1ID;
	public int skill2ID;
	public int skill3ID;
	public List<int> buffID;
	public List<int> buffPercent;
	public List<int> buffTurns;
	public List<int> effectID;
	public List<int> effectPercent;
}

[System.Serializable]
public class MagicData:ItemData{
/*
 * 法术类。
*/
	public int basicATKMin;//攻击下限
	public int basicATKMax;//攻击上限
	public int basicSPD;//基准攻击速度
	public int basicACC;//基准命中率
	public int basicCRT;//基准暴击率
	public int interrupt;//法术打断值
	public float cost;//法术消耗
	public int skillID;
}
