using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    public Inventory inventory;
    public Health health;
    public Activity activity;
    
    public Character(Rigidbody rigidbody) {
        inventory = new Inventory();
        health = new Health();
        activity = new Activity(rigidbody);
    }
    
    public void Reset() {
        inventory.Reset();
        health.Reset();
        activity.Reset();
    }
}
