using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	private AIData data;
	private Enemy self;
	private Command command = new CommandNone();

	private Dictionary<CommandType, int> commandFrequency;
	private int attackFrequency;
	private int defenceFrequency;
	private int itemFrequency;
	private int strategyFrequency;
	private bool forceAttack = false;
	private bool forceDefence = false;
	private bool forceItem = false;
	private bool forceStrategy = false;

	public void InitAI(int aiID)
	{
		self = GetComponent<Enemy>();
		data = DataManager.Instance.GetEnemyDataSet ().GetAIData (aiID);
	}

	public void AutoSelectCommand()
	{
		AIAdjustWeight ();
		AISelectCommand ();
		AISelectTarget();
	}

	//根据场上局势动态调整指令权重
	private void AIAdjustWeight()
	{
		attackFrequency = data.attackFrequency;
		defenceFrequency = data.defenceFrequency;
		itemFrequency = data.itemFrequency;
		strategyFrequency = data.strategyFrequency;

		//float hpPercent = self.currentHP / (float)self.maxHP;
	}

	//根据权重随机选择指令
	private void AISelectCommand()
	{
		int[] commandPercent = new int[4]{attackFrequency, defenceFrequency, itemFrequency, strategyFrequency};		
		int[] commandRange = new int[commandPercent.Length];

		commandRange[0] = commandPercent[0];
		for (int i = 1; i < commandRange.Length; i++)
		{
			commandRange[i] = commandRange[i - 1] + commandPercent[i];
		}
		
		int randomValue = UnityEngine.Random.Range(0, commandRange[commandRange.Length - 1] + 1);
		for (int i = 0; i < commandRange.Length; i++)
		{
			if (randomValue <= commandRange[i])
			{
				if(i == 0)
					command = AIAttack();
				else if(i == 1)
					command = AIDefence();
				else if(i == 2)
					command = AIHeal();
				else if(i == 3)
					command = AIStrategy();

				if(command == null) AISelectCommand();//若选择的模式里没有命令，则重新选择
				break;
			}
		}

		command.source = self;
		self.commandToExecute = command;
	}

	//为选择的指令抉择对象
	private void AISelectTarget()
	{
		bool result = SkillHelper.FillCommandTarget(self);
		if(!result)
		{
			switch(self.commandToExecute.targetType)
			{
			case TargetType.SingleEnemy:
				self.commandToExecute.targetList.Add(BattleManager.Instance.GetARandomEnemy(self));
				break;
			case TargetType.SingleAlly:
				self.commandToExecute.targetList.Add(BattleManager.Instance.GetARandomAlly(self));
				break;
			case TargetType.OtherAlly:
				while(true)
				{
					BattleObject ally = BattleManager.Instance.GetARandomAlly(self);
					if(ally != self)
					{
						self.commandToExecute.targetList.Add(ally);
						break;
					}
				}
				break;
			default:
				EventManager.Instance.PostEvent(BattleEvent.OnCommandSelected);
				break;
			}
		}
	}

	private Command AIAttack()
	{
		List<Command> validCommands = self.availableCommands.FindAll((x)=>{return x.commandType == CommandType.Attack;});
		return validCommands[Random.Range(0, validCommands.Count)];
	}

	private Command AIDefence()
	{
		List<Command> validCommands = self.availableCommands.FindAll((x)=>{return x.commandType == CommandType.Defence;});
		return validCommands[Random.Range(0, validCommands.Count)];
	}

	private Command AIHeal()
	{
		Command command = new CommandUseItem(1, self.GetItemCount(1));
		return command;
	}

	private Command AIStrategy()
	{
		Command command = new CommandEscape();
		return command;
	}
}
