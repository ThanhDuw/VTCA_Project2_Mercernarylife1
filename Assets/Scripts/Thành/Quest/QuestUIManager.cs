using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public Text questText; // Tham chiếu đến Text UI
    public QuestManager questNPC; // Tham chiếu đến NPC giao nhiệm vụ

    private bool isQuestVisible = false; // Trạng thái hiển thị nhiệm vụ

    private void Start()
    {
        if (questNPC != null)
        {
            // Đăng ký sự kiện từ NPC
            questNPC.QuestActivatedEvent += ShowQuestUI;
        }

        // Ẩn Text UI ban đầu
        questText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isQuestVisible && questNPC != null)
        {
            questText.text = $"Nhiệm vụ: Tiêu diệt {questNPC.currentKills}/{questNPC.requiredKills} quái vật";
        }
    }

    private void ShowQuestUI()
    {
        isQuestVisible = true;
        questText.gameObject.SetActive(true); // Hiển thị Text UI
    }
}
