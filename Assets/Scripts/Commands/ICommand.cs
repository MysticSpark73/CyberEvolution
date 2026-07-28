using System;

namespace CyberEvolution.Commands
{
    public interface ICommand
    {
        void Execute(Action<float> callback = null);
    }
}