using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string sceneToLoad; // Tên scene bạn muốn teleport đến
    public Transform playerSpawnPoint; // Vị trí spawn cho người chơi

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Chuyển cảnh
            SceneManager.LoadScene(sceneToLoad);
            // Đặt vị trí cho người chơi
            other.transform.position = playerSpawnPoint.position;
        }
    }
}
