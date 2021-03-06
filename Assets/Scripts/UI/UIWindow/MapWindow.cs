using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapWindow: MonoBehaviour {

	public GameObject playerPanel;
	public GameObject itemPanel;
	public GameObject settingPanel;

	public Button leaderButton;
	private ConfigData configData;

	void OnEnable() 
	{
		configData = DataManager.Instance.GetConfigData();
		leaderButton.image.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_BATTLE + "Avatar0" + configData.currentLeaderID, typeof(Sprite)) as Sprite;
	}
	
	/*UI CALLBACK*/
	public void EnterBattle(int battleType)
	{
		MessageEventArgs arg = new MessageEventArgs();
		arg.AddMessage("WindowID", UIWindowID.BattleWindow);
		EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, arg);
		
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("BattleType",battleType);
		if(battleType == 0)
		{
			args.AddMessage("Man",true);
			args.AddMessage("Girl",true);
			args.AddMessage("Enemy",new int[3]{101,102,103});
		}
		else if(battleType == 1)
		{
			args.AddMessage("Man",true);
			args.AddMessage("Girl",true);
			args.AddMessage("Enemy",new int[1]{104});
		}
		else 
		{
			args.AddMessage("Man",true);
			args.AddMessage("Enemy",new int[1]{105});
		}
		EventManager.Instance.PostEvent (BattleEvent.OnBattleEnter, args);
	}

	public void OnPlayerButtonClick()
	{
		playerPanel.SetActive(!playerPanel.activeSelf);
		itemPanel.SetActive(false);	
		settingPanel.SetActive(false);
	}

	public void OnItemButtonClick()
	{
		itemPanel.SetActive(!itemPanel.activeSelf);	
		playerPanel.SetActive(false);	
		settingPanel.SetActive(false);
	}

	public void OnSettingButtonClick()
	{
		settingPanel.SetActive(!settingPanel.activeSelf);
		playerPanel.SetActive(false);	
		itemPanel.SetActive(false);
	}

	public void OnLeaderButtonClick()
	{
		configData.currentLeaderID = 1 - configData.currentLeaderID;// 0->1, 1->0
		leaderButton.image.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_BATTLE + "Avatar0" + configData.currentLeaderID, typeof(Sprite)) as Sprite;
		SaveManager.Instance.SaveConfig();

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("LeaderID",configData.currentLeaderID);
		EventManager.Instance.PostEvent(UIEvent.OnSwitchLeader, args);
	}
}
