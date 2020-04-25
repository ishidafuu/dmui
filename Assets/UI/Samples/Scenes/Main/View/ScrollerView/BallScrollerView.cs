using System;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class BallScrollerView : MonoBehaviour
    {
        [SerializeField] public EnhancedScroller m_EnhancedScroller;
        // [SerializeField] private IDragHandler[] m_ParentDragHandlers;
        // [SerializeField] private IBeginDragHandler[] m_ParentBeginDragHandlers;
        // [SerializeField] private IEndDragHandler[] m_ParentEndDragHandlers;

        private bool isSelf = false;
        void Start () 
        {
            // m_EnhancedScroller = GetComponentInParent<EnhancedScroller>();
            // m_ParentDragHandlers = GetComponentsInParent<IDragHandler>().ToArray();
            // m_ParentBeginDragHandlers = GetComponentsInParent<IBeginDragHandler>().ToArray();
            // m_ParentEndDragHandlers = GetComponentsInParent<IEndDragHandler>().ToArray();
        }
    }
}