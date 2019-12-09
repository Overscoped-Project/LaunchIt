using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 30f;
    [SerializeField] private int aggression = 70;
    [SerializeField] private int damage = 20;

    private GameObject enemy;
    [SerializeField] private float attackRate = 1f;
    private bool canAttack = true;
    private float timeSinceAttack = 0;
    [SerializeField] private float hitJumpBack = 3f;

    private int ambientTime = 0;
    private int reducer = 50;
    [SerializeField] private int maxRandomAmbientTime = 100;
    [SerializeField] private int minRandomAmbientTime = 20;
    [SerializeField] private float ambientRange = 3f;
    private Vector2 newPosition;
    private Vector2 seqPosition;
    private Queue<Vector2> sequencePoint;

    private List<GameObject> objectsInRange = new List<GameObject>();
    void Start()
    {
        newPosition = transform.position;
        sequencePoint = new Queue<Vector2>();
    }

    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.C))
        {
            newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    }

    private void AmbientMovement()
    {
        if (ambientTime == 0)
        {
            ambientTime = Random.Range(minRandomAmbientTime, maxRandomAmbientTime);
            newPosition += new Vector2(Random.Range(-ambientRange, ambientRange), Random.Range(-ambientRange, ambientRange));
            //Pathfinding(GetComponent<Rigidbody2D>().position, newPosition);
            seqPosition = sequencePoint.Dequeue();
        }
        else if ((GetComponent<Rigidbody2D>().position.x >= seqPosition.x + 1 || GetComponent<Rigidbody2D>().position.x <= seqPosition.x - 1) && (GetComponent<Rigidbody2D>().position.y >= seqPosition.y + 1 || GetComponent<Rigidbody2D>().position.y <= seqPosition.y - 1))
        {
            Vector2 moveToPoint = seqPosition - GetComponent<Rigidbody2D>().position;
            moveToPoint = moveToPoint.normalized;
            GetComponent<Rigidbody2D>().position += new Vector2(moveToPoint.x * (speed / 2) * Time.deltaTime, moveToPoint.y * (speed / 2) * Time.deltaTime);
            if (reducer <= 0)
            {
                //Pathfinding(GetComponent<Rigidbody2D>().position, newPosition);
                seqPosition = sequencePoint.Dequeue();
                reducer = 50;
            }
            else
            {
                //Debug.Log(reducer);
                reducer--;
            }
        }
        else if (sequencePoint.Count > 0)
        {
            seqPosition = sequencePoint.Dequeue();
        }
        else
        {

            ambientTime--;
        }


    }

    //TODO neue methode ambesten um nicht bei jeder collsion schaden zu nehmen
    private void AttackMovement(GameObject player)
    {
        if (canAttack)
        {
            Vector2 moveToPlayer = player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position += new Vector2(moveToPlayer.x * speed * Time.deltaTime, moveToPlayer.y * speed * Time.deltaTime);
        }
        else
        {
            timeSinceAttack += attackRate * Time.deltaTime;
            if (timeSinceAttack >= 1)
            {
                timeSinceAttack = 0;
                canAttack = true;
            }
        }
    }

    private void Pathfinding(Vector2 position, Vector2 moveToPosition)
    {
        sequencePoint = new Queue<Vector2>();
        if (objectsInRange.Count != 0)
        {

            Vector2 path = moveToPosition - position;
            Vector2 pathVector = path.normalized;
            foreach (GameObject obj in objectsInRange)
            {
                if (obj.tag == "Player" || obj.tag == "Bullet")
                {
                    //nothing
                }
                else if (obj.tag == "Entity")
                {
                    //umständlich :O
                }
                else
                {

                    Ray ray = new Ray(moveToPosition, position);
                    if (obj.GetComponent<SpriteRenderer>().bounds.IntersectRay(ray))
                    {
                        Vector2 min = obj.GetComponent<SpriteRenderer>().bounds.min;
                        Vector2 max = obj.GetComponent<SpriteRenderer>().bounds.max;
                        Vector2 extents = obj.GetComponent<SpriteRenderer>().bounds.extents;
                        Vector2 center = (Vector2)obj.GetComponent<SpriteRenderer>().bounds.center;
                        Vector2 vectorCenter = center - position;
                        if (Mathf.Abs(vectorCenter.x) > Mathf.Abs(vectorCenter.y))
                        {
                            if (vectorCenter.x > 0)
                            {
                                if (newPosition.y < center.y)
                                {
                                    Vector2 tempPos = position;
                                    while (tempPos.y < obj.GetComponent<SpriteRenderer>().bounds.min.y)
                                    {
                                        tempPos += pathVector;
                                    }
                                    if (tempPos.x > obj.GetComponent<SpriteRenderer>().bounds.min.x && tempPos.y < center.y)
                                    {
                                        Vector2 newMin = new Vector2(center.x + extents.x, center.y - extents.y);
                                        Vector2 newMax = new Vector2(center.x - extents.x, center.y + extents.y);

                                        if (CalulateNormals(newMin, pathVector, position) < CalulateNormals(newMax, pathVector, position))
                                        {
                                            sequencePoint.Enqueue(new Vector2(newMin.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, newMin.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                        else
                                        {
                                            sequencePoint.Enqueue(new Vector2(newMax.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, newMax.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }

                                    }
                                    else
                                    {
                                        if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                        {
                                            sequencePoint.Enqueue(new Vector2(min.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, min.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                        else
                                        {
                                            sequencePoint.Enqueue(new Vector2(max.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, max.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                    }

                                }
                                else
                                {
                                    if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                    {
                                        sequencePoint.Enqueue(new Vector2(min.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, min.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                    }
                                    else
                                    {
                                        sequencePoint.Enqueue(new Vector2(max.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, max.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                    }
                                }
                            }
                            else
                            {
                                if (newPosition.y > center.y)
                                {
                                    Vector2 tempPos = position;
                                    while (tempPos.y < obj.GetComponent<SpriteRenderer>().bounds.max.y)
                                    {
                                        tempPos += pathVector;
                                    }
                                    if (tempPos.x < obj.GetComponent<SpriteRenderer>().bounds.max.x && tempPos.y > center.y)
                                    {
                                        Vector2 newMin = new Vector2(center.x + extents.x, center.y - extents.y);
                                        Vector2 newMax = new Vector2(center.x - extents.x, center.y + extents.y);

                                        if (CalulateNormals(newMin, pathVector, position) < CalulateNormals(newMax, pathVector, position))
                                        {
                                            sequencePoint.Enqueue(new Vector2(newMin.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, newMin.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                        else
                                        {
                                            sequencePoint.Enqueue(new Vector2(newMax.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, newMax.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }

                                    }
                                    else
                                    {
                                        if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                        {
                                            sequencePoint.Enqueue(new Vector2(min.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, min.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                        else
                                        {
                                            sequencePoint.Enqueue(new Vector2(max.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, max.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                        }
                                    }

                                }
                                else
                                {
                                    if (CalulateNormals(min, pathVector, position) < CalulateNormals(max, pathVector, position))
                                    {
                                        sequencePoint.Enqueue(new Vector2(min.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, min.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                    }
                                    else
                                    {
                                        sequencePoint.Enqueue(new Vector2(max.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x, max.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y));
                                    }
                                }
                            }

                        }
                        else
                        {
                            //NEXT TODO
                        }
                    }
                    if (sequencePoint.Count == 0)
                    {
                        sequencePoint.Enqueue(moveToPosition);
                    }

                }
            }

        }
        else
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && GetComponent<CircleCollider2D>().IsTouching(collision.GetComponent<CapsuleCollider2D>()))
        {
            enemy = collision.gameObject;
            ContactSwarm(enemy);
        }
        if (collision.gameObject.tag != "Dialogue" && collision.gameObject.tag != "Bullet")
        {
            objectsInRange.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy = null;
        }
        if (collision.gameObject.tag != "Dialogue" && collision.gameObject.tag != "Bullet")
        {
            objectsInRange.Remove(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hit(damage);
            //Little Knockback
            Vector3 moveToPlayer = collision.transform.position - transform.position;
            moveToPlayer = moveToPlayer.normalized;
            GetComponent<Rigidbody2D>().position -= new Vector2((moveToPlayer.x * speed * Time.deltaTime) * hitJumpBack, (moveToPlayer.y * speed * Time.deltaTime) * hitJumpBack);

            canAttack = false;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void ContactSwarm(GameObject target)
    {
        foreach (GameObject obj in objectsInRange)
        {
            if (obj.tag == "Entity")
            {
                obj.GetComponent<Alien>().SetEnemy(target);
            }                         
        }
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
    }
}
