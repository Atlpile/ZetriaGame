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
        StartCoroutine(IE_LoadingEffect());

        if (ShowCallBack != null)
            SetTransitionEffect(E_UITransitionType.Fade, true, ShowCallBack);
    }

    public override void Hide(TweenCallback RemoveCallBack = null)
    {
        StopCoroutine(IE_LoadingEffect());

        if (RemoveCallBack != null)
            SetTransitionEffect(E_UITransitionType.Fade, false, RemoveCallBack);
    }

    public void LoadingToTarget(UnityAction LoadAction)
    {
        StartCoroutine(IE_LoadingToTarget(LoadAction));
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

    private IEnumerator IE_LoadingToTarget(UnityAction LoadAction)
    {
        yield return new WaitForSeconds(waitTime);
        LoadAction?.Invoke();
    }

}
