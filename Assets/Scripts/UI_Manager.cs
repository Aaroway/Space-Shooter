using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]private Text gameOverText;
    [SerializeField]private Text scoreText;
    [SerializeField]private int playerScore = 0;
    [SerializeField]private Image livesIMG;
    [SerializeField]private Sprite[] liveSprites;
    private float minGameOverFlicker = 0.5f;
    private float maxGameOverFlicker = 2f;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd; // Add the provided score to the player's total score
        UpdateScoreUI(); // Update the UI to reflect the new score
    }
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + playerScore.ToString(); // Update the Text component with the new score
    }

    public void UpdateLives(int currentLives)
    {
        livesIMG.sprite = liveSprites[currentLives];

        if (currentLives == 0)
        {
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(FlickerGameOverText());
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
}
