using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPanel: MonoBehaviour {

	public Text nameText;
	public Text attributeText;
	public Text weaponText;
	public Text typeText;
	public Image image;
	private PlayerData playerData;
	
	void OnEnable() 
	{
		playerData = DataManager.Instance.GetPlayerDataSet().GetPlayerData(DataManager.Instance.GetConfigData().currentLeaderID);
		SetBasicView();
		SetAttributeView();
	}

	private void SetBasicView()
	{
		nameText.text = playerData.name;
		image.sprite = Resources.Load("UI/Common/Player0" + playerData.id, typeof(Sprite)) as Sprite;
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
