using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBarController : MonoBehaviour
{
    public Image pressBar;  // PressBar UI 이미지
    public string nextSceneName;  // 로드할 다음 씬 이름

    // 씬 로드를 시작하는 함수
    public void StartLoading()
    {
        // 코루틴을 시작하여 비동기 로딩과 Fill Amount 업데이트 진행
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // 비동기 씬 로딩 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        operation.allowSceneActivation = false; // 로딩이 완료돼도 자동으로 씬 전환하지 않도록 설정

        // 로딩이 완료될 때까지 Fill Amount를 업데이트
        while (!operation.isDone)
        {
            // 로딩 진행 상황을 기반으로 Fill Amount 업데이트
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 유니티에서 progress는 최대 0.9까지만 차오름
            pressBar.fillAmount = progress;

            // 로딩이 끝난 경우 씬 활성화
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}
