using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    //public string sceneToLoad; // Tên scene bạn muốn teleport đến
    public Vector3 playerSpawnPosition; // Vị trí spawn cho người chơi

    public Animator transition;
    public float transitionTime = 1f;


    private void Start()
    {
        if (PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY") && PlayerPrefs.HasKey("SpawnZ"))
        {
            // Đặt vị trí spawn cho người chơi
            Vector3 spawnPosition = new Vector3(
                PlayerPrefs.GetFloat("SpawnX"),
                PlayerPrefs.GetFloat("SpawnY"),
                PlayerPrefs.GetFloat("SpawnZ")
            );

            // Tìm đối tượng người chơi và đặt vị trí
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPosition;
            }

            // Xóa dữ liệu spawn sau khi sử dụng
            PlayerPrefs.DeleteKey("SpawnX");
            PlayerPrefs.DeleteKey("SpawnY");
            PlayerPrefs.DeleteKey("SpawnZ");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Đặt vị trí cho người chơi
            PlayerPrefs.SetFloat("SpawnX", playerSpawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", playerSpawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", playerSpawnPosition.z);

            StartCoroutine(LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex + 1));           
        }

    }
    private IEnumerator LoadSceneWithTransition(int levelIndex)
    {
        // Bật animation chuyển cảnh
        transition.SetTrigger("Start");
        

        // Đợi animation hoàn tất
        yield return new WaitForSeconds(transitionTime);

        // Load scene mới
        SceneManager.LoadScene(levelIndex);
    }

}
