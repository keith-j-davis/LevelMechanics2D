using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform {
    private int _counter;
    private float _center;
    private float _radius;
    
    public float north { get { return _center + _radius; } }
    public float south { get { return _center - _radius; } }
    
    public Platform() {
        _counter = 0;
        _center = 0f;
        _radius = 0f;
    }
    
    public bool IsInCollisionBox() {
        return _counter > 0;
    }
    
    public void Enter(Collision collision) {
        _counter += 1;
        _center = collision.collider.bounds.center.y;
        _radius = collision.collider.bounds.extents.y;
    }
    
    public void Exit(Collision collision) {
        _counter -= 1;
    }
}
