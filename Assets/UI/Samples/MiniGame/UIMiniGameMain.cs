﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DM;
using UniRx.Async;

public class UIMiniGameMain : UIBase {
	private const int AlphabetNum = 26;

	private List<PartMiniGameAlphabet> m_alphabets = new List<PartMiniGameAlphabet>();
	private int m_targetIndex = 0;
	private float m_erapsedTime = 0.0f;
	private Text m_timeText;

	public UIMiniGameMain() : base("MiniGame/MiniGameMain", EnumUIGroup.MainScene) {
	}

	public override async UniTask OnLoadedBase() {
		for (int i = AlphabetNum - 1; i >= 0; i--) {
			char a = GetAlphabetByIndex(i);
			m_alphabets.Add(new PartMiniGameAlphabet(this, a));
		}
		
		await UIController.Instance.YieldAttachParts(this, m_alphabets.ConvertAll<UIPart>(x => x));

		m_timeText = RootTransform.Find("Panel/Time").GetComponent<Text>();
		Initialize();
	}

	public override void OnDispatchedEvent(DispatchedEvent dispatchedEvent) {
		switch (dispatchedEvent.EventName) {
			case "start": {
				Start();
				break;
			}
			case "retry": {
				Initialize();
				break;
			}
		}
	}

	public override bool OnClick(TouchEvent touch, UISound uiSound) {
		switch (touch.Listener.name) {
			case "HowToPlay": {
				IsScheduleUpdate = false;
				UIController.Instance.AddFront(new UIMiniGameHowToPlay());
				return true;
			}
		}
		return false;
	}

	public override void OnReTouchable() {
		IsScheduleUpdate = true;
	}

	public override void OnUpdate() {
		SetTimeText(m_erapsedTime + Time.deltaTime);
	}

	public bool Check(char alphabet) {
		if (alphabet != GetAlphabetByIndex(m_targetIndex)) {
			return false;
		}

		m_targetIndex++;
		if (m_targetIndex == AlphabetNum) {
			IsScheduleUpdate = false;
			UIController.Instance.AddFront(new UIMiniGameResult(m_erapsedTime));
		}
		return true;
	}

	private char GetAlphabetByIndex(int index) {
		return (char)((int)'A' + (index));
	}

	private void Initialize() {
		m_targetIndex = 0;
		SetTimeText(0.0f);
		UIController.Instance.AddFront(new UIMiniGameStartEffect());
		for (int i = 0; i < m_alphabets.Count; i++) {
			m_alphabets[i].SetPosition(GetRandomPos());
		}
	}

	private void Start() {
		IsScheduleUpdate = true;
		for (int i = 0; i < m_alphabets.Count; i++) {
			m_alphabets[i].Open();
		}
	}

	private Vector2 GetRandomPos() {
		return new Vector2(Random.Range(-260, 260), Random.Range(-508, 408));
	}

	private void SetTimeText(float time) {
		m_erapsedTime = time;
		m_timeText.text = time.ToString("N2");
	}
}
