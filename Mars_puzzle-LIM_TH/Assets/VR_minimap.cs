using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VRMinimap : MonoBehaviour
{
   [System.Serializable]
   public class PlayerMarkerSettings
   {
       public GameObject markerPrefab;
       public Color markerColor = Color.red;
       [Range(1f, 50f)]
       public float markerSize = 10f;
   }

   [Header("References")]
   [SerializeField] private Transform player;           
   [SerializeField] private Camera minimapCamera;       
   [SerializeField] private RawImage minimapDisplay;    
   [SerializeField] private Button zoomInButton;        
   [SerializeField] private Button zoomOutButton;       

   [Header("Player Marker")]
   [SerializeField] 
   private PlayerMarkerSettings playerMarkerSettings = new PlayerMarkerSettings();

   [Header("Minimap Settings")]
   [SerializeField] private float minimapSize = 100f;    
   [SerializeField] [Range(0.1f, 2f)] 
   private float minimapDistance = 0.5f;   
   [SerializeField] [Range(-1f, 1f)] 
   private float minimapHeight = -0.3f;    

   [Header("Zoom Settings")]
   [SerializeField] private float zoomSpeed = 10f;      
   [SerializeField] private float minZoom = 10f;        
   [SerializeField] private float maxZoom = 200f;       

   [Header("Display Settings")]
   [SerializeField] 
   private Vector2Int minimapResolution = new Vector2Int(256, 256);

   private RenderTexture minimapTexture;
   private Transform vrCamera;
   private GameObject playerMarker;

   void Start()
   {
       if (!CheckComponents()) return;
       SetupMinimap();
       CreateZoomButtons();
       CreatePlayerMarker();
   }

   bool CheckComponents()
   {
       if (player == null)
       {
           Debug.LogError("Player Transform이 설정되지 않았습니다!");
           enabled = false;
           return false;
       }

       if (minimapCamera == null)
       {
           Debug.LogError("Minimap Camera가 설정되지 않았습니다!");
           enabled = false;
           return false;
       }

       if (minimapDisplay == null)
       {
           Debug.LogError("Minimap Display가 설정되지 않았습니다!");
           enabled = false;
           return false;
       }

       vrCamera = Camera.main.transform;
       if (vrCamera == null)
       {
           Debug.LogError("Main Camera를 찾을 수 없습니다!");
           enabled = false;
           return false;
       }

       return true;
   }

   void CreateZoomButtons()
   {
       if (zoomInButton == null)
       {
           GameObject zoomInObj = new GameObject("ZoomInButton");
           zoomInObj.transform.SetParent(minimapDisplay.transform);
           zoomInButton = zoomInObj.AddComponent<Button>();
           
           Image zoomInImage = zoomInObj.AddComponent<Image>();
           zoomInImage.color = Color.white;
           
           GameObject textObj = new GameObject("Text");
           textObj.transform.SetParent(zoomInObj.transform);
           Text text = textObj.AddComponent<Text>();
           text.text = "+";
           text.alignment = TextAnchor.MiddleCenter;
           text.color = Color.black;
           
           RectTransform rect = zoomInObj.GetComponent<RectTransform>();
           rect.anchoredPosition = new Vector2(50, 50);
           rect.sizeDelta = new Vector2(40, 40);
       }

       if (zoomOutButton == null)
       {
           GameObject zoomOutObj = new GameObject("ZoomOutButton");
           zoomOutObj.transform.SetParent(minimapDisplay.transform);
           zoomOutButton = zoomOutObj.AddComponent<Button>();
           
           Image zoomOutImage = zoomOutObj.AddComponent<Image>();
           zoomOutImage.color = Color.white;
           
           GameObject textObj = new GameObject("Text");
           textObj.transform.SetParent(zoomOutObj.transform);
           Text text = textObj.AddComponent<Text>();
           text.text = "-";
           text.alignment = TextAnchor.MiddleCenter;
           text.color = Color.black;
           
           RectTransform rect = zoomOutObj.GetComponent<RectTransform>();
           rect.anchoredPosition = new Vector2(50, -50);
           rect.sizeDelta = new Vector2(40, 40);
       }

       zoomInButton.onClick.AddListener(ZoomIn);
       zoomOutButton.onClick.AddListener(ZoomOut);
   }

   void CreatePlayerMarker()
   {
       if (playerMarker != null)
       {
           Destroy(playerMarker);
       }

       if (playerMarkerSettings.markerPrefab != null)
       {
           playerMarker = Instantiate(playerMarkerSettings.markerPrefab, minimapDisplay.transform);
       }
       else
       {
           playerMarker = new GameObject("PlayerMarker");
           playerMarker.transform.SetParent(minimapDisplay.transform);
           Image markerImage = playerMarker.AddComponent<Image>();
           markerImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
       }

       Image image = playerMarker.GetComponent<Image>();
       if (image != null)
       {
           image.color = playerMarkerSettings.markerColor;
       }

       RectTransform rect = playerMarker.GetComponent<RectTransform>();
       if (rect != null)
       {
           rect.sizeDelta = new Vector2(playerMarkerSettings.markerSize, 
                                      playerMarkerSettings.markerSize);
       }
   }

   void SetupMinimap()
   {
       minimapTexture = new RenderTexture(minimapResolution.x, minimapResolution.y, 16);
       minimapCamera.targetTexture = minimapTexture;
       minimapDisplay.texture = minimapTexture;

       minimapCamera.orthographic = true;
       minimapCamera.orthographicSize = minimapSize;
       minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
   }

   void ZoomIn()
   {
       minimapSize = Mathf.Max(minZoom, minimapSize - zoomSpeed);
       minimapCamera.orthographicSize = minimapSize;
   }

   void ZoomOut()
   {
       minimapSize = Mathf.Min(maxZoom, minimapSize + zoomSpeed);
       minimapCamera.orthographicSize = minimapSize;
   }

   void LateUpdate()
   {
       UpdateMinimapPosition();
       UpdatePlayerMarker();
   }

   void UpdateMinimapPosition()
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

   void UpdatePlayerMarker()
   {
       if (playerMarker != null)
       {
           Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(player.position);
           
           RectTransform markerRect = playerMarker.GetComponent<RectTransform>();
           RectTransform minimapRect = minimapDisplay.GetComponent<RectTransform>();

           float x = (viewportPoint.x * minimapRect.rect.width) - (minimapRect.rect.width * 0.5f);
           float y = (viewportPoint.y * minimapRect.rect.height) - (minimapRect.rect.height * 0.5f);
           
           markerRect.anchoredPosition = new Vector2(x, y);
           markerRect.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
       }
   }

   void OnDestroy()
   {
       if (minimapTexture != null)
       {
           minimapTexture.Release();
           Destroy(minimapTexture);
       }
   }
}