// [概要]
// Replace()によるUIレイヤーの入れ替えを行います。
// 1. 追加するUIレイヤーと同グループのUIは全て削除されます。
// 2. 第2引数にUIGroupを指定すると、そのグループのUIレイヤーは全て削除されます。
// 追加UIレイヤーは対応するUIGroupへ挿入されるので、手前に別のUIレイヤーがある場合は
// その背後へ挿入されます。
// [操作]
// ボタンをクリックする
// [結果]
// 黄色の四角内のボタンを押すとフェードアウト後、
// 青の画面とその手前上下に赤い帯, 黄色の四角が表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample04 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample04Scene());
		}
	}

	class Sample04Scene : UIBase {

		public Sample04Scene() : base("UISceneA", UIGroup.Scene) {
			UIController.Instance.AddFront(new Sample04Frame());
		}

		public override IEnumerator OnLoadedBase() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			UIController.Instance.AddFront(new Sample04Dialog());
			yield break;
		}
	}

	class Sample04Frame : UIBase {

		public Sample04Frame() : base("UIFrame", UIGroup.Floater, UIPreset.BackVisible) {
		}
	}

	class Sample04Dialog : UIBase {

		public Sample04Dialog() : base("UIDialog", UIGroup.Dialog, UIPreset.BackVisible) {
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.Replace(new UIBase[]{new Sample04Scene(), new Sample04SceneB()}, new UIGroup[]{UIGroup.Dialog});
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}

	class Sample04SceneB : UIBase {

		public Sample04SceneB() : base("UISceneB", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			Debug.Log("Scene04 : All Right");
			yield break;
		}
	}
}
