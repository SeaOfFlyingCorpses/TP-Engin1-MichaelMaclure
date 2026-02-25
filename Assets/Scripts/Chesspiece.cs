using UnityEngine;

namespace Checker
{
    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public class ChessPiece : MonoBehaviour
    {
        public PieceType type;
        public bool isWhite;

        [HideInInspector] public int boardX;
        [HideInInspector] public int boardZ;
    }
}