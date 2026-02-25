using UnityEngine;

namespace Checker
{
    public class SpawnPieceOnBoard : MonoBehaviour
    {
        [Header("Board")]
        public Transform gridPos;
        public float tileSize = 10f;

        [Header("White Pieces")]
        public GameObject whitePawn;
        public GameObject whiteRook;
        public GameObject whiteKnight;
        public GameObject whiteBishop;
        public GameObject whiteQueen;
        public GameObject whiteKing;

        [Header("Black Pieces")]
        public GameObject blackPawn;
        public GameObject blackRook;
        public GameObject blackKnight;
        public GameObject blackBishop;
        public GameObject blackQueen;
        public GameObject blackKing;

        public ChessPiece[,] board = new ChessPiece[8, 8];

        private void Start()
        {
            SpawnAllPieces();

            
            PieceController pc = FindObjectOfType<PieceController>();
            PlayerController player = FindObjectOfType<PlayerController>();
            if (pc != null)
            {
                pc.board = board;
            }
        }

        private void SpawnAllPieces()
        {
            SpawnPawns();
            SpawnBackRank(true);
            SpawnBackRank(false);
        }

        private void SpawnPawns()
        {
            for (int x = 0; x < 8; x++)
            {
                SpawnPiece(whitePawn, x, 1, true, PieceType.Pawn);
                SpawnPiece(blackPawn, x, 6, false, PieceType.Pawn);
            }
        }

        private void SpawnBackRank(bool isWhite)
        {
            int z = isWhite ? 0 : 7;

            SpawnPiece(isWhite ? whiteRook : blackRook, 0, z, isWhite, PieceType.Rook);
            SpawnPiece(isWhite ? whiteKnight : blackKnight, 1, z, isWhite, PieceType.Knight);
            SpawnPiece(isWhite ? whiteBishop : blackBishop, 2, z, isWhite, PieceType.Bishop);
            SpawnPiece(isWhite ? whiteQueen : blackQueen, 3, z, isWhite, PieceType.Queen);
            SpawnPiece(isWhite ? whiteKing : blackKing, 4, z, isWhite, PieceType.King);
            SpawnPiece(isWhite ? whiteBishop : blackBishop, 5, z, isWhite, PieceType.Bishop);
            SpawnPiece(isWhite ? whiteKnight : blackKnight, 6, z, isWhite, PieceType.Knight);
            SpawnPiece(isWhite ? whiteRook : blackRook, 7, z, isWhite, PieceType.Rook);
        }

        private void SpawnPiece(GameObject prefab, int x, int z, bool isWhite, PieceType type)
        {
            Vector3 worldPos = gridPos.position + new Vector3(x * tileSize, 0.1f, z * tileSize);
            GameObject pieceObj = Instantiate(prefab, worldPos, Quaternion.identity, gridPos);

            ChessPiece piece = pieceObj.GetComponent<ChessPiece>();
            if (piece == null)
                piece = pieceObj.AddComponent<ChessPiece>();

            piece.isWhite = isWhite;
            piece.type = type;
            piece.boardX = x;
            piece.boardZ = z;

            board[x, z] = piece;
        }
    }
}
