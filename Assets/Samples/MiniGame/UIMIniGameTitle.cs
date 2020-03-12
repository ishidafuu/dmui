using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DM;

public class UIMiniGameTitle : UIBase {

	public UIMiniGameTitle() : base("MiniGame/MiniGameTitle", UIGroup.MainScene) {
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		switch (touch.Listener.name) {
			case "HowToPlay": {
				UIController.Instance.AddFront(new UIMiniGameHowToPlay());
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
