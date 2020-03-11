// [概要]
// ボタンクリック時の挙動としてUIレイヤーの削除を行います。
// また、OnDestroy()による削除時に実行されるメソッドも使用しています。
// [操作]
// ボタンをクリックする
// [結果]
// 緑の画面中央にボタンが表示され、ボタンを押すと画面上に何も表示されません。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample02 : MonoBehaviour {
		private void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample02Scene());
		}
	}

	class Sample02Scene : UIBase {

		public Sample02Scene() : base("UISceneA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
			switch (name) {
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
			Debug.Log("Scene02 : All Right");
		}
	}
}
