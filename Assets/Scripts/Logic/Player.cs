using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	PlayerData data = new PlayerData();
	
	public PlayerData GetData()
	{
		return data;
	}
}
