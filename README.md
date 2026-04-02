# study
墓場：試験用

## mocopi アプリ開発ベース

Sony mocopi を使ったゲーム/アプリのベースプロジェクトです。  
Unity と Unreal Engine の両方に対応しています。

| フォルダ | エンジン | 概要 |
|---|---|---|
| `mocopi-unity/` | Unity 2022.3 LTS | mocopi Receiver Plugin + C# スクリプト |
| `mocopi-unreal/` | Unreal Engine 5.3 | mocopi Live Link Plugin + C++ |

### 共通の仕組み

- mocopi スマートフォンアプリが **UDP ポート 12351** でポーズデータを送信
- PC と mocopi 端末が **同一 LAN** にいる必要あり
- 27ボーンの BVH 形式で全身モーションを受信

### クイックスタート

詳細は各フォルダの README を参照してください：

- Unity: [`mocopi-unity/Assets/Scenes/MainScene.md`](mocopi-unity/Assets/Scenes/MainScene.md)
- Unreal Engine: [`mocopi-unreal/Content/Blueprints/README.md`](mocopi-unreal/Content/Blueprints/README.md)
