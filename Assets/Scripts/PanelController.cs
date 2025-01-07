using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;        // Panel chứa 3 nút
    public Button closeButton;      // Nút tắt Panel
    public Button quitButton;       // Nút thoát game
    public Button continueButton;   // Nút tiếp tục (chức năng tùy chỉnh)

    private bool isPanelActive = false; // Trạng thái hiển thị Panel

    void Start()
    {
        // Đảm bảo Panel ban đầu bị ẩn
        if (panel != null)
            panel.SetActive(false);

        // Gắn sự kiện cho các nút
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueAction);
    }

    void Update()
    {
        // Kiểm tra phím Esc để mở/tắt Panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    // Hàm mở/tắt Panel
    public void TogglePanel()
    {
        isPanelActive = !isPanelActive;

        if (panel != null)
        {
            panel.SetActive(isPanelActive);
            Time.timeScale = isPanelActive ? 0 : 1; // Dừng hoặc tiếp tục thời gian
        }
    }

    // Hàm tắt Panel
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            Time.timeScale = 1; // Khôi phục thời gian về bình thường
        }
        isPanelActive = false;
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

    // Hàm tiếp tục (chức năng có thể tuỳ chỉnh theo ý bạn)
    public void ContinueAction()
    {
        Debug.Log("Tiếp tục thực hiện hành động!");
        ClosePanel(); // Đóng Panel sau khi nhấn nút này
    }
}
