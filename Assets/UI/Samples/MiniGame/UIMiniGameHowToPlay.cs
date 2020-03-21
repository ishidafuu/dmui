using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DM;

public class UIMiniGameHowToPlay : UIBase {

	public UIMiniGameHowToPlay() : base("MiniGame/MiniGameHowToPlay", EnumUIGroup.Dialog, EnumUIPreset.BackVisible | EnumUIPreset.TouchEventCallable) {
	}

	public override bool OnTouchUp(TouchEvent touch) {
		UIController.Instance.Remove(this);
		return true;
	}
}
