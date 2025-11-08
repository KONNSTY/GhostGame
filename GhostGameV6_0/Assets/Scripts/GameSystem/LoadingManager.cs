using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class LoadingManager : MonoBehaviour
{

    public string SceneName = "World";

    void Start()
    {
        gameObject.SetActive(false);

        if (SceneName != "World")
        SceneName = "World";
    }
    void Awake()
    {
      

    SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName)
        {
            // Zerstöre das GameObject
            Destroy(gameObject);
            
            // Optional: Kündige das Abonnement, um Speicherlecks zu vermeiden
            SceneManager.sceneLoaded -= OnSceneLoaded; 
        }
    }
}
