using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DM;

public class UIMiniGameStartEffect : UIBase {

	public UIMiniGameStartEffect() : base("MiniGame/MiniGameStartEffect", EnumUIGroup.Dialog, EnumUIPreset.BackVisible) {
	}

	public override void OnActive() {
		UIController.Instance.Dispatch("start", null);
		UIController.Instance.Remove(this);
	}
}
