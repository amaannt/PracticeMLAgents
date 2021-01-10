using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    public float agentSpeed = 2.0f;
    [SerializeField] Material winMaterial;
    [SerializeField] Material loseMaterial;
    [SerializeField] Material normalMaterial;
    [SerializeField] MeshRenderer floorMeshRenderer;
    public override void OnEpisodeBegin()
    {

        //random anywhere
        transform.localPosition = new Vector3(Random.Range(-3.7f, 3.7f), 0, Random.Range(-3.4f, 3.5f));

        //left area
        //transform.localPosition = new Vector3(Random.Range(1.1f,3.6f), 0, Random.Range(-3.1f, 3.1f));
        //floorMeshRenderer.material = normalMaterial;
        //random anywhere
        targetTransform.localPosition =  new Vector3(Random.Range(-3.31f, 3.31f), 0, Random.Range(-3.3f, 3.3f));

        //targetTransform.localPosition = new Vector3(Random.Range(-3.71f, -0.84f), 0, Random.Range(-3.3f, 3.3f));
    }
    float stepCount = 0.0f;
    //this controls actions 
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = agentSpeed;
        transform.position += new Vector3(moveX, 0 ,moveZ) * Time.deltaTime * moveSpeed;
        stepCount = StepCount;
    }

    //this is where the agent collects information
    [SerializeField] private Transform targetTransform;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f );

            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        } 
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
}
