using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera: MonoBehaviour {

    private Camera playerCamera;

    private GameObject target;

    private Vector3 currentCameraPos;

    private const int leftRayoutX = -18;
    private const int rightRayoutX = 18;

    private void Start() {
        playerCamera = GetComponent<Camera>();

        target = GameObject.FindGameObjectWithTag("Player");

        playerCamera.fieldOfView = 80.0f;
    }

    private void Update() {
        currentCameraPos = transform.position;
    }
    private void LateUpdate() {

        // 카메라가 스크린 레이아웃을 벗어 나는것을 방지
        if (leftRayoutX > target.transform.position.x || rightRayoutX < target.transform.position.x) {
            transform.position = currentCameraPos;
        }
        else {
            // 카메라의 이동
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -12);
        }
    }
}