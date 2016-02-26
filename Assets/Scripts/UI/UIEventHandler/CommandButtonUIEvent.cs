using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

	public Text nameText;
	public Text descriptionText;
	public Text atk;
	public Text acc;
	public Text crt;
	public Text pre;
	public Text post;
	public Text kck;

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

		if(command.commandType != CommandType.Attack)
		{
			atk.text = "N/A";
			acc.text = "N/A";
			crt.text = "N/A";
			kck.text = "N/A";
		}
		else
		{
			SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(command.skillID);
			if(DataManager.Instance.GetItemDataSet().IsWeaponSkill(command.skillID))
			{
				WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(command.source.GetWeapon());
				int minATK = Mathf.RoundToInt(weaponData.basicATKMin * skillData.ATKMultiplier * BattleAttribute.AttackMulti(command.source));
				int maxATK = Mathf.RoundToInt(weaponData.basicATKMax * skillData.ATKMultiplier * BattleAttribute.AttackMulti(command.source));
				int accuracy = Mathf.RoundToInt(weaponData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(command.source));
				int critical = Mathf.RoundToInt(weaponData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(command.source));
				int interrupt = Mathf.RoundToInt(weaponData.interrupt * skillData.interruptMultiplier);

				atk.text = minATK.ToString() + "-" + maxATK.ToString();
				acc.text = accuracy.ToString() + "%";
				crt.text = critical.ToString() + "%";
				kck.text = interrupt.ToString() + "%";
			}
			else
			{
				MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(command.skillID);

				int minATK = Mathf.RoundToInt(magicData.basicATKMin * skillData.ATKMultiplier * BattleAttribute.AttackMulti(command.source));
				int maxATK = Mathf.RoundToInt(magicData.basicATKMax * skillData.ATKMultiplier * BattleAttribute.AttackMulti(command.source));
				int accuracy = Mathf.RoundToInt(magicData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(command.source));
				int critical = Mathf.RoundToInt(magicData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(command.source));
				int interrupt = Mathf.RoundToInt(magicData.interrupt * skillData.interruptMultiplier);

				atk.text = minATK.ToString() + "-" + maxATK.ToString();
				acc.text = accuracy.ToString() + "%";
				crt.text = critical.ToString() + "%";
				kck.text = interrupt.ToString() + "%";
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
