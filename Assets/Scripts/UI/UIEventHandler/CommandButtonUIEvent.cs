using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

	public Text nameText;
	public Text descriptionText;
	public Text pre;
	public Text post;
	public Text acc;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(BattleEvent.OnCommandClicked, OnCommandClicked);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(BattleEvent.OnCommandClicked, OnCommandClicked);
	}

	public void Init(Command command, bool availAble)
	{
		nameText.text = command.commandName;
		descriptionText.text = command.commandDescription;
		pre.text = (100f / command.preExecutionSpeed).ToString("F2") + "s";
		post.text = (command.postExecutionRecover / 60f).ToString("F2") + "s";
		acc.text = "N/A";
		if(command.commandType == CommandType.Attack)
		{
			SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(command.skillID);
			if(DataManager.Instance.GetItemDataSet().IsWeaponSkill(command.skillID))
			{
				WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(command.source.GetWeapon());
				int accuracy = Mathf.RoundToInt(weaponData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(command.source));
				acc.text = accuracy.ToString() + "%";
			}
			else
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(command.skillID);
				int accuracy = Mathf.RoundToInt(magicData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(command.source));
				acc.text = accuracy.ToString() + "%";
			}
		}

		if(availAble)
		{
			GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});	
			nameText.color = Color.white;
			descriptionText.color = Color.white;
		}
		else
		{
			nameText.color = Color.gray;
			descriptionText.color = Color.gray;
		}  
	}

	void OnClick()
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandName", nameText.text);
		EventManager.Instance.PostEvent (BattleEvent.OnCommandClicked, args);
	}

	void OnCommandClicked(MessageEventArgs args)
	{
		if(args.GetMessage<string>("CommandName") == nameText.text)
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Light", typeof(Sprite)) as Sprite;
		}
		else
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Dark", typeof(Sprite)) as Sprite;
		}
	}
}
