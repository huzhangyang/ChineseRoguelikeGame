using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BasicCommand {Attack,Defence,Item,Strategy}
public enum BattleStatus
{
	Prepare,//等待选择行动(0~8000)
	Ready,//选择行动中(8000)
	Action,//即将行动(8000~10000)
}

public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */	
	public BattleStatus battleStatus = BattleStatus.Prepare;
	public List<Command> availableCommands;
	public Command commandToExecute = Command.None();
	public bool isPaused = true;
	public bool isGuarding = false;
	public bool isEvading = false;
	public bool isDied = false;
	public int recoverTime;
	private int _timelinePosition;
	public int timelinePosition//max:10000
	{
		set
		{
			if(value < 0) value = 0;
			if(value > 10000) value = 10000;
			_timelinePosition = value;
			GetComponent<BattleObjectUIEvent>().SetAvatarPositionX(value / 20, isPaused);//max:500
		}
		get
		{
			return _timelinePosition;
		}
	}

	protected ObjectData data;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);		
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
	}

	protected void OnUpdateTimeline(MessageEventArgs args)
	{
		if(!isPaused && !isDied)
		{
			if(recoverTime > 0)
				recoverTime--;
			else if(battleStatus == BattleStatus.Prepare)
				timelinePosition += data.agility * 10;
			else if(battleStatus == BattleStatus.Action)
				timelinePosition += commandToExecute.preExecutionSpeed;
		}

		if(timelinePosition >= 8000 && battleStatus == BattleStatus.Prepare)
		{
			SelectCommand();
		}
		if(timelinePosition >= 10000 && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected virtual void SelectCommand()
	{
		timelinePosition = 8000;
		isGuarding = false;
		isEvading = false;
		battleStatus = BattleStatus.Ready;
	}

	protected virtual void ExecuteCommand()
	{
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("CommandType", ((int)commandToExecute.commandType).ToString());
		args.AddMessage("SkillOrItemID", commandToExecute.skillOrItemID.ToString());
		args.AddMessage("CommandName", commandToExecute.commandName);
		EventManager.Instance.PostEvent(EventDefine.ExecuteCommand, args);
		//decide target
		List<BattleObject> targetList = new List<BattleObject>();
		switch(commandToExecute.targetType)
		{
		case TargetType.Self:
			targetList.Add(this);
			break;
		case TargetType.SingleEnemy:
		case TargetType.SingleAlly:
			if(commandToExecute.target != null)
				targetList.Add(commandToExecute.target);
			break;
		case TargetType.AllEnemies:
			if(this is Enemy)
				targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			else
				targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			break;
		case TargetType.AllAllies:
			if(this is Enemy)
				targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			else
				targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			break;
		}
		//decide command
		switch(commandToExecute.commandType)
		{
		case CommandType.UseSkill:
			UseSkill(commandToExecute.skillOrItemID, targetList);
			break;
		case CommandType.Defence:
			Defend(commandToExecute.commandName);
			break;
		case CommandType.UseItem:
			UseItem(commandToExecute.skillOrItemID, targetList);
			break;
		case CommandType.Strategy:
			UseStrategy(commandToExecute.commandName);
			break;
		case CommandType.None:
			break;
		}
		//post process
		recoverTime = commandToExecute.postExecutionRecover;
		timelinePosition = 0;
		battleStatus = BattleStatus.Prepare;
		commandToExecute = Command.None();
	}

	public void RefreshAvailableCommands(BasicCommand basicCommand)
	{
		availableCommands = new List<Command> ();
		switch(basicCommand)
		{
		case BasicCommand.Attack:
			if(data.battleType != BattleType.Magical)
			{
				WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
				availableCommands.Add(Command.UseWeaponSkill(weaponData, weaponData.skill1ID));
				availableCommands.Add(Command.UseWeaponSkill(weaponData, weaponData.skill2ID));
				availableCommands.Add(Command.UseWeaponSkill(weaponData, weaponData.skill3ID));
			}
			if(data.battleType != BattleType.Physical)
			{
				foreach(int magicID in data.magicIDs)
				{
					MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
					availableCommands.Add(Command.UseMagicSkill(magicData, magicData.skillID));
				}
			}
			break;
		case BasicCommand.Defence:
			availableCommands.Add(Command.Guard());
			availableCommands.Add(Command.Evade());
			break;
		case BasicCommand.Item:
			if(data.battleType != BattleType.Magical)
				availableCommands.Add(Command.SwitchWeapon());
			if(data.GetItemCount(1) > 0)
				availableCommands.Add(Command.Healing(data.GetItemCount(1)));
			break;
		case BasicCommand.Strategy:
			availableCommands.Add(Command.None());
			availableCommands.Add(Command.Escape());
			break;
		}
		for(int i = 0; i < availableCommands.Count; i++)
		{
			availableCommands[i].commandID = i;
		}
	}

	public void UseSkill(int skillID, List<BattleObject> targetList)
	{
		foreach(BattleObject target in targetList)
		{
			//判断是否触发防御反击
			if(target.commandToExecute.commandType == CommandType.Defence)
			{
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", target.data.name);
				EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);

				this.InflictDamage(Random.Range(0,100));
				continue;
			}
			//计算是否命中，是否暴击
			SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
			float hitPercent = 0;
			float evadePercent = 0;
			float criticalPercent = 0;
			float damage = 0;
			if(skillData.skillType == SkillType.Physical)
			{
				WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
				hitPercent = data.skill + data.luck / 10 + weaponData.basicACC * skillData.ACCMultiplier;//命中率
				evadePercent = target.data.skill + target.data.luck / 10;//闪避率
				criticalPercent = target.data.skill + target.data.luck / 10 + weaponData.basicCRT * skillData.CRTMultiplier / 100;//暴击率
				damage = data.power * weaponData.basicATK * skillData.ATKMultiplier / target.data.toughness;//伤害值
			}
			else if(skillData.skillType == SkillType.Magical)
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});
				hitPercent = data.skill + data.luck / 10 + magicData.basicACC * skillData.ACCMultiplier;//命中率
				evadePercent = target.data.skill + target.data.luck / 10;//闪避率
				criticalPercent = target.data.skill + target.data.luck / 10 + magicData.basicCRT * skillData.CRTMultiplier / 100;//暴击率
				damage = data.power * magicData.basicATK * skillData.ATKMultiplier / target.data.insight;//伤害值
			}
			//如果命中，则对方受伤
			if(target.isEvading) evadePercent += 50;
			string SEName = "hit";
			bool hit = Random.Range(0,101) <= (hitPercent - evadePercent)?true:false;
			if(hit)
			{		
				bool critical = Random.Range(0,101) <= criticalPercent?true:false;
				if(target.isGuarding)
				{
					damage /= 2;
					critical = false;
					SEName = "guard";
				}
				if(critical)
				{
					damage *= 2;
					SEName = "critical";

					MessageEventArgs args = new MessageEventArgs();
					args.AddMessage("Name", data.name);
					EventManager.Instance.PostEvent(EventDefine.BattleObjectCritical, args);
				}
				AudioManager.Instance.PlaySE(SEName);
				target.InflictDamage((int)damage);
			}
			else
			{
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", target.data.name);
				EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
			}
		}
	}

	public void Defend(string commandName)
	{
		if(commandName == "格挡")
			isGuarding = true;
		if(commandName == "闪避")
			isEvading = true;
	}

	public void UseItem(int itemID, List<BattleObject> targetList)
	{
		foreach(BattleObject target in targetList)
		{
			if(DataManager.Instance.GetItemDataSet().IsWeapon(itemID))
			{
				target.data.weaponID = itemID;
				return;
			}
			
			if(itemID == 1)
			{
				target.Heal((int)(target.data.maxHP * 0.8f - target.data.currentHP));
				target.data.ConsumeItem(itemID);
				return;
			}
		}
	}

	public void UseStrategy(string commandName)
	{
		if(commandName == "逃跑")
		{
			EventManager.Instance.PostEvent(EventDefine.BattleObjectEscape);
		}
	}

	void InflictDamage(int damage)
	{
		data.currentHP -= damage;
		isPaused = true;// so that timeline adjust is smooth
		if(battleStatus == BattleStatus.Action)
			timelinePosition -= damage * 50;
		else
			timelinePosition -= damage * 20;

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("Damage", damage.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHurt, args);

		if(data.currentHP <= 0)
		{
			data.currentHP = 0;
			isDied = true;
			if(this is Enemy)
				BattleLogic.enemys.Remove((Enemy)this);
			else
				BattleLogic.players.Remove((Player)this);
			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Name", data.name);
			EventManager.Instance.PostEvent(EventDefine.BattleObjectDied, args2);
		}

		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);
	}

	void Heal(int amount)
	{
		data.currentHP += amount;
		if(data.currentHP > data.maxHP)
			data.currentHP = data.maxHP;

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("Amount", amount.ToString());
		args.AddMessage("CurrentHP", data.currentHP.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHeal, args);

		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);
	}
}

