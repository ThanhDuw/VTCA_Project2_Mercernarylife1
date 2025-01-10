using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Text questText; // Tham chiếu đến Text UI
    public QuestManager questNPC; // Tham chiếu đến NPC giao nhiệm vụ

    private bool isQuestVisible = false; // Trạng thái hiển thị nhiệm vụ


    public AudioSource backgroundMusic;
    public AudioSource npcVoice;
    private void Start()
    {
        if (questNPC != null)
        {
            // Đăng ký sự kiện từ NPC
            questNPC.QuestActivatedEvent += ShowQuestUI;
            questNPC.QuestCompletedEvent += ShowQuestCompletionMessage;
        }
        // Ẩn Text UI ban đầu
        questText.gameObject.SetActive(false);
    }
    private void ShowQuestCompletionMessage()
    {
        questText.text = "Nhiệm vụ đã hoàn thành! Quay lại NPC để nhận thưởng.";
    }
    private void ShowQuestUI()
    {
        isQuestVisible = true;
        questText.gameObject.SetActive(true); // Hiển thị Text UI
    }
    
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
        if (isQuestVisible && questNPC != null)
        {
            questText.text = $"Nhiệm vụ: Tiêu diệt {questNPC.currentKills}/{questNPC.requiredKills} quái vật";
        }
    }
    
    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        if (isQuestVisible && questNPC != null)
        {
            questText.text = $"Nhiệm vụ: Tiêu diệt {questNPC.currentKills}/{questNPC.requiredKills} quái vật";
        }

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
