using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Realtime;
using Unity.VisualScripting;

public class End : MonoBehaviour
{
    public Canvas Loading;

    public Transform endObj;

    public GameObject ImageBackground;

    public Animator ImageAnimator;


    void Start()
    {
        if (Loading == null)
        {
            Loading = GameObject.Find("Loading").GetComponent<Canvas>();
        }

        if (ImageBackground == null)
        {
            ImageBackground = Loading.transform.GetChild(0).gameObject;
        }

        if (ImageAnimator == null)
        {
            ImageAnimator = ImageBackground.GetComponent<Animator>();

        }




    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, endObj.position);

        Debug.Log("Distance to End: " + distance);

        if (distance < 10)
        {
            StartCoroutine(WaitForDarkness(3f));
        }
    }


    IEnumerator WaitForDarkness(float waitTime)
    {
        ImageAnimator.SetBool("isFading", true);
        yield return new WaitForSeconds(waitTime);
   SceneManager.LoadScene(0);
    }
}
