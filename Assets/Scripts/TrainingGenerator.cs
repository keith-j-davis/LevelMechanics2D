using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TrainingGenerator : MonoBehaviour {
    public GameObject breakable_wall;
    public GameObject food;
    public GameObject goal;
    public GameObject ladder;
    public GameObject gun;
    public GameObject padlock;
    public GameObject key;
    public GameObject pit_trap;
    public GameObject platform;
    public GameObject projectile;
    public GameObject armor;
    public GameObject wall;
    
    public GameObject[] stage_list;
    
    public GameObject stage;
    public GameObject goal_object;
    private List<GameObject> game_object_list;
    
    public TrainingGenerator() {
        game_object_list = new List<GameObject>();
    }
    
    public void PrepareStage(Transform character_transform) {
        // Generate stage
        //stage = Instantiate(stage_list[0], new Vector3(0f,0f,0f), Quaternion.identity);
        stage = Instantiate(stage_list[1], new Vector3(0f,0f,0f), Quaternion.identity);
        goal_object = stage.transform.Find(Environment.GOAL_TAG).gameObject;
        
        // Place agent
        float x = Random.Range(-1f,1f);
        //Vector3 position = new Vector3(x, 1f, 0f);
        Vector3 position = new Vector3(-12f, 4f, 0f);
        character_transform.position = position;
        
        // Place goal
        //x = Random.Range(3f,7f) * (Random.value < .5 ? -1f : 1f);
        //goal_object = Instantiate(goal, new Vector3(x,3f,0f), Quaternion.identity);
        //game_object_list.Add(goal_object);
        
        game_object_list.Add(Instantiate(food, new Vector3(-12f,15f,0f), Quaternion.identity));
        game_object_list.Add(Instantiate(key, new Vector3(-12f,21f,0f), Quaternion.identity));
        game_object_list.Add(Instantiate(gun, new Vector3(-5f,20f,0f), Quaternion.identity));
        game_object_list.Add(Instantiate(armor, new Vector3(14f,15f,0f), Quaternion.identity));
    }
    
    public GameObject GetInstance(string tag) {
        foreach (GameObject obj in game_object_list) {
            if (obj && (obj.tag == tag)) {
                return obj;
            }
        }
        return null;
    }
    
    public void DestroyStage() {
        foreach (GameObject gameObject in game_object_list) {
            if (gameObject != null) {
                GameObject.Destroy(gameObject);
            }
        }
        if (stage != null) {
            GameObject.Destroy(stage);
        }
    }
}
