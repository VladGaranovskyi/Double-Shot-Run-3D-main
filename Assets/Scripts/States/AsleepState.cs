using UnityEngine;

namespace States
{
    public class AsleepState : State
    {
        public AsleepState(PlayerController charachter, StateMachine stateMachine) : base(charachter, stateMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();
            character._animator.enabled = true;
        }
    }
}
