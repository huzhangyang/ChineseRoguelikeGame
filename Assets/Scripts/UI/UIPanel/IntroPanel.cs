using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroPanel : MonoBehaviour {

	Text text;
	string[] content = new string[5];
	int currentPlaying = 0;

	void OnEnable()
	{
		AudioManager.Instance.PlayBGM ("Intro");

		text = GetComponentInChildren<Text>();
		content[0] = "盘古开天辟地，阳清为天，阴浊为地。后而日月更移，天地之间始有生灵。人以天地万物为师，师法自然，脱出兽性，故被尊为万物之灵长。万物生于大地之上，是为地气所化，然长于天穹之下，得造化所钟。";
		content[1] = "世间清浊之气循环往复；有神山，上达天界，名“天柱”，是为清气运行之枢纽。其时天地间灵气充盈，时有人兽修习驭使灵气之法。驭使灵气者，或为神仙，或为兽怪，各据一方，相争不下。又有魑魅魍魉妖魔精怪，祸乱人间，莫能尽除。";
		content[2] = "时过千年，修习灵气者日众，终不能共戴一天。是而，神魔作兵，各展其能，以争帝位。开战之时，则翻江倒海，移山裂地，天地为之动摇。一时生灵涂炭，人不得活。";
		content[3] = "天不兼覆，地不周载，洪水泛滥，天火蔓延，猛兽食颛民，精怪兴祸乱。于是有女娲者，炼五色石以补苍天；苍天补，四极正，淫水涸，九州平。然天柱断折，苍天已补，清气难行。帝禹感于精怪魔兽为祸人间，万民如蝼蚁不能自保。";
		content[4] = "欲治九州，必绝灵气。故而封天绝地，使清气居于天上，不入下界。灵气为法术之根本，故神仙者避居天上，妖魔者匿于地下，不再出。复治洪水，除蛟虫，下界始归于人，是为人界，万民修养生息，人类兴而禽兽隐。自此，人界灵气淡薄，偶有人兽得道，然万中无一，魔鬼精怪之说灭绝矣。";
		currentPlaying = 0;
		PlayNextText();
	}

	public void OnClick()
	{
		PlayNextText();
	}

	void PlayNextText()
	{
		text.DOKill();
		text.text = "";
		if(currentPlaying < content.Length)
		{
			text.DOText(content[currentPlaying], 10).SetEase(Ease.Linear);
			currentPlaying++;
		}
		else
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("WindowID", UIWindowID.LoadingWindow);
			EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, args);

			this.gameObject.SetActive(false);
		}

	}

}
