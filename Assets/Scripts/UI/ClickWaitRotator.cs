using UnityEngine;
using DG.Tweening;

/// <summary>
/// アタッチされたオブジェクトをアクティブな間、1秒に1回z軸で360度回転させる
/// </summary>
public class ClickWaitRotator : MonoBehaviour
{
    private Tween rotateTween;

    private void OnEnable()
    {
        // 毎秒1回転（360度）をループ
        rotateTween = transform
            .DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuint)
            .SetLoops(-1, LoopType.Restart)
            .OnStepComplete(() => transform.localEulerAngles = Vector3.zero);
    }

    private void OnDisable()
    {
        if (rotateTween != null)
        {
            rotateTween.Kill();
            rotateTween = null;
        }
        // 角度をリセット
        transform.localEulerAngles = Vector3.zero;
    }
}
