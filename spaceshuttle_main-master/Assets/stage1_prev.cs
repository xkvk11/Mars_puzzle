using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stage1_prev : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("start");
    }
}
