using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ダイアログウィンドウのUI部品の表示/非表示を管理するクラス
/// </summary>
public class DialogueWindowPartsController : MonoBehaviour
{
    [Header("外ゲーム用ダイアログ本体のImage（背景や枠など）")]
    [SerializeField] private Image outsideDialogueImage;

    [Header("中ゲーム用ダイアログ本体のImage（背景や枠など）")]
    [SerializeField] private Image insideDialogueImage;

    [Header("外側ゲームの立ち絵")]
    [SerializeField] private Image outsideStandingImage;

    [Header("クリック待ち表示用オブジェクト（例：アイコン等）")]
    [SerializeField] private GameObject clickWaitIndicator;

    /// <summary>
    /// 外ゲーム用ダイアログImageを非表示にする
    /// </summary>
    public void HideOutsideDialogueImage()
    {
        if (outsideDialogueImage != null)
        {
            // 即時非表示（Activeは維持）
            outsideDialogueImage.gameObject.SetActive(true);
            var c = outsideDialogueImage.color;
            outsideDialogueImage.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    /// <summary>
    /// 外ゲーム用ダイアログImageを表示する
    /// </summary>
    public void ShowOutsideDialogueImage()
    {
        if (outsideDialogueImage != null)
        {
            outsideDialogueImage.gameObject.SetActive(true);
            outsideDialogueImage.DOFade(1f, 0.3f);
        }
    }

    /// <summary>
    /// 中ゲーム用ダイアログImageを非表示にする
    /// </summary>
    public void HideInsideDialogueImage()
    {
        if (insideDialogueImage != null)
        {
            // フェードアウトのみ（Activeは維持）
            insideDialogueImage.DOFade(0f, 0.3f);
        }
    }

    /// <summary>
    /// 中ゲーム用ダイアログImageを表示する
    /// </summary>
    public void ShowInsideDialogueImage()
    {
        if (insideDialogueImage != null)
        {
            insideDialogueImage.gameObject.SetActive(true);
            insideDialogueImage.DOFade(1f, 0.3f);
        }
    }

    /// <summary>
    /// 外側ゲームの立ち絵Imageを非表示にする
    /// </summary>
    public void HideOutsideStandingImage()
    {
        if (outsideStandingImage != null)
        {
            outsideStandingImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 外側ゲームの立ち絵Imageを表示する
    /// </summary>
    public void ShowOutsideStandingImage()
    {
        if (outsideStandingImage != null)
        {
            outsideStandingImage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// クリック待ち表示用オブジェクトを表示
    /// </summary>
    public void ShowClickWaitIndicator()
    {
        if (clickWaitIndicator != null)
        {
            clickWaitIndicator.SetActive(true);
        }
    }

    /// <summary>
    /// クリック待ち表示用オブジェクトを非表示
    /// </summary>
    public void HideClickWaitIndicator()
    {
        if (clickWaitIndicator != null)
        {
            clickWaitIndicator.SetActive(false);
        }
    }
}
