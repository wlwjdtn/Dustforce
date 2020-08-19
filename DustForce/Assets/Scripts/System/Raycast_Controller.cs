using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Raycast_Controller: MonoBehaviour {
    // 상호작용을 이루어 줄 레이캐스트 수
    [SerializeField] public int _HoriRayCount = 4;
    [SerializeField] public int _VerRayCount = 4;

    // 레이캐스트 간격
    [HideInInspector] public float _HoriRaySpacing;
    [HideInInspector] public float _VerRaySpacing;

    // 박스 콜라이더 겉두께
    public const float _SkinWidth = 0.015f;

    // 박스 콜라이더 접근
    [HideInInspector] public BoxCollider2D _Collider;
    // 콜리전 레이어
    public LayerMask _CollisionMask;
    // 레이캐스트 시작점
    public RayCastOrigins _RayOrigins;

    public virtual void Start() {
        _Collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    // 레이 시작점 동기화
    public void UpdateRayCastOrigins() {
        Bounds bounds = _Collider.bounds;
        bounds.Expand(_SkinWidth * -2);
        _RayOrigins._TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _RayOrigins._TopRight = new Vector2(bounds.max.x, bounds.max.y);
        _RayOrigins._BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _RayOrigins._BottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    // 레이 간격 계산
    public void CalculateRaySpacing() {
        Bounds bounds = _Collider.bounds;
        bounds.Expand(_SkinWidth * -2);
        _HoriRayCount = Mathf.Clamp(_HoriRayCount, 2, int.MaxValue);
        _VerRayCount = Mathf.Clamp(_VerRayCount, 2, int.MaxValue);
        _HoriRaySpacing = bounds.size.y / (_HoriRayCount - 1);
        _VerRaySpacing = bounds.size.x / (_VerRayCount - 1);
    }

    // 박스 콜라이더 각 꼭지점 좌표를 저장할 변수 선언
    public struct RayCastOrigins {
        public Vector2 _TopLeft, _TopRight;
        public Vector2 _BottomLeft, _BottomRight;
    }
}