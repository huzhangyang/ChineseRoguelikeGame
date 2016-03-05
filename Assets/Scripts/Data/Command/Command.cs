using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Command {
	
	public CommandType commandType;//指令类型
	public string commandName;//指令名称
	public string commandDescription;//指令描述
	public TargetType targetType;//目标类型 
	public int preExecutionSpeed;//前摇
	public int postExecutionRecover;//后摇

	public int skillID;//对应的技能ID(仅在使用技能时生效)
	public int itemID;//对应的物品ID(仅在使用物品时生效)
	public BattleObject source;//执行者
	public List<BattleObject> targetList = new List<BattleObject>();//执行目标
	public string executeMessage;//执行时显示的信息 

	public abstract void Execute ();
	public virtual bool IsAvailable()
	{
		return true;
	}

	protected void SendExecuteMessage()
	{
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Message", executeMessage);
		EventManager.Instance.PostEvent(BattleEvent.OnCommandExecute, args);
	}

	public static List<Command> GetAvailableCommands(BattleObject bo)
	{
		List<Command> availableCommands = new List<Command>();
		//检查攻击类技能
		if(bo.GetBattleType() != BattleType.Magical && bo.GetWeapon() > 1000)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(bo.GetWeapon());
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill1ID));
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill2ID));
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill3ID));
		}
		if(bo.GetBattleType() != BattleType.Physical)
		{
			foreach(int magicID in bo.GetMagicList())
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
				availableCommands.Add(new CommandUseMagicSkill(magicData, magicData.skillID));
			}
		}
		//检查防御技能
		availableCommands.Add(new CommandBlock());
		availableCommands.Add(new CommandEvade());
		availableCommands.Add(new CommandGuard());
		//检查物品
		if(bo.GetBattleType() != BattleType.Magical)
		{
			foreach(int weaponID in bo.GetWeaponList())
			{
				if(weaponID != bo.GetWeapon())
					availableCommands.Add(new CommandSwitchWeapon(weaponID));
			}
		}
		foreach(var item in bo.GetItemList())
		{
			availableCommands.Add(new CommandUseItem(item.Key, item.Value));
		}
		//检查策略
		availableCommands.Add(new CommandNone());
		availableCommands.Add(new CommandEscape());

		foreach(Command command in availableCommands)
		{
			command.source = bo;
		}

		return availableCommands;
	}
}