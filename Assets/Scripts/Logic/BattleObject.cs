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
	public List<Buff> buffList;
	public Command commandToExecute = new CommandNone();
	public bool isPaused = true;
	public bool isGuarding = false;
	public bool isEvading = false;
	public bool isDied = false;
	private int _timelinePosition;
	public int timelinePosition//max:10000
	{
		set
		{
			if(value > 10000) value = 10000;
			_timelinePosition = value;
			if(value < 0) value = 0;
			UIEvent.SetAvatarPositionX(value / 20, isPaused);//max:500
		}
		get
		{
			return _timelinePosition;
		}
	}

	protected BattleObjectUIEvent UIEvent;
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
			if(battleStatus == BattleStatus.Prepare)
				timelinePosition += (int)(Mathf.Log10(data.agility) * 100);
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

	public virtual ObjectData GetData()
	{
		return data;
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
		//decide target
		switch(commandToExecute.targetType)
		{
		case TargetType.Self:
			commandToExecute.targetList.Add(this);
			break;
		case TargetType.SingleEnemy:
		case TargetType.SingleAlly:
			break;
		case TargetType.AllEnemies:
			if(this is Enemy)
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			else
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			break;
		case TargetType.AllAllies:
			if(this is Enemy)
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			else
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			break;
		}
		//decide command
		commandToExecute.source = this;
		commandToExecute.Execute();
		//post process
		timelinePosition =  -commandToExecute.postExecutionRecover;
		battleStatus = BattleStatus.Prepare;
		commandToExecute = new CommandNone();
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
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill1ID));
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill2ID));
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill3ID));
			}
			if(data.battleType != BattleType.Physical)
			{
				foreach(int magicID in data.magicIDs)
				{
					MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
					availableCommands.Add(new CommandUseMagicSkill(magicData, magicData.skillID));
				}
			}
			break;
		case BasicCommand.Defence:
			availableCommands.Add(new CommandGuard());
			availableCommands.Add(new CommandEvade());
			break;
		case BasicCommand.Item:
			if(data.battleType != BattleType.Magical)
				availableCommands.Add(new CommandSwitchWeapon());
			if(data.GetItemCount(1) > 0)
				availableCommands.Add(new CommandUseHealing(data.GetItemCount(1)));
			break;
		case BasicCommand.Strategy:
			availableCommands.Add(new CommandNone());
			availableCommands.Add(new CommandEscape());
			break;
		}
		for(int i = 0; i < availableCommands.Count; i++)
		{
			availableCommands[i].commandID = i;
		}
	}

	public void OnBeHit(BattleObject source, SkillType type, float hit, float crit, float damage)
	{
		//判断防反
		if(commandToExecute.commandType == CommandType.Defence)
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name",data.name);
			EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);
			
			source.InflictDamage(Random.Range(0,100));//TODO改进反击算法
			return;
		}
		//计算真实命中、暴击、伤害
		hit -= (data.skill + data.luck / 10.0f);
		if(isEvading) hit -= 50;
		crit -= (data.skill / 10.0f + data.luck / 10.0f);
		if(isGuarding) hit = 0;
		switch(type)
		{
		case SkillType.Magical:
			damage *= (1 - data.insight / 250);
			break;
		case SkillType.Physical:
			damage *= (1 - data.toughness / 250);
			break;
		}
		if(isGuarding) damage /= 2;
		//判断是否命中，是否暴击
		bool isHit = Random.Range(0,101) <= hit?true:false;
		bool isCrit = Random.Range(0,101) <= crit?true:false;

		string SEName = "hit";
		if(isHit)
		{		
			if(isGuarding)
			{
				SEName = "guard";
			}
			if(isCrit)
			{
				damage *= 2;
				SEName = "critical";
				
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", data.name);
				EventManager.Instance.PostEvent(EventDefine.BattleObjectCritical, args);
			}
			AudioManager.Instance.PlaySE(SEName);
			InflictDamage((int)damage);
		}
		else
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name", data.name);
			EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
		}
	}

	public void InflictDamage(int damage)
	{
		data.currentHP -= damage;
		isPaused = true;// so that timeline adjust is smooth
		if(battleStatus == BattleStatus.Action)
			timelinePosition -= damage * 10000 / data.maxHP;
		else
			timelinePosition -= damage * 5000 / data.maxHP;

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

		UIEvent.SetHPBar(data.currentHP);
	}

	public void Heal(int amount)
	{
		if(amount < 0) amount = 0;
		data.currentHP += amount;
		if(data.currentHP > data.maxHP)
			data.currentHP = data.maxHP;

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("Amount", amount.ToString());
		args.AddMessage("CurrentHP", data.currentHP.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHeal, args);

		UIEvent.SetHPBar(data.currentHP);
	}

	public void AddBuff(int id)
	{
		BuffData data = DataManager.Instance.GetSkillDataSet().GetBuffData(id);
		Buff buff = new Buff(data);
		buffList.Add(buff);
	}
}

