using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Dark
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Dark UI/Layout/Layout Group Fix")]
    public class LayoutGroupFix : MonoBehaviour
    {
        [SerializeField] private bool fixOnEnable = true;
        [SerializeField] private bool fixWithDelay = true;
        const float fixDelay = 0.025f;

        void OnEnable()
        {
            if (!fixWithDelay && fixOnEnable) { LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); }
            else if (fixWithDelay) { StartCoroutine(FixDelay()); }
        }

        public void FixLayout()
        {
            if (!fixWithDelay) { LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); }
            else { StartCoroutine(FixDelay()); }
        }

        IEnumerator FixDelay()
        {
            yield return new WaitForSecondsRealtime(fixDelay);
            LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
        }
    }
}