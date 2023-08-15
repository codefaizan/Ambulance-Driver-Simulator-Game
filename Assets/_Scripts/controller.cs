using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class controller : MonoBehaviour
{
    Animator animator;
    public bool walking;
    NavMeshAgent agent;
    public bool isInVehicle;

    public static controller i;

    [SerializeField] float waveDistace = 10f;


    private void Awake()
    {
        i = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if(Vector3.Distance(transform.position, Ambulance.i.transform.position) <= waveDistace
            && !walking
            && !isInVehicle)
        {
            animator.SetBool("wave", true);
            transform.LookAt(Ambulance.i.transform);
        }

        if (walking && !isInVehicle)
        {
            animator.SetBool("wave", false);
            animator.SetBool("walk", true);
            agent.SetDestination(Ambulance.i.characterTargetPos.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("character target"))
        {
            if(!isInVehicle && walking)
                StartCoroutine(EnterVehicle());
        }
    }

    IEnumerator EnterVehicle()
    {
        isInVehicle = true;
        walking = false;
        agent.isStopped = true;
        animator.SetBool("walk", false);
        UIController.i.FadeScreen();

        yield return new WaitForSeconds(1f);

        GameController.i.OnEnterVehicle(gameObject);
    }

    private void OnEnable()
    {
        if (isInVehicle)
        {
            agent.isStopped = false;
            animator.SetBool("walk", true);
            GameController.i.OnExitVehicle();
        }
    }
}
