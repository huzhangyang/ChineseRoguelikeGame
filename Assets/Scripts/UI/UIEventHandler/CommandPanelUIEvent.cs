using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandPanelUIEvent : MonoBehaviour {

	public Image commandAttack;
	public Image commandDefence;
	public Image commandItem;
	public Image commandStrategy;

	public void SetButtonActive()
	{
		commandAttack.color = new Color(1,1,1,0.5f);
		commandDefence.color = new Color(1,1,1,0.5f);
		commandItem.color = new Color(1,1,1,0.5f);
		commandStrategy.color = new Color(1,1,1,0.5f);

		Player player = BattleManager.Instance.GetCurrentPlayer();
		for(int i = 0; i < player.availableCommands.Count; i++)
		{
			switch(player.availableCommands[i].commandType)
			{
			case CommandType.Attack:
				commandAttack.color = new Color(1,1,1,1);
				break;
			case CommandType.Defence:
				commandDefence.color = new Color(1,1,1,1);
				break;
			case CommandType.Item:
				commandItem.color = new Color(1,1,1,1);
				break;
			case CommandType.Strategy:
				commandStrategy.color = new Color(1,1,1,1);
				break;
			}
		}
	}
}
