using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Brain : Agent {
    public Actor actor;
    public TrainingGenerator training_generator;
    
    public override void Initialize() {
        actor = transform.parent.GetComponent<Actor>();
        training_generator = actor.transform.parent.GetComponent<TrainingGenerator>();
    }
    
    public override void OnEpisodeBegin() {
        training_generator.DestroyStage();
        actor.character.Reset();
        training_generator.PrepareStage(actor.transform);
    }
    
     public override void CollectObservations(VectorSensor sensor) {
        // Actor position
        sensor.AddObservation(new Vector2(actor.transform.position.x, actor.transform.position.y));
        
        // Goal position
        GameObject goal = training_generator.goal_object;
        
        if (goal) {
            sensor.AddObservation(new Vector2(goal.transform.position.x, goal.transform.position.y));
            sensor.AddObservation(Vector3.Distance(actor.transform.position, goal.transform.position));
        } else {
            sensor.AddObservation(new Vector2(0f, -10000f));
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut) {
        actor.controller.GetPlayerInput(actionsOut);
    }
    
    public override void OnActionReceived(ActionBuffers actions) {
        var inputs = actions.DiscreteActions;
        int input0 = inputs[0]; // Branch size of 10
        int input1 = inputs[1]; // Branch size of 3
        
        actor.character.activity.Increment();
        actor.controller.Call(input0, input1);
        
        if (!actor.observer.IsAlive()) {
            EndEpisode();
        } else if (actor.observer.IsAtGoal()) {
            float time_remaining = MaxStep - actor.observer.StepCount();
            float reward;
            float time_bonus = (time_remaining / MaxStep) * 5f;
            reward = 5f + time_bonus;
            Debug.Log("You reached the goal! + " + reward.ToString() + " points!");
            SetReward(reward);
            EndEpisode();
        }
    }
}
