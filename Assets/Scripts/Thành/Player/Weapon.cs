using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public AudioSource _audioSource;
    [SerializeField] private AudioClip shootingClip;
    public float cooldownTime = 0.5f; 
    private float nextFireTime = 0f;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            animator.SetBool("IsShooting", true);
            nextFireTime = Time.time + cooldownTime;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("IsShooting", false);
        }
    }
    void Shoot()
    {
        _audioSource.PlayOneShot(shootingClip);
        Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
    }
}
