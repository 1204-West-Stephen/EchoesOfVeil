using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Start()
    { 
        SceneManager.LoadSceneAsync("Jail Cell", LoadSceneMode.Additive);
    }
}
