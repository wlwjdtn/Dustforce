using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_Controller: Raycast_Controller {

	// Layer 검사
	public LayerMask passengerMask;

	// 이동 플랫폼의 반복포인트 설정
	public Vector3[] localWaypoints;
	// 
	private Vector3[] globalWaypoints;

	// 플랫폼 이동속도
	public float speed;
	// 순환 결정여부
	public bool cyclic;
	// 대기시간
	public float waitTime;

	[Range(0,2)]
	public float easeAmount;

	private int fromWaypointIndex;
	private float percentBetweenWaypoins;
	private float nextMoveTime;

	// 이동플랫폼에 접근한 Player 의 transform List, Dictionary
	List<PassengerMovement> passengerMovement;
	Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

	public override void Start() {
		base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for(int i = 0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
	}

	private void Update() {

		// 레이캐스트 시작지점 업데이트
		UpdateRayCastOrigins();

		// 플랫폼 속도 값 설정
		Vector3 velocity = CalculatePlatformMovement();
		// 접근된 Layer 가 움직이는 플랫폼과 호환되기 위한 계산 메서드
		CalculatePassengerMovement(velocity);

		// 움직이는 플랫폼에 접근 했으면 플랫폼 벡터값(움직이는 플랫폼)을 가지고 이동 가능.
		MovePassengers(true);
		transform.Translate(velocity);

		// 움직이는 플랫포멩 접근 하지 안았으면 원래 벡터값으로 이동 가능.
		MovePassengers(false);
	}

	private float Ease(float x) {
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

	private Vector3 CalculatePlatformMovement() {

		if(Time.time < nextMoveTime) {
			return Vector3.zero;
        }

		fromWaypointIndex %= globalWaypoints.Length;

		// 두번째 인덱스를 설정
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length; 
		// Waypoints 간격(거리) : 첫번째 인덱스 transfrom 과 두번째 인덱스 transform
		float distanceBetweenWEaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoins += Time.deltaTime * speed/distanceBetweenWEaypoints;
		percentBetweenWaypoins = Mathf.Clamp01(percentBetweenWaypoins);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoins);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);
			
		if(percentBetweenWaypoins >= 1) {
			percentBetweenWaypoins = 0;
			fromWaypointIndex++;

			if(!cyclic) {
				if (fromWaypointIndex >= globalWaypoints.Length - 1) {
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
            }
			nextMoveTime = Time.time + waitTime;
        }
		return newPos - transform.position;
    }
	 
	// 플랫폼 Layer 접근 현황
	private void MovePassengers(bool beforeMovePlatform) {
		foreach (PassengerMovement passenger in passengerMovement) {
			if (!passengerDictionary.ContainsKey(passenger.transform)) {
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			}

			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	// 플랫폼이 움직일 수 있도록 계산하는 함수
	private void CalculatePassengerMovement(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovement = new List<PassengerMovement>();

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
					Debug.Log("누가 범인이냐?");
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - _SkinWidth) * directionY;

						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs(velocity.x) + _SkinWidth;

			for (int i = 0; i < _HoriRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1) ? _RayOrigins._BottomLeft : _RayOrigins._BottomRight;
				rayOrigin += Vector2.up * (_HoriRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				if (hit) {
					Debug.Log("누가 범인이냐2?");
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - _SkinWidth) * directionX;
						float pushY = -_SkinWidth;

						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = _SkinWidth * 2;

			for (int i = 0; i < _VerRayCount; i++) {
				Vector2 rayOrigin = _RayOrigins._TopLeft + Vector2.right * (_VerRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit) {
					Debug.Log("누가 범인이냐3?");
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;

						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
					}
				}
			}
		}
	}

	// 플랫폼에 필요한 데이터
	public struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}

    private void OnDrawGizmos() {
        if(localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i++) {
				Vector3 globalWaypointsPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointsPos - Vector3.up * size, globalWaypointsPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointsPos - Vector3.left * size, globalWaypointsPos + Vector3.left * size);
			}
        }
    }
}