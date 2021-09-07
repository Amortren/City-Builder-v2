using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void SceneSwitch(int i)
    {
        SceneManager.LoadScene(i);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
