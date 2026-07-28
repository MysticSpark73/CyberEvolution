namespace CyberEvolution.Commands
{
    public enum CommandType : byte
    {
        DoNothing = 0,
        
        TurnCW45 = 1,
        TurnCW90 = 2,
        TurnCCW45 = 3,
        TurnCCW90 = 4,
        
        MoveForward = 5,
        MoveUp = 6,
        MoveRight = 7,
        MoveDown = 8,
        MoveLeft = 9,
        MoveUpRight = 10,
        MoveDownRight = 11,
        MoveDownLeft = 12,
        MoveUpLeft = 13,
        
        EatForward = 14,
        EatUp = 15,
        EatRight = 16,
        EatDown = 17,
        EatLeft = 18,
        EatUpRight = 19,
        EatDownRight = 20,
        EatDownLeft = 21,
        EatUpLeft = 22,
        
        AttackForward = 23,
        AttackUp = 24,
        AttackRight = 25,
        AttackDown = 26,
        AttackLeft = 27,
        AttackUpRight = 28,
        AttackDownRight = 29,
        AttackDownLeft = 30,
        AttackUpLeft = 31,
    }
}