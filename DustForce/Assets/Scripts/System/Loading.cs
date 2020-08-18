using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    // 텍스트 페이드 효과
    [SerializeField] private Text[] _AryText;

    // Load TIme
    private float Load_Time;

    // Load Check
    private bool Load_Check;

    private Image _CircleImg;

    // 시작과 동시에 ScreenManager 가 있으면 삭제 시키고 없으면 새로 생성, 씬 전환이 되어도 오브젝트를 남김
    private void Awake() {
        _CircleImg = GameObject.Find("Load_Image_Mask").GetComponent<Image>();

        // File Load
        StartCoroutine(FadeInToZeroAlpha());
        StartCoroutine(FadeInCircleAlphaOff());

    }

    private void Update() {
        LoadTime();
    }

    // 알파 값을 Full 조정
    private IEnumerator FadeInToFullAlpha() {
        for (int i = 0; i < _AryText.Length; i++) {
            _AryText[i].color = new Color(_AryText[i].color.r, _AryText[i].color.g, _AryText[i].color.b, 0);
            // 한 글자씩 페이드
            while (_AryText[i].color.a < 1.0f) {
                _AryText[i].color = new Color(_AryText[i].color.r, _AryText[i].color.g, _AryText[i].color.b, _AryText[i].color.a + (Time.deltaTime / 0.5f));
                yield return null;
            }

        }
        StartCoroutine(FadeInToZeroAlpha());
    }

    // 알파 값을 Zero 조정
    private IEnumerator FadeInToZeroAlpha() {
        for (int i = 0; i < _AryText.Length; i++) {
            _AryText[i].color = new Color(_AryText[i].color.r, _AryText[i].color.g, _AryText[i].color.b, 1);
            while (_AryText[i].color.a > 0.0f) {
                _AryText[i].color = new Color(_AryText[i].color.r, _AryText[i].color.g, _AryText[i].color.b, _AryText[i].color.a - (Time.deltaTime / 0.5f));
                yield return null;
            }
        }
        StartCoroutine(FadeInToFullAlpha());
    }

    private IEnumerator FadeInCircleAlphaOn() {
        _CircleImg.color = new Color(_CircleImg.color.r, _CircleImg.color.g, _CircleImg.color.b, 0);
        while (_CircleImg.color.a < 1.0f) {
            _CircleImg.color = new Color(_CircleImg.color.r, _CircleImg.color.g, _CircleImg.color.b, _CircleImg.color.a + (Time.deltaTime / 1.5f));
            yield return null;
        }
        StartCoroutine(FadeInCircleAlphaOff());
    }

    private IEnumerator FadeInCircleAlphaOff() {
        _CircleImg.color = new Color(_CircleImg.color.r, _CircleImg.color.g, _CircleImg.color.b, 1);
        while (_CircleImg.color.a > 0.0f) {
            _CircleImg.color = new Color(_CircleImg.color.r, _CircleImg.color.g, _CircleImg.color.b, _CircleImg.color.a - (Time.deltaTime / 1.5f));
            yield return null;
        }
        StartCoroutine(FadeInCircleAlphaOn());
    }

    private void LoadTime() {

        // 트리거 작동 후 로딩 경과시간 실행
        if (GameManager.GetInstance.Get_TimeSet) {
            Load_Time += Time.deltaTime;
            if (Load_Time > 5.0f) {
                Load_Check = true;
                Load_Time = .0f;
                GameManager.GetInstance.Set_TimeClear(Load_Check);
            } 
            else if (Load_Time <= .0f) {
                Load_Check = false;
            }

            if (GameManager.GetInstance.Get_TimeClear) {
                SceneManager.LoadScene(GameManager.GetInstance.Get_SceneName);
                GameManager.GetInstance.Set_TimeClear(false);
            }
            Debug.Log(Load_Time);
        }
    }
}