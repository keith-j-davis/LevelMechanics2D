using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder {
    private int _counter;
    private float _center;
    private float _top;
    private float _bottom;
    
    public float x { get { return _center; } }
    public float north { get { return _top; } }
    public float south { get { return _bottom; } }
    
    public Ladder() {
        _counter = 0;
        _center = 0f;
        _top = 0f;
        _bottom = 0f;
    }
    
    public bool IsAvailable() {
        return _counter > 0;
    }
    
    public void Enter(Collider other) {
        _counter += 1;
        float center = other.bounds.center.x;
        float top = other.bounds.center.y + other.bounds.extents.y;
        float bottom = other.bounds.center.y - other.bounds.extents.y;
        if ((center != _center) || _counter == 1) {
            _center = center;
            _top = top;
            _bottom = bottom;
        } else {
            if (top > _top) {
                _top = top;
            }
            if (bottom < _bottom) {
                _bottom = bottom;
            }
        }
    }
    
    public void Exit(Collider other) {
        _counter -= 1;
    }
}
