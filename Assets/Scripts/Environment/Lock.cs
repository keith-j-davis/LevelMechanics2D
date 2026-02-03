using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock {
    private GameObject _gameObject;
    
    public GameObject gameObject { get { return _gameObject; } }
    
    public Lock() {
        _gameObject = null;
    }
    
    public void UseKey() {
        if (_gameObject) {
            GameObject.Destroy(_gameObject);
            Debug.Log("You unlocked the padlock!");
        } else {
            throw new Exception ("Only call Lock.UseKey() if Observer.PadlockAvailable()!");
        }
    }
    
    public bool IsAvailable() {
        return _gameObject != null;
    }
    
    public void Enter(Collider other) {
        _gameObject = other.gameObject;
    }
    
    public void Exit(Collider other) {
        _gameObject = null;
    }
}
