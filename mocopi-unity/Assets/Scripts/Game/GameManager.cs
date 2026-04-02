using UnityEngine;
using UnityEngine.UI;
using MocopiGame.Mocopi;
using TMPro;

namespace MocopiGame.Game
{
    /// <summary>
    /// ゲームのメインフロー管理。
    /// mocopi の接続状態に応じて UI を切り替えます。
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject connectingPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Button startButton;
        [SerializeField] private Button stopButton;

        [Header("参照")]
        [SerializeField] private MocopiManager mocopiManager;

        private void Start()
        {
            startButton?.onClick.AddListener(OnStartClicked);
            stopButton?.onClick.AddListener(OnStopClicked);

            mocopiManager ??= MocopiManager.Instance;
            if (mocopiManager != null)
            {
                mocopiManager.onConnected.AddListener(OnConnected);
                mocopiManager.onDisconnected.AddListener(OnDisconnected);
                mocopiManager.onPoseUpdated.AddListener(OnPoseUpdated);
            }

            ShowConnectingUI();
        }

        private void OnStartClicked()
        {
            mocopiManager?.StartReceiving();
            SetStatus("接続中...");
        }

        private void OnStopClicked()
        {
            mocopiManager?.StopReceiving();
        }

        private void OnConnected()
        {
            SetStatus("mocopi 接続済み");
            connectingPanel?.SetActive(false);
            gamePanel?.SetActive(true);
        }

        private void OnDisconnected()
        {
            SetStatus("未接続");
            ShowConnectingUI();
        }

        private void OnPoseUpdated(MocopiPoseData data)
        {
            // ポーズ更新時のゲームロジックをここに実装
            // 例: ジャンプ判定、ポーズ認識など
        }

        private void ShowConnectingUI()
        {
            connectingPanel?.SetActive(true);
            gamePanel?.SetActive(false);
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
            Debug.Log($"[GameManager] {message}");
        }

        private void OnDestroy()
        {
            if (mocopiManager != null)
            {
                mocopiManager.onConnected.RemoveListener(OnConnected);
                mocopiManager.onDisconnected.RemoveListener(OnDisconnected);
                mocopiManager.onPoseUpdated.RemoveListener(OnPoseUpdated);
            }
        }
    }
}
