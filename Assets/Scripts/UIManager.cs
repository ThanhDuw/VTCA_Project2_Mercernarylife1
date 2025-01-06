using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject volumePanel; // Panel âm lượng
    private bool isPanelActive = false; // Trạng thái của Panel

    private void Start()
    {
        // Ẩn Panel ban đầu
        if (volumePanel != null)
        {
            volumePanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Kiểm tra phím ESC để đóng Panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleVolumePanel(false); // Tắt Panel khi nhấn ESC
        }
    }

    // Hàm bật/tắt Panel
    public void ToggleVolumePanel(bool state)
    {
        if (volumePanel != null)
        {
            isPanelActive = state;
            volumePanel.SetActive(state);
        }
    }

    // Hàm Toggle khi bấm nút Setting (bật hoặc tắt)
    public void OnSettingButtonClicked()
    {
        ToggleVolumePanel(!isPanelActive);
    }
}
