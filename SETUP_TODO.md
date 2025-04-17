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
