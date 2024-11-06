using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public Transform targetPosition;
    public string objectTag;
    
    private List<GameObject> trackedObjects = new List<GameObject>();

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(objectTag) && !trackedObjects.Contains(other.gameObject))
        {
            trackedObjects.Add(other.gameObject);

            other.transform.position = targetPosition.position;
            other.transform.rotation = targetPosition.rotation;

            // MeshRenderer 활성화
            MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            // 이 오브젝트의 MeshRenderer도 활성화
            MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
            if (thisMeshRenderer != null)
            {
                thisMeshRenderer.enabled = true;
            }
        }
    }

    void Update()
    {
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