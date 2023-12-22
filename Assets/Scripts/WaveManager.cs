using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    // References to other scripts and variables
    public Spawner spawner;
    public PlayerScore playerScore;

    // Time between waves and the current wave timer
    public float timeBetweenWaves = 10f;
    public float waveTimer;
    private int _currentRound = 1; // Variable to keep track of the current round

    // Reference to TextMeshPro text for displaying the current round
    public TextMeshProUGUI roundText;

    // Flag to track if a wave is currently active
    private bool _canSpawnWave = false;

    private void Awake()
    {
        // Find references to PlayerScore and Spawner scripts
        playerScore = FindObjectOfType<PlayerScore>();
        spawner = FindObjectOfType<Spawner>();
    }

    void Start()
    {
        // Initialize wave timer and update the round text
        waveTimer = timeBetweenWaves;
        UpdateRoundText();
    }

    void Update()
    {
        // Decrement the wave timer
        waveTimer -= Time.deltaTime;

        // Check if a wave is ready to be spawned
        if (_canSpawnWave)
        {
            _canSpawnWave = false; // Reset the wave as not active
            waveTimer = timeBetweenWaves; // Reset the timer for the next wave
        }
        else
        {
            // Check if the wave timer has reached zero
            if (waveTimer <= 0)
            {
                spawner.Spawn(); // Trigger enemy spawning
                _canSpawnWave = true; // Set the wave as active
                playerScore.StartRound(); // Start a new round in the player score
                _currentRound++; // Increment the current round
                UpdateRoundText(); // Update the round display
            }
        }
    }

    // Update the TextMeshPro text to display the current round
    private void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = "Round: " + _currentRound;
        }
    }
}
