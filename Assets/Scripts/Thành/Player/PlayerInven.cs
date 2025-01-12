using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    public List<string> inventory = new List<string>(); // Danh sách tên các item

    // Hàm thêm item vào inventory
    public void AddItem(string itemName)
    {
        inventory.Add(itemName);
        Debug.Log($"Đã thêm {itemName} vào túi đồ. Tổng số item: {inventory.Count}");
    }
}
