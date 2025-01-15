using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseScreen; // Màn hình đen khi pause
    public GameObject buttonPanel; // Panel chứa các nút

    public Button closeButton;      // Nút tắt Panel
    public Button quitButton;       // Nút thoát game
    public Button continueButton;   // Nút tiếp tục (chức năng tùy chỉnh)

    private bool isGamePaused = false; // Trạng thái tạm dừng game

    void Start()
    {
        // Đảm bảo các panel ban đầu bị ẩn
        if (pauseScreen != null)
            pauseScreen.SetActive(false);

        if (buttonPanel != null)
            buttonPanel.SetActive(false);

        // Gắn sự kiện cho các nút
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePauseMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueAction);
    }

    void Update()
    {
        // Kiểm tra phím Esc để mở/tắt Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // Hàm mở/tắt Pause Menu
    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;

        if (pauseScreen != null && buttonPanel != null)
        {
            pauseScreen.SetActive(isGamePaused); // Hiển thị màn hình đen
            buttonPanel.SetActive(isGamePaused); // Hiển thị panel nút
            Time.timeScale = isGamePaused ? 0 : 1; // Dừng hoặc tiếp tục thời gian
        }
    }

    // Hàm đóng Pause Menu
    public void ClosePauseMenu()
    {
        if (pauseScreen != null && buttonPanel != null)
        {
            pauseScreen.SetActive(false);
            buttonPanel.SetActive(false);
            Time.timeScale = 1; // Khôi phục thời gian
        }
        isGamePaused = false;
    }

    // Hàm thoát game
    public void QuitGame()
    {
        // Lấy danh sách các root object từ scene "DontDestroyOnLoad"
        List<GameObject> dontDestroyObjects = GetDontDestroyOnLoadObjects();

        // Xóa các object trong "DontDestroyOnLoad"
        foreach (GameObject obj in dontDestroyObjects)
        {
            Destroy(obj);
        }



        SceneManager.LoadScene("MainMenu");
    }
    private List<GameObject> GetDontDestroyOnLoadObjects()
    {
        // Tạo một scene tạm
        var tempScene = new UnityEngine.SceneManagement.Scene();
        tempScene = UnityEngine.SceneManagement.SceneManager.CreateScene("TempScene");

        // Chuyển tất cả object sang scene tạm, trừ các object trong "DontDestroyOnLoad"
        List<GameObject> dontDestroyObjects = new List<GameObject>();
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null || obj.scene.name != tempScene.name)
            {
                dontDestroyObjects.Add(obj);
            }
        }

        // Xóa scene tạm (không ảnh hưởng đến các object khác)
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(tempScene);
        return dontDestroyObjects;
    }

    // Hàm tiếp tục (chức năng có thể tuỳ chỉnh theo ý bạn)
    public void ContinueAction()
    {
        Debug.Log("Tiếp tục thực hiện hành động!");
        ClosePauseMenu(); // Đóng Pause Menu sau khi nhấn nút này
    }
}

