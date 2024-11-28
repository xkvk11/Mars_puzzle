using System.Collections;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public Light lightComponent; // Unity의 Light 컴포넌트를 연결할 변수
    public float minInterval = 0.1f; // 최소 간격
    public float maxInterval = 1.0f; // 최대 간격

    private void Start()
    {
        if (lightComponent == null)
        {
            lightComponent = GetComponent<Light>();
        }

        StartCoroutine(ToggleLightRoutine());
    }

    private IEnumerator ToggleLightRoutine()
    {
        while (true)
        {
            lightComponent.enabled = !lightComponent.enabled;
            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
