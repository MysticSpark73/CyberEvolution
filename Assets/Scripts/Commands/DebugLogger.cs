using System.Collections.Generic;
using UnityEngine;

namespace CyberEvolution.Commands
{
    public class DebugLogger : ICommandLogger
    {
        private Stack<CommandBase> _stack = new Stack<CommandBase>();
        public void Log(CommandBase command, string message)
        {
            // Debug.Log($"[DebugLogger] {message}");
        }
    }
}