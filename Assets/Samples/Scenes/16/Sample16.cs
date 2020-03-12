// [概要]
// これまでは、UIPartを追加に関しては、UIPartでリソースのパスを指定しますが、
// すでに生成されたオブジェクトに対してはAttachParts()を使用することで、
// UIPartを割り当てることができます　（他のオブジェクトから新しく複製するのも同等です。）
// この場合、オブジェクトが削除対象となるときはDetachParts()を呼び出す必要があります。
// [操作]
// 1. 下部のボタンを押す (ログが出る)
// 2. 上部のボタンを押す
// 3. 下部のボタンを押す
// 4. 中央のボタンを押す
// [結果]
// 初回下部のボタンを押してもログが出るだけです。
// 上部のボタンを押すと下部が"create"と変化します。
// 下部ボタンを押すと中央に"delete"ボタンが追加されます。
// 中央のボタンを押すとボタンが消えます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample16 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample16Scene());
		}
	}

	class Sample16Scene : UIBase {

		private bool m_attached = false;

		public Sample16Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonTop": {
					if (m_attached) { return false; }

					Transform bottom = RootTransform.Find("Layer/ButtonBottom");
					UIController.Instance.AttachParts(this, new List<UIPart>(){new Sample16Factory(this, bottom)});
					m_attached = true;
					return true;
				}
				case "ButtonBottom": {
					Debug.Log("before attach bottom button");
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}

	class Sample16Factory : UIPart {

		private Sample16Scene m_ui = null;

		public Sample16Factory(Sample16Scene ui, Transform transform) : base(transform) {
			m_ui = ui;
		}

		public override IEnumerator OnLoadedPart(UIBase targetLayer) {
			Text text = RootTransform.Find("Text").GetComponent<Text>();
			text.text = "create";
			yield break;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			GameObject button = GameObject.Instantiate(this.RootTransform.gameObject);
			button.transform.SetParent(RootTransform.parent);
			button.transform.localPosition = new Vector3(426, 568, 0);
			button.transform.localScale = Vector3.one;

			UIController.Instance.AttachParts(m_ui, new List<UIPart>(){new Sample16Button(m_ui, button.transform)});

			return true;
		}

		protected override void OnDestroy() {
			m_ui = null;
		}
	}

	class Sample16Button : UIPart {

		private Sample16Scene m_ui = null;

		public Sample16Button(Sample16Scene ui, Transform transform) : base(transform) {
			m_ui = ui;
		}

		public override IEnumerator OnLoadedPart(UIBase targetLayer) {
			Text text = RootTransform.Find("Text").GetComponent<Text>();
			text.text = "delete";
			yield break;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			UIController.Instance.DetachParts(m_ui, new List<UIPart>(){this});
			return true;
		}

		protected override void OnDestroy() {
			Debug.Log("Scene16 : All Right");
			m_ui = null;
		}
	}
}
