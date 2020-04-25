using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DM
{
    public class HomeSceneView : MonoBehaviour
    {
        [SerializeField] public HomeScrollerView m_HomeScrollerView;
        [SerializeField] public HomeScrollerController m_HomeScrollerController;
    }
}