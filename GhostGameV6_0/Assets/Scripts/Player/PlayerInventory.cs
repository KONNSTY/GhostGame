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

                            // ✅ KORRIGIERT: Ntype basierend auf itemNumber setzen
                            switch (itemId.itemNumber)
                            {
                                case 1: Ntype = NoteInt.Note1; break;
                                case 2: Ntype = NoteInt.Note2; break;
                                case 3: Ntype = NoteInt.Note3; break;
                                case 4: Ntype = NoteInt.Note4; break;
                                case 5: Ntype = NoteInt.Note5; break;
                                case 6: Ntype = NoteInt.Note6; break;
                                case 7: Ntype = NoteInt.Note7; break;
                                default: Ntype = NoteInt.Note1; break;
                            }

                            Debug.Log($"🎯 Note Type gesetzt auf: {Ntype}");

                            Destroy(other.gameObject);

                            // Jetzt wird der richtige Case ausgeführt
                            switch (Ntype)
                            {
                                case NoteInt.Note1:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note1;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note1)");
                                    break;
                                case NoteInt.Note2:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note2;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note2)");
                                    break;
                                case NoteInt.Note3:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note3;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note3)");
                                    break;
                                case NoteInt.Note4:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note4;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note4)");
                                    break;
                                case NoteInt.Note5:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note5;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note5)");
                                    break;
                                case NoteInt.Note6:
                                    noteSkript.NoteUiArray[0].SetActive(true);
                                    noteSkript.NoteUiArray[1].SetActive(true);
                                    noteSkript.NoteUiArray[2].SetActive(true);
                                    noteSkript.type = NoteSkript.NoteType.Note6;
                                    Time.timeScale = 0f;
                                    Debug.Log("✅ Time.timeScale auf 0 gesetzt (Note6)");
                                    break;
                                case NoteInt.Note7:
                                    Debug.Log("No Skript Assigned");
                                    break;
                            }
                        }
                    }
                }
            
        }
    }

    

            
            
    


