using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MocopiGame.Mocopi;

namespace MocopiGame.Game
{
    /// <summary>
    /// mocopi のポーズデータからゲーム用ジェスチャーを認識します。
    ///
    /// 実装済みの認識:
    ///   - ジャンプ (腰ボーンの垂直速度)
    ///   - 腕を上げる (両腕の角度)
    ///   - しゃがみ (腰の高さ)
    /// </summary>
    public class PoseRecognizer : MonoBehaviour
    {
        [Header("ジャンプ判定")]
        [SerializeField] private float jumpVelocityThreshold = 1.5f;

        [Header("腕上げ判定")]
        [SerializeField] private float armsUpAngleThreshold = 120f;

        [Header("しゃがみ判定")]
        [SerializeField] private float crouchHeightThreshold = 0.8f;

        [Header("イベント")]
        public UnityEvent onJump;
        public UnityEvent onArmsUp;
        public UnityEvent onCrouch;
        public UnityEvent onStand;

        // ボーンID 定数 (mocopi 27ボーン)
        private const int BONE_HIPS = 0;
        private const int BONE_LEFT_UPPER_ARM = 6;
        private const int BONE_RIGHT_UPPER_ARM = 9;

        private Vector3 _prevHipPosition;
        private float _prevHipY;
        private bool _isCrouching;
        private float _baseHipHeight = -1f;

        private void OnEnable()
        {
            if (MocopiManager.Instance != null)
                MocopiManager.Instance.onPoseUpdated.AddListener(Recognize);
        }

        private void OnDisable()
        {
            if (MocopiManager.Instance != null)
                MocopiManager.Instance.onPoseUpdated.RemoveListener(Recognize);
        }

        private void Recognize(MocopiPoseData poseData)
        {
            if (!poseData.Bones.TryGetValue(BONE_HIPS, out var hips)) return;

            // 基準身長を最初のフレームで設定
            if (_baseHipHeight < 0f)
            {
                _baseHipHeight = hips.Position.y;
                _prevHipY = hips.Position.y;
                return;
            }

            CheckJump(hips.Position);
            CheckCrouch(hips.Position);

            if (poseData.Bones.TryGetValue(BONE_LEFT_UPPER_ARM, out var leftArm) &&
                poseData.Bones.TryGetValue(BONE_RIGHT_UPPER_ARM, out var rightArm))
            {
                CheckArmsUp(leftArm.Rotation, rightArm.Rotation);
            }

            _prevHipY = hips.Position.y;
        }

        private void CheckJump(Vector3 hipPos)
        {
            float verticalVelocity = (hipPos.y - _prevHipY) / Time.deltaTime;
            if (verticalVelocity > jumpVelocityThreshold)
            {
                Debug.Log($"[PoseRecognizer] ジャンプ検出 velocity={verticalVelocity:F2}");
                onJump?.Invoke();
            }
        }

        private void CheckArmsUp(Quaternion leftArmRot, Quaternion rightArmRot)
        {
            // 腕の向きと上方向の角度を比較
            Vector3 leftArmDir = leftArmRot * Vector3.up;
            Vector3 rightArmDir = rightArmRot * Vector3.up;
            float leftAngle = Vector3.Angle(leftArmDir, Vector3.up);
            float rightAngle = Vector3.Angle(rightArmDir, Vector3.up);

            if (leftAngle < (180f - armsUpAngleThreshold) &&
                rightAngle < (180f - armsUpAngleThreshold))
            {
                onArmsUp?.Invoke();
            }
        }

        private void CheckCrouch(Vector3 hipPos)
        {
            float relativeHeight = hipPos.y / _baseHipHeight;
            bool shouldCrouch = relativeHeight < crouchHeightThreshold;

            if (shouldCrouch && !_isCrouching)
            {
                _isCrouching = true;
                Debug.Log("[PoseRecognizer] しゃがみ検出");
                onCrouch?.Invoke();
            }
            else if (!shouldCrouch && _isCrouching)
            {
                _isCrouching = false;
                onStand?.Invoke();
            }
        }
    }
}
