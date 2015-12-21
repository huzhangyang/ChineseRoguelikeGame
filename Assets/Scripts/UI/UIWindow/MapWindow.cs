using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapWindow: MonoBehaviour {
	
	public Canvas battleCanvas;
	public Canvas mapCanvas;

	public GameObject statusPanel;
	public GameObject upgradePanel;
	public GameObject itemPanel;
	public GameObject settingPanel;

	public Button leaderButton;
	private ConfigData configData;

	void Start() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleFinish, OnBattleFinish);
		configData = DataManager.Instance.GetConfigData();
		leaderButton.image.sprite = Resources.Load("UI/Battle/Avatar0" + configData.currentLeaderID, typeof(Sprite)) as Sprite;
	}

	void OnDestroy()
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleFinish, OnBattleFinish);
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

	public void OnStatusButtonClick()
	{
		statusPanel.SetActive(!statusPanel.activeSelf);
	}

	public void OnUpgradeButtonClick()
	{
		
	}

	public void OnItemButtonClick()
	{
		
	}

	public void OnSettingButtonClick()
	{

	}

	public void OnLeaderButtonClick()
	{
		configData.currentLeaderID = 1 - configData.currentLeaderID;// 0->1, 1->0
		leaderButton.image.sprite = Resources.Load("UI/Battle/Avatar0" + configData.currentLeaderID, typeof(Sprite)) as Sprite;
	}

	/*EVENT CALLBACK*/
	void OnBattleFinish(MessageEventArgs args)
	{
		mapCanvas.gameObject.SetActive (true);
		battleCanvas.gameObject.SetActive (false);
	}
}
