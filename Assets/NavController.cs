using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] float range;

    private Renderer capRenderer;
    [SerializeField] Transform center;
    [SerializeField] GameObject playerModel;
    [SerializeField] Transform Player;

    private Color newCapColor;
    bool tagged;
    // Start is called before the first frame update
    void Start()
    {
        tagged = false;
        capRenderer = playerModel.GetComponent<Renderer>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (tagged == true)
        {
            Agent.SetDestination(Player.position);
        }

        if (Agent.remainingDistance <= Agent.stoppingDistance && tagged == false)
        {
            Vector3 point;
            if (RandomPoint(center.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                Agent.SetDestination(point);
            }
        }
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector4.zero;
        return false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            tagged = true;
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        newCapColor = new Color(0f, 1f, 0.8f, 1f);

        capRenderer.material.SetColor("_Color", newCapColor);
    }
}
