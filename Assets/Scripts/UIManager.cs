using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _game;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Sprite[] _lifeSprites;

    // Start is called before the first frame update
    void Start()
    {
        _game = FindObjectOfType<GameManager>();
        if (_game == null)
        {
            Debug.Log("Game Manager not found!");
        }

        UpdateLivesDisplay(3);
        UpdateScoreText(0);
        HideGameOver();
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        _livesDisplay.sprite = _lifeSprites[currentLives];
        if (currentLives == 0)
        {
            ShowGameOver();
        }
    }

    public void UpdateScoreText(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void ShowGameOver()
    {
        _game.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverRoutine());
    }

    public void HideGameOver()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    IEnumerator FlickerGameOverRoutine()
    {
        float delay = 0.30f;
        string text = "GAME OVER";

        while (true)
        {
            _gameOverText.text = text;
            yield return new WaitForSeconds(delay);
            _gameOverText.text = string.Empty;
            yield return new WaitForSeconds(delay);
            _gameOverText.text = text;
        }
    }
}
