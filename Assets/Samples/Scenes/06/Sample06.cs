﻿// [概要]
// 表示領域以外をタッチすることによる挙動を行います。
// この際、表示物のタッチ反応は後ろへ透過させなくしております。
// [操作]
// 1. 画面中央の黄色の四角をタッチする（何も反応しない）
// 2. 黄色の四角の範囲外をタッチする、または、画面下のボタン上をタッチ
// 3. 黄色の画面が削除されたら中央のボタンを押す。
// [結果]
// 黄色の範囲外の領域を押すと黄色の画面が消えます。
// 黄色が削除された後、中央のボタンを押すと緑の画面が消え何も表示されなくなります。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample06 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddUIBase(new Sample06Scene());
		}
	}

	class Sample06Scene : UIBase {

		public Sample06Scene() : base("UISceneA", UIGroup.Scene) {
			UIController.Instance.AddUIBase(new Sample06Dialog());
		}

		public override IEnumerator OnLoaded() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
			switch (name) {
				case "ButtonCenter": {
					UIController.Instance.RemoveUIBase(this);
					return true;
				}
				default: {
					return false;
				}
			}
		}

		protected override void OnDestroy() {
			Debug.Log("Scene06 : All Right");
		}
	}

	class Sample06Dialog : UIBase {
		bool m_layerTouch = false;

		public Sample06Dialog()
		: base("UIDialog", UIGroup.Dialog, UIPreset.BackVisible | UIPreset.TouchEventCallable) {
		}

		public override IEnumerator OnLoaded() {
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			yield break;
		}

		public override bool OnTouchDown(string name, GameObject gameObject, PointerEventData pointer) {
			if (name == UIController.LAYER_TOUCH_AREA_NAME) {
				m_layerTouch = true;
			}
			return false;
		}

		public override bool OnTouchUp(string name, GameObject gameObject, PointerEventData pointer) {
			if (name == UIController.LAYER_TOUCH_AREA_NAME) {
				if (m_layerTouch) {
					UIController.Instance.RemoveUIBase (this);
					return true;
				}
			} else {
				m_layerTouch = false;
			}
			return false;
		}
	}
}
