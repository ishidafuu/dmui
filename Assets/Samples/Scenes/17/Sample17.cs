﻿// [概要]
// onSwitchFront()/onSwitchBack()による背後のUIが切り替わった、
// または初回生成時に前後にUIが存在していた場合、
// コールバックの呼び出しが発生します。

// [操作]
// 1. 緑画面のボタンを押す
// 2. 黄色四角内のボタンを押す

// [結果]
// ボタンを押すと黄色四角が表示され、さらにボタンを押すと
// 黄色四角が消え、青画面が表示されます。
// この際 front/back の切り替わったログが表示されます。
//
// switch front: UIFade
// switch back: Sample17Scene
// switch front:
// switch front: Sample17Dialog
// switch front: UIFade
// switch back: Sample17SceneB
// switch front:

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample17 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), null, new FadeCreator());
			UIController.Instance.AddUIBase(new Sample17Scene());
		}
	}

	class Sample17Scene : UIBase {

		public Sample17Scene() : base("UISceneA", UIGroup.Scene) {
			UIController.Instance.AddUIBase(new Sample17Frame());
		}

		public override IEnumerator OnLoaded() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
			switch (name) {
				case "ButtonCenter": {
					UIController.Instance.AddUIBase(new Sample17Dialog());
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}

	class Sample17Frame : UIBase {

		public Sample17Frame() : base("UIFrame", UIGroup.Floater, UIPreset.BackVisible | UIPreset.BackTouchable) {
		}

		public override void OnSwitchFrontUI(string uiName) {
			Debug.Log("switch front: " + uiName);
		}

		public override void OnSwitchBackUI(string uiName) {
			Debug.Log("switch back: " + uiName);
		}
	}

	class Sample17Dialog : UIBase {

		public Sample17Dialog() : base("UIDialog", UIGroup.Dialog, UIPreset.BackVisible) {
		}

		public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
			switch (name) {
				case "ButtonCenter": {
					UIController.Instance.AddUIBase(new Sample17SceneB());
					UIController.Instance.RemoveUIBase(this);
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}

	class Sample17SceneB : UIBase {

		public Sample17SceneB() : base("UISceneB", UIGroup.Scene) {
		}

		public override IEnumerator OnLoaded() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}
	}
}
