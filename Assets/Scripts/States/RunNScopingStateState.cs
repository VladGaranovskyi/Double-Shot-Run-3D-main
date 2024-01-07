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
        private RaycastHit _currentHit;
        public RunNScopingState(PlayerController charachter, StateMachine stateMachine) : base(charachter, stateMachine)
        {
        }

        private void Shoot() 
        {
            if (IsShoot)
            {
                IsShoot = false;
                Time.timeScale = 1f;
                character._shootControls.shooter.Shoot(_currentHit.point);
                _shootTime = Time.time;
            }
        }

        public override void Enter()
        {
            base.Enter();
            character.playerAnimator.SwitchToUpperLowerLayer();
            character.stateDrivenCameraAnimator.Play("Behind");
            Holder.instance.OnTouch += Shoot;
            character.playerSound.PlayRun();
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
                Time.timeScale = 0.25f;
                Ray ray = character.Cam.ScreenPointToRay(_currentPosition);
                if(Physics.Raycast(ray, out _currentHit, Mathf.Infinity, character._shootControls.TargetLayer))
                {
                    character._shootControls.bodyRotater.LookAtPoint(_currentHit.point);
                    character._shootControls.laserProjector.ShowLaserPistol(character._shootControls.TargetLayer);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Holder.instance.OnTouch -= Shoot;
            character.playerSound.StopRun();
            Shoot();
        }
    }
}
