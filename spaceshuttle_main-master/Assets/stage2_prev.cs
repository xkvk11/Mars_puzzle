using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stage2_prev : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("Stage_1");
    }
}
