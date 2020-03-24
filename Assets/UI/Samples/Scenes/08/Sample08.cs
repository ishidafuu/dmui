// [概要]
// 同UIレイヤー内に複数の登場アニメーションがある場合、
// もっとも最後に終わるアニメーションを持って、登場アニメーションの完了となります。
// また、登場アニメーション後に呼び出されるOnActive()のメソッドも使用しています。
// [操作]
// 1. 緑画面の登場アニメーション中にボタンを押す。（なにも反応しない）
// 2. 緑画面は止まった中、白い四角が点滅している間にボタンを押す。（なにも反応しない）
// 3. アニメーション終了後、ボタンを押す。
// [結果]
// 緑の画面が右から登場します。同時に白い四角が点滅します。
// 緑の画面が止まっても、まだ白い四角が点滅します。
// 白い四角の点滅が終わり次第、ボタンが押せるようになります。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample08 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample08Scene());
		}
	}

	class Sample08Scene : UIBase {

		public Sample08Scene() : base("UISceneAnimA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
			RootTransform.Find("Layer/Square").gameObject.SetActive(true);
		}

		public void onActive() {
			Debug.Log("in animation end");
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					Debug.Log("start removing animation");
					UIController.Instance.Remove(this);
					return true;
				}
				default: {
					return false;
				}
			}
		}

		protected override void OnDestroy() {
			Debug.Log("Scene08 : All Right");
		}
	}
}
