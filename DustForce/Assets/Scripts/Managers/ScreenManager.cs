using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager: MonoBehaviour {
    public static ScreenManager _Instance { get; set; }
    // Scene Info
    public static string sceneName;
    public static int sceneIndex;
    // Load_Time
    private static float loadTime;

    // 시작과 동시에 ScreenManager 가 있으면 삭제 시키고 없으면 새로 생성, 씬 전환이 되어도 오브젝트를 남김
    private void Awake() {
        if (_Instance != null) {
            Destroy(gameObject);
        }
        else { _Instance = this; }
        DontDestroyOnLoad(gameObject);
    }
    public void OnEnable() {
        StartCoroutine(LoadScene());
    }

    public static void LoadSceneHandle(string name, int loadType) {
        sceneName = name;
        sceneIndex = loadType;
        SceneManager.LoadScene("LoadingScene");
    }

    public static IEnumerator LoadScene() {
        yield return null;
        // 다음 씬 으로 전환할 이름을 가지고 있습니다.
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        Debug.Log(op);
        // 로딩이 완료되면 다음 씬 전환
        op.allowSceneActivation = false;

        // true 가 되기전까지 실행해줍니다.
        while (!op.isDone) {
            Debug.Log("들어옴?");

            yield return null;
            if (sceneIndex == 0) {
                Debug.Log("새 게임!");
            }
            else if (sceneIndex == 1) {
                Debug.Log("이어하기!");
            }
            // 로드 시작!!
            loadTime += Time.deltaTime;
            // 로드가 끝났을 경우?
            if (loadTime > 5.0f) {
                Debug.Log(loadTime);
                // 로드 타임 초기화
                loadTime = .0f;
                // 로드 완료 유/무
                op.allowSceneActivation = true;
            }
        }
    }
}