﻿// [概要]
// UIPreset.TouchEventCallable使用した最前面に透明の画面全体のタッチ領域をAddFront()します。
// 画面を押すとタッチの反応としてログが流れます。ボタンを押すと緑の画面は削除されます。
// また、背後のボタンが反応するので、UIPreset.BackTouchableの利用例となります。
// [操作]
// 適宜画面をタッチ操作する
// [結果]
// 操作挙動に応じたログが表示されます。
// ボタンを押すと画面に何も表示されなくなります。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample05 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample05Scene());
		}
	}

	class Sample05Scene : UIBase {

		public Sample05Scene() : base("UISceneA", UIGroup.Scene) {
			UIController.Instance.AddFront(new UISample05TouchLayer1());
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

		protected override void OnDestroy() {
			Debug.Log("Scene05 : All Right");
		}
	}

	class UISample05TouchLayer1 : UIBase {

		public UISample05TouchLayer1()
		: base("", UIGroup.System, UIPreset.BackVisible | UIPreset.SystemUntouchable | UIPreset.TouchEventCallable) {

		}

		public override bool OnTouchDown(string name, GameObject gameObject, PointerEventData pointer) {
			Debug.Log("touch down " + name + ": " + pointer.position);
			return false;
		}

		public override bool OnTouchUp(string name, GameObject gameObject, PointerEventData pointer) {
			Debug.Log("touch up " + name + ": " + pointer.position);
			return false;
		}

		public override bool OnDrag(string name, GameObject gameObject, PointerEventData pointer) {
			Debug.Log("touch drag " + name + ": " + pointer.position);
			return false;
		}
	}
}
