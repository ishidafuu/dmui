// [概要]
// UIレイヤーが画面に表示した後にUIPartを追加したい場合は
// UIControler.AttachParts() で追加することができます。
// [操作]
// 1. 上部のボタンを押す
// 2. 増えたボタンを押す
// [結果]
// 上部のボタンを押すことで、下部にボタンが表示されます。
// 下部のボタンは押すとログを表示します。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample15 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample15Scene());
		}
	}

	class Sample15Scene : UIBase {

		private int m_count = 0;

		public Sample15Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonTop": {
					m_count++;
					UIController.Instance.AttachParts(this, new List<UIPart>(){new Sample15Button(m_count)});
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}

	class Sample15Button : UIPart {

		private int m_id = 0;

		public Sample15Button(int id) : base("UIButton") {
			m_id = id;
		}

		public override async UniTask OnLoadedPart(UIBase targetLayer) {
			Text text = RootTransform.Find("Text").GetComponent<Text>();
			text.text = m_id.ToString();

			Transform layer = targetLayer.RootTransform.Find("Layer");
			RootTransform.SetParent(layer);
			RootTransform.localPosition = new Vector3(426, 100 * m_id, 0);
			RootTransform.localScale = Vector3.one;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			Debug.Log("push button: " + m_id);
			Debug.Log("Scene15 : All Right");

			return true;
		}
	}
}
