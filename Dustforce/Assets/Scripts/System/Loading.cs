using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading: MonoBehaviour {

    // 텍스트 페이드 효과
    [SerializeField] private Text[] _AryText;
    // Load Check
    private Image _CircleImg;

    private void Awake() {
        _CircleImg = GameObject.Find("Load_Image_Mask").GetComponent<Image>();
        // File Load
        StartCoroutine(FadeInToZeroAlpha());
        StartCoroutine(FadeInCircleAlphaOff());
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
}
