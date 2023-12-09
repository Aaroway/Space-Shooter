using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] 
    private Text _restartText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private int _playerScore = 0;
    [SerializeField]
    private Image _livesIMG;
    [SerializeField]
    private Sprite[] _liveSprites;
    private float _minGameOverFlicker = 0.5f;
    private float _maxGameOverFlicker = 2f;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Slider _shieldSlider;


    void Start()
    {
        InitializeShieldSlider();
        _gameOverText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game manager is null");
        }
    }


    public void AddScore(int scoreToAdd)
    {
        _playerScore += scoreToAdd;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        _scoreText.text = "Score: " + _playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesIMG.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void InitializeShieldSlider()
    {
        _shieldSlider.value = 3f;
    }

    public void UpdateShieldSlider(float shieldPercentage)
    {
        _shieldSlider.value = shieldPercentage;
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverText());
        StartCoroutine(RestartScene());       
    }

    private IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minGameOverFlicker, _maxGameOverFlicker));
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(Random.Range(_minGameOverFlicker, _maxGameOverFlicker));
            _gameOverText.enabled = true;
        }
    }

   

    private IEnumerator RestartScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            _restartText.gameObject.SetActive(true);
        }
    }
}
