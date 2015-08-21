using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public Command AISelectCommand()
	{
		Command command= Command.BuildWithSkillID(Random.Range(1,11));
		command.target = AISelectTarget();
		return command;
	}

	public BattleObject AISelectTarget()
	{
		while(true)
		{
			foreach(Player player in BattleLogic.players)
			{
				if(Random.Range(0,2) > 0)
					return player;
			}
		}
	}
}
