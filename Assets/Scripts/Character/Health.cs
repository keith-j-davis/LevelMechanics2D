using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health {
    public const int MAX_HIT_POINTS = 3;
    
    private int _hit_points;
    private bool _armor;
    private bool _attacked;
    
    public int hp { get { return _hit_points; } }
    public int max_hp { get { return MAX_HIT_POINTS + (_armor ? 1 : 0); } }
    public bool armor { get { return _armor; } }
    public bool attacked { get { return _attacked; } }
    
    public Health() {
        _hit_points = MAX_HIT_POINTS;
        _armor = false;
        _attacked = false;
    }
    
    public void Reset() {
        _hit_points = MAX_HIT_POINTS;
        _armor = false;
        _attacked = false;
    }
    
    public void Hurt(GameObject gameObject) {
        _hit_points -= 1;
        _attacked = true;
        if (_armor) {
            _armor = false;
            Debug.Log("Your armor has been destroyed! Health = " + hp.ToString());
        } else {
            Debug.Log("Injured! Health = " + hp.ToString());
        }
        GameObject.Destroy(gameObject);
    }
    
    public void Kill() {
        _hit_points = 0;
        _armor = false;
        Debug.Log("Killed! Health = " + hp.ToString());
    }
    
    public void AddArmor(GameObject gameObject) {
        if (_armor) {
            Debug.Log("Already wearing armor, can't pick it up!");
        } else {
            _armor = true;
            _hit_points += 1;
            Debug.Log("You put on the armor! Health = " + hp.ToString());
            GameObject.Destroy(gameObject);
        }
    }
    
    public void AttackProcessed() {
        _attacked = false;
    }
    
    public void Heal() {
        if (_hit_points < max_hp) {
            _hit_points += 1;
            Debug.Log("Healed! Health = " + hp.ToString());
        } else {
            throw new Exception("Only use Health.Heal() if Observer.IsInjured()!");
        }
    }
}
