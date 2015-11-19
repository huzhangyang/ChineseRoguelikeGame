using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	Enemy self;
	Command command = new CommandNone();

	public void InitAI()
	{
		self = GetComponent<Enemy>();
	}

	public Command AISelectCommand()
	{
		float hpPercent = self.currentHP / (float)self.maxHP;

		if(hpPercent > 0.8)
		{
			command = AIAttack();	
		}
		else if(hpPercent > 0.5)
		{
			if(Random.Range(0,2) > 0)
				command = AIAttack();
			else
				command = AIDefence();
		}
		else if(hpPercent > 0.2)
		{
			if(Random.Range(0,2) > 0 && self.GetItemCount(1) > 0)
				command = AIHeal();
			else
				command = AIDefence();
		}
		else
		{
			if(self.GetItemCount(1) > 0)
				command = AIHeal();
			else if(Random.Range(0,2) > 0)
				command = AIDefence();
			else
				command = AIAttack();
		}

		switch(command.targetType)
		{
		case TargetType.Self:
			command.targetList.Add(this.GetComponent<BattleObject>());
			break;
		case TargetType.SingleEnemy:
		case TargetType.SingleAlly:
			command.targetList.Add(AISelectTarget());
			break;
		case TargetType.AllEnemies:
			command.targetList = new List<BattleObject>(BattleManager.Instance.GetPlayerList().ToArray());
			break;
		case TargetType.AllAllies:
			command.targetList = new List<BattleObject>(BattleManager.Instance.GetEnemyList().ToArray());
			break;
		}
		return command;
	}

	public Command AIAttack()
	{
		List<Command> availableCommands = new List<Command> ();
		if(self.GetBattleType() != BattleType.Magical)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(self.GetWeapon());
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill1ID));
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill2ID));
			availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill3ID));
		}
		if(self.GetBattleType() != BattleType.Physical)
		{
			foreach(int magicID in self.GetMagic())
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
				availableCommands.Add(new CommandUseMagicSkill(magicData, magicData.skillID));
			}
		}
		Command command = availableCommands[Random.Range(0, availableCommands.Count)];
		return command;
	}

	public Command AIDefence()
	{
		if(Random.Range(0,2) > 0)
			return new CommandGuard();
		else
			return new CommandEvade();
	}

	public Command AIHeal()
	{
		Command command = new CommandUseHealing(self.GetItemCount(1));
		command.targetList.Add(self);
		return command;
	}

	public BattleObject AISelectTarget()
	{
		while(true)
		{
			foreach(Player player in BattleManager.Instance.GetPlayerList())
			{
				if(Random.Range(0,2) > 0)
					return player;
			}
		}
	}
}
