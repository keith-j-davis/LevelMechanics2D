using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer {
    private Character _character;
    private Environment _environment;
    
    public Observer(Character character, Environment environment) {
        _character = character;
        _environment = environment;
    }
    
    // Character observations
    public bool FoodAvailable() { return _character.inventory.food > 0; }
    public bool KeyAvailable() { return _character.inventory.keys > 0; }
    
    public bool IsInjured() { return _character.health.hp < _character.health.max_hp; }
    public bool IsAlive() { return _character.health.hp > 0; }
    public bool WearingArmor() { return _character.health.armor; }
    public bool WasAttacked() { return _character.health.attacked; }
    
    public bool IsRising() { return _character.activity.velocity > 0f; }
    public bool IsFalling() { return _character.activity.velocity < 0f; }
    public bool IsGrounded() { return _character.activity.velocity == 0f; }
    
    public bool IsJumpHeld() { return _character.activity.jump.held; }
    
    public bool IsClimbLocked() { return _character.activity.climb.locked; }
    public bool IsJumpLocked() { return _character.activity.jump.locked; }
    public bool IsRamLocked() { return _character.activity.ram.locked; }
    public bool IsInteractLocked() { return _character.activity.interact.locked; }
    public bool IsUseItemLocked() { return _character.activity.use_item.locked; }
    
    // Environment observations
    public bool PlatformCollision() { return _environment.platform.IsInCollisionBox(); }
    public bool WallCollision() { return _environment.wall.IsInCollisionBox(); }
    public bool BreakableWallCollision() { return _environment.breakable_wall.IsInCollisionBox(); }
    public bool LadderAvailable() { return _environment.ladder.IsAvailable(); }
    public bool PadlockAvailable() { return _environment.interactable_lock.IsAvailable(); }
    public bool IsAtGoal() { return _environment.goal.IsInCollisionBox(); }
    
    public bool OnTopOfLadder() {
        bool above = _character.activity.bottom >= _environment.ladder.north;
        bool near_y = Mathf.Abs(_character.activity.bottom - _environment.ladder.north) < 1f;
        bool near_x = Mathf.Abs(_environment.ladder.x - _character.activity.x) < 2f;
        return above && near_y && near_x;
    }
    public bool AtBottomOfLadder() {
        bool below = (_character.activity.bottom <= _environment.ladder.south);
        bool near_y = Mathf.Abs(_character.activity.bottom - _environment.ladder.south) < 1f;
        bool near_x = Mathf.Abs(_environment.ladder.x - _character.activity.x) < 2f;
        return below && near_y && near_x;
    }
    
    // Steps in episode
    public int StepCount() {
        return _character.activity.steps;
    }
}
