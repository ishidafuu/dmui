// [概要]
// AddFront()によるUIレイヤーの追加を行ない、prefab内のラベルを書き換えます。
// 通信処理などを想定したWaitForSeconds()により時間を空けています。
// [操作]
// 操作なし
// [結果]
// 緑の画面が表示され、中央に"Scene"と表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;

namespace DM {

	public class Sample01 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddUIBase(new Sample01Scene());
		}
	}

	class Sample01Scene : UIBase {

		public Sample01Scene() : base("UISceneA", UIGroup.Scene) {
		}

		public override IEnumerator OnLoaded() {
			yield return new WaitForSeconds(2);

			Text text = Root.Find("Layer/Text").GetComponent<Text>();
			text.text = "Scene";

			Root.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			Root.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			Root.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			Debug.Log("Scene01 : All Right");
		}
	}
}
