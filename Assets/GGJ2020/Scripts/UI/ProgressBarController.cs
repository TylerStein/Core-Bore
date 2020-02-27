using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public RectTransform progressBar;

    public void SetProgress(float progress) {
        progressBar.anchorMax = new Vector2(progress, progressBar.anchorMax.y);
    }
}
