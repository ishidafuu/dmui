// ----------------------------------------------------------------------
// DMUIFramework
// Copyright (c) 2018 Takuya Nishimura (tnishimu)
//
// This software is released under the MIT License.
// https://opensource.org/licenses/mit-license.php
// ----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UITouchListener : MonoBehaviour, 
        IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public UIBaseLayer Layer { get; private set; }
        public UIPart Part { get; private set; }
        
        private int m_Generation = int.MaxValue;

        public void SetLayerAndPart(UIBaseLayer layer, UIPart part)
        {
            int generation = GetGeneration(transform, part.RootTransform);
            if (m_Generation < generation)
            {
                return;
            }

            Layer = layer;
            Part = part;
            m_Generation = generation;
        }

        public void ClearLayerAndPart()
        {
            Layer = null;
            Part = null;
            m_Generation = int.MaxValue;
        }

        public void OnPointerClick(PointerEventData pointer)
        {
            UIController.Instance.ListenTouch(this, TouchType.Click, pointer);
        }

        public void OnPointerDown(PointerEventData pointer)
        {
            UIController.Instance.ListenTouch(this, TouchType.Down, pointer);
        }

        public void OnPointerUp(PointerEventData pointer)
        {
            UIController.Instance.ListenTouch(this, TouchType.Up, pointer);
        }

        public void OnDrag(PointerEventData pointer)
        {
            UIController.Instance.ListenTouch(this, TouchType.Drag, pointer);
        }

        private static int GetGeneration(Transform target, Object dest, int generation = 0)
        {
            while (true)
            {
                if (target == null || dest == null)
                {
                    return -1;
                }

                if (target == dest) return generation;

                target = target.parent;
                generation += 1;
            }
        }
    }
}