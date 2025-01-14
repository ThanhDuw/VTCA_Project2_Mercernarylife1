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
              
        List<GameObject> dontDestroyObjects = GetDontDestroyOnLoadObjects();

        // Xóa các object trong "DontDestroyOnLoad"
        foreach (GameObject obj in dontDestroyObjects)
        {
            Destroy(obj);
        }
        // Chuyển về Main Menu hoặc thoát ứng dụng
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

    // Hàm chơi lại
    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }
}