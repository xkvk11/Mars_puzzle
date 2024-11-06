using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stage1_next : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("Stage_2");
    }
    
}
