using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private Queue<string> sentences = new Queue<string>();

    private bool isTyping = false;
    private string currentSentence;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(NPCDialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.npcName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentences(currentSentence));
    }

    IEnumerator TypeSentences(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.015f);
        }

        isTyping = false;
    }

    public void OnNextButtonPressed()
    {
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
