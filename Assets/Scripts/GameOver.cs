using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện này để tải lại Scene

public class GameOver : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;        // Panel chứa các nút điều khiển Game Over    
    public Button quitButton;       // Nút thoát game
    public Button replayButton;     // Nút chơi lại (restart)

    private bool isPanelActive = false; // Trạng thái hiển thị Panel

    void Start()
    {
        // Đảm bảo Panel ban đầu bị ẩn
        if (panel != null)
            panel.SetActive(false);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (replayButton != null)
            replayButton.onClick.AddListener(ReplayGame); // Gán sự kiện cho nút chơi lại
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Thoát game khi build ứng dụng
#endif
    }

    // Hàm chơi lại
    public void ReplayGame()
    {
        Debug.Log("Chơi lại trò chơi!");

        // Ẩn panel Game Over
        if (panel != null)
            panel.SetActive(false);

        // Tải lại Scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Khôi phục thời gian
        Time.timeScale = 1;
    }
}