using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip _restartAudioClip;
    private Vector3 _audioClipPlayPosition = new Vector3(0.0f, 0.0f, -10.0f); // TO-DO: need to find a way to centralize this

    private bool _isGameOver = false;

    private float _timeToQuit = 0.0f;

    private void Update()
    {
        // Handle inputs

        // Restart on game over
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            StartCoroutine(RestartRoutine());
        }

        // Open pause screen
        // TODO

        // Quit on Escape key (after 2 seconds)
        if (Input.GetKey(KeyCode.Escape))
        {
            _timeToQuit += Time.deltaTime;

            if (_timeToQuit >= 3.0f)
            {
                Debug.Log("Quitting...");
                Application.Quit();
            }
        } else
        {
            _timeToQuit = 0.0f;
        }
    }

    private IEnumerator RestartRoutine()
    {
        // reload scene/level
        // TO-DO: review solution and optimize where possible
        AudioSource.PlayClipAtPoint(_restartAudioClip, _audioClipPlayPosition);
        yield return new WaitForSeconds(_restartAudioClip.length);
        SceneManager.LoadScene(1);
        _isGameOver = false;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
