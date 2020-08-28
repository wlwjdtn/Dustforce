using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager: MonoBehaviour {
    // 사운드 매니저 씬 전환시 파괴방지를 위한 싱글톤 패턴
    private static SoundManager instance;

    // 싱글톤 패턴 구현
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    // AudioSource 에 접근
    private AudioSource audioSource;
    // BGM Sound 목록
    [SerializeField]
    private AudioClip[] BGMGroup;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        // 현재 Scene 이 'Stage Scene' 이라면?
        if (SceneManager.GetActiveScene().buildIndex > 2) {
            // AudioSource 가 재생중이지 않다면?
            if (!audioSource.isPlaying) {
                BGMRandomPlaying();
            }
        }
    }

    // Stage_BGM_RandomPlaying
    private void BGMRandomPlaying() {
        // 오디오 목록에 있는 사운드 중 하나를 랜덤으로 지정
        audioSource.clip = BGMGroup[Random.Range(0, BGMGroup.Length)];
        // 랜덤으로 지정된 사운드를 재생
        audioSource.Play();
        // 오디오 볼륨
        audioSource.volume = 0.15f;
    }
}