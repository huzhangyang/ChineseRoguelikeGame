using UnityEngine;
using System.Collections;

public abstract class ObjectData{

/*
 * 物体数据基类。怪物和玩家继承此类。
*/
	//basic information
	public string name;//名称
	public Talent talent;//天赋
	//TODO 性格是一个复合值（即，由多个值综合计算而来的主性格），而不是单个枚举
	//basic attribute
	public int maxHP;//最大生命值
	public int currentHP;//当前生命值
	public int maxMP;//最大灵力值
	public int currentMP;//当前灵力值
	public int power;//力量
	public int skill;//技术
	public int agility;//敏捷
	public int toughness;//韧性
	public int luck;//运气
	//for battle
	//Equipment
	public Weapon equippedWeapon;
}
