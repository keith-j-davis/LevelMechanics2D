using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal {
    private int _counter;
    
    public Goal() {
        _counter = 0;
    }
    
    public bool IsInCollisionBox() {
        return _counter > 0;
    }
    
    public void Enter(Collider other) {
        _counter += 1;
    }
    
    public void Exit(Collider other) {
        _counter -= 1;
    }
}
