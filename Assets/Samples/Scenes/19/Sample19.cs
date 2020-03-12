// [概要]
// UIレイヤーの存在チェックをいくつかの調べ方で確認できます。
// ・HasUI() : UIとして存在しているかチェックします
// ・GetFrontUINameInGroup() : 指定グループ内の最前面のUIの名前を取得します。
// ・GetUINumInGroup() : 指定グループ内にいくつUIが存在しているか取得します。
// [操作]
// 黄四角内のボタンを押す
// [結果]
// ログにそれぞれの調べた内容が表示されます。
//
// SceneA: True
// SceneB: True
// SceneC: False
// SceneFront: Sample19SceneB
// FloaterFront: Sample19Frame
// SystemFront:
// SceneNum: 2
// FloaterNum: 1
// SystemNum: 0

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample19 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), null, new FadeCreator());
			UIController.Instance.AddFront(new Sample19Scene());
		}
	}
	class Sample19Scene : UIBase {
		public Sample19Scene() : base("UISceneA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			UIController.Instance.AddFront(new Sample19SceneB());
			UIController.Instance.AddFront(new Sample19Frame());
			UIController.Instance.AddFront(new Sample19Dialog());

			yield break;
		}
	}

	class Sample19SceneB : UIBase {
		public Sample19SceneB() : base("UISceneB", UIGroup.Scene) {
		}

		public override IEnumerator OnLoadedBase() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}
	}

	class Sample19Frame : UIBase {
		public Sample19Frame() : base("UIFrame", UIGroup.Floater, UIPreset.BackVisible) {
		}
	}

	class Sample19Dialog : UIBase {
		public Sample19Dialog() : base("UIDialog", UIGroup.Dialog, UIPreset.BackVisible) {
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					Debug.Log("SceneA: " + UIController.Instance.HasUIBase("Sample19Scene"));
					Debug.Log("SceneB: " + UIController.Instance.HasUIBase("Sample19SceneB"));
					Debug.Log("SceneC: " + UIController.Instance.HasUIBase("Sample19SceneC"));
					Debug.Log("SceneFront: "   + UIController.Instance.GetFrontUINameInGroup(UIGroup.Scene));
					Debug.Log("FloaterFront: " + UIController.Instance.GetFrontUINameInGroup(UIGroup.Floater));
					Debug.Log("SystemFront: "  + UIController.Instance.GetFrontUINameInGroup(UIGroup.System));
					Debug.Log("SceneNum: "   + UIController.Instance.GetLayerCountInGroup(UIGroup.Scene));
					Debug.Log("FloaterNum: " + UIController.Instance.GetLayerCountInGroup(UIGroup.Floater));
					Debug.Log("SystemNum: "  + UIController.Instance.GetLayerCountInGroup(UIGroup.System));
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}
}
