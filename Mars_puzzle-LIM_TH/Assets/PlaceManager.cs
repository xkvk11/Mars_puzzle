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
            countText.text = $"{count} / 23";
        }
    }
}
