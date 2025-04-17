using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// テキスト表示＆テキスト送りを管理するクラス
/// </summary>
public class DialogueWindow : MonoBehaviour
{
    [Header("ノワのテキストUI")]
    [SerializeField] private TextMeshProUGUI nowaText;

    [Header("コッペのテキストUI")]
    [SerializeField] private TextMeshProUGUI koppeText;

    [Header("テキスト表示速度（1文字あたり秒数）")]
    [SerializeField, Tooltip("1文字あたり何秒かけて表示するか（例: 0.05）")]
    private float charInterval = 0.05f;

    private DG.Tweening.Tween nowaTween;
    private DG.Tweening.Tween koppeTween;

    /// <summary>
    /// ノワのセリフを一文字ずつ表示
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void ShowNowa(string text, System.Action onComplete = null)
    {
        if (nowaText != null)
        {
            if (nowaTween != null) nowaTween.Kill();
            nowaText.text = "";
            float duration = charInterval * text.Length;
            nowaTween = nowaText.DOText(text, duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
    }

    /// <summary>
    /// コッペのセリフを一文字ずつ表示
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void ShowKoppe(string text, System.Action onComplete = null)
    {
        if (koppeText != null)
        {
            if (koppeTween != null) koppeTween.Kill();
            koppeText.text = "";
            float duration = charInterval * text.Length;
            koppeTween = koppeText.DOText(text, duration).SetEase(DG.Tweening.Ease.Linear).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
    }
}
