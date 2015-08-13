using UnityEngine;
using System.Collections;

/*
管理全局数据结构,如常量,枚举,结构体等.
*/

public class GlobalDataStructure{
	//path
	public const string PATH_GAMEDATA = "GameData/";
	public const string PATH_BATTLE = "Battle/";
	public static string PATH_SAVE = Application.persistentDataPath + "/Player.sav";
	public static string PATH_CONFIG = Application.persistentDataPath + "/Config.sav";
	//encrypt
	public const string ENCRYPT_KEY = "CRGProject";
	public static bool ENCRYPT_ENABLED = true;
}
