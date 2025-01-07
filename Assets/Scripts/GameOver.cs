using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện này để tải lại Scene

public class GameOver : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;        // Panel chứa 3 nút    
    public Button quitButton;       // Nút thoát game
    public Button replayButton;     // Nút chơi lại (đã thay thế nút tiếp tục)

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

    // Hàm thoát game
    public void QuitGame()
    {
        Debug.Log("Thoát game!");

        // Thoát ứng dụng khi build
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Hàm chơi lại
    public void ReplayGame()
    {
        Debug.Log("Chơi lại trò chơi!");

        // Ẩn Panel
        if (panel != null)
            panel.SetActive(false);

        // Tải lại Scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}