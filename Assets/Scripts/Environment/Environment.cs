using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment {
    // Trigger event tags
    public const string LADDER_TAG = "Ladder";
    public const string LOCK_TAG = "Lock";
    public const string FOOD_TAG = "Food";
    public const string KEY_TAG = "Key";
    public const string ARMOR_TAG = "Armor";
    public const string PROJECTILE_TAG = "Projectile";
    public const string PIT_TRAP_TAG = "Pit Trap";
    public const string GOAL_TAG = "Goal";
    
    // Trigger & Collision
    public const string BREAKABLE_WALL_TAG = "Breakable Wall";
    
    // Collision event tags
    public const string PLATFORM_TAG = "Platform";
    public const string WALL_TAG = "Wall";
    
    // Other
    public const string GUN_TAG = "Gun";
    
    private Character _character;
    
    public Platform platform;
    public Wall wall;
    public Ladder ladder;
    public Lock interactable_lock;
    public BreakableWall breakable_wall;
    public Goal goal;
    
    public Environment(Character character) {
        _character = character;
        
        platform = new Platform();
        wall = new Wall();
        ladder = new Ladder();
        interactable_lock = new Lock();
        breakable_wall = new BreakableWall();
        goal = new Goal();
    }
    
    public void TriggerEnter(Collider other) {
        switch (other.tag) {
            case LADDER_TAG:
                ladder.Enter(other);
                break;
            case LOCK_TAG:
                interactable_lock.Enter(other);
                break;
            case ARMOR_TAG:
                _character.health.AddArmor(other.gameObject);
                break;
            case FOOD_TAG:
                _character.inventory.Add(other.gameObject);
                break;
            case KEY_TAG:
                _character.inventory.Add(other.gameObject);
                break;
            case PROJECTILE_TAG:
                _character.health.Hurt(other.gameObject);
                break;
            case PIT_TRAP_TAG:
                _character.health.Kill();
                break;
            case BREAKABLE_WALL_TAG:
                breakable_wall.Enter(other);
                break;
            case GOAL_TAG:
                goal.Enter(other);
                break;
        }
    }
    
    public void TriggerExit(Collider other) {
        switch (other.tag) {
            case LADDER_TAG:
                ladder.Exit(other);
                break;
            case LOCK_TAG:
                interactable_lock.Exit(other);
                break;
            case BREAKABLE_WALL_TAG:
                breakable_wall.Exit(other);
                break;
            case GOAL_TAG:
                goal.Exit(other);
                break;
        }
    }
    
    public void CollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case PLATFORM_TAG:
                platform.Enter(collision);
                break;
            case WALL_TAG:
                wall.Enter(collision);
                break;
        }
    }
    
    public void CollisionExit(Collision collision) {
        switch (collision.gameObject.tag) {
            case PLATFORM_TAG:
                platform.Exit(collision);
                break;
            case WALL_TAG:
                wall.Exit(collision);
                break;
        }
    }
}
