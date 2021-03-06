﻿// [概要]
// バックキー挙動はUIBackableに指定してあるUIGroupのうち
// (デフォルトではScene, Dialog)
// もっとも手前にあるUIレイヤーを削除します。
// 削除時はOnBack()の呼び出しがあるので、削除自体を回避し、
// 別の挙動に変更することができます。
// [操作]
// 画面左下のボタンを押す
// [結果]
// ボタンを押すと黄四角が削除されます。
// さらにボタンを押すと青画面が削除されます。
// さらにボタンを押すと"All Right"ログが表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample12 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample12Scene());
		}
	}

	class Sample12Scene : UIBase {

		public Sample12Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			UIController.Instance.AddFront(new Sample12SceneB());
			UIController.Instance.AddFront(new Sample12Frame());
			UIController.Instance.AddFront(new Sample12Dialog());
			UIController.Instance.AddFront(new Sample12Back());
		}

		public override bool OnBack() {
			Debug.Log("Scene12 : All Right");

			return false;
		}
	}

	class Sample12SceneB : UIBase {

		public Sample12SceneB() : base("UISceneB", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}
	}

	class Sample12Frame : UIBase {

		public Sample12Frame() : base("UIFrame", EnumUIGroup.Floater, EnumUIPreset.BackVisible) {
		}
	}

	class Sample12Dialog : UIBase {

		public Sample12Dialog() : base("UIDialog", EnumUIGroup.Dialog, EnumUIPreset.BackVisible) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
		}
	}

	class Sample12Back : UIBase {

		public Sample12Back() : base("UIBack", EnumUIGroup.System, EnumUIPreset.BackVisible) {
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			UIController.Instance.Back();
			return true;
		}
	}
}
