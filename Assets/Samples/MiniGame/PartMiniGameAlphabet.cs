using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DM;

public class PartMiniGameAlphabet : UIPart {

	private UIMiniGameMain m_main;
	private char m_alphabet;

	public PartMiniGameAlphabet(UIMiniGameMain main, char alphabet) : base("MiniGame/MiniGameAlphabet") {
		m_main = main;
		m_alphabet = alphabet;
	}

	public override IEnumerator OnLoaded(UIBase targetLayer) {
		Root.SetParent(targetLayer.Root.Find("Panel"));
		Root.localScale = Vector3.one;

		Transform alphabet = Root.Find("Button/Alphabet");
		Image img = alphabet.GetComponent<Image>();
		img.sprite = Resources.Load<Sprite>("MiniGame/Images/" + m_alphabet.ToString());

		Root.Find("Button").gameObject.SetActive(false);

		yield break;
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		if (m_main.Check(m_alphabet)) {
			Root.Find("Button").gameObject.SetActive(false);
		}
		return true;
	}

	public void SetPosition(Vector2 pos) {
		Root.localPosition = pos;
	}

	public void Open() {
		Root.Find("Button").gameObject.SetActive(true);
		Root.GetComponent<Animation>().Play();
	}
}
