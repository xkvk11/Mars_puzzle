using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro; // TextMeshPro 사용

public class Place : MonoBehaviour
{
    public Transform targetPosition;  // 오브젝트가 배치될 위치
    public string objectTag;          // 트리거할 오브젝트 태그
    private List<GameObject> trackedObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // 트리거된 오브젝트가 지정된 태그를 가지고 있고, 아직 목록에 없을 때만 처리
        if (other.CompareTag(objectTag) && !trackedObjects.Contains(other.gameObject))
        {
            trackedObjects.Add(other.gameObject);

            // 오브젝트 위치 및 회전 고정
            other.transform.position = targetPosition.position;
            other.transform.rotation = targetPosition.rotation;

            // MeshRenderer 활성화
            MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            // Place 오브젝트의 MeshRenderer 활성화
            MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
            if (thisMeshRenderer != null)
            {
                thisMeshRenderer.enabled = true;
            }

            // XRGrabInteractable 비활성화를 위한 Coroutine 시작
            StartCoroutine(DisableXRGrabAfterDelay(other));

            // PlaceManager에서 카운트 증가
            PlaceManager.Instance.IncrementCount();
        }
    }

    private IEnumerator DisableXRGrabAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1f); // 1초 대기

        // XRGrabInteractable 비활성화
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }
    }

    void Update()
    {
        // 모든 추적된 오브젝트 위치 및 회전 업데이트
        foreach (GameObject obj in trackedObjects)
        {
            if (obj != null)
            {
                obj.transform.position = targetPosition.position;
                obj.transform.rotation = targetPosition.rotation;

                // MeshRenderer 활성화
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true;
                }
            }
        }
    }
}
