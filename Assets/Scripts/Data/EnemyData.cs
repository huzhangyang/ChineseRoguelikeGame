using UnityEngine;
using System.Collections;

public class EnemyData : ObjectData {
/*
 * 怪物数据。除了通用属性外，还有一些怪物独有的属性。
*/

	public EnemyData(int enemyID)
	{
		name = "Enemy";
		maxHP = (int)(100 * Random.Range(0.8f, 1.2f));
		maxMP = (int)(100 * Random.Range(0.8f, 1.2f));
		power = (int)(10 * Random.Range(0.8f, 1.2f));
		skill = (int)(10 * Random.Range(0.8f, 1.2f));
		agility = (int)(10 * Random.Range(0.8f, 1.2f));
		toughness = (int)(10 * Random.Range(0.8f, 1.2f));
		luck = (int)(10 * Random.Range(0.8f, 1.2f));
		currentHP = maxHP;
		currentMP = maxMP;
	}
}
