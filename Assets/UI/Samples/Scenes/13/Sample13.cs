// [概要]
// UIImplementsにより音再生は外部からアタッチされますが、
// OnClick()メソッドでreturn trueを行うとデフォルトのクリック音を再生します。
// また、BGMはBGMが設定してあり表示されているレイヤーのうち、
// 最も手前のUIレイヤーのBGMを再生します。
// [操作]
// ボタンを押す
// [結果]
// ボタンを押すとデフォルトではないSE再生ログが表示されます。
// また、緑/青画面で設定したBGM再生のログが表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample13 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample13Scene());
		}
	}

	class Sample13Scene : UIBase {

		public Sample13Scene() : base("UISceneA", EnumUIGroup.Scene, EnumUIPreset.None, "BGM_SceneA") {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.AddFront(new Sample13SceneB());

					uiSound.m_PlayName = "Center SE";
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}


	class Sample13SceneB : UIBase {

		public Sample13SceneB() : base("UISceneAnimB", EnumUIGroup.Scene, EnumUIPreset.None, "BGM_SceneB") {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.Remove(this);
					Debug.Log("Scene13 : All Right");
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}
}
