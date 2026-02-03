using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Controller {
    // Directional input function callers
    public static Func<int> X = () => (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
    public static Func<int> Y = () => (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
    
    // Action input function callers
    public static Func<bool> JUMP = () => Input.GetKey(KeyCode.Q);
    public static Func<bool> RAM = () => Input.GetKey(KeyCode.W);
    public static Func<bool> INTERACT = () => Input.GetKey(KeyCode.E);
    public static Func<bool> USE_ITEM = () => Input.GetKey(KeyCode.R);
    
    private Character _character;
    private Environment _environment;
    private Observer _observer;
    private Animator _animator;
    
    public Controller(Character character, Environment environment, Observer observer, Animator animator) {
        _character = character;
        _environment = environment;
        _observer = observer;
        _animator = animator;
    }
    
    public void Move(int direction) {
        _character.activity.move.Press();
        if (direction == DIRECTION_FLAG.ZERO) {
            if (_observer.IsClimbLocked()) {
                // Hang
            } else {
                // Do nothing
            }
        } else {
            if (_observer.IsClimbLocked()) {
                // Let go
                _character.activity.climb.Unlock();
            }
            // Move
            _character.activity.Translate(ACTION_FLAG.MOVE, direction);
        }
    }
    
    public void Climb(int direction) {
        _character.activity.climb.Press();
        if (_observer.LadderAvailable()) {
            // Climb
            if (_observer.OnTopOfLadder() && (direction == DIRECTION_FLAG.POSITIVE)) {
                // Do nothing
            } else if (_observer.AtBottomOfLadder() && (direction == DIRECTION_FLAG.NEGATIVE)) {
                _character.activity.climb.Unlock();
            } else {
                _character.activity.climb.Lock();
                _character.activity.Translate(ACTION_FLAG.CLIMB, direction);
                _character.activity.CenterOnX(_environment.ladder.x);
            }
        } else {
            // Let go
            _character.activity.climb.Unlock();
            if (_observer.OnTopOfLadder()) {
                _character.activity.BottomOnY(_environment.ladder.north);
            }
        }
    }
    
    public void Jump(int direction) {
        _character.activity.jump.Press();
        if (_observer.IsClimbLocked()) {
            // Let go
            _character.activity.climb.Unlock();
        } else if (_observer.IsRamLocked()) {
            // Continue ramming
        } else if (_observer.IsGrounded()) {
            // Jump
            _character.activity.StartJump();
            _character.activity.jump.Lock();
        } else if (_observer.IsJumpLocked() && _observer.IsJumpHeld()) {
            // Accelerate Jump
            _character.activity.AccelerateJump();
        } else {
            // Free-fall
        }
        _character.activity.Translate(ACTION_FLAG.MOVE, direction);
    }
    
    public void Ram(int direction) {
        _character.activity.ram.Press();
        if ((direction == DIRECTION_FLAG.ZERO) || _observer.IsClimbLocked()) {
            // Do nothing
        } else if (!_observer.IsGrounded()) {
            // Free-fall
            _character.activity.Translate(ACTION_FLAG.MOVE, direction);
        } else {
            _character.activity.ram.Lock();
            _character.activity.Translate(ACTION_FLAG.MOVE, direction);
        }
    }
    
    public void Interact(int direction) {
        _character.activity.interact.Press();
        if (!_observer.IsInteractLocked()) {
            if (_observer.PadlockAvailable()) {
                _character.activity.interact.Lock();
                if (_observer.KeyAvailable()) {
                    _character.inventory.Remove(Environment.KEY_TAG);
                    _environment.interactable_lock.UseKey();
                } else {
                    Debug.Log("Sorry, you need a key!");
                }
            } else {
                Debug.Log("Sorry, no padlock to interact with!");
            }
        }
    }
    
    public void UseItem(int direction) {
        _character.activity.use_item.Press();
        if (!_observer.IsUseItemLocked()) {
            _character.activity.use_item.Lock();
            if (_observer.FoodAvailable()) {
                if (_observer.IsInjured()) {
                    _character.inventory.Remove(Environment.FOOD_TAG);
                    _character.health.Heal();
                    _animator.ApplySfx();
                } else {
                    Debug.Log("You are already at full health!");
                }
            } else {
                Debug.Log("Sorry, you have no food!");
            }
        }
    }
    
    // COMMAND CALLER
    public void Call(int input0, int input1) {
        int axis;
        int action;
        int direction;
        switch (input0) {
            case INPUT0.MOVE_LEFT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.MOVE;
                direction = DIRECTION_FLAG.NEGATIVE;
                break;
            case INPUT0.MOVE_RIGHT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.MOVE;
                direction = DIRECTION_FLAG.POSITIVE;
                break;
            case INPUT0.CLIMB_UP:
                axis = AXIS_FLAG.Y;
                action = ACTION_FLAG.CLIMB;
                direction = DIRECTION_FLAG.POSITIVE;
                break;
            case INPUT0.CLIMB_DOWN:
                axis = AXIS_FLAG.Y;
                action = ACTION_FLAG.CLIMB;
                direction = DIRECTION_FLAG.NEGATIVE;
                break;
            case INPUT0.RAM_LEFT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.RAM;
                direction = DIRECTION_FLAG.NEGATIVE;
                break;
            case INPUT0.RAM_RIGHT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.RAM;
                direction = DIRECTION_FLAG.POSITIVE;
                break;
            case INPUT0.JUMP:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.JUMP;
                direction = DIRECTION_FLAG.ZERO;
                break;
            case INPUT0.JUMP_LEFT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.JUMP;
                direction = DIRECTION_FLAG.NEGATIVE;
                break;
            case INPUT0.JUMP_RIGHT:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.JUMP;
                direction = DIRECTION_FLAG.POSITIVE;
                break;
            default:
                axis = AXIS_FLAG.X;
                action = ACTION_FLAG.MOVE;
                direction = DIRECTION_FLAG.ZERO;
                break;
        }
        
        int subaction;
        switch (input1) {
            case INPUT1.USE_KEY:
                subaction = ACTION_FLAG.INTERACT;
                break;
            case INPUT1.EAT_FOOD:
                subaction = ACTION_FLAG.USE_ITEM;
                break;
            default:
                subaction = -1;
                break;
        }
        
        // Release conditions
        if (action != ACTION_FLAG.MOVE) {
            _character.activity.move.Release();
        }
        if (action != ACTION_FLAG.CLIMB) {
            _character.activity.climb.Release();
        }
        if (action != ACTION_FLAG.JUMP) {
            _character.activity.jump.Release();
        }
        if (action != ACTION_FLAG.RAM) {
            _character.activity.ram.Release();
        }
        if (subaction != ACTION_FLAG.INTERACT) {
            _character.activity.interact.Release();
            _character.activity.interact.Unlock();
        }
        if (subaction != ACTION_FLAG.USE_ITEM) {
            _character.activity.use_item.Release();
            _character.activity.use_item.Unlock();
        }
        
        // Unlock conditions
        if (_observer.IsGrounded()) {
            _character.activity.jump.Unlock();
        }
        if ((direction == DIRECTION_FLAG.ZERO) || (direction != _character.activity.direction_flag.x) || _observer.WallCollision()) {
            _character.activity.ram.Unlock();
        }
        
        if (_observer.WasAttacked()) {
            // Applies the 'hurt' status
            _character.activity.climb.Unlock();
            _character.activity.jump.Unlock();
            _character.activity.ram.Unlock();
            _character.activity.EnableGravity();
            _animator.Set(ANIMATION_FLAG.HURT);
        } else {
            // Calls the indicated action command
            switch (action) {
                case ACTION_FLAG.MOVE:
                    Move(direction);
                    break;
                case ACTION_FLAG.CLIMB:
                    Climb(direction);
                    break;
                case ACTION_FLAG.JUMP:
                    Jump(direction);
                    break;
                case ACTION_FLAG.RAM:
                    Ram(direction);
                    break;
            }
            switch (subaction) {
                case ACTION_FLAG.INTERACT:
                    Interact(direction);
                    break;
                case ACTION_FLAG.USE_ITEM:
                    UseItem(direction);
                    break;
            }
            
            // Manage gravity
            if (_observer.IsClimbLocked()) {
                _character.activity.DisableGravity();
            } else if (_observer.OnTopOfLadder()) {
                // Do nothing
            } else {
                _character.activity.EnableGravity();
            }
            
            // Applies the correct animation
            if (_observer.IsRising()) {
                _animator.Set(ANIMATION_FLAG.RISE);
            } else if (_observer.IsFalling()) {
                _animator.Set(ANIMATION_FLAG.FALL);
            } else if (_observer.IsClimbLocked()) {
                _animator.Set(ANIMATION_FLAG.CLIMB);
            } else if (_observer.IsRamLocked()) {
                _animator.Set(ANIMATION_FLAG.RAM);
                if (_observer.BreakableWallCollision()) {
                    _environment.breakable_wall.Break();
                }
            } else if ((axis == AXIS_FLAG.Y) || (direction == DIRECTION_FLAG.ZERO)) {
                _animator.Set(ANIMATION_FLAG.STAND);
            } else if (_observer.WallCollision() || _observer.BreakableWallCollision()) {
                _animator.Set(ANIMATION_FLAG.PUSH);
            } else {
                _animator.Set(ANIMATION_FLAG.WALK);
            }
        }
        
        // Update direction
        if (axis == AXIS_FLAG.X) {
            _character.activity.SetDirection(direction, DIRECTION_FLAG.ZERO);
        } else {
            _character.activity.SetDirection(DIRECTION_FLAG.ZERO, direction);
        }
        
        // Call animation updater
        bool pause = _observer.IsClimbLocked() && (direction == DIRECTION_FLAG.ZERO);
        _animator.Update(_observer, _character, pause);
    }
    
    // FUNCTION
    public void GetPlayerInput(in ActionBuffers actionsOut) {
        // Only applies when human user is providing input
        int x = X();
        int y = Y();
        
        bool jump = JUMP();
        bool ram = RAM();
        bool use_key = INTERACT();
        bool eat_food = USE_ITEM();
        
        int input0;
        if (y > 0) {
            input0 = INPUT0.CLIMB_UP;
        } else if (y < 0) {
            input0 = INPUT0.CLIMB_DOWN;
        } else if (x < 0) {
            if (ram) {
                input0 = INPUT0.RAM_LEFT;
            } else if (jump) {
                input0 = INPUT0.JUMP_LEFT;
            } else {
                input0 = INPUT0.MOVE_LEFT;
            }
        } else if (x > 0) {
            if (ram) {
                input0 = INPUT0.RAM_RIGHT;
            } else if (jump) {
                input0 = INPUT0.JUMP_RIGHT;
            } else {
                input0 = INPUT0.MOVE_RIGHT;
            }
        } else {
            if (jump) {
                input0 = INPUT0.JUMP;
            } else {
                input0 = INPUT0.PASSIVE;
            }
        }
        
        int input1;
        if (eat_food) {
            input1 = INPUT1.EAT_FOOD;
        } else if (use_key) {
            input1 = INPUT1.USE_KEY;
        } else {
            input1 = INPUT1.PASSIVE;
        }
        
        var inputs = actionsOut.DiscreteActions;
        inputs[0] = input0;
        inputs[1] = input1;
    }
}
