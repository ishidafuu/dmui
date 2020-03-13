// [概要]
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
			UIController.Instance.AddFront(new Sample06Scene());
		}
	}

	class Sample06Scene : UIBase {

		public Sample06Scene() : base("UISceneA", EnumUIGroup.Scene) {
			UIController.Instance.AddFront(new Sample06Dialog());
		}

		public override IEnumerator OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.Remove(this);
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
		 const string LAYER_TOUCH_AREA_NAME = "LayerTouchArea";

		public Sample06Dialog()
		: base("UIDialog", EnumUIGroup.Dialog, EnumUIPreset.BackVisible | EnumUIPreset.TouchEventCallable) {
		}

		public override IEnumerator OnLoadedBase() {
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			yield break;
		}

		public override bool OnTouchDown(TouchEvent touch) {
			if (touch.Listener.name == LAYER_TOUCH_AREA_NAME) {
				m_layerTouch = true;
			}
			return false;
		}

		public override bool OnTouchUp(TouchEvent touch) {
			if (touch.Listener.name == LAYER_TOUCH_AREA_NAME) {
				if (m_layerTouch) {
					UIController.Instance.Remove (this);
					return true;
				}
			} else {
				m_layerTouch = false;
			}
			return false;
		}
	}
}
