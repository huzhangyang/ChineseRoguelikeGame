using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : BattleObject {
/*
 * 角色在战斗中的数据实体与逻辑。
 * */	
	PlayerData data = new PlayerData();
	
	public PlayerData GetData()
	{
		return data;
	}


}
