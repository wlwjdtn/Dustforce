using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_Controller: Raycast_Controller {

    public LayerMask passengerMask;
    public Vector3 move;

    public override void Start() {
        base.Start();
    }

    void Update() {
        UpdateRayCastOrigins();

        Vector3 velocity = move * Time.deltaTime;

        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    void MovePassengers(Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0) {
            float rayLength = Mathf.Abs(velocity.y) + _SkinWidth;

            for (int i = 0; i < _VerRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? _RayOrigins._BottomLeft : _RayOrigins._TopLeft;
                rayOrigin += Vector2.right * (_VerRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - _SkinWidth) * directionY;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
    }

    //public Vector3 move;
    //public LayerMask passengerMask;

    //public override void Start() {
    //    base.Start();
    //}

    //private void Update() {
    //    UpdateRayCastOrigins();

    //    Vector3 velocity = move * Time.deltaTime;
    //    MovePassengers(velocity);
    //    transform.Translate(velocity);
    //}

    //private void MovePassengers(Vector3 velocity) {
    //    HashSet<Transform> movedPassengers = new HashSet<Transform>();


    //    float directionX = Mathf.Sign(velocity.x);
    //    float directionY = Mathf.Sign(velocity.y);

    //    if (velocity.y != 0) {
    //        float rayLength = Mathf.Abs(velocity.y) + _SkinWidth;

    //        // 레이 캐스트 사용
    //        for (int i = 0; i < _VerRayCount; i++) {
    //            Vector2 rayOrigin = (directionY == -1) ? _RayOrigins._BottomLeft : _RayOrigins._TopLeft;
    //            rayOrigin += Vector2.right * (_VerRaySpacing * i);
    //            RaycastHit2D _Hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

    //            if (_Hit) {
    //                if (!movedPassengers.Contains(_Hit.transform)) {
    //                    movedPassengers.Add(_Hit.transform);
    //                    float pushX = (directionY == 1) ? velocity.x : 0;
    //                    float pushY = velocity.y - (_Hit.distance - _SkinWidth) * directionY;

    //                    _Hit.transform.Translate(new Vector3(pushX, pushY));
    //                }
    //            }
    //        }
    //    }
    //}
}