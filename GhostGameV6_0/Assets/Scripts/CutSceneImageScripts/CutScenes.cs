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

        storyTextsCutScenes = new string[27];
        storyTextsCutScenes[0] = "Eight years ago, Jill's sister disappeared under mysterious circumstances.";
        storyTextsCutScenes[1] = "Back then, the two were on vacation in a small lakeside town somewhere in Europe.";
        storyTextsCutScenes[2] = "While they played in the woods, it happened:";
        storyTextsCutScenes[3] = "Jill looked away for just a moment—when she turned back,";
        storyTextsCutScenes[4] = "her sister had vanished without a trace.";
        storyTextsCutScenes[5] = "As Jill searched for her, she wandered deeper into the forest.";
        storyTextsCutScenes[6] = "From that moment on,";
        storyTextsCutScenes[7] = "she remembers only the voice of her sister calling from within the darkness of the woods:";
        storyTextsCutScenes[8] = "\"Where are you? It's so dark… help me.\"";
        storyTextsCutScenes[9] = "One day Jill wakes up drenched in sweat in her apartment.";
        storyTextsCutScenes[10] = "She had a strange dream about her little sister calling out over and over:";
        storyTextsCutScenes[11] = "\"Where are you? Where are you? Where were you?\"";
        storyTextsCutScenes[12] = "After this dream, Jill can no longer think clearly.";
        storyTextsCutScenes[13] = "When she meets her best friend—whom she has known since childhood and who also knew her sister—";
        storyTextsCutScenes[14] = "she tells her about the dream. Jill asks:";
        storyTextsCutScenes[15] = "\"Do you… do you think she's still alive? Do you think she's still in that forest, waiting for me?\"";
        storyTextsCutScenes[16] = "Suddenly, Jill sees a vision of her sister standing right in front of her.";
        storyTextsCutScenes[17] = "When she blinks, the image is gone. Lina—Jill's best friend—says:";
        storyTextsCutScenes[18] = "\"She's dead. She's gone, and she isn't coming back. I know it was hard, especially after your parents divorced over it and you ended up living with mine.\"";
        storyTextsCutScenes[19] = "But Jill becomes convinced that her sister is still alive. She says:";
        storyTextsCutScenes[20] = "\"We have to go there. We have to save her.\"";
        storyTextsCutScenes[21] = "Lina: \"Save her? Jill, what are you talking about? She's been missing for eight years. Why would she still be alive?\"";
        storyTextsCutScenes[22] = "Jill: \"I can't explain it, but sometimes I can see her.";
        storyTextsCutScenes[23] = "I know you always say I'm imagining it, but today… I don't think it was a dream. It felt like a cry for help. I have to save her.\"";
        storyTextsCutScenes[24] = "Lina sighs: \"Okay… if you insist, I'll go with you. But she won't be there.\"";
        storyTextsCutScenes[25] = "Jill: \"Maybe she won't be. But what if we never check?\"";
        storyTextsCutScenes[26] = "";

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
