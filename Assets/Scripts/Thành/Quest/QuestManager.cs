using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string questDescription = "Tiêu diệt 5 quái vật!"; // Mô tả nhiệm vụ
    public int requiredKills = 5; // Số lượng quái vật cần tiêu diệt
    public bool isQuestActive = false; // Trạng thái nhiệm vụ
    public int currentKills = 0; // Số quái vật đã tiêu diệt

    public delegate void OnQuestActivated(); // Tạo sự kiện
    public event OnQuestActivated QuestActivatedEvent;
    public delegate void Action();
    public event Action QuestCompletedEvent;
    public int xpReward = 500;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isQuestActive)
            {
                // Kích hoạt nhiệm vụ
                isQuestActive = true;
                Debug.Log($"NPC: {questDescription}");

                // Gọi sự kiện để UI hiển thị
                QuestActivatedEvent?.Invoke();
            }
            else if (currentKills < requiredKills)
            {
                Debug.Log($"NPC: Bạn chưa hoàn thành nhiệm vụ! Quái đã tiêu diệt: {currentKills}/{requiredKills}");
            }
            else if (currentKills == requiredKills)
            {
                Debug.Log("NPC: Chúc mừng! Bạn đã hoàn thành nhiệm vụ!");
                PlayerStats playerStats = FindObjectOfType<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.AddXP(xpReward);
                }
                CompleteQuest();
            }
        }
    }

    public void AddKill() // Gọi hàm này khi quái vật bị tiêu diệt
    {
        if (isQuestActive && currentKills < requiredKills)
        {
            currentKills++;
        }
    }
    private void CompleteQuest()
    {
        isQuestActive = false;
        currentKills = 0;
        QuestCompletedEvent?.Invoke();
    }
}
