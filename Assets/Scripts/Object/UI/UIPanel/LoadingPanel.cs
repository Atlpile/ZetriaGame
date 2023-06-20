using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    private Text text_Loading;

    [SerializeField] private float loadingRate = 0.5f;

    protected override void Awake()
    {
        GetChildrenAllUIComponent<Text>();

        text_Loading = GetUIComponent<Text>("text_Loading");
    }

    public override void HideSelf()
    {
        StopCoroutine(IE_LoadingEffect());
    }

    public override void ShowSelf()
    {
        StartCoroutine(IE_LoadingEffect());
    }

    private void SetLoadingText(string text)
    {
        text_Loading.text = text;
    }

    private IEnumerator IE_LoadingEffect()
    {
        while (true)
        {
            SetLoadingText("Loading");
            yield return new WaitForSeconds(loadingRate);
            SetLoadingText("Loading.");
            yield return new WaitForSeconds(loadingRate);
            SetLoadingText("Loading..");
            yield return new WaitForSeconds(loadingRate);
            SetLoadingText("Loading...");
            yield return new WaitForSeconds(loadingRate);
        }


    }
}
