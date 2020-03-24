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
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample19 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), null, new FadeCreator(), new LoadingCreator(), null);
			UIController.Instance.AddFront(new Sample19Scene());
		}
	}
	class Sample19Scene : UIBase {
		public Sample19Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			UIController.Instance.AddFront(new Sample19SceneB());
			UIController.Instance.AddFront(new Sample19Frame());
			UIController.Instance.AddFront(new Sample19Dialog());
		}
	}

	class Sample19SceneB : UIBase {
		public Sample19SceneB() : base("UISceneB", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}
	}

	class Sample19Frame : UIBase {
		public Sample19Frame() : base("UIFrame", EnumUIGroup.Floater, EnumUIPreset.BackVisible) {
		}
	}

	class Sample19Dialog : UIBase {
		public Sample19Dialog() : base("UIDialog", EnumUIGroup.Dialog, EnumUIPreset.BackVisible) {
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					Debug.Log("SceneA: " + UIController.Instance.HasUIBase("Sample19Scene"));
					Debug.Log("SceneB: " + UIController.Instance.HasUIBase("Sample19SceneB"));
					Debug.Log("SceneC: " + UIController.Instance.HasUIBase("Sample19SceneC"));
					Debug.Log("SceneFront: "   + UIController.Instance.GetFrontUINameInGroup(EnumUIGroup.Scene));
					Debug.Log("FloaterFront: " + UIController.Instance.GetFrontUINameInGroup(EnumUIGroup.Floater));
					Debug.Log("SystemFront: "  + UIController.Instance.GetFrontUINameInGroup(EnumUIGroup.System));
					Debug.Log("SceneNum: "   + UIController.Instance.GetLayerCountInGroup(EnumUIGroup.Scene));
					Debug.Log("FloaterNum: " + UIController.Instance.GetLayerCountInGroup(EnumUIGroup.Floater));
					Debug.Log("SystemNum: "  + UIController.Instance.GetLayerCountInGroup(EnumUIGroup.System));
					return true;
				}
				default: {
					return false;
				}
			}
		}
	}
}
