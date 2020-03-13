using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DM;
using UniRx.Async;

public class UIMiniGameResult : UIBase {

	private float m_score;

	public UIMiniGameResult(float score) : base("MiniGame/MiniGameResult", EnumUIGroup.Dialog, EnumUIPreset.BackVisible) {
		m_score = score;
	}

	public override async UniTask OnLoadedBase() {
		Text score = RootTransform.Find("Panel/Score").GetComponent<Text>();
		score.text = m_score.ToString("N2");
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		switch (touch.Listener.name) {
			case "Title": {
				UIController.Instance.Replace(new UIBase[] { new UIMiniGameTitle() }, new EnumUIGroup[]{ EnumUIGroup.Dialog });
				return true;
			}
			case "Retry": {
				UIController.Instance.Dispatch("retry", null);
				UIController.Instance.Remove(this);
				return true;
			}
		}
		return false;
	}
}
