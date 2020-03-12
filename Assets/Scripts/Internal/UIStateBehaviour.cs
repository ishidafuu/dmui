using System;
using UnityEngine;

namespace DM
{
    public class UIStateBehaviour : StateMachineBehaviour
    {
        public const string LAYER_NAME = "UI.";
        private Action<Animator> m_ExitCallback;
        private string m_PlayName;
        
        public void SetExitCallback(Action<Animator> value) => m_ExitCallback = value;
        public void SetPlayName(string value) => m_PlayName = value;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AnimatorClipInfo[] infos = animator.GetCurrentAnimatorClipInfo(layerIndex);
            if (!stateInfo.IsName(m_PlayName) || infos.Length != 0 || m_ExitCallback == null)
            {
                return;
            }

            m_ExitCallback.Invoke(animator);
            m_ExitCallback = null;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateInfo.IsName(m_PlayName) || m_ExitCallback == null)
            {
                return;
            }

            m_ExitCallback.Invoke(animator);
            m_ExitCallback = null;
        }
    }
}