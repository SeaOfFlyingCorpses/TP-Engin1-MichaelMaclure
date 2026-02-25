using UnityEngine;
using System.Collections;

namespace Checker
{
    public enum PlayerTurn
    {
        White,
        Black
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        public PieceController pieceController;
        public PlayerController playerController;
        public Camera mainCamera;

        [Header("Game State")]
        public PlayerTurn currentTurn = PlayerTurn.White;
        public float turnSwitchDelay = 0.5f; 

        [Header("Player Positions")]
        public Vector3 whiteTurnPosition = new Vector3(35f, 0.5f, 5f);
        public Vector3 blackTurnPosition = new Vector3(35f, 0.5f, 64f);

        private bool isSwitchingTurn = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            if (pieceController != null)
                pieceController.OnPieceMoved += OnPieceMovedHandler;
        }

        private void OnDisable()
        {
            if (pieceController != null)
                pieceController.OnPieceMoved -= OnPieceMovedHandler;
        }

        private void Start()
        {
            ResetTurn();
        }

        private void OnPieceMovedHandler()
        {
            if (!isSwitchingTurn)
                StartCoroutine(SwitchTurnRoutine());
        }

        private IEnumerator SwitchTurnRoutine()
        {
            isSwitchingTurn = true;

            yield return new WaitForSeconds(turnSwitchDelay);

            currentTurn = currentTurn == PlayerTurn.White ? PlayerTurn.Black : PlayerTurn.White;

            pieceController.selectedPiece = null;
            pieceController.UpdateHighlights();

            if (playerController != null)
                playerController.transform.position =
                    currentTurn == PlayerTurn.White ? whiteTurnPosition : blackTurnPosition;

            if (mainCamera != null)
                mainCamera.RotateForTurn(currentTurn);

            Debug.Log($"Turn ended. Now it's {currentTurn} turn.");
            isSwitchingTurn = false;
        }

        public bool CanSelectPiece(ChessPiece piece)
        {
            if (piece == null) return false;

            return (currentTurn == PlayerTurn.White && piece.isWhite) ||
                   (currentTurn == PlayerTurn.Black && !piece.isWhite);
        }

        public void ResetTurn()
        {
            pieceController.selectedPiece = null;
            pieceController.UpdateHighlights();

            if (playerController != null)
                playerController.transform.position =
                    currentTurn == PlayerTurn.White ? whiteTurnPosition : blackTurnPosition;

            if (mainCamera != null)
                mainCamera.RotateForTurn(currentTurn);
        }
    }
}