using UnityEngine;

public class RigidbodyConstraintsController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
    }

    // 충돌 시작 시 호출
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 시작되면 Constraints 설정
        FreezeRigidbody();
    }

    // 충돌 종료 시 호출
    private void OnCollisionExit(Collision collision)
    {
        // 충돌이 끝나면 Constraints 해제
        UnfreezeRigidbody();
    }

    // Constraints 설정 메서드
    private void FreezeRigidbody()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Constraints 해제 메서드
    private void UnfreezeRigidbody()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
