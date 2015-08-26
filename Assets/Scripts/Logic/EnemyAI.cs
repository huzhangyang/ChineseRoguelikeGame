using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	Enemy self;
	EnemyData data;

	public void InitAI()
	{
		self = GetComponent<Enemy>();
		data = self.GetData();
	}

	public Command AISelectCommand()
	{
		float hpPercent = data.currentHP / (float)data.maxHP;
		Command command = Command.None();
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
			if(Random.Range(0,2) > 0 && data.GetItemCount(1) > 0)
				command = AIHeal();
			else
				command = AIDefence();
		}
		else
		{
			if(self.GetData().GetItemCount(1) > 0)
				command = AIHeal();
			else if(Random.Range(0,2) > 0)
				command = AIDefence();
			else
				command = AIAttack();
		}
		return command;
	}

	public Command AIAttack()
	{
		List<Command> availableCommands = new List<Command> ();
		if(data.battleType != BattleType.Magical)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
			availableCommands.Add(Command.UseSkill(weaponData.skill1ID));
			availableCommands.Add(Command.UseSkill(weaponData.skill2ID));
			availableCommands.Add(Command.UseSkill(weaponData.skill3ID));
		}
		if(data.battleType != BattleType.Physical)
		{
			foreach(int magicID in data.magicIDs)
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
				availableCommands.Add(Command.UseSkill(magicData.skillID));
			}
		}
		Command command = availableCommands[Random.Range(0, availableCommands.Count)];
		command.target = AISelectTarget();
		return command;
	}

	public Command AIDefence()
	{
		if(Random.Range(0,2) > 0)
			return Command.Guard();
		else
			return Command.Evade();
	}

	public Command AIHeal()
	{
		Command command = Command.Healing(data.GetItemCount(1));
		command.target = self;
		return command;
	}

	public BattleObject AISelectTarget()
	{
		while(true)
		{
			foreach(Player player in BattleLogic.players)
			{
				if(Random.Range(0,2) > 0)
					return player;
			}
		}
	}
}
