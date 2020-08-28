using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTxt_Effact: MonoBehaviour {

    private int blinkCount;
    private Text colorTxt;

    private float _reTime;
    private bool _reTxtOn;

    private void Start() {
        colorTxt = GetComponent<Text>();

        StartCoroutine(Flicker());
    }

    private void FixedUpdate() {
        if(_reTxtOn) {
            _reTime += Time.deltaTime;
            if(_reTime > 2.0f) {
                _reTime = .0f;
                _reTxtOn = false;
            }
        }
    }
    private IEnumerator Flicker() {
        blinkCount = 0;
        while (blinkCount < 10 && !_reTxtOn) {
            if (blinkCount % 2 == 0)
                colorTxt.color = new Color32(150, 150, 150, 255);
            else
                colorTxt.color = new Color32(255, 255, 255, 255);

            yield return new WaitForSeconds(0.3f);
            blinkCount++;
        }
        _reTxtOn = true;
        yield return StartCoroutine(Flicker());
    }
}