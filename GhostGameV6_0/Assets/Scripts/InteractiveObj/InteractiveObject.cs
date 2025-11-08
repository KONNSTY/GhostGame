
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public enum InteractiveType
    {
        StoryNote,
    }
    public InteractiveObject state;



    private void OnTriggerEnter(Collider other)
    {
        DestroyButReturnState();
        Destroy(gameObject);
    }
        

public InteractiveObject DestroyButReturnState()
    {
       
    
    return state;

    }
    }

