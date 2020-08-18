using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {

    // 호출 시 한번만 메모리 할당
    private static ScreenManager Screen_Instance;

    // 싱글톤으로 한번만 호출하기위해 게터와 세터 설정
    public static ScreenManager _Instance {
        get { return Screen_Instance; }
        set { Screen_Instance = value; }
    }

    // 시작과 동시에 ScreenManager 가 있으면 삭제 시키고 없으면 새로 생성, 씬 전환이 되어도 오브젝트를 남김
    private void Awake() {
        if(_Instance != null) {
            Destroy(gameObject);
        }
        else { _Instance = this; }
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {

    }

    // 다음 씬 전환 함수
    public void NextScene() {
        LoadingScene();
        
    }

    // 이전 씬 전환 함수
    public void PreviousScene() {
        LoadingScene();
    }

    // 로딩 씬
    public void LoadingScene() {
        SceneManager.LoadScene("LoadingScene");
    }
}