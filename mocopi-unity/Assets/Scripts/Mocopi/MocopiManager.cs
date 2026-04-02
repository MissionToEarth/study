using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MocopiGame.Mocopi
{
    /// <summary>
    /// mocopi受信を管理するシングルトンマネージャー。
    /// Sony mocopi Receiver Plugin (com.sony.mocopi.receiver) が
    /// プロジェクトに追加されていることを前提とします。
    ///
    /// 導入手順:
    ///   1. https://github.com/sony/mocopi-receiver-plugin-unity から
    ///      unitypackage をダウンロード・インポート
    ///   2. このスクリプトの #if MOCOPI_SDK ブロックのコメントを解除
    ///   3. MocopiSimpleReceiver プレハブをシーンに配置
    /// </summary>
    public class MocopiManager : MonoBehaviour
    {
        public static MocopiManager Instance { get; private set; }

        [Header("受信設定")]
        [SerializeField] private int receivePort = 12351;
        [SerializeField] private bool autoStartReceiving = true;

        [Header("イベント")]
        public UnityEvent onConnected;
        public UnityEvent onDisconnected;
        public UnityEvent<MocopiPoseData> onPoseUpdated;

        public bool IsReceiving { get; private set; }

        // 受信した各ボーンのポーズデータ (ボーンID → Transform情報)
        private readonly Dictionary<int, BoneTransform> _boneData = new();

        // ========================================================
        // mocopi SDK が導入済みの場合は以下のコメントを解除してください
        // #if MOCOPI_SDK
        // private Sony.Mocopi.MocopiSimpleReceiver _receiver;
        // #endif
        // ========================================================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (autoStartReceiving)
                StartReceiving();
        }

        /// <summary>mocopi からのデータ受信を開始します。</summary>
        public void StartReceiving()
        {
            if (IsReceiving) return;

            // ---- SDK 導入後はここを置き換えてください ----
            // _receiver = FindObjectOfType<Sony.Mocopi.MocopiSimpleReceiver>();
            // if (_receiver == null)
            // {
            //     Debug.LogError("[MocopiManager] MocopiSimpleReceiver がシーンに見つかりません。");
            //     return;
            // }
            // _receiver.OnPoseReceived += HandlePoseReceived;
            // _receiver.StartReceive(receivePort);
            // -----------------------------------------------

            IsReceiving = true;
            Debug.Log($"[MocopiManager] 受信開始 port={receivePort} (SDK スタブモード)");
            onConnected?.Invoke();

#if UNITY_EDITOR
            // エディタ確認用のダミーデータを流す
            StartDummyPose();
#endif
        }

        /// <summary>受信を停止します。</summary>
        public void StopReceiving()
        {
            if (!IsReceiving) return;

            // _receiver?.StopReceive();
            IsReceiving = false;
            Debug.Log("[MocopiManager] 受信停止");
            onDisconnected?.Invoke();
        }

        /// <summary>
        /// SDK から呼ばれるコールバック。
        /// 実際の SDK では Sony.Mocopi.PoseEventArgs を受け取ります。
        /// </summary>
        private void HandlePoseReceived(/* Sony.Mocopi.PoseEventArgs args */)
        {
            // ---- SDK 導入後はここを実装してください ----
            // foreach (var bone in args.Bones)
            //     _boneData[bone.Id] = new BoneTransform(bone.Position, bone.Rotation);

            var poseData = new MocopiPoseData(_boneData);
            onPoseUpdated?.Invoke(poseData);
        }

        private void OnDestroy()
        {
            StopReceiving();
        }

#if UNITY_EDITOR
        private System.Collections.IEnumerator dummyCoroutine;
        private void StartDummyPose()
        {
            dummyCoroutine = DummyPoseLoop();
            StartCoroutine(dummyCoroutine);
        }

        private System.Collections.IEnumerator DummyPoseLoop()
        {
            // 27ボーン分のダミーデータを毎フレーム送出
            while (true)
            {
                float t = Time.time;
                for (int i = 0; i < 27; i++)
                {
                    _boneData[i] = new BoneTransform(
                        new Vector3(0, i * 0.1f, 0),
                        Quaternion.Euler(Mathf.Sin(t + i) * 10f, 0, 0)
                    );
                }
                HandlePoseReceived();
                yield return null;
            }
        }
#endif
    }

    // ----------------------------------------------------------------
    // データ構造
    // ----------------------------------------------------------------

    [Serializable]
    public struct BoneTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public BoneTransform(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }

    public class MocopiPoseData
    {
        public IReadOnlyDictionary<int, BoneTransform> Bones { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public MocopiPoseData(Dictionary<int, BoneTransform> bones)
        {
            Bones = bones;
        }
    }
}
