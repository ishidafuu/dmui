    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DM
{
    public class LaboSceneView : MonoBehaviour
    {
        [SerializeField] public LaboScrollerView m_LaboScrollerView;
        [SerializeField] public LaboScrollerController m_LaboScrollerController;
    }
}