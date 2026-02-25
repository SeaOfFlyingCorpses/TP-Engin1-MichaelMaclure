using UnityEngine;
using System.Collections;

namespace Checker
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 12f;

        [Header("Dash")]
        public float dashDistance = 10f;
        public float dashDuration = 0.15f;

        [Header("References")]
        public PieceController pieceController;

        [Header("Animation")]
        public AnimationHandler animationHandler;

        private Vector2 moveInput;
        private Vector3 lastMoveDir = Vector3.forward;
        private bool isDashing;

        private InputController input;

        private void Awake()
        {
            input = FindObjectOfType<InputController>();
        }

        private void OnEnable()
        {
            input.OnMove += HandleMove;
            input.OnDash += HandleDash;
        }

        private void OnDisable()
        {
            input.OnMove -= HandleMove;
            input.OnDash -= HandleDash;
        }

        private void HandleMove(Vector2 input)
        {
            moveInput = input;

            if (input.sqrMagnitude > 0.01f)
            {
                lastMoveDir = new Vector3(input.x, 0, input.y).normalized;
            }
        }

        private void Update()
        {
            if (!isDashing)
            {
                Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

                // ✅ Reverse movement on Black turn
                if (GameManager.Instance != null &&
                    GameManager.Instance.currentTurn == PlayerTurn.Black)
                {
                    move = -move;
                }

                transform.position += move * (moveSpeed * Time.deltaTime);

                CheckMovePieceOnTile();
            }
        }

        private void HandleDash()
        {
            if (!isDashing && lastMoveDir.sqrMagnitude > 0.01f)
            {
                if (animationHandler != null)
                    animationHandler.PlayDash();

                StartCoroutine(DashRoutine());
            }
        }

        private IEnumerator DashRoutine()
        {
            isDashing = true;

            Vector3 dashDir = lastMoveDir;

            // ✅ Reverse dash direction on Black turn
            if (GameManager.Instance != null &&
                GameManager.Instance.currentTurn == PlayerTurn.Black)
            {
                dashDir = -dashDir;
            }

            Vector3 start = transform.position;
            Vector3 end = start + dashDir * dashDistance;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / dashDuration;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }

            transform.position = end;
            isDashing = false;

            pieceController.UpdateHighlights();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isDashing) return;

            ChessPiece piece = other.GetComponent<ChessPiece>();
            if (piece != null)
            {
                pieceController.TrySelectPiece(piece);
            }
        }

        public Vector2 GetMoveInput()
        {
            return moveInput;
        }

        private void CheckMovePieceOnTile()
        {
            if (pieceController == null || pieceController.selectedPiece == null) return;

            for (int x = 0; x < 8; x++)
            {
                for (int z = 0; z < 8; z++)
                {
                    GameObject highlight = pieceController.tileHighlights[x, z];
                    if (!highlight.activeSelf) continue;

                    float dist = Vector2.Distance(
                        new Vector2(transform.position.x, transform.position.z),
                        new Vector2(highlight.transform.position.x, highlight.transform.position.z)
                    );

                    if (dist <= pieceController.tileSize / 2f)
                    {
                        pieceController.MoveSelectedPiece(x, z);
                        return;
                    }
                }
            }
        }
    }
}