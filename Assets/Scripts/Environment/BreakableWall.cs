using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall {
    private float _center;
    private float _radius;
    private GameObject _breakable;
    
    public float east { get { return _center + _radius; } }
    public float west { get { return _center - _radius; } }
    
    public BreakableWall() {
        _center = 0f;
        _radius = 0f;
        _breakable = null;
    }
    
    public void Break() {
        if (_breakable) {
            GameObject.Destroy(_breakable);
            Debug.Log("You broke through the wall!");
        } else {
            throw new Exception("Only call BreakableWall.Break() if Observer.BreakableWallCollision()!");
        }
    }
    
    public bool IsInCollisionBox() {
        return _breakable != null;
    }
    
    public void Enter(Collider other) {
        _center = other.bounds.center.x;
        _radius = other.bounds.extents.x;
        _breakable = other.gameObject;
    }
    
    public void Exit(Collider other) {
        _breakable = null;
    }
}
