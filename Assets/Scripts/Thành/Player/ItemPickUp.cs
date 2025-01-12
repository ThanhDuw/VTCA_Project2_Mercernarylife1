using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public string itemName = "Key";
    public int amount = 1;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip pickUpClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Kiểm tra xem đối tượng chạm có phải là Player
        {
            _audioSource.PlayOneShot(pickUpClip);
            PickUp(collision.gameObject);  // Gọi hàm nhặt item
        }

    }

    void PickUp(GameObject player)
    {
       
        // Lấy PlayerInventory và thêm item vào túi đồ
        PlayerInven inventory = player.GetComponent<PlayerInven>();
        if (inventory != null)
        {
            inventory.AddItem(itemName);
        }

        Destroy(gameObject);
    }
    
}
