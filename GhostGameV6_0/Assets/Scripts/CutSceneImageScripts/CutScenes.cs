using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenes : MonoBehaviour
{
    public Canvas CutSceneCanvas;
    
    public Image storyImagesCutScenes;
    public Image Background;

    public GameObject STextObj;

    public TMP_Text SText; // ✅ FIX: TMP_Text ist die Basis-Klasse für beide Typen

    public string[] storyTextsCutScenes;

    

    private int index;


    void Start()
    {
        // ✅ FIX: Null-Check und hole TMP_Text Komponente
        if (STextObj != null)
        {
            // Versuche zuerst TextMeshProUGUI (für UI Canvas)
            SText = STextObj.GetComponent<TextMeshProUGUI>();
            
            // Falls das nicht funktioniert, versuche TextMeshPro (für 3D World)
            if (SText == null)
            {
                SText = STextObj.GetComponent<TextMeshPro>();
            }
            
            if (SText == null)
            {
                Debug.LogError($"❌ SText konnte nicht zugewiesen werden! STextObj hat keine TextMeshPro oder TextMeshProUGUI Komponente.");
            }
            else
            {
                Debug.Log($"✅ SText erfolgreich zugewiesen: {SText.GetType().Name}");
            }
        }
        else
        {
            Debug.LogError("❌ STextObj ist null! Bitte im Inspector zuweisen.");
        }

        storyTextsCutScenes = new string[5];
        storyTextsCutScenes[0] = "Platzhalter Text für Cutscene 1";
        storyTextsCutScenes[1] = "Text";
        storyTextsCutScenes[2] = "text";
        storyTextsCutScenes[3] = "platz";
        storyTextsCutScenes[4] = "halter";

    }

    void Update()
    {
        // ✅ FIX: Null-Check vor Verwendung
        if (SText != null && storyTextsCutScenes != null && index < storyTextsCutScenes.Length)
        {
            SText.text = storyTextsCutScenes[index];
        } 

      
    }

    public void NextStoryText()
    {
        if (index <= storyTextsCutScenes.Length)
        {
            index++;
        }
         if(index >= storyTextsCutScenes.Length)
        {
            
            gameObject.SetActive(false);
            CutSceneCanvas.enabled = false;
        }
    }
}
