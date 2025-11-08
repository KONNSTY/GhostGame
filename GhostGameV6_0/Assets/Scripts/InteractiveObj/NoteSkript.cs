using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class NoteSkript : MonoBehaviour
{
    public string[] notePages = new string[7]; // Array to hold the pages of the note

    public PlayerInventory playerInventoryREF;

    public TextMeshProUGUI NoteCanvasText; // ✅ FIX: TextMeshProUGUI für UI, nicht TextMeshPro

    public GameObject[] NoteUiArray = new GameObject[3];
    public enum NoteType
    {
        Note1,
        Note2,
        Note3,
        Note4,
        Note5,
        Note6,
        Note7
    }

    public NoteType type;

    void Start()
    {
        // ✅ FIX: Null-Checks hinzugefügt
        if (transform.childCount >= 3)
        {
            NoteUiArray[0] = transform.GetChild(0).gameObject;
            NoteUiArray[1] = transform.GetChild(1).gameObject;
            NoteUiArray[2] = transform.GetChild(2).gameObject;
        }
        else
        {
            Debug.LogError("❌ NoteSkript benötigt mindestens 3 Child GameObjects!");
        }

        if (NoteCanvasText == null && NoteUiArray[1] != null)
        {
            // ✅ FIX: Versuche sowohl TextMeshProUGUI als auch TextMeshPro zu finden
            NoteCanvasText = NoteUiArray[1].GetComponent<TextMeshProUGUI>();
            if (NoteCanvasText == null)
            {
                // Fallback für TextMeshPro (3D)
                TextMeshPro textMeshPro3D = NoteUiArray[1].GetComponent<TextMeshPro>();
                if (textMeshPro3D != null)
                {
                    Debug.LogWarning("⚠️ TextMeshPro (3D) gefunden, aber TextMeshProUGUI erwartet!");
                }
            }
        }

        // ✅ FIX: Null-Check vor SetActive
        for (int i = 0; i < NoteUiArray.Length; i++)
        {
            if (NoteUiArray[i] != null)
            {
                NoteUiArray[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        // ✅ FIX: Null-Check für NoteCanvasText vor Verwendung
        if (NoteCanvasText == null) return;

        switch (type)
        {
            case NoteType.Note1:
                notePages[0] = "____________NO Text______";
                NoteCanvasText.text = notePages[0];
                break;
            case NoteType.Note2:
                notePages[1] = "";
                NoteCanvasText.text = notePages[1];
                break;
            case NoteType.Note3:
                notePages[2] = "";
                NoteCanvasText.text = notePages[2];
                break;
            case NoteType.Note4:
                notePages[3] = "";
                NoteCanvasText.text = notePages[3];
                break;
            case NoteType.Note5:
                notePages[4] = "";
                NoteCanvasText.text = notePages[4];
                break;
            case NoteType.Note6:
                notePages[5] = "";
                NoteCanvasText.text = notePages[5];
                break;
            case NoteType.Note7:
                notePages[6] = "";
                NoteCanvasText.text = notePages[6];
                break; // ✅ FIX: Doppeltes Semikolon entfernt
        }
    }

    public void OnCloseButtonPushed()
    {
        Time.timeScale = 1f;
        
        // ✅ FIX: Null-Checks vor SetActive
        for (int i = 0; i < NoteUiArray.Length; i++)
        {
            if (NoteUiArray[i] != null)
            {
                NoteUiArray[i].SetActive(false);
            }
        }
    }
}