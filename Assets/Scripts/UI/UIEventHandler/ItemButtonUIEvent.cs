using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonUIEvent : MonoBehaviour {
	
	public Text nameText;
	public Text descriptionText;
	public Text numText;

	ItemData itemData;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}
	
	public void Init(int itemID, int num)
	{
		itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);

		nameText.text = itemData.name;
		descriptionText.text = itemData.shortDesc;
		numText.text = "*" + num.ToString();

		if(itemData.type == ItemType.Weapon)
		{
			WeaponData weaponData = itemData as WeaponData;
			descriptionText.text = System.String.Format("攻击:{0}~{1}   命中:{2}   速度:{3}   暴击:{4}",
				weaponData.basicATKMin, weaponData.basicATKMax, weaponData.basicACC, weaponData.basicSPD, weaponData.basicCRT);
		}
		else if(itemData.type == ItemType.Magic)
		{
			MagicData magicData = itemData as MagicData;
			descriptionText.text = System.String.Format("攻击:{0}~{1}   命中:{2}   速度:{3}   暴击:{4}",
				magicData.basicATKMin, magicData.basicATKMax, magicData.basicACC, magicData.basicSPD, magicData.basicCRT);
		}

		GetComponent<Button>().onClick.AddListener(delegate()  
		{  
			OnClick(name);  
		});		  
	}
	
	void OnClick(string name)
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("ItemID", itemData.id);
		EventManager.Instance.PostEvent (UIEvent.OnItemClicked, args);
	}

	void OnItemClicked(MessageEventArgs args)
	{
		if(args.GetMessage<int>("ItemID") == itemData.id)
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Light", typeof(Sprite)) as Sprite;
		}
		else
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Dark", typeof(Sprite)) as Sprite;
		}
	}
	
}
