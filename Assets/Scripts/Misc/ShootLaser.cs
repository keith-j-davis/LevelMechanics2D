using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour {
    // Delay after which each next beam will be fired and countdown time keeper
    private float _direction;
    private float _delay;
    private float _timer;
    
    public float direction { get { return _direction; } }
    
    // Location from which beam will originate
    private Vector3 _spawn_origin;
    
    // Handle to Beam game object prefab
    public GameObject _projectile;
    
    void Start() {
        _direction = transform.localScale.x < 0f ? 1f : -1f;
        _delay = 3f;
        _timer = 0f;
        _spawn_origin = new Vector3(0,.5f,0f);
    }

    void Update() {
        // React to time keeper exceeding delay (fire beam)
        _timer += Time.deltaTime;
        if (_timer > _delay) {
            GameObject projectile = Instantiate(_projectile, this.transform.position + _spawn_origin, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(_direction);
            _timer -= _delay;
        }
    }
}
