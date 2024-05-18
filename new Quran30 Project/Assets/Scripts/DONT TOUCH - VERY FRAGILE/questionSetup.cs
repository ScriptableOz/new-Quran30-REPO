using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class questionSetup : MonoBehaviour
{
    public GameObject devPanel;

    public GameObject answerPanel; // Reference to the answer panel
    private string questionFilePath;

    [SerializeField] public List<questionData> questions;
    private questionData currentQuestion;
    [SerializeField] private GameObject questionPhotoPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI questionText2;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private RawImage questionImageRaw;
    [SerializeField] private answerButton[] answerButtons;
    [SerializeField] private int correctAnswerChoice;

    [SerializeField] private TextMeshProUGUI counterText;

    public int counter;

    private int totalCount;

    private answerButton answerButton;

    public void Awake()
    {
        if (DevBtn.devMode != false)
        {
            devPanel.SetActive(true);
        }

        GetQuestionAsset();
        counter = 0;
        totalCount = questions.Count;
    }

    public void Start()
    {
        SelectNewQuestion();
        SetQuestionValues();
        SetAnswerValues();
    }

    void GetQuestionAsset()
    {
        questionFilePath = "EnglishQuestion/" + PlayerPrefs.GetString("Question Set");
        questions = new List<questionData>(Resources.LoadAll<questionData>(questionFilePath));
    }

    public void SelectNewQuestion()
    {
        int randomQuestionIndex = Random.Range(0, questions.Count);

        currentQuestion = questions[randomQuestionIndex];

        questions.RemoveAt(randomQuestionIndex);

        counter++;
        counterText.text = counter + "/" + totalCount;

        // Activate all children of the answer panel
        ActivateAnswerPanelChildren();
    }

    void SetQuestionValues()
    {
        codeText.text = "Question Code: " + currentQuestion.questionCode;
        questionText.text = currentQuestion.question;
        questionText2.text = currentQuestion.question;

        // Load and set the question image
        Texture2D imageTexture = currentQuestion.image;
        if (imageTexture != null)
        {
            questionImageRaw.texture = imageTexture;
            questionPhotoPanel.SetActive(true);
        }
        else
        {
            questionPhotoPanel.SetActive(false);
            questionImageRaw.texture = null;
        }
    }

    private void SetAnswerValues()
    {
        // Randomize the answer button order
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));
        // Set up the answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Create a temporary boolean to pass to the buttons
            bool isCorrect = false;
            // If it is the correct answer, set the bool to true
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
            }
            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
        }
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerChosen = false;
        List<string> newList = new List<string>();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Get a random number of the remaining choices
            int random = Random.Range(0, originalList.Count);
            // If the random number is 0, this is the correct answer, MAKE SURE THIS IS ONLY USED ONCE
            if (random == 0 && !correctAnswerChosen)
            {
                correctAnswerChoice = i;
                correctAnswerChosen = true;
            }
            // Add this to the new list
            newList.Add(originalList[random]);
            //Remove this choice from the original list (it has been used)
            originalList.RemoveAt(random);
        }
        return newList;
    }

    private void ActivateAnswerPanelChildren()
    {
        // Activate all children of the answer panel
        foreach (Transform child in answerPanel.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
