using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour {

	public Text manInfo;
	public Text girlInfo;

	void OnEnable () 
	{
		PlayerData mandata = DataManager.Instance.GetPlayerDataSet().GetPlayerData(0);
		manInfo.text = "";
		manInfo.text += mandata.name + "\n";
		manInfo.text += "Talent:" + "???\n";
		manInfo.text += "Nature:" + "???\n";
		manInfo.text += "BattleType:" + mandata.battleType.ToString() + "\n";
		manInfo.text += "MaxHP:" + mandata.maxHP.ToString() + "\n";
		manInfo.text += "Power:" + mandata.power.ToString() + "\n";
		manInfo.text += "Agility:" + mandata.agility.ToString() + "\n";
		manInfo.text += "Toughness:" + mandata.toughness.ToString() + "\n";
		manInfo.text += "Insight:" + mandata.insight.ToString() + "\n";
		manInfo.text += "Skill:" + mandata.skill.ToString() + "\n";
		manInfo.text += "Luck:" + mandata.luck.ToString() + "\n";
		manInfo.text += "Eloquence:" + mandata.eloquence.ToString() + "\n";
		manInfo.text += "Weapon:" + DataManager.Instance.GetItemDataSet().GetWeaponData(mandata.weaponID).name + "\n";
		for(int i = 0;  i < mandata.magicIDs.Count; i++)
		{
			manInfo.text += "Magic" + (i + 1) + ":" + DataManager.Instance.GetItemDataSet().GetMagicData(mandata.magicIDs[i]).name + "\n";
		}

		PlayerData girldata = DataManager.Instance.GetPlayerDataSet().GetPlayerData(1);
		girlInfo.text = "";
		girlInfo.text += girldata.name + "\n";
		girlInfo.text += "Talent:" + "???\n";
		girlInfo.text += "Nature:" + "???\n";
		girlInfo.text += "BattleType:" + girldata.battleType.ToString() + "\n";
		girlInfo.text += "MaxHP:" + girldata.maxHP.ToString() + "\n";
		girlInfo.text += "Power:" + girldata.power.ToString() + "\n";
		girlInfo.text += "Agility:" + girldata.agility.ToString() + "\n";
		girlInfo.text += "Toughness:" + girldata.toughness.ToString() + "\n";
		girlInfo.text += "Insight:" + girldata.insight.ToString() + "\n";
		girlInfo.text += "Skill:" + girldata.skill.ToString() + "\n";
		girlInfo.text += "Luck:" + girldata.luck.ToString() + "\n";
		girlInfo.text += "Eloquence:" + girldata.eloquence.ToString() + "\n";
		girlInfo.text += "Weapon:" + DataManager.Instance.GetItemDataSet().GetWeaponData(girldata.weaponID).name + "\n";

		for(int i = 0;  i < girldata.magicIDs.Count; i++)
		{
			girlInfo.text += "Magic" + (i + 1) + ":" + DataManager.Instance.GetItemDataSet().GetMagicData(girldata.magicIDs[i]).name + "\n";
		}
	}

}
