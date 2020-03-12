﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DM;

public class UIMiniGameResult : UIBase {

	private float m_score;

	public UIMiniGameResult(float score) : base("MiniGame/MiniGameResult", UIGroup.Dialog, UIPreset.BackVisible) {
		m_score = score;
	}

	public override IEnumerator OnLoadedBase() {
		Text score = Root.Find("Panel/Score").GetComponent<Text>();
		score.text = m_score.ToString("N2");

		yield break;
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		switch (touch.Listener.name) {
			case "Title": {
				UIController.Instance.Replace(new UIBase[] { new UIMiniGameTitle() }, new UIGroup[]{ UIGroup.Dialog });
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
