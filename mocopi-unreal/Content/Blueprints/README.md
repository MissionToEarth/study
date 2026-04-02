# Blueprint セットアップ手順

## 作成が必要な Blueprint

### 1. BP_MocopiCharacter
- 親クラス: `MocopiGameCharacter` (C++)
- Skeletal Mesh: お好みの Humanoid アバター (Metahuman 推奨)
- Animation Blueprint: ABP_MocopiCharacter を設定

### 2. ABP_MocopiCharacter (Animation Blueprint)
```
AnimGraph:
  [LiveLinkPose] → [Retarget Pose] → [Output Pose]
  
LiveLinkPose ノード設定:
  - Subject Name: "mocopi"  (mocopi アプリの送信名と一致させる)
  - Role: Live Link Animation Role
```

### 3. BP_MocopiGameMode
- 親クラス: `MocopiGameMode`
- Default Pawn Class: BP_MocopiCharacter

## Live Link 接続手順

1. UE エディタで `Window > Virtual Production > Live Link` を開く
2. `+ Source` → `MocopiLiveLink Source` を追加
3. Port: `12351` (デフォルト)
4. mocopi スマートフォンアプリで `設定 > データ送信 > 送信先IP` を
   PC の IP アドレスに設定
5. mocopi アプリでキャリブレーション実行
6. Live Link ウィンドウにサブジェクト `mocopi` が表示されれば接続完了

## Metahuman での使用

1. Quixel Bridge から Metahuman をダウンロード
2. BP_MocopiCharacter の Skeletal Mesh を Metahuman の Body に変更
3. ABP_MocopiCharacter の LiveLinkPose で
   `Retarget Asset` に `MocopiToMetahuman_Retarget` を設定
   (mocopi プラグインに同梱)
