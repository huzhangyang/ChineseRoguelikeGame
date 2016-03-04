using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BuffIconUIEvent : MonoBehaviour {

	public Image icon;
	public Text description;
	public Text turns;

	public void Init(BuffData data, int effectTurns)
	{
		string path = GlobalDataStructure.PATH_UIIMAGE_ICON + "Buff_" + data.buffEffect;
		this.icon.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
		description.text = data.name + ":" + data.description;
		turns.text = effectTurns >= 0 ? effectTurns.ToString() : "";
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});

		description.transform.parent.localScale = new Vector3(0, 0, 0);
	}

	public void SetEffectTurns(int effectTurns)
	{
		turns.text = effectTurns >= 0 ? effectTurns.ToString() : "";
	}

	void OnClick()
	{
		if(!DOTween.IsTweening(description.transform.parent))
		{
			description.transform.parent.DOScale(1, 0.2f).OnComplete(()=>{description.transform.parent.DOScale(0, 0.5f).SetDelay(1);});
		}
	}

}
