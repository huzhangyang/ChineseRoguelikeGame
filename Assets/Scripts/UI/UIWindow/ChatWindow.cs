using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour {

	public Text dialogText;
	public Text tellerName;
	public Image tellerImage;
	public int dialogID = 0;

	DialogueData dialogueData;
	int currentTextCount;

	void Start () 
	{
		dialogueData = DataManager.Instance.GetDialogueDataSet ().GetDialogueData(dialogID);
		currentTextCount = 1;
		OnNextDialogue ();
	}

	public void OnNextDialogue()
	{
		if (dialogueData == null)
			return;
		if(currentTextCount <= dialogueData.sentences.Count)
		{
			SentenceData sentence = dialogueData.sentences[currentTextCount - 1];
			dialogText.text = sentence.content;
			tellerName.text = sentence.tellerName;
			//TODO set tellerImage according to tellerID
			//tellerImage.sprite = Resources.Load("" + sentence.avatarID, typeof Sprite) as Sprite;
			currentTextCount++;     
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}
	
	public void OnSkipDialogue()
	{
		this.gameObject.SetActive(false);
	}
}
