// [概要]
// UIController.SetScreenTouchable()を使用することによって
// 画面全体のタッチ反応を切ることができます。
// フェイルセーフとして呼び出したUIBaseが削除された場合、
// タッチ判定は元に戻ります。
// [操作]
// 画面下のボタンを押す
// [結果]
// ボタンを押すと数字が表示されますが、
// 100までの間は再度ボタンを押すことができません。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample18 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample18Scene());
		}
	}

	class Sample18Scene : UIBase {
		private int m_count = 0;

		public Sample18Scene() : base("UISceneA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonBottom": {
					UIController.Instance.SetScreenTouchable(this, false);
					IsScheduleUpdate = true;
					m_count = 0;
					return true;
				}
				default: {
					return false;
				}
			}
		}

		public override void OnUpdate() {
			if (++m_count >= 100) {
				UIController.Instance.SetScreenTouchable(this, true);
				IsScheduleUpdate = false;
			}
			Text text = RootTransform.Find("Layer/Text").GetComponent<Text>();
			text.text = m_count.ToString();
		}
	}
}
