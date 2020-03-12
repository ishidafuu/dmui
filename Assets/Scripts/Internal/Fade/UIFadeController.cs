﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UIFadeController
    {
        private UIBaseLayer m_Fade;
        
        // このグループのレイヤが追加／削除されるとフェードが発生
        private static readonly List<EnumUIGroup> s_FadeTargetGroups = new List<EnumUIGroup>()
        {
            EnumUIGroup.Floater,
            EnumUIGroup.MainScene,
            EnumUIGroup.View3D,
        };
        
        // この数値以下のレイヤ数で追加削除が行われたときのみフェードが発生
        private static readonly Dictionary<EnumUIGroup, int> s_FadeThresholdGroups = new Dictionary<EnumUIGroup, int>()
        {
            {EnumUIGroup.Scene, 1},
        };

        public bool IsFadeIn()
        {
            return (m_Fade != null && m_Fade.State <= EnumLayerState.InAnimation);
        }

        public bool IsHidden()
        {
            return (m_Fade != null && m_Fade.State == EnumLayerState.Active);
        }
        
        public bool IsShouldFadeByAdding(UIBase uiBase, UIBaseLayerController uiController)
        {
            if (m_Fade != null)
            {
                return false;
            }

            if (s_FadeTargetGroups.Contains(uiBase.Group))
            {
                return true;
            }

            if (!s_FadeThresholdGroups.ContainsKey(uiBase.Group))
            {
                return false;
            }

            return uiController.GetCountInGroup(uiBase.Group) <= s_FadeThresholdGroups[uiBase.Group];
        }

        public bool IsShouldFadeByRemoving(UIBase uiBase, UIBaseLayerController uiController,
            IEnumerable<UIBaseLayer> removingList)
        {
            if (m_Fade != null)
            {
                return false;
            }

            if (s_FadeTargetGroups.Contains(uiBase.Group))
            {
                return true;
            }

            if (!s_FadeThresholdGroups.ContainsKey(uiBase.Group))
            {
                return false;
            }

            int sceneNum = UIBaseLayerController.GetCountInGroup(uiBase.Group, removingList);

            return uiController.GetCountInGroup(uiBase.Group) - sceneNum <= s_FadeThresholdGroups[uiBase.Group];
        }

        public void FadeIn(UIImplements implements, List<UIBaseLayer> addingList, Action<UIBase> addFront)
        {
            if (m_Fade != null)
            {
                return;
            }

            if (implements.FadeCreator == null)
            {
                return;
            }

            UIFade fade = implements.FadeCreator.Create();
            addFront.Invoke(fade);
            m_Fade = addingList.Find(layer => layer.Base == fade);
        }

        public void FadeOut(Action<UIBase> remove)
        {
            if (m_Fade == null)
            {
                return;
            }

            remove.Invoke(m_Fade.Base);
            m_Fade = null;
        }
    }
}