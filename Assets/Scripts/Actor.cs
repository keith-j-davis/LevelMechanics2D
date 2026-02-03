using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Actor : MonoBehaviour {
    public Character character;
    public Environment environment;
    public Observer observer;
    public Animator animator;
    public Controller controller;
    
    public Texture2D[] texture_array;
    
    public void Start() {
        Animator.Initialize();
        
        MeshRenderer sprite_renderer = transform.Find("Sprite").gameObject.GetComponent<MeshRenderer>();
        MeshRenderer sfx_renderer = transform.Find("Sfx").gameObject.GetComponent<MeshRenderer>();
        
        character = new Character(GetComponent<Rigidbody>());
        environment = new Environment(character);
        observer = new Observer(character, environment);
        animator = new Animator(sprite_renderer, sfx_renderer, texture_array);
        controller = new Controller(character, environment, observer, animator);
    }
    
    private void OnTriggerEnter(Collider other) {
        environment.TriggerEnter(other);
    }
    
    private void OnTriggerExit(Collider other) {
        environment.TriggerExit(other);
    }
    
    private void OnCollisionEnter(Collision collision) {
        environment.CollisionEnter(collision);
    }
    
    private void OnCollisionExit(Collision collision) {
        environment.CollisionExit(collision);
    }
}
