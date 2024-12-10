using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    public int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    public Quest quest;

    public GameObject interactButton;

    public AudioSource backgroundMusic;
    public AudioSource npcVoice;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E) && playerIsClose)
        {
            if(dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                interactButton.SetActive(false);
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }
    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
            if (npcVoice != null && !npcVoice.isPlaying)
            {
                npcVoice.Play(); // Phát âm thanh khi gõ ký tự
            }         
        }
        npcVoice.Pause();
    }
    public void NextLine()
    {
        contButton.SetActive(false);

        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText() ;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(true);
            playerIsClose = true;
            if(backgroundMusic!= null)
            {
                backgroundMusic.volume = 0.1f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(false);
            playerIsClose = false;
            zeroText();
            npcVoice.Pause();
            if (backgroundMusic != null)
            {
                backgroundMusic.volume = 1f;
            }
        }
    }
}
