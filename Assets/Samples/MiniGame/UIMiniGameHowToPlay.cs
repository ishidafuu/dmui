﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DM;

public class UIMiniGameHowToPlay : UIBase {

	public UIMiniGameHowToPlay() : base("MiniGame/MiniGameHowToPlay", UIGroup.Dialog, UIPreset.BackVisible | UIPreset.TouchEventCallable) {
	}

	public override bool OnTouchUp(TouchEvent touch) {
		UIController.Instance.Remove(this);
		return true;
	}
}
