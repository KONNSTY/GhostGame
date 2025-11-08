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
        if (BigMapEnable == true)
        {

            BigMapRawImage.SetActive(true); // Assuming you want to show the raw image when the map is enabled
        }
        else
        {

            BigMapRawImage.SetActive(false);
        }
    }

    public void ClickMapEnable()
    {
        BigMapEnable = !BigMapEnable;  // Toggle the BigMapEnable state
    }

    

}
