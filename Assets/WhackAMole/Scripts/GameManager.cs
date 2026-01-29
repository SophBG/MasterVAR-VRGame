using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private List<Mole> moles;

    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameStartClip; 
    [SerializeField] private AudioClip gameOverClip;   
    private AudioSource audioSource;

    // Hardcoded variables
    [Header("Time variables")]
    [SerializeField] private float startingTime = 30f;
    [SerializeField] private float startingDelay = 2f;

    // Global variables
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    // This is public so the play button can see it.
    public void StartGame() {
        // Hide/show the UI elements we don't/do want to see.
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);
        // Hide all the visible moles.
        for (int i = 0; i < moles.Count; i++) {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        // Remove any old game state.
        currentMoles.Clear();
        // Start with 30 seconds.
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        
        if (gameStartClip != null) audioSource.PlayOneShot(gameStartClip);
        StartCoroutine(StartGameDelay());
    }

    public void GameOver(int type) {
        // Show the message.
        if (type == 0) {
            outOfTimeText.SetActive(true);
        } else {
            bombText.SetActive(true);
        }
        if (gameOverClip != null) audioSource.PlayOneShot(gameOverClip);

        // Hide all moles.
        foreach (Mole mole in moles) {
            mole.StopGame();
        }
        // Stop the game and show the start UI.
        playing = false;
        playButton.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (playing) {
            // Update time.
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0) {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            
            // Check if we need to start any more moles.
            if (currentMoles.Count <= (score / 10)) {
                // Choose a random mole.
                int index = Random.Range(0, moles.Count);
                // Doesn't matter if it's already doing something, we'll just try again next frame.
                if (!currentMoles.Contains(moles[index])) {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }
    }

    public void AddScore(int moleIndex) {
        // Add and update score.
        score += 1;
        scoreText.text = $"{score}";
        // Increase time by a little bit.
        timeRemaining += 1;
        
        // Note: The mole is not removed from currentMoles here.
        // It remains "occupied" until the death animation finishes.
    }

    public void Missed(int moleIndex, bool isMole) {
        if (isMole) {
            // Decrease time by a little bit.
            timeRemaining -= 2;
        }
        // Remove from active moles immediately.
        currentMoles.Remove(moles[moleIndex]);
    }

    // Called by Mole script after death animation finishes
    public void ReleaseMole(int moleIndex) {
        if (currentMoles.Contains(moles[moleIndex])) {
            currentMoles.Remove(moles[moleIndex]);
        }
    }

    private System.Collections.IEnumerator StartGameDelay() {
        yield return new WaitForSeconds(startingDelay);
        playing = true;
    }
}