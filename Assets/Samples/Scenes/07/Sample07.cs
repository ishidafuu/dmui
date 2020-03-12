// [概要]
// UIレイヤーがアニメーションを伴って登場/退場します。
// また、ループとして設定してあるアニメーションはすでに再生されています。
// この登場/退場時はどのボタンも押せないようになっています。
// [操作]
// 1. 登場アニメーション中にボタンを押す。（なにも反応しない）
// 2. アニメーション終了後、ボタンを押す。
// 3. 退場アニメーション中にボタンを押す。（なにも反応しない）

// [結果]
// 緑の画面が右から登場します。その間白い四角が左右へループして動きます。
// ボタンを押すと緑の画面が左へ退場します。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample07 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample07Scene());
		}
	}

	class Sample07Scene : UIBase {

		public Sample07Scene() : base("UISceneAnimSlowlyA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
			RootTransform.Find("Layer/Square").gameObject.SetActive(true);

			yield break;
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
			Debug.Log("Scene07 : All Right");
		}
	}
}
