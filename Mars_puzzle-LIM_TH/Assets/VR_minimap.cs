using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VRMinimap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform player;           
    [SerializeField] public Camera minimapCamera;       
    [SerializeField] public RawImage minimapDisplay;    
    [SerializeField] public GameObject playerMarkerPrefab;  // 플레이어 위치 마커 프리팹
    [SerializeField] public Button zoomInButton;        
    [SerializeField] public Button zoomOutButton;       

    [Header("Tag Settings")]
    [SerializeField] public GameObject tagIndicatorPrefab;  
    [SerializeField] public float baseIndicatorSize = 1f;   

    [Header("Player Marker Settings")] // 플레이어 마커 관련 설정 추가
    [SerializeField] public Color playerMarkerColor = Color.red;  // 플레이어 마커 색상
    [SerializeField] public float playerMarkerSize = 10f;        // 플레이어 마커 크기

    [Header("Minimap Settings")]
    [SerializeField] public float minimapSize = 100f;    
    [SerializeField] [Range(0.1f, 2f)] public float minimapDistance = 0.5f;   
    [SerializeField] [Range(-1f, 1f)] public float minimapHeight = -0.3f;    

    [Header("Zoom Settings")]
    [SerializeField] public float zoomSpeed = 10f;      
    [SerializeField] public float minZoom = 10f;        
    [SerializeField] public float maxZoom = 200f;       

    [Header("Display Settings")]
    [SerializeField] public Vector2Int minimapResolution = new Vector2Int(256, 256);

    private RenderTexture minimapTexture;
    private Transform vrCamera;
    private Dictionary<MinimapTag, GameObject> tagIndicators = new Dictionary<MinimapTag, GameObject>();
    private GameObject playerMarker; // 플레이어 마커 오브젝트

    void Start()
    {
        if (!CheckComponents()) return;
        SetupMinimap();
        CreateZoomButtons();
        InitializeTagIndicators();
        CreatePlayerMarker(); // 플레이어 마커 생성
    }

    void CreatePlayerMarker()
    {
        if (playerMarkerPrefab == null)
        {
            // 프리팹이 없는 경우 기본 마커 생성
            GameObject markerObj = new GameObject("PlayerMarker");
            markerObj.transform.SetParent(minimapDisplay.transform);
            
            // 이미지 컴포넌트 추가
            Image markerImage = markerObj.AddComponent<Image>();
            markerImage.color = playerMarkerColor;

            // RectTransform 설정
            RectTransform rectTransform = markerObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(playerMarkerSize, playerMarkerSize);
            
            playerMarker = markerObj;
        }
        else
        {
            // 프리팹이 있는 경우 프리팹으로 생성
            playerMarker = Instantiate(playerMarkerPrefab, minimapDisplay.transform);
            playerMarker.GetComponent<Image>().color = playerMarkerColor;
            playerMarker.GetComponent<RectTransform>().sizeDelta = new Vector2(playerMarkerSize, playerMarkerSize);
        }
    }

    void UpdatePlayerMarker()
    {
        if (playerMarker != null)
        {
            // 플레이어의 월드 좌표를 뷰포트 좌표로 변환
            Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(player.position);
            
            // RectTransform 가져오기
            RectTransform rectTransform = playerMarker.GetComponent<RectTransform>();
            RectTransform minimapRect = minimapDisplay.GetComponent<RectTransform>();

            // 미니맵 UI 내의 위치 계산
            float x = (viewportPoint.x * minimapRect.rect.width) - (minimapRect.rect.width * 0.5f);
            float y = (viewportPoint.y * minimapRect.rect.height) - (minimapRect.rect.height * 0.5f);
            
            // 위치 설정
            rectTransform.anchoredPosition = new Vector2(x, y);

            // 플레이어의 회전 반영
            rectTransform.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
        }
    }

    void LateUpdate()
    {
        UpdateMinimapPosition();
        UpdatePlayerMarker(); // 플레이어 마커 위치 업데이트
        UpdateTagIndicators();
    }

    // ... (나머지 기존 코드는 그대로 유지)
}