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
    private bool gameRestart = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
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
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(FlickerGameOverText());
            StartCoroutine(FadeInGameOverText());
            gameRestart = true; // Enable restart text
        }
    }

    void Update()
    {
        if (restartText && Input.GetKeyDown(KeyCode.R))
        {
            // Restart the game (You'll need to implement your own restart logic here)
            // For example:
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Restarting the game...");
        }
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

    private IEnumerator FadeInGameOverText()
    {
        Color textColor = gameOverText.color;
        textColor.a = 0f;
        gameOverText.color = textColor;

        while (gameOverText.color.a < 1f)
        {
            textColor.a += Time.deltaTime;
            gameOverText.color = textColor;
            yield return null;
        }
    }

    private IEnumerator RestartScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            // Your restart scene logic here
        }
    }
}
