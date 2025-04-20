/// <summary>
/// SetActiveがtrueになった際、親のImageの透明度を2秒かけて完全に不透明にするコンポーネント。
/// </summary>
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SetActive時に親のImageの透明度を2秒かけて1.0にするコンポーネント。
/// </summary>
public class ParentImageFadeIn : MonoBehaviour
{
    /// <summary>
    /// フェードにかける秒数
    /// </summary>
    [SerializeField]
    private float duration = 2.0f;

    [SerializeField]private Image _parentImage;
    private Coroutine _fadeCoroutine;

    /// <summary>
    /// 有効化時にフェードイン処理を開始
    /// </summary>
    private void OnEnable()
    {
        //duration秒で親のImageの透明度を0にする
        _parentImage.DOFade(1.0f, duration).SetEase(Ease.Linear);
    }
}
