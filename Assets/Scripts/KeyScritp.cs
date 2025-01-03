using UnityEngine;

public class KeyScritp : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab; // Prefab của chìa khóa

    // Hàm này được gọi khi Boss chết
    public void DropKey(Vector3 position)
    {
        if (keyPrefab != null)
        {
            // Lấy vị trí của Boss và thêm khoảng cách 1f trên trục Y
            Vector3 dropPosition = transform.position + Vector3.up * 2f;

            // Tạo chìa khóa tại vị trí đã tính toán
            Instantiate(keyPrefab, dropPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Key prefab chưa được gắn!");
        }
    }
}
