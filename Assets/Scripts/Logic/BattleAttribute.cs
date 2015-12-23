using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleAttribute {
/*
 * 高阶数值相关
 * */	

	//最大生命值
	public static int MaxHP(BattleObject bo)
	{
		if(bo.GetBattleType() == BattleType.Magical)
			return (int)Mathf.Round(Mathf.Pow(bo.stamina, 1.5f) * 3);
		else if(bo.GetBattleType() == BattleType.Physical)
			return (int)Mathf.Round(Mathf.Pow(bo.stamina, 1.3f) * 5);
		else
			return (int)Mathf.Round(Mathf.Pow(bo.stamina, 1.4f) * 4);
	}

	//时间轴速度
	public static int Speed(BattleObject bo)
	{
		return (int)Mathf.Round(Mathf.Log10(bo.agility) * 80);
	}

	//攻击倍率
	public static float AttackMulti(BattleObject bo)
	{
		return 1 + bo.power / 100f;
	}

	//防御倍率
	public static float DefenceMulti(BattleObject bo)
	{
		return bo.toughness / (100f + bo.toughness);
	}

	//命中率
	public static float ExtraAccuracy(BattleObject bo)
	{
		return bo.skill + bo.luck / 9f;
	}

	//回避率
	public static float ExtraEvasion(BattleObject bo)
	{
		return bo.skill * 0.9f + bo.luck / 4.5f;
	}

	//暴击率
	public static float ExtraCrit(BattleObject bo)
	{
		return bo.skill / 4.5f + bo.luck / 9f;
	}

	//抗暴击率
	public static float ExtraCritResist(BattleObject bo)
	{
		return bo.skill / 4.5f + bo.luck / 4.5f;
	}

	//最大生命值
	public static int MaxHP(ObjectData data)
	{
		if(data.battleType == BattleType.Magical)
			return (int)Mathf.Round(Mathf.Pow(data.stamina, 1.5f) * 3);
		else if(data.battleType == BattleType.Physical)
			return (int)Mathf.Round(Mathf.Pow(data.stamina, 1.3f) * 5);
		else
			return (int)Mathf.Round(Mathf.Pow(data.stamina, 1.4f) * 4);
	}
	
	//时间轴速度
	public static int Speed(ObjectData data)
	{
		return (int)Mathf.Round(Mathf.Log10(data.agility) * 80);
	}
	
	//攻击倍率
	public static float AttackMulti(ObjectData data)
	{
		return 1 + data.power / 100f;
	}
	
	//防御倍率
	public static float DefenceMulti(ObjectData data)
	{
		return data.toughness / (100f + data.toughness);
	}
	
	//命中率
	public static float ExtraAccuracy(ObjectData data)
	{
		return data.skill + data.luck / 9f;
	}
	
	//回避率
	public static float ExtraEvasion(ObjectData data)
	{
		return data.skill * 0.9f + data.luck / 4.5f;
	}
	
	//暴击率
	public static float ExtraCrit(ObjectData data)
	{
		return data.skill / 4.5f + data.luck / 9f;
	}
	
	//抗暴击率
	public static float ExtraCritResist(ObjectData data)
	{
		return data.skill / 4.5f + data.luck / 4.5f;
	}
}
