public class PlayerStates
{
    public readonly StateBase Idle;
    public readonly StateBase Jumping;
    public readonly StateBase Falling;
    public readonly StateBase Walking;
    public readonly StateBase Gliding;
    public readonly StateBase Attacking;
    public readonly StateBase DodgeRoll;
    public readonly StateBase HurtState;
    public readonly StateBase Death;

    
    public PlayerStates(Player player)
    {
        Idle = new IdleState(player);
        Jumping = new JumpingState(player);
        Falling = new FallingState(player);
        Walking = new WalkingState(player);
        Gliding = new GlidingState(player);
        Attacking = new AttackingState(player);
        DodgeRoll = new DodgeRollState(player);

        HurtState = new HurtState(player);

        Death = new DeathState(player);
    }
}
