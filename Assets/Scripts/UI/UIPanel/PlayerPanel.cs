using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPanel: MonoBehaviour {

	public Text nameText;
	public Text attributeText;
	public Text moreAttributeText;
	public Text weaponText;
	public Text typeText;
	public Image image;
	private PlayerData playerData;

	void Start()
	{
		SetData();
	}
	
	void OnEnable()
	{
		EventManager.Instance.RegisterEvent(UIEvent.OnSwitchLeader, OnSwitchLeader);
	}
	
	void OnDisable()
	{
		EventManager.Instance.UnRegisterEvent(UIEvent.OnSwitchLeader, OnSwitchLeader);
	}
	
	void OnSwitchLeader(MessageEventArgs args)
	{
		SetData();
	}

	void SetData() 
	{
		playerData = DataManager.Instance.GetPlayerDataSet().GetPlayerData(DataManager.Instance.GetConfigData().currentLeaderID);
		SetBasicView();
		SetAttributeView();
	}

	private void SetBasicView()
	{
		nameText.text = playerData.name;
		image.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Player0" + playerData.id, typeof(Sprite)) as Sprite;
		typeText.text = playerData.battleType.ToString();

		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(playerData.weaponID);
		weaponText.text = weaponData.name;
	}

	private void SetAttributeView()
	{
		attributeText.text = "";
		attributeText.text += playerData.stamina + "\n";
		attributeText.text += playerData.power + "\n";
		attributeText.text += playerData.agility + "\n";
		attributeText.text += playerData.skill + "\n";
		attributeText.text += playerData.toughness + "\n";
		attributeText.text += playerData.insight + "\n";
		attributeText.text += playerData.eloquence + "\n";
		attributeText.text += playerData.luck + "\n";
		SetMoreAttributeView();
	}

	private void SetMoreAttributeView()
	{
		moreAttributeText.text = "";
		moreAttributeText.text += "生命值:" + BattleAttribute.MaxHP(playerData) + "\t\t\t";
		moreAttributeText.text += "速度:" + BattleAttribute.Speed(playerData) + "\n";
		moreAttributeText.text += "攻击倍率:" + (int)(BattleAttribute.AttackMulti(playerData) * 100) + "%\t\t";
		moreAttributeText.text += "免伤率:" + (int)(BattleAttribute.DefenceMulti(playerData) * 100) + "%\n";
		moreAttributeText.text += "命中率:" + (int)BattleAttribute.ExtraAccuracy(playerData) + "%\t\t\t";
		moreAttributeText.text += "回避率:" +  (int)BattleAttribute.ExtraEvasion(playerData) + "%\n";
		moreAttributeText.text += "暴击率:" + (int)BattleAttribute.ExtraCrit(playerData)+ "%\t\t\t";
		moreAttributeText.text += "抗暴击率:" + (int)BattleAttribute.ExtraCritResist(playerData) + "%\n";
	}
	
	/*UI CALLBACK*/

	public void OnSetStamina(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.stamina = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetPower(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.power = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetAgility(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.agility = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetSkill(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.skill = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetToughness(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.toughness = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetInsight(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.insight = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetEloquence(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.eloquence = int.Parse(value);
		SetAttributeView();
	}

	public void OnSetLuck(string value)
	{
		if(string.IsNullOrEmpty(value)) return;
		playerData.luck = int.Parse(value);
		SetAttributeView();
	}
	
	public void OnSetWeapon(string weaponID)
	{
		if(string.IsNullOrEmpty(weaponID)) return;
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(int.Parse(weaponID));
		if(weaponData != null)
		{
			playerData.weaponID = int.Parse(weaponID);
			SetBasicView();
		}
	}
}
