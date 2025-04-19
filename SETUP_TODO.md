# ストーリーイベントCSV化セットアップ手順

1. ストーリーイベントデータは「Assets/Resources/StoryEvent.csv」に記入してください。
   - カラム構成は「id, type, isNeedClick, content, arg1, arg2, arg3, arg4, arg5, arg6, memo」です
   - 詳細は「Assets/Resources/StoryEventCsvの読み方.md」を参照
   - 編集後は必ず「拡張子 .csv」で保存

2. StoryEvent.csvは「Resources」フォルダ直下で読み込まれます。
   - 「Assets/Resources/StoryEvent.csv」となるように配置してください
   - ファイル名は「StoryEvent.csv」（大文字小文字も一致させること）
   - 拡張子は必ず「.csv」（.CSVや.txtは不可）
   - フォルダがなければ作成してください
   - Unityエディタ上で「StoryEvent.csv」を右クリック→「Reimport」も推奨
   - .metaファイルが生成されているかも確認してください

3. Unityエディタでスクリプト参照エラーやcsv読み込みエラーが出る場合は、以下を試してください
   - 「Assets/Resources/StoryEvent.csv」を右クリック→「Reimport」
   - 「Assets/Scripts/Data/StoryEventCsvLoader.cs」を右クリック→「Reimport」
   - または「Assets」フォルダ全体を右クリック→「Reimport All」
   - それでも直らない場合はUnityエディタを再起動

4. 旧ScriptableObject（EventGroup, EventData）は不要です。今後はcsvで管理してください。

5. csvの記入ルール・各カラムの意味は「Assets/Resources/StoryEventCsvの読み方.md」を参照してください。
