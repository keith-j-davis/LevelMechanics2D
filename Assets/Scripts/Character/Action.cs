using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {
    private bool _pressed;
    private bool _locked;
    private bool _held;
    
    public bool pressed { get { return _pressed; } }
    public bool locked { get { return _locked; } }
    public bool held { get { return _held; } }
    
    public Action() {
        _pressed = false;
        _locked = false;
        _held = false;
    }
    
    public void Press() {
        _pressed = true;
        if (!_locked) {
            _held = true;
        }
    }
    
    public void Release() {
        _pressed = false;
        _held = false;
    }
    
    public void Lock() {
        _locked = true;
    }
    
    public void Unlock() {
        _locked = false;
    }
}
