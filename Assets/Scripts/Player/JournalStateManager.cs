using System.Collections.Generic;
using Unity.InferenceEngine.Tokenization.Truncators;
using UnityEngine;

public class JournalStateManager : MonoBehaviour
{
    public static JournalStateManager Instance;
    public Material leftMaterial;
    public Material rightMaterial;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateMaterial(Material left, Material right)
    {
        leftMaterial = left;
        rightMaterial = right;
    }

    
}