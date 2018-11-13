using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Word[] words;
    public Word[] wordsMedium;
    public Word[] wordsHard;
    private static List<Word> unansweredWords;
    private static int answerCount;
    private static string difficulty;

    private Word currentWord;

    [SerializeField]
    private Text wordText;

    [SerializeField]
    private float timeBetweenWords = 1f;
    [SerializeField]
    private float timeBeforeWords = 1.5f;
    [SerializeField]
    private Animator animator;

    Timer timer;

    [SerializeField]
    private Text teamText;
    [SerializeField]
    private Text winText;

    public GameObject teamScreen;
    public GameObject gameScreen;
    public GameObject endScreen;

    private static bool firstRound = true;
    private static bool canPress = true;

    private static string currentTeam;
    private static Team team1;
    private static Team team2;

    private static int score1;
    private static int score2;


    private void Start()
    {
        timer = GetComponent<Timer>();
        team1 = GetComponent<Team>();
        team2 = GetComponent<Team>();
        canPress = true;

        VerifyDificulty();

        if (unansweredWords == null || unansweredWords.Count == 0)
        {
            unansweredWords = words.ToList<Word>();
        }

        SetCurrentWord();
        if (firstRound)
        {
            InitializeTeam();
        }
    }

    void SetCurrentWord()
    {
        int randomWordIndex = Random.Range(0, unansweredWords.Count);
        currentWord = unansweredWords[randomWordIndex];

        StartCoroutine(TransitionCurrentWord());
    }

    IEnumerator StartRound()
    {
        gameScreen.SetActive(false);
        teamScreen.SetActive(true);
        teamText.text = "Agora é a vez do time " + currentTeam;
        yield return new WaitForSeconds(1.5f);
        firstRound = false;
        teamScreen.SetActive(false);
        gameScreen.SetActive(true);

        
    }

    IEnumerator TransitionCurrentWord()
    {
        wordText.text = "Sou "+ currentWord.category;
        yield return new WaitForSeconds(timeBeforeWords);
        animator.SetTrigger("before");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("after");
        wordText.text = currentWord.word;
    }

    IEnumerator TransitionToNextWord()
    {
        unansweredWords.Remove(currentWord);
        yield return new WaitForSeconds(timeBetweenWords);
        timer.resetTime();
        answerCount++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void InitializeTeam()
    {
        if (currentTeam == null)
        {
            currentTeam = "1";
        }
        StartCoroutine(StartRound());
        
    }

    void ChangeTeam()
    {
        if(currentTeam == "1")
        {
            score1 = team1.ScoreResult();
            team1.ResetScore();
            Debug.Log("Score 1: " + score1);

            currentTeam = (currentTeam == "1") ? "2" : "1";
            Debug.Log("Current team: " + currentTeam);
            answerCount = 0;
            firstRound = true;
            StartCoroutine(StartRound());
        }
        else
        {
            score2 = team2.ScoreResult();
            team2.ResetScore();
            Debug.Log("Score 2: " + score2);
            GameOver();
            
        }
        
    }

    void ChangeDificulty()
    {
        unansweredWords.Clear();
        if(difficulty == "")
        {
            unansweredWords = words.ToList<Word>();
        }
        else if(difficulty == "Medium")
        {
            unansweredWords = wordsMedium.ToList<Word>();
        }
        else if(difficulty == "Hard")
        {
            unansweredWords = wordsHard.ToList<Word>();
        }
    }

    void VerifyDificulty()
    {
        if(answerCount < 2)
        {
            difficulty = "";
        }
        if (answerCount == 2)
        {
            difficulty = "Medium";
            ChangeDificulty();
        }
        else if(answerCount == 4)
        {
            difficulty = "Hard";
            ChangeDificulty();

        }
        else if(answerCount > 4)
        {
            ChangeTeam();
            ChangeDificulty();
            
        }
    }

    void GameOver()
    {
        gameScreen.SetActive(false);
        teamScreen.SetActive(false);
        endScreen.SetActive(true);

        if(score1 > score2)
        {
            winText.text = "O vencedor é o time " + team1;
        }
        else if(score1 < score2)
        {
            winText.text = "O vencedor é o time " + team2;
        }
        else
        {
            winText.text = "EMPATOU!";
        }
        
    }

    public void UserAnswers()
    {
        if (canPress)
        {
            if (currentTeam == "1")
            {
                team1.AddScore(1);
            }
            else
            {
                team2.AddScore(1);
            }
            StartCoroutine(TransitionToNextWord());
        }
      
        canPress = false;
        
    }

    void UserTimesOut()
    {
        if(timer.CalculatedTime() >= 1)
        {
            StartCoroutine(TransitionToNextWord());
        }
    }

    void UserSkips()
    {

    }

    private void Update()
    {
        UserTimesOut();
    }
}

