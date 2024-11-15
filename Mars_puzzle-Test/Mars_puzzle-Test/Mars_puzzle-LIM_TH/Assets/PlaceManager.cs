using UnityEngine;
using TMPro;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance; // 싱글톤 패턴
    public TMP_Text countText;           // TextMeshPro UI 텍스트
    private int count = 0;

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            UpdateCountText(); // 초기 텍스트 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 카운트 증가 메서드
    public void IncrementCount()
    {
        count++;
        UpdateCountText();
    }

    // TMP UI 텍스트 업데이트 메서드
    private void UpdateCountText()
    {
        if (countText != null)
        {
            // 두 자리 형식으로 표시 (예: 00, 01, 02, ...)
            countText.text = $"{count.ToString("D2")} / 25";
        }
    }
}
