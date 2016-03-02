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

	void Awake()
	{
		EventManager.Instance.RegisterEvent (GameEvent.PlayDialogue, OnPlayDialogue);
	}

	void OnDestroy()
	{
		EventManager.Instance.UnRegisterEvent (GameEvent.PlayDialogue, OnPlayDialogue);
	}

	void OnPlayDialogue(MessageEventArgs args)
	{
		dialogID = args.GetMessage<int>("DialogueID");
		PlayDialogue();
	}

	public void PlayDialogue() 
	{
		this.gameObject.SetActive(true);
		this.GetComponent<CanvasGroup>().alpha = 1;
		dialogueData = DataManager.Instance.GetDialogueDataSet ().GetDialogueData(dialogID);
		currentTextCount = 0;
		OnNextDialogue ();
	}

	public void OnNextDialogue()
	{
		if (dialogueData == null)
			return;
		if(currentTextCount < dialogueData.sentences.Count)
		{
			SentenceData sentence = dialogueData.sentences[currentTextCount];
			dialogText.text = sentence.content;
			tellerName.text = sentence.tellerName;
			//TODO set tellerImage according to tellerID
			//tellerImage.sprite = Resources.Load("" + sentence.avatarID, typeof Sprite) as Sprite;
			currentTextCount++;     
		}
		else
		{
			OnSkipDialogue();
		}
	}
	
	public void OnSkipDialogue()
	{
		this.gameObject.SetActive(false);
		this.GetComponent<CanvasGroup>().alpha = 0;
	}
}
