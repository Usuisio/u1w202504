using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SetActiveがtrueになった際、親のImageの透明度を指定秒数かけて0（透明）にし、完了後に非表示にするコンポーネント。
/// </summary>
public class ParentImageFadeOut : MonoBehaviour
{
    /// <summary>
    /// フェードにかける秒数
    /// </summary>
    [SerializeField]
    private float duration = 2.0f;

    [SerializeField]
    private Image _parentImage;

    /// <summary>
    /// 有効化時にフェードアウト処理を開始
    /// </summary>
    private void OnEnable()
    {
        // duration秒で親のImageの透明度を0にし、完了後に非表示
        _parentImage.DOFade(0.0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
