using System;
using UnityEngine;

namespace DefaultNamespace
{
    public struct AiMoveBase : IAiMove
    {
        public uint PosX { get; }
        public uint PosY { get; }
        
        private Action _aiMoveAction;
        private string _comment;

        public AiMoveBase(uint posX, uint posY, Action aiMoveAction, string comment = "")
        {
            PosX = posX;
            PosY = posY;

            _aiMoveAction = aiMoveAction;
            _comment = comment;
        }

        public void DoMove()
        {
            _aiMoveAction();
            if (_comment != "")
            {
                Debug.Log($"MarkAiMove at [{PosX}, {PosY}]: {_comment}");
            }
        }
    }
}