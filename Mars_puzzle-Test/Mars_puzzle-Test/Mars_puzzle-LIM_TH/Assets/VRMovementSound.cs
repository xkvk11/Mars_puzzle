using UnityEngine;
using UnityEngine.XR;

public class VRMovementSound : MonoBehaviour
{
    public AudioClip movementSound;      // 이동 사운드
    private AudioSource audioSource;     // AudioSource 컴포넌트
    private Vector3 lastPosition;        // 이전 프레임의 위치
    public float minMoveDistance = 0.1f; // 사운드가 재생될 최소 이동 거리

    void Start()
    {
        // AudioSource 컴포넌트를 추가하고 오디오 클립을 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = movementSound;
        
        // 오디오 속성 설정
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        // 초기 위치 저장
        lastPosition = transform.position;
    }

    void Update()
    {
        // 현재 위치와 이전 위치의 차이 계산
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // 최소 이동 거리 이상일 때 사운드 재생
        if (distanceMoved >= minMoveDistance && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // 현재 위치를 이전 위치로 업데이트
        lastPosition = transform.position;
    }
}
