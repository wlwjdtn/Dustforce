using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class DustgirlMovement: MonoBehaviour {

    // GameManager 접근
    private GameManager _Instance;

    // DustGirl 이동속도
    private int _Speed = 12;

    // 점프 높이, 최대 높이까지 걸리는 시간. (정적 점프 높이)
    public float _JumpHeight = 4f;
    public float _ToTimeJumpApex = .4f;

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

        // 충돌정보에 관섭 되었을 때 중력값을 계속 받지 않고 좀 더 안정적으로 힘을 주기위해 값을 초기화 
        if (_Controll2D._ColliderInfo._Above || _Controll2D._ColliderInfo._Below) {
            _Velocity.y = 0;
            _JumpCount = 1;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 점프
        if (Input.GetKey(KeyCode.Z) && _Controll2D._ColliderInfo._Below && _JumpCount > 0) {
            _Velocity.y = _JumpPower;
            _JumpCount--;
        }

        // 공격하는 중
        if (_CharacterAnim.Get_LaunchCoolTime < 0.683f && 0 < _CharacterAnim.Get_LaunchCoolTime) {
            _JumpCount = 0;

            // 점프 중이라면?
            if (!_Controll2D._ColliderInfo._Below && _JumpCount > 0) {
                _Velocity.y = _JumpPower;
            }
        }

        float _TargetVelocityX = input.x * _Speed;      // 좌,우 이동

        // 좌, 우 이동을 부드럽게 설정 (현재 속도에서 목표속도로 가기까지 땅에 있으면 0.1초의 속도로 이동, 공중에 있으면 0.2초의 속도로 이동)
        _Velocity.x = Mathf.SmoothDamp(_Velocity.x, _TargetVelocityX, ref _VeloctiyXSmoothing, (_Controll2D._ColliderInfo._Below) ? _AccelerationTimeGrounded : _AccelerationTimeAirborne);
        _Velocity.y += _Gravity * Time.deltaTime;       // 중력

        // 공격중일 때 이동 막기
        _Controll2D.Move(_Velocity * Time.deltaTime);  // 움직임에 관련된 판정여부
        _CharacterAnim.DustGirlMoveAnim(_Velocity, _Controll2D);     // 움직임 애니메이션 판정여부
    }
}