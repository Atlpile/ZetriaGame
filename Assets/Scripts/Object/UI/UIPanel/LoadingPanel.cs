using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    private Text text_Loading;

    private readonly float loadingRate = 0.5f;
    private readonly float waitTime = 2.5f;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Text>();

        text_Loading = GetUIComponent<Text>(nameof(text_Loading));
    }

    public override void Show(TweenCallback ShowCallBack)
    {
        base.Show(ShowCallBack);
        StartCoroutine(IE_LoadingEffect());

        //BUG:未知canvasGroup为何自动变false
        canvasGroup.enabled = true;
    }

    public override void Hide(TweenCallback HideCallBack)
    {
        base.Hide(HideCallBack);
        StopCoroutine(IE_LoadingEffect());
    }


    public void WaitComplete(UnityAction LoadCompleteAction)
    {
        StartCoroutine(IE_WaitComplete(LoadCompleteAction));
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

    private IEnumerator IE_WaitComplete(UnityAction LoadCompleteAction)
    {
        yield return new WaitForSeconds(waitTime);
        LoadCompleteAction?.Invoke();
    }

}
