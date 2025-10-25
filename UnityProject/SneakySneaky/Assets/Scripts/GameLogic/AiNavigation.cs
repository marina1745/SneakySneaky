using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(ChefSoundHandling))]
public class AiNavigation : MonoBehaviour
{
    public float viewAngle = 70;
    public float viewRange = 6;
    public bool staticc = false;
    public Transform sink;
    public Transform oven;
    public Transform fridge;
    public Transform cam;
    public Transform eyes;
    public float minDistanceToCatchMouse = 2f;

    Animator animator;
    NavMeshAgent agent;
    bool chase = false;
    bool investigating = false;
    bool meal = false;
    Coroutine prepMeal = null;



    public delegate void CaughtMouse();
    public static event CaughtMouse OnCaughtMouse;

    public delegate void InteractWithFridge();
    public static event InteractWithFridge OnInteractWithFridge;

    public delegate void PutOutInteractableObject(GameObject obj);
    public static event PutOutInteractableObject OnPutOutInteractableObject;
    private bool seeTarget = false;
    private ChefSoundHandling sounds;
    // Start is called before the first frame update
    bool CanSeeTarget()
    {
        Vector3 toTarget = (cam.position - eyes.position) + new Vector3(0, 0.1f, 0);
        //Debug.DrawRay(eyes.position, toTarget, Color.red, 3);
        if (Vector3.Angle(eyes.forward, toTarget) <= viewAngle)
        {
            //Debug.Log(Vector3.Angle(eyes.forward, toTarget));
            if (Physics.Raycast(eyes.position, toTarget, out RaycastHit hit, viewRange))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.name == "Mouse")
                {
                    //hit.transform.root == cam
                    //Debug.Log("Hit");
                    return true;
                }
            }

        }
        return false;

    }
    public void Noise(GameObject trans, GameObject front)
    {
        StopCoroutine(prepMeal);
        meal = false;
        animator.SetInteger("Working", 0);
        if(!meal||staticc)
        StartCoroutine(Investigate(trans, front, true));
    }
    public void Confusion(GameObject trans, GameObject front)
    {
        Debug.Log("Confusion");
        if (!investigating)
        {
            investigating = true;
            if(meal)
            StopCoroutine(prepMeal);
            meal = false;
            animator.SetInteger("Working", 0);
            StartCoroutine(Investigate(trans, front, false));
        }
    }
    IEnumerator Investigate(GameObject trans, GameObject front, bool smallObject)
    {
        yield return new WaitForSeconds(1);
        animator.SetInteger("Working", 0);
        sounds.PlayConfusedClip();
        yield return new WaitForSeconds(3);
        agent.SetDestination(front.transform.position);
        yield return new WaitForSeconds(3);
        while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.3)) { yield return null; Debug.Log(agent.remainingDistance); }
        yield return new WaitForSeconds(1);
        if (!smallObject)
        {
            animator.SetInteger("Working", 3);
            yield return new WaitForSeconds(3);
            trans.GetComponent<InteractableObject>().EndInteraction(trans);
        }
        animator.SetInteger("Working", 0);
        yield return new WaitForSeconds(1);
        if (!staticc&&!meal)
        {
            prepMeal = StartCoroutine(PrepareMeal());
        }
        else
        {
            agent.SetDestination(sink.position);
            yield return new WaitForSeconds(1);
            while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.3)) { yield return null; }
            yield return new WaitForSeconds(1);
            animator.SetInteger("Working", 1);
        }
        investigating = false;
    }

    private void CheckIfCaughtMouse()
    {
        Vector2 xzPos = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 xzPosMouse = new Vector2(cam.transform.position.x, cam.transform.position.z);

        if ((xzPos - xzPosMouse).magnitude <= minDistanceToCatchMouse)
            OnCaughtMouse();
    }
    IEnumerator Chase()
    {
        Debug.Log("CHASEEEEEE");
        sounds.PlaySurprisedClip();
        bool chasing = true;
        while (chasing)
        {
            animator.SetInteger("Working", 0);
            animator.SetBool("Detected", true);
            while (CanSeeTarget())
            {
                agent.destination = cam.position;
                yield return null;
                CheckIfCaughtMouse();
            }
            yield return new WaitForSeconds(2);
            if (!CanSeeTarget())
                chasing = false;
            else
                CheckIfCaughtMouse();
        }
        animator.SetBool("Detected", false);
        yield return new WaitForSeconds(1);
        if(!meal)
        prepMeal = StartCoroutine(PrepareMeal());
        chase = false;
    }
    IEnumerator StaticChase()
    {
        bool chasing = true;
        while (chasing)
        {
            animator.SetInteger("Working", 0);
            animator.SetBool("Detected", true);
            while (CanSeeTarget())
            {
                agent.destination = cam.position;
                yield return null;
                CheckIfCaughtMouse();
            }
            yield return new WaitForSeconds(2);
            if (!CanSeeTarget())
                chasing = false;
            else
                CheckIfCaughtMouse();
        }
        animator.SetBool("Detected", false);
        yield return new WaitForSeconds(1);
        chase = false;
        agent.SetDestination(sink.position);
        while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.31)) { yield return null; }
        animator.SetInteger("Working", 2);
    }
    IEnumerator PrepareMeal()
    {
        meal = true;
        //move to sink and start working
        yield return new WaitForSeconds(1);
        agent.destination = sink.position;
        yield return new WaitForSeconds(3);
        //wait till arrival at sink
        while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.31)) { yield return null;Debug.Log(agent.remainingDistance); }
        //starting to work at sink
        agent.transform.LookAt(sink.position);
        sounds.PlayWorkingClip();
        animator.SetInteger("Working", 1);
        yield return new WaitForSeconds(10);
        animator.SetInteger("Working", 0);
        yield return new WaitForSeconds(3);
        //set new destination
        agent.destination = fridge.position;
        yield return new WaitForSeconds(3);
        //wait till arrival at fridge
        while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.35)) { yield return null; }
        //starting to work at fridge
        agent.transform.LookAt(fridge);
        fridge.GetComponent<FridgeBehavior>().InteractWithFridgeDoor();
        animator.SetInteger("Working", 1);
        sounds.PlayWorkingClip();
        yield return new WaitForSeconds(10);
        animator.SetInteger("Working", 0);
        fridge.GetComponent<FridgeBehavior>().InteractWithFridgeDoor();
        yield return new WaitForSeconds(3);
        Debug.Log("walkin to oven");
        agent.destination = oven.position;
        yield return new WaitForSeconds(3);
        while (!(agent.remainingDistance != 0 && agent.remainingDistance < 0.25)) { yield return null; }
        Debug.Log("arrived at oven");
        agent.transform.LookAt(oven.position);
        animator.SetInteger("Working", 3);
        sounds.PlayWorkingClip();
        yield return new WaitForSeconds(10);
        animator.SetInteger("Working", 0);
        yield return new WaitForSeconds(3);
        prepMeal = StartCoroutine(PrepareMeal());
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sounds = GetComponent<ChefSoundHandling>();
        if (!staticc) { prepMeal = StartCoroutine(PrepareMeal()); }
        else { animator.SetInteger("Working", 2); };

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        seeTarget = CanSeeTarget();
        //Debug.Log(agent.destination);
        //Debug.Log(this+" "+agent.remainingDistance);
        if (seeTarget && !chase && !staticc)
        {
            chase = true;
            StopCoroutine(prepMeal);
            meal = false;
            animator.SetInteger("Working", 0);
            StartCoroutine(Chase());
        }
        if (seeTarget && !chase && staticc)
        {
            chase = true;
            animator.SetInteger("Working", 0);
            StartCoroutine(StaticChase());
        }
        if (Input.GetKey(KeyCode.I))
        {
            StopCoroutine(prepMeal);
            animator.SetInteger("Working", 0);
            prepMeal = StartCoroutine(PrepareMeal());

        }

        animator.SetFloat("Blend", agent.velocity.magnitude);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
