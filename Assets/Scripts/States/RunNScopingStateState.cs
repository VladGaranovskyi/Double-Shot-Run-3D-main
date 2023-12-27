using UnityEngine;

namespace States
{
    public class RunNScopingState : State
    {
        private Vector2 _currentPosition;
        private bool IsHold;
        private float _shootTime;
        private bool IsShoot;
        private const float MASS = 5f;
        public RunNScopingState(PlayerController charachter, StateMachine stateMachine) : base(charachter, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.playerAnimator.SwitchToUpperLowerLayer();
            character.stateDrivenCameraAnimator.Play("Behind");
        }

        public override void HandleInput()
        {
            base.HandleInput();
            _currentPosition = Holder.instance.CurrentPosition;
            IsHold = Holder.instance.Hold;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsHold && Time.time - _shootTime > character.playerAnimator.RShootPeriod) IsShoot = true;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Vector3 dirMove = character.SpeedRNS * Vector3.forward;
            if (!character.characterController.isGrounded)
            {
                dirMove += Physics.gravity * MASS;
            }
            character.characterController.Move(dirMove * Time.deltaTime);
            if (IsShoot)
            {
                RaycastHit hit;
                Ray ray = character.Cam.ScreenPointToRay(_currentPosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, character._shootControls.TargetLayer))
                {
                    character._shootControls.shooter.Shoot(hit.point);
                    IsShoot = false;
                    _shootTime = Time.time;
                }
            }
        }
    }
}
