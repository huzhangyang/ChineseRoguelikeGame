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
			command.targetList.Add(self);
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
		List<Command> validCommands = self.availableCommands.FindAll((x)=>{return x.commandType == CommandType.Attack;});
		return validCommands[Random.Range(0, validCommands.Count)];
	}

	public Command AIDefence()
	{
		List<Command> validCommands = self.availableCommands.FindAll((x)=>{return x.commandType == CommandType.Defence;});
		return validCommands[Random.Range(0, validCommands.Count)];
	}

	public Command AIHeal()
	{
		Command command = new CommandUseHealing(self.GetItemCount(1));
		command.targetList.Add(self);
		return command;
	}

	public BattleObject AISelectTarget()
	{
		return BattleManager.Instance.GetPlayerList()[Random.Range(0, BattleManager.Instance.GetPlayerList().Count)];
	}
}
