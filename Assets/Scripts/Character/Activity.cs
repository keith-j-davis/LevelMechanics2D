using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity {
    public const float HORIZONTAL_SPEED = 8f;
    public const float CLIMB_SPEED = 6f;
    
    public static Vector3 RIGHT = new Vector3(1f, 0f, 0f) * HORIZONTAL_SPEED;
    public static Vector3 LEFT = new Vector3(-1f, 0f, 0f) * HORIZONTAL_SPEED;
    public static Vector3 UP = new Vector3(0f, 1f, 0f) * CLIMB_SPEED;
    public static Vector3 DOWN = new Vector3(0f, -1f, 0f) * CLIMB_SPEED;
    
    public const float JUMP_VELOCITY = 8f;
    public const float JUMP_ACCELERATION = 3f;
    
    public static Vector3 JUMP = new Vector3(0f, 1f, 0f) * JUMP_VELOCITY;
    public static Vector3 JUMP_HIGHER = new Vector3(0f, 1f, 0f) * JUMP_ACCELERATION;
    
    private Rigidbody _rigidbody;
    
    private int _steps;
    private Vector2 _direction_flag;
    
    public Action move;
    public Action climb;
    public Action jump;
    public Action ram;
    public Action interact;
    public Action use_item;
    
    public int steps { get { return _steps; } }
    public float velocity { get { return _rigidbody.velocity.y; } }
    public Vector2 direction_flag { get { return _direction_flag; } }
    public float radius { get { return _rigidbody.transform.localScale.y / 2; } }
    public float bottom { get { return _rigidbody.position.y - radius; } }
    public float x { get { return _rigidbody.position.x; } }
    
    public Activity(Rigidbody rigidbody) {
        _steps = 0;
        _rigidbody = rigidbody;
        
        _direction_flag = new Vector2(DIRECTION_FLAG.ZERO, DIRECTION_FLAG.ZERO);
        move = new Action();
        climb = new Action();
        jump = new Action();
        ram = new Action();
        interact = new Action();
        use_item = new Action();
    }
    
    public void Increment() {
        _steps += 1;
    }
    
    public void Reset() {
        _steps = 0;
        _direction_flag = new Vector2(DIRECTION_FLAG.ZERO, DIRECTION_FLAG.ZERO);
        move.Unlock();
        climb.Unlock();
        jump.Unlock();
        ram.Unlock();
        interact.Unlock();
        use_item.Unlock();
        DisableGravity();
        EnableGravity();
        SetDirection (DIRECTION_FLAG.POSITIVE, DIRECTION_FLAG.ZERO);
    }
    
    public void EnableGravity() {
        _rigidbody.useGravity = true;
    }
    
    public void DisableGravity() {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }
    
    public void SetDirection(int direction_flag_x, int direction_flag_y) {
        float x = Mathf.Abs(_rigidbody.transform.localScale.x);
        if (direction_flag_x == DIRECTION_FLAG.NEGATIVE) {
            _rigidbody.transform.localScale = new Vector3(-x, _rigidbody.transform.localScale.y, _rigidbody.transform.localScale.z);
        } else if (direction_flag_x == DIRECTION_FLAG.POSITIVE) {
            _rigidbody.transform.localScale = new Vector3(x, _rigidbody.transform.localScale.y, _rigidbody.transform.localScale.z);
        }
        _direction_flag = new Vector2(direction_flag_x, direction_flag_y);
    }
    
    public void CenterOnX(float x) {
        _rigidbody.position = new Vector3(x, _rigidbody.position.y, _rigidbody.position.z);
    }
    
    public void BottomOnY(float y) {
        _rigidbody.position = new Vector3(_rigidbody.position.x, y + radius, _rigidbody.position.z);
    }
    
    public void Translate(int action_flag, int direction_flag) {
        Vector3 position = _rigidbody.transform.position;
        Vector3 deltaPosition = Vector3.zero;
        if (action_flag == ACTION_FLAG.CLIMB) {
            switch (direction_flag) {
                case DIRECTION_FLAG.NEGATIVE:
                    deltaPosition = DOWN;
                    break;
                case DIRECTION_FLAG.POSITIVE:
                    deltaPosition = UP;
                    break;
            }
        } else {
            switch (direction_flag) {
                case DIRECTION_FLAG.NEGATIVE:
                    deltaPosition = LEFT;
                    break;
                case DIRECTION_FLAG.POSITIVE:
                    deltaPosition = RIGHT;
                    break;
            }
        }
        
        position += (deltaPosition * Time.deltaTime);
        _rigidbody.MovePosition(position);
    }
    
    public void AccelerateJump() {
        _rigidbody.AddForce(JUMP_HIGHER, ForceMode.Acceleration);
    }
    
    public void StartJump() {
        _rigidbody.AddForce(JUMP, ForceMode.VelocityChange);
    }
}
