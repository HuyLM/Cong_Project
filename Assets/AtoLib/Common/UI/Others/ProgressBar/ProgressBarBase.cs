using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarBase : MonoBehaviour {
    [SerializeField] protected Image imgCurrentValueLerp;
    [SerializeField] protected Image imgCurrentValueReal;
    [SerializeField] protected RangeFloatValue updateSpeedSecondRange;

    protected float maxWidth;
    protected float distace;
    protected bool isComplete;
    bool isLoaded;
    protected Action onComplete;

    protected virtual void Start() {
        if (!isLoaded) {
            maxWidth = imgCurrentValueLerp.rectTransform.rect.width;
            isLoaded = true;
        }
    }

    public void FillBar(Image img, float fillAmount) {
        if (!isLoaded) {
            maxWidth = imgCurrentValueLerp.rectTransform.rect.width;
            isLoaded = true;
        }
        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillAmount * maxWidth);
        // Vector3 localScale = img.transform.localScale;
        //  img.transform.localScale = new Vector3(fillAmount, localScale.y, localScale.z);

    }
    public void ChangeMaxWidth(float modifier, bool reset = false) {
        maxWidth = reset ? modifier : maxWidth + modifier;
    }
    public void FillBar(float startPct, float endPct) {
        ForceFillBar(startPct);
        HandleBarChanged(endPct);
    }

    public virtual void HandleBarChanged(float pct) {
        if (!gameObject.activeInHierarchy) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ChangingBar(pct));
        FillBar(imgCurrentValueReal, pct);
    }

    protected virtual IEnumerator ChangingBar(float pct) {
        isComplete = false;
        WaitForSeconds deltaTime = new WaitForSeconds(UnityEngine.Time.fixedDeltaTime);
        yield return deltaTime;
        float preChange = imgCurrentValueLerp.rectTransform.rect.width / maxWidth;
        distace = Mathf.Abs(pct - preChange);
        float elapsed = 0f;
        float updateSpeedSecond = updateSpeedSecondRange.GetRatioValue(distace);
        while (elapsed < distace) {
            elapsed += updateSpeedSecond * UnityEngine.Time.fixedDeltaTime;
            float fillAmount = Mathf.Lerp(preChange, pct, elapsed / distace);
            FillBar(imgCurrentValueLerp, fillAmount);
            yield return deltaTime;
        }
        FillBar(imgCurrentValueLerp, pct);
        Completed();
    }

    protected virtual void Completed() {
        isComplete = true;
        if (onComplete != null) {
            Action onAction = onComplete;
            onComplete = null;
            onAction.Invoke();
        }
    }

    public void ForceFillBar(float pct) {
        FillBar(imgCurrentValueLerp, pct);
        FillBar(imgCurrentValueReal, pct);

    }

    public void AddOnComplete(Action onComplete) {
        this.onComplete = onComplete;
    }

    public void RemoveOnComplete() {
        this.onComplete = null;
    }
}
