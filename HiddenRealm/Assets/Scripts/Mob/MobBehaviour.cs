using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobBehaviour : NetworkBehaviour
{
    [SerializeField]
    private Mob data = null;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    public GameObject LootBagPrefab;

    public GameObject hardCollider;
    public GameObject nameplate;

    public Animator anim;
    public Rigidbody2D rbody;
    public Transform spriteObj;

    private bool inCombat = false;
    private GameObject target;
    private bool isAttacking = false;
    private bool goingBackToSpawnpoint = false;

    private Vector2 startingPosition;
    private Quaternion startingRotation;

    [SerializeField]
    private SyncListUInt teammates;

    public MobSpawn parentSpawn = null;

    // Start is called before the first frame update
    void Start()
    {
        SetStartingPosition(transform.position);
        SetStartingRotation(spriteObj.rotation);
        SetupPhysics();
        rbody.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(GameObject.Find("MobContainer").transform);
    }

    // Update is called once per frame
    void Update()
    {
        CombatHandler();
    }

    private void SetupPhysics()
    {
        if(!isServer)
        {
            rbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void SetStartingRotation(Quaternion value)
    {
        startingRotation = value;
    }

    public void SetStartingPosition(Vector2 value)
    {
        startingPosition = value;
    }

    public Mob GetData()
    {
        return data;
    }

    public void SetTarget(GameObject tar)
    {
        if(target == null)
        {
            target = tar;
        }
    }

    public PlayerRpg GetTarget()
    {
        if(target == null)
        {
            return null;
        }
        return target.GetComponent<PlayerRpg>();
    }

    public void AddTeammate(uint mob)
    {
        teammates.Add(mob);
    }

    public void InitTeammates()
    {
        teammates = new SyncListUInt();
    }

    public void ExitCombat()
    {
        inCombat = false;
    }

    private void CombatHandler()
    {
        Vector3 myPosition = transform.position;
        float mobLeeway = 1.5f;
        if(inCombat)
        {
            if ((target == null) || (target.GetComponent<PlayerRpg>().IsDead))
            {
                inCombat = false;
                target = null;
                isAttacking = false;
                goingBackToSpawnpoint = true;
                hardCollider.SetActive(false);
            }
            else
            {
                if (((Vector2.Distance(myPosition, target.transform.position) > 0.5f) && (!isAttacking))
                || ((Vector2.Distance(myPosition, target.transform.position) > mobLeeway) && (isAttacking)))
                {
                    isAttacking = false;
                    Vector2 movementVector = Vector3.Normalize(target.transform.position - myPosition);
                    spriteObj.right = movementVector;
                    anim.SetBool("isrunning", true);
                    anim.SetBool("isattacking", false);
                    rbody.MovePosition(rbody.position + movementVector * Time.fixedDeltaTime * speed);
                }
                if ((Vector2.Distance(myPosition, target.transform.position) < 0.5f)
                    || ((Vector2.Distance(myPosition, target.transform.position) > 0.5f)
                    && (Vector2.Distance(myPosition, target.transform.position) < mobLeeway)
                    && (isAttacking)))
                {
                    isAttacking = true;
                    Vector2 movementVector = Vector3.Normalize(target.transform.position - myPosition);
                    spriteObj.right = movementVector;
                    anim.SetBool("isrunning", false);
                    anim.SetBool("isattacking", true);
                }
                if (Vector2.Distance(startingPosition, target.transform.position) > 15f)
                {
                    inCombat = false;
                    target = null;
                    isAttacking = false;
                    goingBackToSpawnpoint = true;
                    hardCollider.SetActive(false);
                }
            }
        }
        if (goingBackToSpawnpoint)
        {
            if (Vector2.Distance(myPosition, startingPosition) > 0.1f)
            {
                //finality
                if (Vector2.Distance(myPosition, startingPosition) > 25f)
                {
                    transform.Translate(startingPosition);
                }
                Vector2 movementVector = Vector3.Normalize((Vector3)startingPosition - myPosition);
                spriteObj.right = movementVector;
                anim.SetBool("isrunning", true);
                anim.SetBool("isattacking", false);
                rbody.MovePosition(rbody.position + movementVector * Time.fixedDeltaTime * speed);
            }
            else
            {
                GetComponent<MobRpg>().ResetHealth();
                anim.SetBool("isrunning", false);
                goingBackToSpawnpoint = false;
                rbody.bodyType = RigidbodyType2D.Static;
                rbody.bodyType = RigidbodyType2D.Dynamic;
                spriteObj.rotation = startingRotation;
                hardCollider.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetHit(collision);
    }

    [ClientRpc]
    private void RpcGetHit(GameObject go)
    {
        GetHit(go.GetComponent<Collider2D>());
    }

    public void GetHit(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            //collision is the player(players weapon to be exact)
            if (collision.transform.name == "Weapon")
            {
                SetTarget(collision.transform.parent.gameObject);
                inCombat = true;
                rbody.bodyType = RigidbodyType2D.Dynamic;
                foreach (uint idmb in teammates)
                {
                    MobBehaviour mb = NetworkIdentity.spawned[idmb].gameObject.GetComponent<MobBehaviour>();
                    mb.SetTarget(collision.transform.parent.gameObject);
                    mb.inCombat = true;
                    mb.rbody.bodyType = RigidbodyType2D.Dynamic;
                }
                target.GetComponent<PlayerRpg>().Attack(gameObject);
            }
        }
    }

    public void GetHitOnServer(GameObject tar)
    {
        SetTarget(tar);
        inCombat = true;
        rbody.bodyType = RigidbodyType2D.Dynamic;
        foreach (uint idmb in teammates)
        {
            MobBehaviour mb = NetworkIdentity.spawned[idmb].gameObject.GetComponent<MobBehaviour>();
            mb.SetTarget(tar);
            mb.inCombat = true;
            mb.rbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void DealDamageToPlayer()
    {
        if(target != null)
        {
            target.GetComponent<PlayerRpg>().GetDamage(GetComponent<MobRpg>().damage);
        }
    }

    public void UpdateParent()
    {
        if (isServer)
        {
            parentSpawn.aliveMobs--;
        }
    }
}
