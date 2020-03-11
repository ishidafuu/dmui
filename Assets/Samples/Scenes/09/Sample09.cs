﻿// [概要]
// UIレイヤーは再度表示状態になるとOnRevisible()が呼び出されます。
// UI生成時は呼び出されません。
// [操作]
// 1. 緑画面のボタンを押す
// 2. 青画面のボタンを押す
// [結果]
// 緑画面でボタンを押すと青画面になります。
// その後青画面でボタンを押すと緑画面になりますが、
// この際"All Right"ログが表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample09 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample09Scene());
		}
	}

	class Sample09Scene : UIBase {

		public Sample09Scene() : base("UISceneA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoaded() {
			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			yield break;
		}

		public override bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound) {
			switch (name) {
				case "ButtonCenter": {
					UIController.Instance.AddFront(new Sample09SceneB());
					return true;
				}
		 		default: {
					return false;
				}
			}
		}

		public override void OnReVisible() {
			Debug.Log("Scene09 : All Right");
		}
	}


	class Sample09SceneB : UIBase {

		public Sample09SceneB() : base("UISceneAnimB", UIGroup.Scene) {
		}

		public override IEnumerator OnLoaded() {
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
	}
}
