using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Start()
    {
        SceneManager.LoadScene(sceneName);
    }
}
