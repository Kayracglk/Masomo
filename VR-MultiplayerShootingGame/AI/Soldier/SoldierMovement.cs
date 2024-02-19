
public class SoldierMovement : AIMovement
{
    private SoldierManager agent;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<SoldierManager>();
    }
    public override void Start()
    {
        base.Start();
    }


    public override void Update()
    {
        /* if (!PhotonNetwork.IsMasterClient)
             return;*/
        if (ai.agent.hasPath)
            agent.anim.SetFloat("Speed", agent.agent.velocity.magnitude);
        else
            agent.anim.SetFloat("Speed", 0);
    }
}
