using UnityEngine;
using UnityEngine.UI;

public class VRMinimap : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera minimapCamera;
    public RawImage minimapDisplay;
    public GameObject playerIndicator;
    public Button zoomInButton;    // 줌 인 버튼
    public Button zoomOutButton;   // 줌 아웃 버튼

    [Header("Minimap Settings")]
    [Tooltip("미니맵에 표시될 영역의 크기")]
    public float minimapSize = 100f;

    [Tooltip("플레이어로부터 미니맵까지의 거리")]
    [Range(0.1f, 2f)]
    public float minimapDistance = 0.5f;

    [Tooltip("플레이어로부터 미니맵의 상대적 높이")]
    [Range(-1f, 1f)]
    public float minimapHeight = -0.3f;

    [Header("Display Settings")]
    [Tooltip("미니맵 텍스처의 해상도")]
    public Vector2Int minimapResolution = new Vector2Int(256, 256);

    [Header("Zoom Settings")]
    [Tooltip("줌 인/아웃 시 변경되는 크기")]
    public float zoomStep = 10f;
    [Tooltip("최소 줌 크기")]
    public float minZoom = 50f;
    [Tooltip("최대 줌 크기")]
    public float maxZoom = 200f;

    private RenderTexture minimapTexture;
    private Transform vrCamera;

    private void Start()
    {
        // 기존 컴포넌트 체크
        if (player == null || minimapCamera == null || minimapDisplay == null)
        {
            Debug.LogError("필수 컴포넌트가 설정되지 않았습니다!");
            enabled = false;
            return;
        }

        // 줌 버튼 체크 및 리스너 설정
        if (zoomInButton != null)
        {
            zoomInButton.onClick.AddListener(ZoomIn);
        }
        else
        {
            Debug.LogWarning("Zoom In 버튼이 설정되지 않았습니다!");
        }

        if (zoomOutButton != null)
        {
            zoomOutButton.onClick.AddListener(ZoomOut);
        }
        else
        {
            Debug.LogWarning("Zoom Out 버튼이 설정되지 않았습니다!");
        }

        // VR 카메라 찾기
        vrCamera = Camera.main.transform;
        if (vrCamera == null)
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다!");
            enabled = false;
            return;
        }

        // 미니맵 렌더 텍스쳐 설정
        minimapTexture = new RenderTexture(minimapResolution.x, minimapResolution.y, 16);
        minimapCamera.targetTexture = minimapTexture;
        minimapDisplay.texture = minimapTexture;

        // 미니맵 카메라 설정
        minimapCamera.orthographic = true;
        minimapCamera.orthographicSize = minimapSize;
        minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void LateUpdate()
    {
        UpdateMinimapPosition();
        UpdatePlayerIndicator();
    }

    private void UpdateMinimapPosition()
    {
        Vector3 targetPosition = player.position;
        targetPosition.y += minimapHeight;

        Vector3 forward = Vector3.ProjectOnPlane(vrCamera.forward, Vector3.up).normalized;
        Vector3 offset = forward * minimapDistance;

        transform.position = targetPosition + offset;
        transform.rotation = Quaternion.LookRotation(-forward, Vector3.up);

        minimapCamera.transform.position = new Vector3(
            player.position.x,
            player.position.y + minimapSize,
            player.position.z
        );
    }

    private void UpdatePlayerIndicator()
    {
        if (playerIndicator != null)
        {
            Vector3 playerPos = player.position;
            playerPos.y = playerIndicator.transform.position.y;
            playerIndicator.transform.position = playerPos;

            float playerRotationY = player.eulerAngles.y;
            playerIndicator.transform.rotation = Quaternion.Euler(90f, playerRotationY, 0f);
        }
    }

    public void ZoomIn()
    {
        // 줌 인
        float newSize = Mathf.Max(minimapCamera.orthographicSize - zoomStep, minZoom);
        minimapCamera.orthographicSize = newSize;
        minimapSize = newSize;
    }

    public void ZoomOut()
    {
        // 줌 아웃
        float newSize = Mathf.Min(minimapCamera.orthographicSize + zoomStep, maxZoom);
        minimapCamera.orthographicSize = newSize;
        minimapSize = newSize;
    }

    private void OnDestroy()
    {
        // 버튼 리스너 제거
        if (zoomInButton != null)
        {
            zoomInButton.onClick.RemoveListener(ZoomIn);
        }
        if (zoomOutButton != null)
        {
            zoomOutButton.onClick.RemoveListener(ZoomOut);
        }

        // 렌더 텍스쳐 정리
        if (minimapTexture != null)
        {
            minimapTexture.Release();
            Destroy(minimapTexture);
        }
    }
}