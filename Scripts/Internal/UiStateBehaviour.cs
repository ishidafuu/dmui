using System;
using UnityEngine;

namespace DM
{
    public class UiStateBehaviour : StateMachineBehaviour
    {
        public const string LayerName = "UI.";

        private Action<Animator> m_ExitCallback;

        public Action<Animator> ExitCallback
        {
            set => m_ExitCallback = value;
        }

        private string m_PlayName;

        public string PlayName
        {
            set => m_PlayName = value;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AnimatorClipInfo[] infos = animator.GetCurrentAnimatorClipInfo(layerIndex);
            if (!stateInfo.IsName(m_PlayName) || infos.Length != 0 || m_ExitCallback == null)
            {
                return;
            }

            m_ExitCallback(animator);
            m_ExitCallback = null;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateInfo.IsName(m_PlayName) || m_ExitCallback == null)
            {
                return;
            }

            m_ExitCallback(animator);
            m_ExitCallback = null;
        }
    }
}