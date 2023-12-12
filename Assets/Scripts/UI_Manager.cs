using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _gameOverText;
    public Text _scoreText;
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
    public Slider _shieldSlider;
    [SerializeField]
    private Text _ammoText;
    private Player _player;
    public bool _isAmmoDepleted = false;


    void Start()
    {
        InitializeShieldSlider();

        _gameOverText.gameObject.SetActive(false);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _ammoText.text = _player._ammunition.ToString();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game manager is null");
        }
    }


    public void OnEnemyDestroyed(int scoreValue)
    {
        _playerScore += scoreValue;
        UpdateScoreUI(); // Update the UI with the added score
    }



    public void AddScore(int scoreToAdd)
    {
        _playerScore += scoreToAdd;
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

    public void UpdateAmmoCount(int _ammunition)
    {
        _ammoText.text = _ammunition.ToString();

        if (_ammunition <= 0)
        {
            _isAmmoDepleted = true;
        }
    }
}
