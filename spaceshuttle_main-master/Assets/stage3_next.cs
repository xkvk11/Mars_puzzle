using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stage3_next : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("Stage_4");
    }
}
