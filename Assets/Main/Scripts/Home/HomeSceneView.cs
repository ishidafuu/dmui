using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DM
{
    public class HomeSceneView : MonoBehaviour
    {
        [FormerlySerializedAs("m_HomeScrollerController")] [SerializeField] public HomeScrollerView m_HomeScrollerView;
    }
}