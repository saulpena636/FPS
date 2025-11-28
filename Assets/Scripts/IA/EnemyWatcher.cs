using UnityEngine;

using UnityEngine.SceneManagement;

public class EnemyWatcher : MonoBehaviour
{
    public string nextSceneName;

    void Update()
    {
        // Si no quedan enemigos dentro del parent...
        if (transform.childCount == 0)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
