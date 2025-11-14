using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenes : MonoBehaviour
{
    
    public Image storyImagesCutScenes;
    public Image Background;

    public TextMeshPro SText;

    public string[] storyTextsCutScenes;

    

    private int index;


    void Start()
    {
        
    }

    void Update()
    {
       SText.text = storyTextsCutScenes[index]; 
    }
}
