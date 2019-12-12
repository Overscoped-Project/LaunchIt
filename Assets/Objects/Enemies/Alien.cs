using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 30f;
    [SerializeField] private int maxAggression = 70;
    [SerializeField] private int aggression = 70;
    [SerializeField] private int aggressionRegeneration = 10;
    [SerializeField] private int damage = 20;

    private GameObject enemy;
    [SerializeField] private float attackRate = 1f;
    private bool canAttack = true;
    private float timeSinceAttack = 0;
    [SerializeField] private float hitJumpBack = 3f;

    [SerializeField] private int dodgeChance = 10;
    private Queue<Vector2> dodgePoint;
    private Vector2 dodgeSequencePoint;
    private bool dodge = false;

    [SerializeField] private int pathfindingTimer = 20;
    private int reducer;
    [SerializeField] private float gapToObject = 0.5f;

    private int ambientTime = 0;
    [SerializeField] private int maxRandomAmbientTime = 100;
    [SerializeField] private int minRandomAmbientTime = 20;
    [SerializeField] private float ambientRange = 3f;
    private Vector2 newPosition;
    private Vector2 seqPosition;
    private Queue<Vector2> sequencePoint;

    private List<GameObject> objectsInRange = new List<GameObject>();
    private List<Bullet> bulletsInRange = new List<Bullet>();
    private List<Bullet> calculatedBullets = new List<Bullet>();

    [SerializeField] private int maxGapToPoint = 5;
    private int gapToPoint = 1;

    public enum LabyrinthIds { Top, Left, Right, Bottom };
    [SerializeField] private LabyrinthIds labyrinthId;
    [SerializeField] private bool patrouilleUnit = false;
    [SerializeField] private List<GameObject> patrouillePoints = new List<GameObject>();
    [SerializeField] private GameObject triggerZone;
    [SerializeField] private int maxGuardingTicks = 500;
    private int guardingTicks;
    private int currentPoint = 0;
    private bool pointRun = true;
    private List<Alien> patrouilleAlly = new List<Alien>();

    void Start()
    {
        foreach (GameObject ally in GameObject.FindGameObjectsWithTag("Entity"))
        {
            if (ally.GetComponent<Alien>().GetPatrouilleUnit() && ally.GetComponent<Alien>().GetLabyrinthId() == labyrinthId)
            {
                patrouilleAlly.Add(ally.GetComponent<Alien>());
            }
        }
        newPosition = transform.position;
        sequencePoint = new Queue<Vector2>();
        dodgePoint = new Queue<Vector2>();
        reducer = pathfindingTimer;
        if (patrouilleUnit)
        {
            SetNewPosition((Vector2)patrouillePoints[currentPoint].transform.position);
        }
    }

    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.C))
        {
            newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Pathfinding(transform.position, newPosition);
            seqPosition = sequencePoint.Dequeue();

        }
        //DEBUG

        if (enemy != null)
        {
            Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), enemy.GetComponent<CircleCollider2D>());
            AttackMovement(enemy);

        }
        else
        {
            AmbientMovement();
        }
        //that the Entitie not has a velocity after a hit
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public void Hit(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            float temp = aggression;
            aggression -= (int)((temp / 100) * (100 / (health + dmg)) * dmg);
        }
    }
    public void SetNewPosition(Vector2 newPos)
    {
        newPosition = newPos;
        seqPosition = newPosition;
        guardingTicks = maxGuardingTicks;
        gapToPoint = maxGapToPoint;
    }
    private void AmbientMovement()
    {
        if (patrouilleUnit && guardingTicks <= 0)
        {
            if (currentPoint+1 == patrouillePoints.Count)
            {
                pointRun = false;
            }
            else if(currentPoint == 0)
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
            foreach (Alien ally in patrouilleAlly)
            {
                ally.SetNewPosition((Vector2)patrouillePoints[currentPoint].transform.position);
            }
        }
        else
        {
            if (!(Mathf.Abs(GetComponent<Rigidbody2D>().position.x - seqPosition.x) <= gapToPoint) && !(Mathf.Abs(GetComponent<Rigidbody2D>().position.y - seqPosition.y) <= gapToPoint))
            {
                Vector2 moveToPoint = seqPosition - GetComponent<Rigidbody2D>().position;
                moveToPoint = moveToPoint.normalized;
                GetComponent<Rigidbody2D>().position += new Vector2(moveToPoint.x * (speed / 2) * Time.deltaTime, moveToPoint.y * (speed / 2) * Time.deltaTime);

                if (reducer <= 0)
                {
                    Pathfinding(GetComponent<Rigidbody2D>().position, newPosition);
                    if (sequencePoint.Count > 0)
                    {
                        seqPosition = sequencePoint.Dequeue();
                    }
                    reducer = pathfindingTimer;
                }
                else
                {
                    reducer--;
                }
                RegenerateAggression();
            }
            else if (sequencePoint.Count > 0 && (Mathf.Abs(GetComponent<Rigidbody2D>().position.x - seqPosition.x) <= gapToPoint) && (Mathf.Abs(GetComponent<Rigidbody2D>().position.y - seqPosition.y) <= gapToPoint))
            {
                gapToPoint = 1;
                seqPosition = sequencePoint.Dequeue();
            }
            else if (ambientTime <= 0)
            {
                ambientTime = Random.Range(minRandomAmbientTime, maxRandomAmbientTime);
                newPosition += new Vector2(Random.Range(-ambientRange, ambientRange), Random.Range(-ambientRange, ambientRange));

                //TODO Befindet sich die neue position in einem Objekt?
                Pathfinding(GetComponent<Rigidbody2D>().position, newPosition);
                if (sequencePoint.Count > 0)
                {
                    seqPosition = sequencePoint.Dequeue();
                }
            }
            else
            {
                if (patrouilleUnit)
                {
                    guardingTicks--;
                }
                //TODO buggt noch rum und läuft nicht immer wenn er soll.
                //Debug.Log("ambientTime: " + ambientTime);
                //Debug.Log("mep: " + sequencePoint.Count);
                RegenerateAggression();
                ambientTime--;
            }

            
        }
    }

    private void AttackMovement(GameObject player)
    {
        if (dodge && dodgePoint.Count >= 0 && (Mathf.Abs(GetComponent<Rigidbody2D>().position.x - dodgeSequencePoint.x) <= 1) && (Mathf.Abs(GetComponent<Rigidbody2D>().position.y - dodgeSequencePoint.y) <= 1))
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
            Vector2 moveToPosition = dodgeSequencePoint - GetComponent<Rigidbody2D>().position;
            moveToPosition = moveToPosition.normalized;
            GetComponent<Rigidbody2D>().position += (new Vector2(moveToPosition.x * speed * Time.deltaTime, moveToPosition.y * speed * Time.deltaTime));
        }

        if (aggression < player.GetComponent<Player>().GetAggression() && !dodge)
        {
            //TODO er könnte gegen die wand laufen
            Vector2 moveToPlayer = player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position += (new Vector2(moveToPlayer.x * speed * Time.deltaTime, moveToPlayer.y * speed * Time.deltaTime) * (-1));
        }
        else if (canAttack && !dodge)
        {
            Vector2 moveToPlayer = player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position += new Vector2(moveToPlayer.x * speed * Time.deltaTime, moveToPlayer.y * speed * Time.deltaTime);
            Dodge();
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
        if (Random.Range(1, aggression + 1) < (aggression / 100 * dodgeChance) + (100 - aggression))
        {
            foreach (Bullet bullet in bulletsInRange)
            {
                Vector2 bulletVelocity = bullet.gameObject.GetComponent<Rigidbody2D>().velocity;
                Ray ray = new Ray(bullet.transform.position, bulletVelocity);
                if (gameObject.GetComponent<SpriteRenderer>().bounds.IntersectRay(ray) && !calculatedBullets.Contains(bullet))
                {
                    calculatedBullets.Add(bullet);
                    dodge = true;
                    if (Mathf.Abs(bulletVelocity.x) > Mathf.Abs(bulletVelocity.y))
                    {
                        int random = Random.Range(0, 2);
                        if (random == 0)
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x, transform.position.y + bullet.GetComponent<SpriteRenderer>().bounds.size.magnitude * 2));
                        }
                        else
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x, transform.position.y - bullet.GetComponent<SpriteRenderer>().bounds.size.magnitude * 2));
                        }
                    }
                    else
                    {
                        int random = Random.Range(0, 2);
                        if (random == 0)
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x + bullet.GetComponent<SpriteRenderer>().bounds.size.magnitude * 2, transform.position.y));
                        }
                        else
                        {
                            dodgePoint.Enqueue(new Vector2(transform.position.x - bullet.GetComponent<SpriteRenderer>().bounds.size.magnitude * 2, transform.position.y));
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
    private void Pathfinding(Vector2 position, Vector2 moveToPosition)
    {
        sequencePoint = new Queue<Vector2>();
        if (objectsInRange.Count > 0)
        {

            Vector2 path = moveToPosition - position;
            Vector2 pathVector = path.normalized;
            foreach (GameObject obj in objectsInRange)
            {
                if (obj == null)
                {
                    continue;
                }
                else if (obj.tag == "Player" || obj.tag == "Bullet")
                {
                    //nothing
                }
                else if (obj.tag == "Entity")
                {
                    //umständlich :O
                }
                else
                {

                    Ray ray = new Ray(position, moveToPosition);
                    if (obj.GetComponent<SpriteRenderer>().bounds.IntersectRay(ray))
                    {
                        Vector2 pathfindingPos;
                        Bounds bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
                        Vector2 min = obj.GetComponent<SpriteRenderer>().bounds.min;
                        Vector2 max = obj.GetComponent<SpriteRenderer>().bounds.max;
                        Vector2 extents = obj.GetComponent<SpriteRenderer>().bounds.extents;
                        Vector2 center = (Vector2)obj.GetComponent<SpriteRenderer>().bounds.center;
                        Vector2 newMin = new Vector2(center.x + extents.x, center.y - extents.y);
                        Vector2 newMax = new Vector2(center.x - extents.x, center.y + extents.y);
                        Vector2 vectorCenter = center - position;
                        if (Mathf.Abs(vectorCenter.x) > Mathf.Abs(vectorCenter.y))
                        {
                            //Calculation Right Side
                            if (vectorCenter.x < 0)
                            {
                                Vector2 tempPos = position;
                                while (tempPos.x < min.x && (tempPos.y < min.y && tempPos.y > max.y))
                                {
                                    tempPos += pathVector;
                                }
                                //If Ray goes out of box at bottom
                                if (tempPos.x > min.x && tempPos.y < min.y)
                                {
                                    if (position.x - bounds.extents.x <= max.x)
                                    {
                                        pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                    }
                                    else
                                    {
                                        pathfindingPos = new Vector2(position.x + gapToObject, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                    }
                                }
                                else
                                {
                                    if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                    {
                                        if (position.x - bounds.extents.x < max.x)
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                    }
                                    else
                                    {
                                        if (position.x - bounds.extents.x < max.x)
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                    }
                                }

                            }
                            //Calculation Left Side
                            else
                            {
                                Vector2 tempPos = position;
                                while (tempPos.x < newMin.x && (tempPos.y < newMax.y && tempPos.y > newMin.y))
                                {
                                    tempPos += pathVector;
                                }
                                //If Ray goes out of box at top
                                if (tempPos.x < newMin.x && tempPos.y > newMax.y)
                                {
                                    if (position.x + bounds.extents.x > min.x)
                                    {
                                        pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                    }
                                    else
                                    {
                                        pathfindingPos = new Vector2(position.x - gapToObject, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                    }
                                }
                                else
                                {
                                    if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                    {
                                        if (position.x + bounds.extents.x > min.x)
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject, position.y - gapToObject - bounds.extents.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                    }
                                    else
                                    {
                                        if (position.x + bounds.extents.x > min.x)
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject, position.y + gapToObject + bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            //Calculation Top Side
                            if (vectorCenter.y < 0)
                            {
                                Vector2 tempPos = position;
                                while (tempPos.y > newMin.y && (tempPos.x < newMin.x && tempPos.x > newMax.x))
                                {
                                    tempPos += pathVector;
                                }
                                //If Ray goes out of box at right
                                if (tempPos.x > newMin.x && tempPos.y > newMin.y)
                                {
                                    if (position.y - bounds.extents.y < max.y)
                                    {
                                        pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject + bounds.extents.y);
                                    }
                                    else
                                    {
                                        pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject);
                                    }
                                }
                                else
                                {
                                    if (CalulateNormals(newMin, pathVector, position) < CalulateNormals(newMax, pathVector, position))
                                    {
                                        if (position.y - bounds.extents.y < max.y)
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject + bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject);
                                        }
                                    }
                                    else
                                    {
                                        if (position.y - bounds.extents.y < max.y)
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject + bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y + gapToObject);
                                        }
                                    }
                                }

                            }
                            //Calculation Bottom Side
                            else
                            {
                                Vector2 tempPos = position;
                                while (tempPos.y < newMax.y && (tempPos.x < newMin.x && tempPos.x > newMax.x))
                                {
                                    tempPos += pathVector;
                                }
                                //If Ray goes out of box at left
                                if (tempPos.x < newMax.x && tempPos.y < newMax.y)
                                {
                                    if (position.y + bounds.extents.y > min.y)
                                    {
                                        pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject - bounds.extents.y);
                                    }
                                    else
                                    {
                                        pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject);
                                    }
                                }
                                else
                                {
                                    if (CalulateNormals(newMin, pathVector, position) < CalulateNormals(newMax, pathVector, position))
                                    {
                                        if (position.y + bounds.extents.y > min.y)
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + +bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject - bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x + gapToObject + +bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject);
                                        }

                                    }
                                    else
                                    {
                                        if (position.y + bounds.extents.y > min.y)
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject - bounds.extents.y);
                                        }
                                        else
                                        {
                                            pathfindingPos = new Vector2(position.x - gapToObject - bounds.extents.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, position.y - gapToObject);
                                        }

                                    }
                                }
                            }
                        }
                        if (sequencePoint.Contains(pathfindingPos))
                        {
                            Debug.Log("Enthält schon: " + pathfindingPos);
                        }
                        else
                        {
                            sequencePoint.Enqueue(pathfindingPos);
                        }
                    }

                }
            }
        }
        if (sequencePoint.Count == 0)
        {
            sequencePoint.Enqueue(moveToPosition);
        }
    }




    private float CalulateNormals(Vector2 point, Vector2 path, Vector2 position)
    {
        point = point - position;
        float length = (Vector3.Cross(point, path).magnitude) / path.magnitude;
        return length;
    }


    private void ContactSwarm(GameObject target)
    {
        foreach (GameObject obj in objectsInRange)
        {
            if (obj != null && obj.tag == "Entity" && target != null)
            {
                obj.GetComponent<Alien>().SetEnemy(target);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && GetComponent<CircleCollider2D>().IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            enemy = collision.gameObject;
            ContactSwarm(enemy);
        }
        if (collision.gameObject.tag == "Entity" && GetComponent<CircleCollider2D>().IsTouching(collision.GetComponent<PolygonCollider2D>()))
        {
            objectsInRange.Add(collision.gameObject);
        }
        if (collision.gameObject.tag != "Dialogue" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Entity")
        {
            objectsInRange.Add(collision.gameObject);
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
        if (collision.gameObject.tag == "Entity")
        {
            objectsInRange.Remove(collision.gameObject);
        }
        if (collision.gameObject.tag != "Dialogue" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag == "Entity")
        {
            objectsInRange.Remove(gameObject);
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
        if (collision.gameObject.tag == "Player")
        {
            if (canAttack)
            {
                collision.gameObject.GetComponent<Player>().Hit(damage);
                //Little Knockback
                Vector3 moveToPlayer = collision.transform.position - transform.position;
                moveToPlayer = moveToPlayer.normalized;
                GetComponent<Rigidbody2D>().position -= new Vector2((moveToPlayer.x * speed * Time.deltaTime) * hitJumpBack, (moveToPlayer.y * speed * Time.deltaTime) * hitJumpBack);

                canAttack = false;
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
    }

    public bool GetPatrouilleUnit()
    {
        return patrouilleUnit;
    }
    public LabyrinthIds GetLabyrinthId()
    {
        return labyrinthId;
    }
}
