using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    // Player's total score and round-specific score
    public int score;
    public int roundScore;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize roundScore to the current score
        roundScore = score;
    }

    // Reset roundScore to the current score at the start of each round
    public void StartRound()
    {
        roundScore = score;
    }

    // Add points to the player's total score
    public void AddPoints(int points)
    {
        score += points;
    }

    // Check if the player can afford a certain cost in the current round
    public bool CanAfford(int cost)
    {
        return roundScore >= cost;
    }

    // Deduct the cost from roundScore, ensuring it doesn't go below zero
    public void DeductRoundScore(int cost)
    {
        roundScore -= cost;
        roundScore = Mathf.Max(roundScore, 0);
    }

    // Get the current round score
    public int GetRoundScore()
    {
        return roundScore;
    }
}
