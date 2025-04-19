csvの保存先：Assets/Resources/StoryEvent.csv
項目：id, type, isNeedClick, content, arg1, arg2, arg3, arg4, arg5, arg6, memo

id：int、重複不可。idの数字の順にイベントが進行する
type：enum、ストーリーイベントの種類を示す。種類は以下の通り。

    insideSay 内側ゲームのセリフを表示
    outsideSay 外側ゲームのセリフを表示
    flag フラグを指定してtrueにする
    choice プレイヤーが選択可能な選択肢を表示
    screenfade スクリーンをフェードイン/アウトする
    insideCharaFade ゲーム内の立ち絵をフェードイン/アウトする
    setActive 特定のオブジェクトの表示非表示を切り替える
    ifgoto 特定のflagがtrueなら、指定したidにジャンプする

content：イベントの内容についての情報を記載

    insideSay：表示するセリフを記載。セリフの内容は、改行を含む場合は\ nで改行を表現する。
    outsideSay：表示するセリフを記載。セリフの内容は、改行を含む場合は\ nで改行を表現する。
    flag：フラグの名前を記載。ここで記載したフラグ名をもとに、管理するフラグを動的に生成する。FlagManagerを使って管理する。
    choice：今のところ不使用
    screenfade：対象にしたいスクリーンを記載。スクリーンのオブジェクトはScreenFaderを使って管理する。
    insideCharaFade：対象にしたいキャラの立ち絵のファイル名を指定。たとえば、chara1.pngなら「chara1」を指定。立ち絵のスプライトはSpriteManagerを使って管理する。
    setActive：Activeにしたいオブジェクト名を指定。オブジェクト名と対象オブジェクトの紐づけはActiveManagerを使って管理する。
    ifgoto：ジャンプ先のidを指定

arg1-6：イベントに必要な引数を記載。引数の数はイベントの種類によって異なるため、必要なものだけを記載する。

    insideSay：arg1に立ち絵のスプライトのファイル名を記載。たとえば、chara1.pngなら「chara1」を指定。立ち絵のスプライトはSpriteManagerを使って管理する。 arg2に話者名を指定。
    outsideSay：arg1に立ち絵のスプライトのファイル名を記載。たとえば、chara1.pngなら「chara1」を指定。立ち絵のスプライトはSpriteManagerを使って管理する。 
    flag：なし
    choice：arg1に選択肢の内容、arg2にarg1を選んだ時にジャンプするidを指定。arg3に選択肢の内容、arg4にarg3を選んだ時にジャンプするidを指定。arg5に選択肢の内容、arg6にarg5を選んだ時にジャンプするidを指定。
    screenfade：arg1に「in」または「out」を指定。arg2にフェードの時間を指定。
    insideCharaFade：arg1に「in」または「out」を指定。arg2にフェードの時間を指定。
    setActive：arg1にtrueなら表示、falseなら非表示
    ifgoto：arg1にフラグ名を指定

isNeedClick：bool、次のidに進むときにクリックが必要か、それとも処理が終わったらすぐに遷移するか。

memo：メモ。特に必要なければ空欄でOK。処理には使わない
