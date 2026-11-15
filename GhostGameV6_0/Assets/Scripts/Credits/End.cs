using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class End : MonoBehaviour
{
    public Canvas Loading;

    public Transform endObj;

    public GameObject ImageBackground;

    public Animator ImageAnimator;

    bool endingStarted;

    void Start()
    {
        if (Loading == null)
        {
            GameObject found = GameObject.Find("Loading");
            if (found != null)
                Loading = found.GetComponent<Canvas>();
        }

        if (ImageBackground == null && Loading != null && Loading.transform.childCount > 0)
            ImageBackground = Loading.transform.GetChild(0).gameObject;

        if (ImageAnimator == null && ImageBackground != null)
            ImageAnimator = ImageBackground.GetComponent<Animator>();
    }

    void Update()
    {
        if (endingStarted || endObj == null)
            return;

        float distance = Vector3.Distance(transform.position, endObj.position);
        if (distance < 10f)
        {
            endingStarted = true;
            StartCoroutine(WaitForDarkness(3f));
        }
    }

    IEnumerator WaitForDarkness(float waitTime)
    {
        if (ImageAnimator != null)
            ImageAnimator.SetBool("isFading", true);
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
