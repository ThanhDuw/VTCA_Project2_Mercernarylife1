using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    public delegate void ExpChangeHandler(int amount);
    public event ExpChangeHandler OnExpChange;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }       
    }
    public void AddExperience(int amount)
    {
        OnExpChange?.Invoke(amount);
    }
}
