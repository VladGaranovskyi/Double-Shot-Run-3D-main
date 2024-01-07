using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using States;

public class StateFactory
{
    public static State GetState(State_Type t, PlayerController playerController)
    {
        switch (t)
        {
            case State_Type.Running:
                return playerController.runningState;
            case State_Type.Jumping:
                return playerController.jumpingState;
            case State_Type.VerticalScoping:
                return playerController.verticalScopingState;
            case State_Type.Shooting:
                return playerController.shootingState;
            case State_Type.RunNScopingState:
                return playerController.runNScopingState;
            case State_Type.FinishingAnimationState:
                return playerController.finishingAnimationState;
        }
        return playerController.runningState;
    }
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float defaultYCoordinate;
    [SerializeField] private float speedMove;
    [Header("Run N Scope Speed")]
    [SerializeField] private float speedRNS;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Text _coinsText;
    private int _coins = 0;
    public float defaultForceFinish;
    public ShootControls _shootControls;
    public float[] jumpPeriods;
    public Animator stateDrivenCameraAnimator;
    public CinemachineVirtualCamera wallCam;

    public StateMachine stateMachine;

    public AsleepState asleepState;
    public JumpingState jumpingState;
    public RunningState runningState;
    public VerticalScopingState verticalScopingState;
    public ShootingState shootingState;
    public RunNScopingState runNScopingState;
    public FinishingAnimationState finishingAnimationState;

    public Animator _animator { get; private set; }
    public CharacterController characterController { get; private set; }
    public float _defaultCharachterHeight { get; private set; }
    public LineRenderer lineRenderer { get; private set; }
    public PlayerAnimator playerAnimator { get; private set; }
    public Camera Cam { get; private set; }
    public MultiplyingTilesSpawner _tilesSpawner { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerSound playerSound { get; private set; }

    public float GetTime() => Time.time;
    public float GetJumpHeight() => _jumpHeight;
    public float GetDefaultY() => defaultYCoordinate;
    public float Speed { get => speedMove; set => speedMove = value; }
    public float SpeedRNS { get => speedRNS; }
    public int Coins { get => _coins; }

    public void AddCoins(int coins)
    {
        _coins += coins;
        _coinsText.text = _coins.ToString();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        playerAnimator = GetComponent<PlayerAnimator>();
        _tilesSpawner = FindObjectOfType<MultiplyingTilesSpawner>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSound = GetComponent<PlayerSound>();
        Cam = Camera.main;
        _defaultCharachterHeight = characterController.height;

        stateMachine = new StateMachine();

        asleepState = new AsleepState(this, stateMachine);
        runningState = new RunningState(this, stateMachine);
        jumpingState = new JumpingState(this, stateMachine);
        shootingState = new ShootingState(this, stateMachine);
        verticalScopingState = new VerticalScopingState(this, stateMachine);
        runNScopingState = new RunNScopingState(this, stateMachine);
        finishingAnimationState = new FinishingAnimationState(this, stateMachine);

        stateMachine.Initialize(asleepState);
    }

    private void Update()
    {
        stateMachine.CurrentState.HandleInput();

        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
