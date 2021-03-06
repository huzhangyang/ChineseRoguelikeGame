﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueDataSet : ScriptableObject 
{
	public List<DialogueData> dataSet = new List<DialogueData>();
	
	public DialogueData GetDialogueData(int dialogueID)
	{
		foreach (DialogueData data in dataSet)
		{
			if (data.dialogueID == dialogueID)
				return data;
		}
		Debug.LogError("Asking for an non-exist dialogue:" + dialogueID);
		return null;
	}
}

[System.Serializable]
public class DialogueData {
/*
 * 对话数据。
*/
	public int dialogueID;
	public List<SentenceData> sentences = new List<SentenceData> ();
}

[System.Serializable]
public struct SentenceData {
	public string tellerName;//讲话者姓名
	public int avatarID;//讲话者头像ID
	public string content;//讲话内容
}