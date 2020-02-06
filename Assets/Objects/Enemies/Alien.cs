using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Alien : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 30f;
    [SerializeField] private float ambientsSpeedPercentage = 0.5f;
    [SerializeField] private int maxAggression = 70;
    [SerializeField] private int aggression = 70;
    [SerializeField] private int aggressionRegeneration = 10;
    [SerializeField] private int damage = 20;

    [SerializeField] private bool rangeUnit = false;
    [SerializeField] private Bullet shot;
    [SerializeField] private float range = 10f;
    [SerializeField] private float offsetX = 2f;
    [SerializeField] private float offsetY = 2f;

    [SerializeField] private float attackRate = 1f;
    private bool canAttack = true;
    private float timeSinceAttack = 0;
    [SerializeField] private float hitJumpBack = 0.5f;

    [SerializeField] private int dodgeChance = 10;
    private Queue<Vector2> dodgePoint;
    private Vector2 dodgeSequencePoint;
    private bool dodge = false;

    private int ambientTime = 0;
    [SerializeField] private int maxRandomAmbientTime = 100;
    [SerializeField] private int minRandomAmbientTime = 20;
    [SerializeField] private float ambientRange = 3f;
    private Vector3 newPosition;
    private Vector3 nextPosition;

    [SerializeField] private float maxGapToPoint = 5;
    [SerializeField] private float gapToPoint = 1;
    [SerializeField] private float gapMultiplicator = 1.5f;
    private float defaultGap;
    private bool nextArrived = false;
    private bool newArrived = false;

    private List<GameObject> UnitInRange = new List<GameObject>();
    private List<Bullet> bulletsInRange = new List<Bullet>();
    private List<Bullet> calculatedBullets = new List<Bullet>();

    [SerializeField] private bool patrouilleUnit = false;
    [SerializeField] private List<GameObject> patrouillePoints = new List<GameObject>();
    [SerializeField] private GameObject triggerZone;
    [SerializeField] private int maxGuardingTicks = 500;
    private int guardingTicks;
    private int currentPoint = 0;
    private bool pointRun = true;
    [SerializeField] private int patrouilleId = 0;
    private List<Alien> patrouilleAlly = new List<Alien>();

    [SerializeField] private GameObject enemyDeath;
    [SerializeField] private GameObject home;

    private NodeGrid nodeGrid;
    private List<Node> ambientPath = new List<Node>();
    private int nodeIndex = 0;

    private List<Node> homePath = new List<Node>();
    private bool ready = false;

    private GameObject enemy;
    private Rigidbody2D enemyBody;
    private Player enemyPlayer;

    private Rigidbody2D body;
    private AudioManager audioManager;
    private CapsuleCollider2D capsuleCollider2D;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //test
    public bool freeze = false;

    IEnumerator Start()
    {
        nodeGrid = GameObject.FindGameObjectWithTag("NodeManager").GetComponent<NodeGrid>();
        yield return new WaitUntil(() => nodeGrid.GetReady());
        audioManager = FindObjectOfType<AudioManager>();
        body = gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        defaultGap = gapToPoint;
        newPosition = body.position;
        ambientPath.Add(nodeGrid.NodeFromWorldPoint(newPosition));

        dodgePoint = new Queue<Vector2>();

        if (patrouilleUnit)
        {
            foreach (GameObject ally in GameObject.FindGameObjectsWithTag("Entity"))
            {
                Alien alien = ally.GetComponent<Alien>();
                if (alien.GetPatrouilleUnit() && alien.GetPatrouilleId() == patrouilleId)
                {
                    patrouilleAlly.Add(alien);
                    ambientPath = Pathfinding(transform.position, (Vector2)patrouillePoints[currentPoint].transform.position);
                    SetNewPosition(ambientPath, patrouilleAlly.Count);
                }
            }

        }
        if (home == null)
        {
            Debug.Log("!!In " + gameObject.name + " is option [Home] = null. Please assign a GameObject!!");
        }

        ready = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            freeze = !freeze;
        }
        if (!ready || freeze)
        {
            //Wait that Start() get's ready or the Freeze ends
        }
        else
        {
            if (enemy != null)
            {
                Physics2D.IgnoreCollision(capsuleCollider2D, enemy.GetComponent<CircleCollider2D>());
                audioManager.PlayIfNot("EnemyMove");
                AttackMovement(enemy);

            }
            else
            {
                if (homePath.Count > 0)
                {
                    homePath = new List<Node>();
                }
                AmbientMovement();
            }
        }
    }

    public void Hit(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            if (patrouilleUnit)
            {
                try
                {
                    foreach (Alien ally in patrouilleAlly)
                    {
                        if (ally == null)
                        {
                            continue;
                        }
                        else
                        {
                            ally.RemoveUnit(this);
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                }

            }
            Instantiate(enemyDeath, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            float temp = aggression;
            aggression -= (int)((temp / 100) * (100 / (health + dmg)) * dmg);
        }
    }
    private void Shoot(Vector2 direction, float angle)
    {
        Vector2 offset = new Vector2(0,0);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                offset.x = offsetX;
            }
            else
            {
                offset.x = -offsetX;
            }
            offset.y = 0;
        }
        else
        {
            if (direction.y > 0)
            {
                offset.y = offsetY;
            }
            else
            {
                offset.y = -offsetY;
            }
            offset.x = 0;
        }
        Quaternion q = Quaternion.Euler(0, 0, angle);
        Bullet bullet = Instantiate(shot, body.position + offset, q);
        bullet.SetDirection(direction, this.gameObject, audioManager);
        //audioManager.Play("");
    }

    public void RemoveUnit(Alien unit)
    {
        patrouilleAlly.Remove(unit);
    }
    public void SetNewPosition(List<Node> path, int count)
    {
        if (path == null || path.Count == 0)
        {

        }
        else
        {
            Vector3 direction = path[0].GetNodePosition() - transform.position;
            float distance = direction.magnitude;
            direction = direction.normalized;
            if (!Physics2D.Raycast(transform.position, direction, distance, 8))
            {
                this.ambientPath = Pathfinding(transform.position, path[path.Count - 1].GetNodePosition());
            }
            else
            {
                this.ambientPath = path;
            }
            nodeIndex = 0;
            if (ambientPath.Count > 0)
            {
                newPosition = ambientPath[ambientPath.Count - 1].GetNodePosition();
                nextPosition = ambientPath[nodeIndex].GetNodePosition();
                nextArrived = false;
                newArrived = false;
            }
            guardingTicks = maxGuardingTicks;
            gapToPoint = count * gapMultiplicator + maxGapToPoint;
        }
    }

    public void SetNewIndex(List<Node> path, int nodeIndex, int count)
    {
        Vector3 direction = path[nodeIndex].GetNodePosition() - path[nodeIndex - 1].GetNodePosition();
        float distance = direction.magnitude;
        direction = direction.normalized;
        if (!Physics2D.Raycast(transform.position, direction, distance, 8))
        {
            nextPosition = path[nodeIndex].GetNodePosition();
            gapToPoint = count * gapMultiplicator + maxGapToPoint;
            nextArrived = false;
        }
    }
    private void AmbientMovement()
    {
        if (patrouilleUnit && guardingTicks <= 0)
        {
            if (currentPoint + 1 == patrouillePoints.Count)
            {
                pointRun = false;
            }
            else if (currentPoint == 0)
            {
                pointRun = true;
            }

            if (pointRun)
            {
                currentPoint += 1;
            }
            else
            {
                currentPoint -= 1;
            }
            int count = 0;
            foreach (Alien ally in patrouilleAlly)
            {
                if (ally == null)
                {
                    continue;
                }
                else
                {
                    count++;
                    ambientPath = Pathfinding(transform.position, (Vector2)patrouillePoints[currentPoint].transform.position);
                    ally.SetNewPosition(ambientPath, count);
                }    
            }
        }
        else
        {
            if (!newArrived && !((Mathf.Abs(transform.position.x - newPosition.x) <= gapToPoint) && (Mathf.Abs(transform.position.y - newPosition.y) <= gapToPoint)))
            {
                movementPathfinding(ambientPath, speed * ambientsSpeedPercentage);
                RegenerateAggression();
            }
            else if (ambientTime <= 0)
            {
                gapToPoint = defaultGap;
                ambientTime = Random.Range(minRandomAmbientTime, maxRandomAmbientTime);
                newPosition = new Vector3(body.position.x + Random.Range(-ambientRange, ambientRange), body.position.y + Random.Range(-ambientRange, ambientRange), 0);
                while (!nodeGrid.NodeFromWorldPoint(newPosition).GetIsWall())
                {
                    newPosition = new Vector3(body.position.x + Random.Range(-ambientRange, ambientRange), body.position.y + Random.Range(-ambientRange, ambientRange), 0);
                }
                newArrived = false;
                ambientPath = Pathfinding(body.position, newPosition);
                if (ambientPath.Count > 0)
                {
                    nodeIndex = 0;
                    nextPosition = ambientPath[nodeIndex].GetNodePosition();
                    nextArrived = false;
                }

            }
            else
            {
                body.velocity = Vector3.zero;
                if (patrouilleUnit)
                {
                    guardingTicks--;
                }
                RegenerateAggression();
                ambientTime--;
            }

        }
    }

    private void movementPathfinding(List<Node> path, float speed)
    {
        if ((Mathf.Abs(transform.position.x - nextPosition.x) <= gapToPoint) && (Mathf.Abs(transform.position.y - nextPosition.y) <= gapToPoint))
        {
            if (nodeIndex < path.Count - 1)
            {
                nodeIndex++;
                nextPosition = path[nodeIndex].GetNodePosition();
                nextArrived = false;
                if (patrouilleUnit)
                {
                    int count = 0;
                    foreach (Alien ally in patrouilleAlly)
                    {
                        if (ally == null)
                        {
                            continue;
                        }
                        else
                        {
                            count++;
                            SetNewIndex(path, nodeIndex, count);
                        }
                    }
                }
            }
            else
            {
                newArrived = true;
                nextArrived = true;
            }
        }
        else if (!nextArrived)
        {
            Vector3 targetPoint = nextPosition - transform.position;
            targetPoint = targetPoint.normalized;
            body.velocity = targetPoint * speed; //* Time.deltaTime;
            animator.SetFloat("Direction", Vector2.SignedAngle(Vector2.up, targetPoint));
        }
        else
        {
            body.velocity = Vector3.zero;
        }
    }
    private void AttackMovement(GameObject player)
    {
        if (dodge && dodgePoint.Count >= 0 && (Mathf.Abs(body.position.x - dodgeSequencePoint.x) <= 1) && (Mathf.Abs(body.position.y - dodgeSequencePoint.y) <= 1))
        {
            if (dodgePoint.Count == 0)
            {
                dodge = false;
            }
            else
            {
                dodgeSequencePoint = dodgePoint.Dequeue();
            }
        }
        else if (dodge)
        {
            Vector2 targetPosition = dodgeSequencePoint - body.position;
            targetPosition = targetPosition.normalized;
            body.velocity = targetPosition * speed; //* Time.deltaTime;
        }

        if (aggression < enemyPlayer.GetAggression() && !dodge)
        {
            if (homePath == null || homePath.Count < 1)
            {
                homePath = Pathfinding(body.position, home.transform.position);
            }
            movementPathfinding(homePath, speed);
        }
        else if (canAttack && !dodge)
        {
            Vector2 targetPosition = enemyBody.position - body.position;
            targetPosition = targetPosition.normalized;

            if (rangeUnit)
            {
                if (enemyBody.position.magnitude - body.position.magnitude <= range)
                {
                    if (canAttack)
                    {
                        float angle = 180 - Vector2.SignedAngle(targetPosition * (-1), transform.up);
                        Shoot(targetPosition, angle);
                        //audioManager.Play("");
                        canAttack = false;
                    }
                }
            }
            else
            {
            body.velocity = targetPosition * speed;// * Time.deltaTime;
            animator.SetFloat("Direction", Vector2.SignedAngle(Vector2.up, targetPosition));
            Dodge();
            }           
        }
        else if (!dodge)
        {
            timeSinceAttack += attackRate * Time.deltaTime;
            if (timeSinceAttack >= 1)
            {
                timeSinceAttack = 0;
                canAttack = true;
            }
        }
    }

    private void RegenerateAggression()
    {
        if (aggression < maxAggression)
        {
            aggression += aggressionRegeneration;
        }
    }

    private void Dodge()
    {
        if (Random.Range(1, aggression + 1) < (aggression / 100 * dodgeChance) + (maxAggression - aggression))
        {
            foreach (Bullet bullet in bulletsInRange)
            {
                Vector2 bulletVelocity = bullet.gameObject.GetComponent<Rigidbody2D>().velocity;
                Ray ray = new Ray(bullet.transform.position, bulletVelocity);
                if (spriteRenderer.bounds.IntersectRay(ray) && !calculatedBullets.Contains(bullet))
                {
                    calculatedBullets.Add(bullet);
                    dodge = true;
                    SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
                    if (Mathf.Abs(bulletVelocity.x) > Mathf.Abs(bulletVelocity.y))
                    {
                        int random = Random.Range(0, 2);
                        if (random == 0)
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x, transform.position.y + spriteRenderer.bounds.size.magnitude * 2));
                        }
                        else
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.magnitude * 2));
                        }
                    }
                    else
                    {
                        int random = Random.Range(0, 2);
                        if (random == 0)
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x + spriteRenderer.bounds.size.magnitude * 2, transform.position.y));
                        }
                        else
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x - spriteRenderer.bounds.size.magnitude * 2, transform.position.y));
                        }
                    }

                    if (dodgePoint.Count > 0)
                    {
                        dodgeSequencePoint = dodgePoint.Dequeue();
                    }
                }
            }
        }

    }
    private List<Node> Pathfinding(Vector3 position, Vector3 targetPosition)
    {
        Node startNode = nodeGrid.NodeFromWorldPoint(position);
        Node targetNode = nodeGrid.NodeFromWorldPoint(targetPosition);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].GetFCost() < currentNode.GetFCost() || openList[i].GetFCost() == currentNode.GetFCost() && openList[i].GetHCost() < currentNode.GetHCost())
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return GetFinalPath(startNode, targetNode);
            }

            foreach (Node neighborNode in nodeGrid.GetNeighboringNodes(currentNode))
            {
                if (!neighborNode.GetIsWall() || closedList.Contains(neighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.GetGCost() + GetManhattenDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.GetGCost() || !openList.Contains(neighborNode))
                {
                    neighborNode.SetGCost(moveCost);
                    neighborNode.SetHCost(GetManhattenDistance(neighborNode, targetNode));
                    neighborNode.SetParentNode(currentNode);

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        Debug.Log("## No path found ##");
        return null;
    }

    private List<Node> GetFinalPath(Node startingNode, Node endNode)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startingNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.GetParentNode();
        }

        finalPath.Reverse();
        return finalPath;
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int x = Mathf.Abs(nodeA.GetGridX() - nodeB.GetGridX());
        int y = Mathf.Abs(nodeA.GetGridY() - nodeB.GetGridY());

        return x + y;
    }


    private void ContactSwarm(GameObject target, Rigidbody2D rigi, Player player)
    {
        foreach (GameObject obj in UnitInRange)
        {
            if (obj != null && obj.tag == "Entity" && target != null)
            {
                obj.GetComponent<Alien>().SetEnemy(target, rigi, player);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        if (collision.gameObject.tag == "Player" && circleCollider2D.IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            enemy = collision.gameObject;
            enemyBody = enemy.GetComponent<Rigidbody2D>();
            enemyPlayer = enemy.GetComponent<Player>();
            ContactSwarm(enemy, enemyBody, enemyPlayer);
        }
        if (collision.gameObject.tag == "Entity" && circleCollider2D.IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            UnitInRange.Add(collision.gameObject);
            aggression += UnitInRange.Count;
        }
        if (collision.gameObject.tag == "Bullet")
        {
            bulletsInRange.Add(collision.GetComponent<Bullet>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy = null;
        }
        if (collision.gameObject.tag == "Entity" && !collision.isTrigger)
        {
            aggression -= UnitInRange.Count;
            UnitInRange.Remove(collision.gameObject);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            bulletsInRange.Remove(collision.GetComponent<Bullet>());
            if (calculatedBullets.Contains(collision.GetComponent<Bullet>()))
            {
                calculatedBullets.Remove(collision.GetComponent<Bullet>());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        body.velocity = Vector3.zero;
        if (collision.gameObject.tag == "Player")
        {
            if (canAttack)
            {
                enemyPlayer.Hit(damage);
                audioManager.PlayIfNot("EnemyAttack");
                //Little Knockback
                Vector3 targetDirection = collision.transform.position - transform.position;
                targetDirection = targetDirection.normalized;
                body.velocity = targetDirection * (-1) * speed * hitJumpBack; //* Time.deltaTime;

                canAttack = false;
            }
        }

    }
    public float GetSpeed()
    {
        return speed;
    }

    public void SetEnemy(GameObject enemy, Rigidbody2D enemyBody, Player enemyPlayer)
    {
        this.enemy = enemy;
        this.enemyBody = enemyBody;
        this.enemyPlayer = enemyPlayer;
    }

    public bool GetPatrouilleUnit()
    {
        return patrouilleUnit;
    }
    public int GetPatrouilleId()
    {
        return patrouilleId;
    }

    public List<Node> GetCurrentPath()
    {
        if (homePath.Count > 0)
        {
            return homePath;
        }
        else
        {
            return ambientPath;
        }
    }
}
