
public enum AIStateID
{
    ChasePlayer,
    LookAroundState,
    DeathState,
    IdleState,
    FindWeaponState,
    AttackPlayerState,
    CoverState,
}

public interface AIState
{
    AIStateID GetID();
    void Enter(AIManager agent);
    void Update(AIManager agent);
    void Exit(AIManager agent);
}
