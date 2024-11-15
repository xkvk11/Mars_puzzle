using UnityEngine;

public class LightDeactivator : MonoBehaviour
{
    public string targetTag = "YourTag"; // 체크할 태그 이름을 지정하세요.
    private float collisionTime = 0f;
    private bool isColliding = false;
    private GameObject pointLight;
    private bool lightDisabled = false; // 한 번 비활성화되었는지 체크하는 변수

    void Start()
    {
        // 자식 오브젝트 중 "Point Light"라는 이름의 오브젝트를 찾습니다.
        pointLight = transform.Find("Point Light")?.gameObject;
        if (pointLight == null)
        {
            Debug.LogWarning("Point Light 오브젝트를 찾을 수 없습니다.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag) && !lightDisabled)
        {
            isColliding = true;
            collisionTime = 0f; // 충돌 시간을 초기화합니다.
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (isColliding && collision.gameObject.CompareTag(targetTag) && !lightDisabled)
        {
            collisionTime += Time.deltaTime; // 충돌 시간 누적

            if (collisionTime >= 1f) // 충돌 시간이 1초 이상이면
            {
                if (pointLight != null)
                {
                    pointLight.SetActive(false); // Point Light 비활성화
                }
                lightDisabled = true; // 비활성화 상태를 기록하여 재활성화 방지
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            isColliding = false;
            collisionTime = 0f; // 충돌 시간을 초기화합니다.
        }
    }
}
