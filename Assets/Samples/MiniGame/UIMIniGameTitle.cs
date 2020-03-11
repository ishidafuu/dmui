using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DM;

public class UIMiniGameTitle : UIBase {

	public UIMiniGameTitle() : base("MiniGame/MiniGameTitle", UIGroup.MainScene) {
	}

	public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
		switch (name) {
			case "HowToPlay": {
				UIController.Instance.AddUIBase(new UIMiniGameHowToPlay());
				return true;
			}
			case "Panel": {
				UIController.Instance.Replace(new UIBase[]{ new UIMiniGameMain() });
				return true;
			}
		}
		return false;
	}
}
