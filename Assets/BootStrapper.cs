using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        // Load the first content scene additively
        SceneManager.LoadSceneAsync("Jail Cell", LoadSceneMode.Additive);
    }
}
