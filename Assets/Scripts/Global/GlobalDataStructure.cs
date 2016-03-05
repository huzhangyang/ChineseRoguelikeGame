using UnityEngine;
using System.Collections;

/*
管理全局数据结构,如常量,枚举,结构体等.
*/
public enum CommandType{Attack,Defence,Item,Strategy}
public enum TargetType{Self, SingleEnemy, AllEnemies, SingleAlly, AllAllies, EveryoneElse, Everyone, Random, OtherAlly}
public enum BattleType{Physical, Magical, Both}
public enum SkillType{None, Slash, Blunt, Thrust, Neutral, Yin, Yang} 
public enum BuffTrigger{Always, Dead, Ready, Action, Behit, AfterDamage} 
public enum EffectTrigger{SwitchWeapon, OnHit, AfterHit} 
public enum ItemType{Consumable, Weapon, Magic, Accessory, KeyItem}
public enum BattleStatus{Prepare, Ready, Action, Recover}

public class GlobalDataStructure{
	//path
	public const string PATH_GAMEDATA = "GameData/";//数据文件夹
	public const string PATH_UIPREFAB_WINDOW = "UIPrefab/Window/";//UI窗口文件夹
	public const string PATH_UIPREFAB_BATTLE = "UIPrefab/Battle/";//战斗素材文件夹
	public const string PATH_UIPREFAB_COMMON = "UIPrefab/Common/";//通用素材文件夹
	public const string PATH_UIIMAGE_BATTLE = "UI/Battle/";//战斗图片素材文件夹
	public const string PATH_UIIMAGE_COMMON = "UI/Common/";//通用图片素材文件夹
	public const string PATH_UIIMAGE_ICON = "UI/Icon/";//Buff图片素材文件夹
	public const string PATH_MUSIC = "Music/";//音乐相关素材文件夹
	public const string PATH_SE = "SE/";//音效相关素材文件夹
	public static string PATH_SAVE = Application.persistentDataPath + "/Player.sav";//游戏存档存放路径
	public static string PATH_CONFIG = Application.persistentDataPath + "/Config.sav";//游戏设置存放路径
	//encrypt
	public const string ENCRYPT_KEY = "CRGProject";//存档加密Key
	public static bool ENCRYPT_ENABLED = true;//存档是否加密
	//battle
	public const int BATTLE_SLOWSPEED = 50;//战斗策略慢速执行速度
	public const int BATTLE_STANDARDSPEED = 100;//战斗策略标准执行速度
	public const int BATTLE_FASTSPEED = 200;//战斗策略快速执行速度
	public const int BATTLE_MAXSPEED = 6000;//战斗策略最快执行速度
	public const int BATTLE_TIMELINE_READY = 6000;//时间轴Ready点
	public const int BATTLE_TIMELINE_MAX = 10000;//时间轴最大长度
	public const float HP_RECOVER_THRESHOLD = 0.8f;//HP恢复阈值（元素瓶恢复量，以及自动回血阈值）
	public const float HP_RECOVER_AMOUNT = 0.02f;//生命每回合的恢复量
	public const float MP_RECOVER_AMOUNT = 0.1f;//灵力每回合的恢复量
	public const float HP_WEAKEN_THRESHOLD = 0.5f;//HP虚弱阈值（降低力技速）
	public const float HP_WEAKEN_AMOUNT = 0.25f;//HP对力技速的最大削弱比
}
