using UnityEngine;
using System.Collections.Generic;
using System;

namespace Checker
{
    public class PieceController : MonoBehaviour
    {
        [Header("Board")]
        public ChessPiece[,] board;
        public float tileSize = 10f;
        public Transform boardOrigin;

        [Header("Highlighting")]
        public GameObject highlightPrefab;
        public GameObject[,] tileHighlights = new GameObject[8, 8];

        [Header("State")]
        public ChessPiece selectedPiece;

        [Header("Particles")]
        [SerializeField] private ParticleSystem moveParticlePrefab;

        public event Action OnPieceMoved;

        private void Start()
        {
            CreateHighlights();
        }

        private void CreateHighlights()
        {
            for (int x = 0; x < 8; x++)
                for (int z = 0; z < 8; z++)
                {
                    Vector3 pos = boardOrigin.position + new Vector3(x * tileSize, 0.05f, z * tileSize);
                    GameObject h = Instantiate(highlightPrefab, pos, Quaternion.identity, boardOrigin);
                    h.SetActive(false);
                    tileHighlights[x, z] = h;
                }
        }

        public void TrySelectPiece(ChessPiece piece)
        {
            if (piece == null) return;

            GameManager gm = FindObjectOfType<GameManager>();
            if (!gm.CanSelectPiece(piece)) return;

            if (selectedPiece == null)
            {
                selectedPiece = piece;
                UpdateHighlights();
                return;
            }

            if (selectedPiece == piece) return;

            if (selectedPiece.isWhite == piece.isWhite)
            {
                selectedPiece = piece;
                UpdateHighlights();
                return;
            }
        }

        public void UpdateHighlights()
        {
            ClearHighlights();
            if (selectedPiece == null) return;

            List<Vector2Int> moves = GetLegalMoves(selectedPiece);

            foreach (var move in moves)
            {
                GameObject h = tileHighlights[move.x, move.y];
                h.SetActive(true);
                Renderer r = h.GetComponent<Renderer>();
                r.material.color = board[move.x, move.y] == null ? Color.green : Color.yellow;
            }
        }

        public void ClearHighlights()
        {
            for (int x = 0; x < 8; x++)
                for (int z = 0; z < 8; z++)
                    tileHighlights[x, z].SetActive(false);
        }

        public void MoveSelectedPiece(int x, int z)
        {
            if (selectedPiece == null) return;

            Vector3 previousPos = boardOrigin.position + new Vector3(selectedPiece.boardX * tileSize, 0.1f, selectedPiece.boardZ * tileSize);

         
            if (moveParticlePrefab != null)
            {
                ParticleSystem ps = Instantiate(moveParticlePrefab, previousPos, Quaternion.identity);
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }

            if (board[x, z] != null)
                Destroy(board[x, z].gameObject);

            board[selectedPiece.boardX, selectedPiece.boardZ] = null;

            selectedPiece.boardX = x;
            selectedPiece.boardZ = z;
            selectedPiece.transform.position = boardOrigin.position + new Vector3(x * tileSize, 0.1f, z * tileSize);

            board[x, z] = selectedPiece;
            selectedPiece = null;
            ClearHighlights();

            OnPieceMoved?.Invoke();
        }

       
        private List<Vector2Int> GetLegalMoves(ChessPiece piece)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            switch (piece.type)
            {
                case PieceType.Pawn: AddPawnMoves(piece, moves); break;
                case PieceType.Rook:
                    AddLineMoves(piece, moves, Vector2Int.up);
                    AddLineMoves(piece, moves, Vector2Int.down);
                    AddLineMoves(piece, moves, Vector2Int.left);
                    AddLineMoves(piece, moves, Vector2Int.right);
                    break;
                case PieceType.Bishop:
                    AddLineMoves(piece, moves, Vector2Int.up + Vector2Int.right);
                    AddLineMoves(piece, moves, Vector2Int.up + Vector2Int.left);
                    AddLineMoves(piece, moves, Vector2Int.down + Vector2Int.right);
                    AddLineMoves(piece, moves, Vector2Int.down + Vector2Int.left);
                    break;
                case PieceType.Queen:
                    for (int dx = -1; dx <= 1; dx++)
                        for (int dz = -1; dz <= 1; dz++)
                            if (dx != 0 || dz != 0) AddLineMoves(piece, moves, new Vector2Int(dx, dz));
                    break;
                case PieceType.Knight: AddKnightMoves(piece, moves); break;
                case PieceType.King: AddKingMoves(piece, moves); break;
            }
            return moves;
        }

        private void AddLineMoves(ChessPiece piece, List<Vector2Int> moves, Vector2Int dir)
        {
            int x = piece.boardX;
            int z = piece.boardZ;
            while (true)
            {
                x += dir.x; z += dir.y;
                if (x < 0 || x >= 8 || z < 0 || z >= 8) break;
                if (board[x, z] == null) moves.Add(new Vector2Int(x, z));
                else { if (board[x, z].isWhite != piece.isWhite) moves.Add(new Vector2Int(x, z)); break; }
            }
        }

        private void AddKnightMoves(ChessPiece piece, List<Vector2Int> moves)
        {
            Vector2Int[] offsets = { new(2,1), new(2,-1), new(-2,1), new(-2,-1), new(1,2), new(1,-2), new(-1,2), new(-1,-2) };
            foreach (var o in offsets)
            {
                int x = piece.boardX + o.x;
                int z = piece.boardZ + o.y;
                if (x < 0 || x >= 8 || z < 0 || z >= 8) continue;
                if (board[x, z] == null || board[x, z].isWhite != piece.isWhite) moves.Add(new Vector2Int(x, z));
            }
        }

        private void AddKingMoves(ChessPiece piece, List<Vector2Int> moves)
        {
            for (int dx = -1; dx <= 1; dx++)
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dz == 0) continue;
                    int x = piece.boardX + dx;
                    int z = piece.boardZ + dz;
                    if (x < 0 || x >= 8 || z < 0 || z >= 8) continue;
                    if (board[x, z] == null || board[x, z].isWhite != piece.isWhite) moves.Add(new Vector2Int(x, z));
                }
        }

        private void AddPawnMoves(ChessPiece piece, List<Vector2Int> moves)
        {
            int dir = piece.isWhite ? 1 : -1;
            int startRow = piece.isWhite ? 1 : 6;
            int x = piece.boardX; int z = piece.boardZ;
            if (board[x, z + dir] == null)
            {
                moves.Add(new Vector2Int(x, z + dir));
                if (z == startRow && board[x, z + dir * 2] == null) moves.Add(new Vector2Int(x, z + dir * 2));
            }
            for (int dx = -1; dx <= 1; dx += 2)
            {
                int cx = x + dx; int cz = z + dir;
                if (cx < 0 || cx >= 8 || cz < 0 || cz >= 8) continue;
                if (board[cx, cz] != null && board[cx, cz].isWhite != piece.isWhite) moves.Add(new Vector2Int(cx, cz));
            }
        }
    }
}