// [概要]
// UIレイヤーは再度タッチ状態になるとOnRetouchable()が呼び出されます。
// UI生成時は呼び出されません。
// [操作]
// 1. 緑画面のボタンを押す
// 2. 黄色四角内のボタンを押す
// [結果]
// 緑画面でボタンを押すと黄色の四角が表示されます。
// その後黄色四角内のボタンを押すと緑画面に戻りますが、
// この際"All Rigth"ログが表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample10 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample10Scene());
		}
	}

	class Sample10Scene : UIBase {

		public Sample10Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.AddFront(new Sample10Dialog());
					return true;
				}
				default: {
					return false;
				}
			}
		}

		public override void OnReTouchable() {
			Debug.Log("Scene10 : All Right");
		}
	}

	class Sample10Dialog : UIBase {

		public Sample10Dialog() : base("UIDialog", EnumUIGroup.Dialog, EnumUIPreset.BackVisible) {
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
	}
}
