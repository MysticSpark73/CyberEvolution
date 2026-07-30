namespace CyberEvolution.Commands
{
    public enum CommandType : byte
    {
        DoNothing,
        
        TurnCW45,
        TurnCCW45,
        
        MoveForward,
        
        ConsumePlant,
        ConsumeMeat,
        
        AttackEnemy,
        AttackAll,
        
        UndefinedCommand = 255
    }
}