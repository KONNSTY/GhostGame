using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public string[] inventoryItems;
    public int inventoryCount = 6;

    public List<string> inventoryListKey = new List<string>();
    public List<string> inventoryListNote = new List<string>();

    public NoteSkript noteSkript;
    public GameObject NoteCanvas;

    private enum NoteInt
    {
        Note1,
        Note2,
        Note3,
        Note4,
        Note5,
        Note6,
        Note7
    }
    NoteInt Ntype = NoteInt.Note1;


    public void AddItemKey(string item, int id)
    {
        inventoryListKey.Add(item + " " + id);
    }

    public void AddItemNote(string item, int id)
    {
        inventoryListNote.Add(item + " " + id);
    }

    public void RemoveKey(string item, int id)
    {
        inventoryListKey.Remove(item + " " + id);
    }



    void Update()
    {
        // Debug-Controls für Inventory-Anzeige
        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintInventory();
        }
    }

    void Start()
    {
        // Start-Initialisierung (falls nötig)
    }

    static NoteInt NoteTypeFor(int itemNumber)
    {
        switch (itemNumber)
        {
            case 1: return NoteInt.Note1;
            case 2: return NoteInt.Note2;
            case 3: return NoteInt.Note3;
            case 4: return NoteInt.Note4;
            case 5: return NoteInt.Note5;
            case 6: return NoteInt.Note6;
            case 7: return NoteInt.Note7;
            default: return NoteInt.Note1;
        }
    }

    void ShowCollectedNote(NoteInt noteType)
    {
        if (noteSkript == null || noteSkript.NoteUiArray == null)
            return;

        int slots = noteSkript.NoteUiArray.Length < 3 ? noteSkript.NoteUiArray.Length : 3;
        for (int i = 0; i < slots; i++)
        {
            if (noteSkript.NoteUiArray[i] != null)
                noteSkript.NoteUiArray[i].SetActive(true);
        }

        noteSkript.type = (NoteSkript.NoteType)noteType;
        Time.timeScale = 0f;
    }

    // Debug-Methode zum Anzeigen des Inventorys
    public void PrintInventory()
    {
        Debug.Log("=== INVENTORY ===");

        Debug.Log($"Schlüssel ({inventoryListKey.Count}):");
        foreach (string key in inventoryListKey)
        {
            Debug.Log($"  - {key}");
        }

        Debug.Log($"Notizen ({inventoryListNote.Count}):");
        foreach (string note in inventoryListNote)
        {
            Debug.Log($"  - {note}");
        }

        Debug.Log("==================");
    }

    void OnTriggerStay(Collider other)
    {
       
      
        
            
                if (other.CompareTag("Key"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ItemId itemId = other.GetComponent<ItemId>();
                        if (itemId != null)
                        {
                            AddItemKey(itemId.itemName, itemId.itemNumber);
                            Debug.Log($"Schlüssel aufgesammelt: {itemId.itemName} #{itemId.itemNumber}");
                            Destroy(other.gameObject);
                        }
                    }
                }
                else if (other.CompareTag("Note"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ItemId itemId = other.GetComponent<ItemId>();
                        if (itemId != null)
                        {
                            AddItemNote(itemId.itemName, itemId.itemNumber);
                            Debug.Log($"Notiz aufgesammelt: {itemId.itemName} #{itemId.itemNumber}");

                            Ntype = NoteTypeFor(itemId.itemNumber);
                            Destroy(other.gameObject);
                            if (Ntype != NoteInt.Note7)
                                ShowCollectedNote(Ntype);
                            else
                                Debug.Log("No Skript Assigned");
                        }
                    }
                }
            
        }
    }

    

            
            
    


