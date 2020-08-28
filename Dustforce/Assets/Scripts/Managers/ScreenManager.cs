using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager: MonoBehaviour {
    public static ScreenManager _Instance { get; set; }
    // Scene Info
    public static string sceneName;
    public static int sceneIndex;
    // Load_Time
    private static float loadTime;

    // Scene Check
    private static bool loadCheck;

    enum SceneState {
        NULL,
        LOAD
    }

    private static SceneState state = SceneState.NULL;
    
    // Loading 창으로 전환될 때 마다 한번 만 실행시키기
    private bool OneLoad;

    // 시작과 동시에 ScreenManager 가 있으면 삭제 시키고 없으면 새로 생성, 씬 전환이 되어도 오브젝트를 남김
    private void Awake() {
        Debug.Log("싱글톤 실행");
        if (_Instance != null) {
            
            Debug.Log("객체 삭제");
            Destroy(gameObject);
        }
        else {
            Debug.Log("객체 유지");
            _Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {

    }

    public static void LoadSceneHandle(string name, int loadType) {
        Debug.Log(name + " 장면으로 전환");

        sceneName = name;
        sceneIndex = loadType;
        SceneManager.LoadScene("LoadingScene");
    }

    private void LateUpdate() {
        // 현재 Scene 이 LoadingScene 이라면?
        if (SceneManager.GetActiveScene().name == "LoadingScene") {
            Debug.Log("로딩씬 맞음!");
            // 여기서 다음 화면으로 코루틴 진행
            StartCoroutine(LoadScene());
        }
    }


    public static IEnumerator LoadScene() {
        Debug.Log("로딩 실행 코드 작동");
        yield return null;

        // Load Start
        loadTime += Time.deltaTime;
        Debug.Log(loadTime);

        // Load break
        if (loadTime > 5.0f) {
            // Loading, LoadTime Reset
            loadCheck = true;
            loadTime = .0f;
        }

        else loadCheck = false;
        
        // 다음 씬 으로 전환할 이름을 가지고 있습니다.
        // AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        // 로딩이 완료되면 다음 씬 전환
        // op.allowSceneActivation = false;

        //// true 가 되기전까지 실행해줍니다.
        //if (!op.isDone) {
        //    yield return null;
        //    if (sceneIndex == 0) {
        //        Debug.Log("새 게임!");
        //    }
        //    else if (sceneIndex == 1) {
        //        Debug.Log("이어하기!");
        //    }

        //    // 로드 시작!!
        //    loadTime += Time.deltaTime;

        //    Debug.Log(loadTime);
        //    // 로드가 끝났을 경우?
        //    if (loadTime < 5.0f) {
        //        Debug.Log("로드중");
        //    } 
        //    else if (loadTime >= 5.0) {
        //        // 로드 타임 초기화
        //        loadTime = .0f;
        //        // 로드 완료 유/무
        //        op.allowSceneActivation = true;
        //    }
        // }
    }
}