using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour {

	public Canvas battleCanvas;
	public Canvas mapCanvas;
	public Text manInfo;
	public Text girlInfo;

	void OnEnable() 
	{
	}

	void Start() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleFinish, OnBattleFinish);
		ShowAttribute();
	}

	/*UI CALLBACK*/
	public void EnterBattle(int battleType)
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("BattleType",battleType);
		if(battleType == 0)
		{
			args.AddMessage("Man",true);
			args.AddMessage("Girl",true);
			args.AddMessage("Enemy",new int[3]{10,10,10});
		}
		else if(battleType == 1)
		{
			args.AddMessage("Man",true);
			args.AddMessage("Girl",true);
			args.AddMessage("Enemy",new int[1]{11});
		}
		else 
		{
			args.AddMessage("Man",true);
			args.AddMessage("Enemy",new int[1]{12});
		}
		EventManager.Instance.PostEvent (BattleEvent.OnBattleEnter, args);
	}

	public void SetAttribute(int mode)
	{
		PlayerData mandata = DataManager.Instance.GetPlayerDataSet().GetPlayerData(0);
		PlayerData girldata = DataManager.Instance.GetPlayerDataSet().GetPlayerData(1);
		if(mode == 0)
		{
			mandata.stamina = 10;
			mandata.power = 10;
			mandata.agility = 10;
			mandata.skill = 10;
			mandata.toughness = 10;
			mandata.insight = 10;
			mandata.eloquence = 10;
			mandata.luck = 10;

			girldata.stamina = 11;
			girldata.power = 11;
			girldata.agility = 11;
			girldata.skill = 11;
			girldata.toughness = 11;
			girldata.insight = 11;
			girldata.eloquence = 11;
			girldata.luck = 11;
		}
		else if(mode == 1)
		{
			mandata.stamina = 49;
			mandata.power = 49;
			mandata.agility = 49;
			mandata.skill = 49;
			mandata.toughness = 49;
			mandata.insight = 49;
			mandata.eloquence = 49;
			mandata.luck = 49;
			
			girldata.stamina = 50;
			girldata.power = 50;
			girldata.agility = 50;
			girldata.skill = 50;
			girldata.toughness = 50;
			girldata.insight = 50;
			girldata.eloquence = 50;
			girldata.luck = 50;
		}
		else if(mode == 2)
		{
			mandata.stamina = 99;
			mandata.power = 99;
			mandata.agility = 99;
			mandata.skill = 99;
			mandata.toughness = 99;
			mandata.insight = 99;
			mandata.eloquence = 99;
			mandata.luck = 99;
			
			girldata.stamina = 128;
			girldata.power = 128;
			girldata.agility = 128;
			girldata.skill = 128;
			girldata.toughness = 128;
			girldata.insight = 128;
			girldata.eloquence = 128;
			girldata.luck = 128;
		}
		ShowAttribute();
	}

	void OnBattleFinish(MessageEventArgs args)
	{
		mapCanvas.gameObject.SetActive (true);
		battleCanvas.gameObject.SetActive (false);
	}

	void ShowAttribute()
	{
		PlayerData mandata = DataManager.Instance.GetPlayerDataSet().GetPlayerData(0);
		manInfo.text = "";
		manInfo.text += mandata.name + "\n";
		manInfo.text += "Talent:" + "???\n";
		manInfo.text += "Nature:" + "???\n";
		manInfo.text += "BattleType:" + mandata.battleType.ToString() + "\n";
		manInfo.text += "Stamina:" + mandata.stamina.ToString() + "\n";
		manInfo.text += "Power:" + mandata.power.ToString() + "\n";
		manInfo.text += "Agility:" + mandata.agility.ToString() + "\n";
		manInfo.text += "Skill:" + mandata.skill.ToString() + "\n";
		manInfo.text += "Toughness:" + mandata.toughness.ToString() + "\n";
		manInfo.text += "Insight:" + mandata.insight.ToString() + "\n";
		manInfo.text += "Eloquence:" + mandata.eloquence.ToString() + "\n";
		manInfo.text += "Luck:" + mandata.luck.ToString() + "\n";
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
		girlInfo.text += "Stamina:" + girldata.stamina.ToString() + "\n";
		girlInfo.text += "Power:" + girldata.power.ToString() + "\n";
		girlInfo.text += "Agility:" + girldata.agility.ToString() + "\n";
		girlInfo.text += "Skill:" + girldata.skill.ToString() + "\n";
		girlInfo.text += "Toughness:" + girldata.toughness.ToString() + "\n";
		girlInfo.text += "Insight:" + girldata.insight.ToString() + "\n";
		girlInfo.text += "Eloquence:" + girldata.eloquence.ToString() + "\n";
		girlInfo.text += "Luck:" + girldata.luck.ToString() + "\n";
		girlInfo.text += "Weapon:" + DataManager.Instance.GetItemDataSet().GetWeaponData(girldata.weaponID).name + "\n";
		
		for(int i = 0;  i < girldata.magicIDs.Count; i++)
		{
			girlInfo.text += "Magic" + (i + 1) + ":" + DataManager.Instance.GetItemDataSet().GetMagicData(girldata.magicIDs[i]).name + "\n";
		}
	}
}
