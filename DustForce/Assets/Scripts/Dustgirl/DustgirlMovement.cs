using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class DustgirlMovement: MonoBehaviour {

    // GameManager 접근
    private GameManager _Instance;

    // DustGirl 이동속도
    private int _Speed = 12;
    // DustGirl 벽을 걷는 이동속도
    private int _WallSpeed = 10;

    // 점프 높이, 최대 높이까지 걸리는 시간. (정적 점프 높이)
    public float _JumpHeight = 4f;
    public float _ToTimeJumpApex = .4f;

    // 벽에서 점프하여 오를 수 있는 변수
    public Vector2 wallJumpClimb;
    // 점프가능 여부
    public Vector2 wallJumpOff;
    // 벽 점프
    public Vector2 wallLeap;

    // 벽에 붙어서 내려오는 속도 값
    public float wallSlidingSpeedMax = 3f;
    public float wallStickTime = .25f;
    float timeToWallUnStick;

    // 벽에 붙었을 때 검사 값
    public bool wallSliding;
    // 벽을 오르기 위한 검사 값
    public bool inputWallRunKey;
    public bool wallIdle;

    // 중력
    private float _Gravity;
    // DustGirl JumpPower
    private float _JumpPower;

    // 좌, 우 움직임 부드럽게 설정
    private float _VeloctiyXSmoothing;

    // 가속시간공기, 가속 시간
    private float _AccelerationTimeAirborne = 0.2f;
    private float _AccelerationTimeGrounded = 0.1f;

    // 점프카운트
    [SerializeField] private int _JumpCount;

    // 플레이어 속도 저장
    private Vector2 _Velocity;
    // Controller2D
    private Controller2D _Controll2D;
    // BaseAnim
    private BaseAnimator _CharacterAnim;

    private void Start() {
        _Controll2D = GetComponent<Controller2D>();

        _CharacterAnim = GameObject.Find("Managers").GetComponent<BaseAnimator>();

        _Gravity = -(2 * _JumpHeight) / Mathf.Pow(_ToTimeJumpApex, 2);
        _JumpPower = Mathf.Abs(_Gravity) * _ToTimeJumpApex;
    }

    private void Update() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // 벽을 인지하였을 때 왼쪽 벽인지 오른쪽 벽인지 구분하는 필드
        int wallDirX = (_Controll2D._ColliderInfo._Left) ? -1 : 1;
        // 좌,우 이동
        float _TargetVelocityX = input.x * _Speed;

        // 좌, 우 이동을 부드럽게 설정 (현재 속도에서 목표속도로 가기까지 땅에 있으면 0.1초의 속도로 이동, 공중에 있으면 0.2초의 속도로 이동)
        _Velocity.x = Mathf.SmoothDamp(_Velocity.x, _TargetVelocityX, ref _VeloctiyXSmoothing, (_Controll2D._ColliderInfo._Below) ? _AccelerationTimeGrounded : _AccelerationTimeAirborne);

        // 벽 충돌정보와 관련 된 기본 검사값
        wallSliding = false;
        wallIdle = false;
        inputWallRunKey = false;

        // 양쪽 벽 한곳에 닿았으며, 땅에 닿지 않고 있고 캐릭터의 속력을 아래로 받고 있으면?
        if ((_Controll2D._ColliderInfo._Left || _Controll2D._ColliderInfo._Right) && !_Controll2D._ColliderInfo._Below) {
            wallSliding = true;

            // 벽에 붙었을 때 내려오는 속도가 벽에 붙었을 때의 속도보다 적으면 벽에 붙어있는 속도로 전환.
            if (_Velocity.y < -wallSlidingSpeedMax) {
                _Velocity.y = -wallSlidingSpeedMax;
                wallIdle = true;
            }

            if (timeToWallUnStick > 0) {
                _VeloctiyXSmoothing = 0;
                _Velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                    timeToWallUnStick -= Time.deltaTime;
                else
                    timeToWallUnStick = wallStickTime;
            }
            else
                timeToWallUnStick = wallStickTime;
        }

        // 충돌정보에 관섭 되었을 때 중력값을 계속 받지 않고 좀 더 안정적으로 힘을 주기위해 값을 초기화 
        if ((_Controll2D._ColliderInfo._Above || _Controll2D._ColliderInfo._Below)) {
            _Velocity.y = 0;
            _JumpCount = 1;
        }

        if (_Velocity.y < 0) {
            // 벽에 붙었을 때 점프
            if (Input.GetKeyDown(KeyCode.Z)) {
                if (wallSliding) {
                    if (wallDirX == input.x) {
                        _Velocity.x = -wallDirX * wallJumpClimb.x;
                        _Velocity.y = wallJumpClimb.y;
                    }
                    else if (input.x == 0) {
                        _Velocity.x = -wallDirX * wallJumpOff.x;
                        _Velocity.y = wallJumpOff.y;
                    }
                    else {
                        _Velocity.x = -wallDirX * wallLeap.x;
                        _Velocity.y = wallLeap.y;
                    }
                }
                if (_Controll2D._ColliderInfo._Below) {
                    _Velocity.y = _JumpPower;
                    _JumpCount--;
                }
            }
        }

        // 벽에 붙었을 때 점프
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (_Controll2D._ColliderInfo._Below) {
                _Velocity.y = _JumpPower;
                _JumpCount--;
            }
        }
        
        // 벽에 붙어있을 경우?
        if (wallSliding) {
            // 위쪽 방향키를 누르고 있으면?
            if (input.y > 0) {
                inputWallRunKey = true;

                _Velocity.y = _WallSpeed;
            }
        }
        _Velocity.y += _Gravity * Time.deltaTime;       // 중력
        _CharacterAnim._DustAnimator.SetBool("IsWallRun", inputWallRunKey);
        _CharacterAnim._DustAnimator.SetBool("IsWallIdle", wallIdle);
        _CharacterAnim._DustAnimator.SetBool("IsWall", wallSliding);

        // 공격중일 때 이동 막기
        _Controll2D.Move(_Velocity * Time.deltaTime, input);  // 움직임에 관련된 판정여부
        _CharacterAnim.DustGirlMoveAnim(_Velocity, _Controll2D);     // 움직임 애니메이션 판정여부
    }
}