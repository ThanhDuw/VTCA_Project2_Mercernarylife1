using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class GameOver : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;        // Panel chứa các nút điều khiển Game Over    
    public Button quitButton;       // Nút thoát game

    private bool isPanelActive = false; // Trạng thái hiển thị Panel

    void Start()
    {
        // Đảm bảo Panel ban đầu bị ẩn
        if (panel != null)
            panel.SetActive(false);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

         // Gán sự kiện cho nút chơi lại
    }

    // Hàm hiển thị giao diện Game Over
    public void ShowGameOver()
    {
        if (panel != null)
        {
            panel.SetActive(true); // Hiển thị panel Game Over
            Time.timeScale = 0; // Dừng thời gian trong game
            Debug.Log("Hiển thị giao diện Game Over");
        }
        else
        {
            Debug.LogWarning("Panel Game Over chưa được gán trong Inspector!");
        }
    }

    // Hàm thoát game
    public void QuitGame()
    {
        Debug.Log("Thoát game!");
                    
        // Chuyển về Main Menu hoặc thoát ứng dụng
        SceneManager.LoadScene("MainMenu");
    }
    

    // Hàm chơi lại
    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }
}