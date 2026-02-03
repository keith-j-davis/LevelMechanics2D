using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INPUT0 {
    public const int PASSIVE = 0;
    public const int MOVE_LEFT = 1;
    public const int MOVE_RIGHT = 2;
    public const int CLIMB_UP = 3;
    public const int CLIMB_DOWN = 4;
    public const int RAM_LEFT = 5;
    public const int RAM_RIGHT = 6;
    public const int JUMP = 7;
    public const int JUMP_LEFT = 8;
    public const int JUMP_RIGHT = 9;
}

public class INPUT1 {
    public const int PASSIVE = 0;
    public const int USE_KEY = 1;
    public const int EAT_FOOD = 2;
}

public class ANIMATION_FLAG {
    // Enumerated animation cycle
    public const int STAND = 0;
    public const int WALK = 1;
    public const int RISE = 2;
    public const int FALL = 3;
    public const int CLIMB = 4;
    public const int RAM = 5;
    public const int PUSH = 6;
    public const int HURT = 7;
    public const int TOTAL = 8;
}

public class ACTION_FLAG {
    // Action enumeration
    public const int MOVE = 0;
    public const int CLIMB = 1;
    public const int JUMP = 2;
    public const int RAM = 3;
    public const int INTERACT = 4;
    public const int USE_ITEM = 5;
}

public class AXIS_FLAG {
    // Axis enumeration
    public const int X = 0;
    public const int Y = 1;
}

public class DIRECTION_FLAG {
    // Direction enumeration
    public const int NEGATIVE = 0;
    public const int ZERO = 1;
    public const int POSITIVE = 2;
}
