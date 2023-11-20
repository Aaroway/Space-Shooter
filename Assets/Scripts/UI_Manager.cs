using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]private Text scoreText;
    [SerializeField]private int playerScore = 0;

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd; // Add the provided score to the player's total score
        UpdateScoreUI(); // Update the UI to reflect the new score
    }
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + playerScore.ToString(); // Update the Text component with the new score
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScoreUI1()
    {
        scoreText.text = "Score: " + playerScore.ToString(); // Update the Text component with the new score
    }
}
