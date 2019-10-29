using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    private void Update()
    {
        // Handle inputs
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            // reload scene/level
            SceneManager.LoadScene(1);
            _isGameOver = false;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
