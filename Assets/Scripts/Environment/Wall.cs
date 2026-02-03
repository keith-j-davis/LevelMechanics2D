using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall {
    private int _counter;
    private float _center;
    private float _radius;
    
    public float east { get { return _center + _radius; } }
    public float west { get { return _center - _radius; } }
    
    public Wall() {
        _counter = 0;
        _center = 0f;
        _radius = 0f;
    }
    
    public bool IsInCollisionBox() {
        return _counter > 0;
    }
    
    public void Enter(Collision collision) {
        _counter += 1;
        _center = collision.collider.bounds.center.x;
        _radius = collision.collider.bounds.extents.x;
    }
    
    public void Exit(Collision collision) {
        _counter -= 1;
    }
}
