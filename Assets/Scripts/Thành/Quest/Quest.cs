using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public string description; // Mô tả nhiệm vụ
    public int requiredKills;  // Số lượng quái vật cần tiêu diệt
    public int currentKills;   // Số lượng quái vật đã tiêu diệt
    public bool isActive;      // Trạng thái nhiệm vụ
    public bool isCompleted;   // Trạng thái hoàn thành nhiệm vụ

    public Quest(string description, int requiredKills)
    {
        this.description = description;
        this.requiredKills = 5;
        this.currentKills = 0;
        this.isActive = false;
        this.isCompleted = false;
    }

  
}
