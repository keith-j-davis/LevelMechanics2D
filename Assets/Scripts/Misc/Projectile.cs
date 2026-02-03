using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private Vector3 _translate;
    
    private float _speed;
    
    public float speed { get { return _speed; } }
    
    void Start() {
        _speed = 5f;
    }
    
    public void SetDirection(float direction) {
        // Called by spawner to set direction upon instantiation
        _translate = new Vector3(direction, 0f, 0f);
    }
    
    void Update() {
        this.transform.position += _translate * (_speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            // Self-destructs on collision with walls
            case "Wall":
            case "Breakable Wall":
                Destroy(this.gameObject);
                break;
        }
    }
}