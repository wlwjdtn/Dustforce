using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing: MonoBehaviour {


    public Controller2D target;   // 플레이어가 가지고 있는 콜라이더 정보에 접근
    public float verticalOffset;
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize; // 카메라 인식 영역

    private FocusArea focusArea;
    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    private void Start() {
        focusArea = new FocusArea(target._Collider.bounds, focusAreaSize);
    }

    private void LateUpdate() {
        

        focusArea.Update(target._Collider.bounds);
        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0) {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else {
                if (!lookAheadStopped) {
                  lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                }
            }
        }
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition.y   = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;

        Debug.Log(focusPosition);
        // Cemera Escape 방지
        if(transform.position.x > 18) {
            // Vector2 CameraProtect = new Vector2(transform.position.x, )
        
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(0.9f, 0.2f, 0.2f, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    // 카메라 영역설정을 위한 각각의 꼭지점(벡터 값) 설정
    struct FocusArea {
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public  FocusArea(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds) {

            float shiftX = 0;
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }
            else if(targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}