using System;
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

        public bool ShouldFadeByAdding(UIBase uiBase, UIBaseLayerList uiList)
        {
            if (m_Fade != null)
            {
                return false;
            }

            if (UIFadeTarget.s_Groups.Contains(uiBase.Group))
            {
                return true;
            }
            
            if (!UIFadeThreshold.s_Groups.ContainsKey(uiBase.Group))
            {
                return false;
            }
            
            return uiList.GetNumInGroup(uiBase.Group) <= UIFadeThreshold.s_Groups[uiBase.Group];
        }

        public bool ShouldFadeByRemoving(UIBase ui, UIBaseLayerList uiList, IEnumerable<UIBaseLayer> removingList)
        {
            if (m_Fade != null)
            {
                return false;
            }

            if (UIFadeTarget.s_Groups.Contains(ui.Group))
            {
                return true;
            }
            
            if (!UIFadeThreshold.s_Groups.ContainsKey(ui.Group))
            {
                return false;
            }

            int sceneNum = UIBaseLayerList.GetNumInGroup(ui.Group, removingList);

            return uiList.GetNumInGroup(ui.Group) - sceneNum <= UIFadeThreshold.s_Groups[ui.Group];
        }

        public void FadeIn(UIImplements implements, List<UIBaseLayer> addingList,Action<UIBase> addFront)
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
            m_Fade = addingList.Find(l => l.Base == fade);
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

        public bool IsFadeIn()
        {
            return (m_Fade != null && m_Fade.State <= BaseLayerState.InAnimation);
        }

        public bool IsHidden()
        {
            return (m_Fade != null && m_Fade.State == BaseLayerState.Active);
        }
    }
}