using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleLoad : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title"); // Title 씬을 로드
    }
}
