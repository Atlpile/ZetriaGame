using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    private Text text_Loading;

    private float loadingRate = 0.5f;
    private float waitTime = 2.5f;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Text>();

        text_Loading = GetUIComponent<Text>("text_Loading");
    }

    public override void Show(TweenCallback ShowCallBack = null)
    {
        base.Show(ShowCallBack);

        StartCoroutine(IE_LoadingEffect());
    }

    public override void Hide(TweenCallback HideCallBack = null)
    {
        base.Hide(HideCallBack);

        StopCoroutine(IE_LoadingEffect());
    }

    public void LoadingToTarget(UnityAction LoadCompleteAction)
    {
        StartCoroutine(IE_LoadingToTarget(LoadCompleteAction));
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

    private IEnumerator IE_LoadingToTarget(UnityAction LoadCompleteAction)
    {
        yield return new WaitForSeconds(waitTime);
        LoadCompleteAction?.Invoke();
    }

}
