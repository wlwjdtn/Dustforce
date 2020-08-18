using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller2D : Raycast_Controller {
    // 최대 경사각
    private float _MaxClimbAngle = 80.0f;
    // 충돌 정보
    public ColliderInfo _ColliderInfo;

    public override void Start() {
        base.Start();
    }

    public void Move(Vector3 velocity) {
        // 레이 시작점
        UpdateRayCastOrigins();
        // 충돌범위 설정
        _ColliderInfo.Reset();
        if (velocity.x != 0) {
            // 수평레이 설정
            HorizontalCollisions(ref velocity);
        } 
        if (velocity.y != 0) {
            // 수직레이 설정
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
    }

    // 수평 레이캐스트
    private void HorizontalCollisions(ref Vector3 veloctiy) {
        // X 의 방향
        float directionX = Mathf.Sign(veloctiy.x);
        float rayLength = Mathf.Abs(veloctiy.x) + _SkinWidth;
        // 레이 캐스트 사용
        for (int i = 0; i < _HoriRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? _RayOrigins._BottomLeft : _RayOrigins._BottomRight;
            rayOrigin += Vector2.up * (_HoriRaySpacing * i) ;
            RaycastHit2D _Hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _CollisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength * 3.0f, Color.red);

            if (_Hit) {
                // - 법선 설정
                //              충돌 정보를 받아오는 객체에서 닿고있는 부분의 선을 긋고 수직선을 긋음.
                //              경사면에서 서있는 지면을 기준으로부터 수직이 법선.

                float slopeAngle = Vector2.Angle(_Hit.normal, Vector2.up);

                // 첫번째 레이, 최대 각보다 적을경우
                if (i == 0 && slopeAngle <= _MaxClimbAngle) {
                    // 첫 레이가 경사 시작면까지의 거리.
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != _ColliderInfo.slopeAngleOld) {
                        distanceToSlopeStart = _Hit.distance - _SkinWidth;
                        veloctiy.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref veloctiy, slopeAngle);
                    veloctiy.x += distanceToSlopeStart * directionX;
                }

                if (!_ColliderInfo.climbingSlope || slopeAngle > _MaxClimbAngle) {
                    veloctiy.x = (_Hit.distance - _SkinWidth) * directionX;
                    rayLength = _Hit.distance;
                    if (_ColliderInfo.climbingSlope) {
                        veloctiy.y = Mathf.Tan(_ColliderInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(veloctiy.x);
                    }
                }
                _ColliderInfo._Left = directionX == -1;
                _ColliderInfo._Right = directionX == 1;
            }
        }
    }

    // 수직 레이캐스트
    private void VerticalCollisions(ref Vector3 veloctiy) {

        // Y 의 방향
        float directionY = Mathf.Sign(veloctiy.y);
        float rayLength = Mathf.Abs(veloctiy.y) + _SkinWidth;

        // 레이 캐스트 사용
        for (int i = 0; i < _VerRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? _RayOrigins._BottomLeft : _RayOrigins._TopLeft;
            rayOrigin += Vector2.right * (_VerRaySpacing * i + veloctiy.x);
            RaycastHit2D _Hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _CollisionMask);
            Debug.DrawRay(_RayOrigins._BottomLeft + Vector2.right * _VerRaySpacing * i, Vector2.up * -0.1f, Color.red);

            if(_Hit) {
                veloctiy.y = (_Hit.distance - _SkinWidth) * directionY;
                rayLength = _Hit.distance;

                if (_ColliderInfo.climbingSlope) {
                    veloctiy.x = veloctiy.y / Mathf.Tan(_ColliderInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(veloctiy.x);
                }
                _ColliderInfo._Above = directionY == 1;
                _ColliderInfo._Below = directionY == -1;
            }
        }
    }

    // Call-by-value : 값을 인자로 전달하는 함수의 호출방식
    // Call-by-reference : 주소 값을 인자로 전달하는 함수의 호출방식

    // 상승경사면 ( ref 경사속도, 경사각 )
    private void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY) {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            _ColliderInfo._Below = true;
            _ColliderInfo.climbingSlope = true;
            _ColliderInfo.slopeAngle = slopeAngle;
        }
    }


    // 충돌 정보
    public struct ColliderInfo {
        // 충돌정보
        public bool _Above, _Below;
        public bool _Left, _Right;
        // 경사 충돌정보
        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;
        // 충돌정보 초기화
        public void Reset() {
            _Above = _Below = false;
            _Left = _Right = false;
            climbingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}