using UnityEngine;
using System.Collections;

public class CommandUseMagicSkill : Command
{
	public CommandUseMagicSkill(MagicData magicData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.UseSkill;
		commandName = skillData.name;
		commandDescription = skillData.description;
		targetType = skillData.targetType;
		preExecutionSpeed = (int)(magicData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = (int)(6000 / magicData.basicSPD * skillData.postSPDMultiplier);
		
		this.skillID = skillID;
	}

	public override void Execute()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		executeMessage = source.GetData().name + "使用了" + skillData.name + "!";
		base.Execute();
		ObjectData data = source.GetData();
		foreach(BattleObject target in targetList)
		{
			ObjectData targetData = target.GetData();
			//判断是否触发防御反击
			if(target.commandToExecute.commandType == CommandType.Defence)
			{
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name",targetData.name);
				EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);
				
				source.InflictDamage(Random.Range(0,100));
				continue;
			}
			//计算是否命中，是否暴击
			MagicData magicData = DataManager.Instance.GetItemDataSet().magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});
			float hitPercent = data.skill + data.luck / 10.0f + magicData.basicACC * skillData.ACCMultiplier;//命中率
			float evadePercent = targetData.skill + targetData.luck / 10.0f;//闪避率
			float criticalPercent = data.skill / 10.0f + data.luck / 10.0f + magicData.basicCRT * skillData.CRTMultiplier / 100.0f - targetData.skill / 10.0f - targetData.luck / 10.0f;//暴击率
			float damage = (data.power + magicData.basicATK) * skillData.ATKMultiplier * (1 - targetData.insight / 250.0f);//伤害值

			//如果命中，则对方受伤
			if(target.isEvading) evadePercent += 50;
			Debug.Log("Name:" + skillData.name + " Hit:" + hitPercent + " Evade:" + evadePercent + " Crit:" + criticalPercent);
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
				args.AddMessage("Name", targetData.name);
				EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
			}
		}
	}
}
