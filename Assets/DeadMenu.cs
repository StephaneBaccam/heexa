using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public GameObject DeadMenuUI;

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
