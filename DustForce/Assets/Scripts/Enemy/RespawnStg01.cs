using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStg01: MonoBehaviour {
    // 현 Scene 에서 전체 Enemy 오브젝트
    [SerializeField] private List<GameObject> _SpawnEnemy = new List<GameObject>();
    // 활성 시간
    [SerializeField] private List<float> _ActiveTime = new List<float>();
    // 비활성화 된 오브젝트 담을 배열 선언
    [SerializeField] private GameObject[] _InvisibleObj;
    // 오브젝트가 활성상태인지 검사
    private bool _ActiveCheck;

    private void Start() {
        // 배열 크기할당
        _InvisibleObj = new GameObject[_SpawnEnemy.Count];

        foreach(var obj in _InvisibleObj) {
            _ActiveTime.Add(0);
        }

        // 오브젝트 초기화
        for (int i = 0; i < _SpawnEnemy.Count; i++) {
            // 활성화 오브젝트가 있을 경우?
            if (_SpawnEnemy[i].activeSelf) {

                // 비활성화 전환.
                _SpawnEnemy[i].SetActive(false);
            }
            // 비활성화 된 오브젝트는 다른 공간에 저장
            if (!_SpawnEnemy[i].activeSelf) {
                _InvisibleObj[i] = _SpawnEnemy[i];
            } 
        }
    }
    private void Update() {
        // 전체 오브젝트가 활성중인지 검사하는 메서드
        _ActiveCheck = FindAllObjectActive.AllActiveGameObject(ref _SpawnEnemy);

        // 오브젝트가 전체 활성화 중이면?
        if(_ActiveCheck) {
            // 코루틴 정지
            StopCoroutine(ActiveObj(_InvisibleObj));
        }
        // 오브젝트가 비활성화 중이라면? 비활성화 된 오브젝트를 활성화로 전환
        else StartCoroutine(ActiveObj(_InvisibleObj));
    }

    // Enemy 오브젝트를 비활성화에서 활성화로 전환하는 함수
    private IEnumerator ActiveObj(GameObject[] gameObjects) {

        // 비활성화 된 오브젝트만 검사 후 활성화로 전환
        for(int i = 0; i < gameObjects.Length; i++) {
            if (!gameObjects[i].activeSelf) {
                // 각각의 활성시간 설정
                _ActiveTime[i] += Time.deltaTime;

                // '5' 초뒤에 활성화
                if (_ActiveTime[i] > 5.0f) {
                    gameObjects[i].SetActive(true);

                    _ActiveTime[i] = 0.0f;
                }
            }
        }
        yield break;
    }
}