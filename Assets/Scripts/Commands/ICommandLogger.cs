namespace CyberEvolution.Commands
{
    public interface ICommandLogger
    {
        void Log(CommandBase command, string message = "");
    }
}