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
		ObjectData data = source.GetData();
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		MagicData magicData = DataManager.Instance.GetItemDataSet().magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});
		executeMessage = data.name + "使用了" + skillData.name + "!";
		base.Execute();

		foreach(BattleObject target in targetList)
		{
			float hit = data.skill + data.luck / 10.0f + magicData.basicACC * skillData.ACCMultiplier;//命中率
			float crit = data.skill / 10.0f + data.luck / 10.0f + magicData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
			float damage = (data.power + magicData.basicATK) * skillData.ATKMultiplier;//伤害值
			target.OnBeHit(source, skillData.skillType, hit, crit, damage);
		}
	}
}
