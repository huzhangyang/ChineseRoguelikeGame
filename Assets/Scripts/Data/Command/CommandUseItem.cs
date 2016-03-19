using UnityEngine;
using System;
using System.Collections;

public class CommandUseItem : Command
{
	public CommandUseItem(int itemID, int itemCount)
	{
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);
		commandType = CommandType.Item;
		commandName = String.Format("使用道具({0}:{1})", itemData.name, itemCount);
		commandDescription = itemData.shortDesc;

		targetType = itemData.targetType;
		preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		postExecutionRecover = 0;
		
		this.itemID = itemID;
	}

	protected override void SetExecuteMessage()
	{
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);
		executeMessage = String.Format("{0}使用了{1}!", source.GetName(), itemData.name);
	}

	protected override void Execute()
	{
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);
		foreach(BattleObject target in targetList)
		{
			ItemEffect.ExecuteItemEffect(target, itemData.effectString);
			source.ConsumeItem(itemID);
		}
	}

	public override bool IsAvailable()
	{
		return source.GetItemCount(itemID) > 0;
	}
}
