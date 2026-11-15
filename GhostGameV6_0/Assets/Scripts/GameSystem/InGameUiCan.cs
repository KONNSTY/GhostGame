using UnityEngine;
using UnityEngine.UI;

public class InGameUiCan : MonoBehaviour
{
    public Button pauseButton;
    public GameObject pauseMenu;

    public GameObject BigMapRawImage;

    
    public bool BigMapEnable = false;



    void Update()
    {
        if (BigMapRawImage == null)
            return;

        BigMapRawImage.SetActive(BigMapEnable);
    }

    public void ClickMapEnable()
    {
        BigMapEnable = !BigMapEnable;  // Toggle the BigMapEnable state
    }

    

}
