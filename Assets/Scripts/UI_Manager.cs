using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Text restartText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private int playerScore = 0;
    [SerializeField] private Image livesIMG;
    [SerializeField] private Sprite[] liveSprites;
    private float minGameOverFlicker = 0.5f;
    private float maxGameOverFlicker = 2f;
    [SerializeField]
    private GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        {
            if (gameManager == null)
            {
                Debug.Log("Game manager is null");
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + playerScore.ToString();



    }

    public void UpdateLives(int currentLives)
    {
        livesIMG.sprite = liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }
    void GameOverSequence()
    {
        gameManager.GameOver();
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverText());
        StartCoroutine(RestartScene());
        
    }

    private IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minGameOverFlicker, maxGameOverFlicker));
            gameOverText.enabled = false;
            yield return new WaitForSeconds(Random.Range(minGameOverFlicker, maxGameOverFlicker));
            gameOverText.enabled = true;
        }
    }

   

    private IEnumerator RestartScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            restartText.gameObject.SetActive(true); // Enable restart text
        }
    }
}
