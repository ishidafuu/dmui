using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DM;
using DMUIFramework.Samples;

public class MiniGame : MonoBehaviour {

	void Start () {
		UIController.SetImplement(new PrefabLoader(), null, new FadeCreator(), null, null);
		UIController.Instance.AddFront(new UIMiniGameTitle());
	}
}
