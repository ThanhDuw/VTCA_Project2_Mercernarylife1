using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleToBossRoom : MonoBehaviour
{

    public Vector3 playerSpawnPosition; // Vị trí spawn cho người chơi
    public Animator transition;
    public float transitionTime = 1f;
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Làm player trong suốt
            SpriteRenderer playerRenderer = other.GetComponent<SpriteRenderer>();
            if (playerRenderer != null)
            {
                Color color = playerRenderer.color;
                color.a = 0f; // Đặt alpha thành 0 (trong suốt)
                playerRenderer.color = color;
            }
            PlayerPrefs.SetFloat("SpawnX", playerSpawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", playerSpawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", playerSpawnPosition.z);
            DontDestroyOnLoad(other.gameObject);

            StartCoroutine(LoadSceneWithTransition(1f, SceneManager.GetActiveScene().buildIndex + 1, other));

        }

    }
    private IEnumerator LoadSceneWithTransition(float waitTime, int levelIndex, Collider2D other)
    {
        yield return new WaitForSeconds(waitTime);
        // Bật animation chuyển cảnh
        transition.SetTrigger("Start");


        // Đợi animation hoàn tất
        yield return new WaitForSeconds(transitionTime);

        // Load scene mới
        SceneManager.LoadScene(levelIndex);
        SpriteRenderer playerRenderer = other.GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            Color color = playerRenderer.color;
            color.a = 1f;
            playerRenderer.color = color;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPosition = new Vector3(
                PlayerPrefs.GetFloat("SpawnX", playerSpawnPosition.x), // Nếu không có giá trị, lấy giá trị mặc định
                PlayerPrefs.GetFloat("SpawnY", playerSpawnPosition.y),
                PlayerPrefs.GetFloat("SpawnZ", playerSpawnPosition.z)
            );

            player.transform.position = spawnPosition;

        }

    }

}
