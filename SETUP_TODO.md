# Unityエディタ セットアップTODO

- [x] 「SampleScene」または「u1wScene」をUnityエディタで開く
- [x] Canvas配下に「TextMeshPro - Text（UI）」を2つ作成し、それぞれ「NowaText」「KoppeText」とリネームする
- [x] 空のGameObject（例: DialogueWindow）を作成し、DialogueWindowスクリプトをアタッチする
- [x] DialogueWindowのインスペクタで、NowaText/KoppeText欄に対応するTextMeshProUGUIをドラッグ＆ドロップする
- [x] 空のGameObject（例: EventController）を作成し、EventControllerスクリプトをアタッチする
- [x] EventControllerのインスペクタで、DialogueWindow欄に先ほど作成したDialogueWindowオブジェクトをドラッグ＆ドロップする
- [x] プレイモードでクリックまたはスペースキーを押して、ノワ・コッペのセリフが設計資料通りに進行することを確認する

※TextMeshProがインストールされていない場合は、Package Managerから「TextMeshPro」を導入してください。

- [x] DOTween ProのTextMeshPro拡張を有効化する  
  1. Unityメニューから「Tools > Demigiant > DOTween Utility Panel」を開く  
  2. 「Setup Modules」タブで「TextMeshPro Support」をONにする  
  3. 「Apply」ボタンを押してモジュールをインポートする  
  4. Assets/Plugins/Demigiant/DOTween/Modules/ に DOTweenModuleTMP.cs が追加されていることを確認する

※この作業を行うとTextMeshPro用のDOText拡張が使えるようになります。

---

## イベントデータをScriptableObjectで管理する手順

1. Projectウィンドウで右クリック →「Create」→「Game」→「Event Group」を選んで、イベントグループ用のアセットを作成しよう！
2. 作ったEventGroupアセットを選択して、Inspectorで「+」ボタンからセリフやイベント（EventData）をどんどん追加できるよ。
   - キャラ名やセリフ本文、タイプ（InsideGameDialogue/OutsideGameDialogueなど）もここで編集OK！
3. EventControllerオブジェクトのInspectorで「Event Groups」リストに、作成したEventGroupアセットをドラッグ＆ドロップ！
   - 複数グループも追加できるから、シーンごと・イベントごとに分けて管理もラクラク。
4. プレイモードで動作確認して、セリフがちゃんと表示されるかチェック！

※ScriptableObjectアセットを増やせば、セリフやイベントの追加・修正が超カンタンになるよ！
