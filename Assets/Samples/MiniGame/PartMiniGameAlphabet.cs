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

	public override IEnumerator OnLoadedPart(UIBase targetLayer) {
		RootTransform.SetParent(targetLayer.RootTransform.Find("Panel"));
		RootTransform.localScale = Vector3.one;

		Transform alphabet = RootTransform.Find("Button/Alphabet");
		Image img = alphabet.GetComponent<Image>();
		img.sprite = Resources.Load<Sprite>("MiniGame/Images/" + m_alphabet.ToString());

		RootTransform.Find("Button").gameObject.SetActive(false);

		yield break;
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		if (m_main.Check(m_alphabet)) {
			RootTransform.Find("Button").gameObject.SetActive(false);
		}
		return true;
	}

	public void SetPosition(Vector2 pos) {
		RootTransform.localPosition = pos;
	}

	public void Open() {
		RootTransform.Find("Button").gameObject.SetActive(true);
		RootTransform.GetComponent<Animation>().Play();
	}
}
