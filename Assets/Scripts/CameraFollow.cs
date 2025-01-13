using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
/*{
    private static CameraFollow instance;
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}*/
{
    public GameObject playerPrefab; // Prefab của player
    public CinemachineVirtualCamera virtualCamera; // Tham chiếu Virtual Camera

    private GameObject currentPlayer; // Lưu trữ player hiện tại

    // Gọi hàm này khi player chết
    public void RespawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer); // Hủy player cũ
        }

        // Tạo player mới từ prefab tại vị trí hiện tại của player cũ
        currentPlayer = Instantiate(playerPrefab, currentPlayer.transform.position, Quaternion.identity);

        // Cập nhật Virtual Camera theo dõi player mới
        virtualCamera.Follow = currentPlayer.transform;
        virtualCamera.LookAt = currentPlayer.transform;

        Debug.Log("Player mới đã được tạo và Virtual Camera đã được cập nhật!");
    }
}
