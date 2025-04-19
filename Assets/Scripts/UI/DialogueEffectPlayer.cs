using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 立ち絵や画面全体のフェードなど、演出系の処理をまとめるクラス
/// </summary>
public class DialogueEffectPlayer : MonoBehaviour
{
    [Header("内側ゲームの立ち絵")]
    [SerializeField] private Image insideStandingImage;

    [Header("外側ゲームの立ち絵")]
    [SerializeField] private Image outsideStandingImage;

    [Header("画面全体フェード用イメージ")]
    [SerializeField] private Image screenFadeImage;

    /// <summary>
    /// 立ち絵の表示・フェード
    /// </summary>
    public void PlayStandingEffect(Sprite standingImage, StandingFadeType fadeType, float fadeDuration)
    {
        if (insideStandingImage == null) return;

        // 画像が指定されている場合のみ差し替え
        if (standingImage != null)
        {
            insideStandingImage.sprite = standingImage;
            insideStandingImage.enabled = true;
        }

        if (fadeType == StandingFadeType.FadeIn)
        {
            insideStandingImage.color = new Color(1, 1, 1, 0);
            insideStandingImage.DOFade(1f, fadeDuration);
        }
        else if (fadeType == StandingFadeType.FadeOut)
        {
            insideStandingImage.DOFade(0f, fadeDuration);
        }
    }

    /// <summary>
    /// 画面全体のフェード
    /// </summary>
    public void PlayScreenFade(ScreenFadeType fadeType, Color color, float duration)
    {
        Debug.Log($"[DialogueEffectPlayer] PlayScreenFade: type={fadeType}, color={color}, duration={duration}, image={screenFadeImage}");
        if (screenFadeImage == null)
        {
            Debug.LogWarning("[DialogueEffectPlayer] screenFadeImageがInspectorでセットされていません！");
            return;
        }
        screenFadeImage.color = color;
        if (fadeType == ScreenFadeType.FadeIn)
        {
            screenFadeImage.gameObject.SetActive(true);
            screenFadeImage.color = new Color(color.r, color.g, color.b, 0);
            screenFadeImage.DOFade(1f, duration);
        }
        else if (fadeType == ScreenFadeType.FadeOut)
        {
            screenFadeImage.DOFade(0f, duration).OnComplete(() => screenFadeImage.gameObject.SetActive(false));
        }
    }
    /// <summary>
    /// 外側ゲームの立ち絵の表示
    /// </summary>
    public void PlayOutsideStandingEffect(Sprite standingImage)
    {
        if (outsideStandingImage == null) return;
        if (standingImage != null)
        {
            outsideStandingImage.sprite = standingImage;
            outsideStandingImage.enabled = true;
        }
    }
}
