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

	public void AISelectCommand()
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

		command.source = self;
		self.commandToExecute = command;
		AISelectTarget();
	}

	public void AISelectTarget()
	{
		bool result = SkillHelper.FillCommandTarget(self);
		if(!result)
		{
			if(self.commandToExecute.targetType == TargetType.SingleEnemy)
				self.commandToExecute.targetList.Add(BattleManager.Instance.GetARandomEnemy(self));
			else
				self.commandToExecute.targetList.Add(self);
		}
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
		Command command = new CommandUseItem(1, self.GetItemCount(1));
		command.targetList.Add(self);
		return command;
	}
}
