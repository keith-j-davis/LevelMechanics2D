using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator {
    // Random chance that character blinks in standing state
    public const float BLINK_CHANCE = .85f;
    public const float SFX_FRAME_COUNT = 4;
    public const float SFX_DELAY = .175f;
    public const float SFX_WIDTH = 1f/SFX_FRAME_COUNT;
    
    public static int[] FRAME_COUNT = new int[ANIMATION_FLAG.TOTAL];
    public static float[] DELAY = new float[ANIMATION_FLAG.TOTAL];
    public static float[] WIDTH = new float[ANIMATION_FLAG.TOTAL];
    public static Vector2[] SCALE = new Vector2[ANIMATION_FLAG.TOTAL];
    
    public static void Initialize() {
        // Animation cycle frame counts
        FRAME_COUNT[ANIMATION_FLAG.STAND] = 2;
        FRAME_COUNT[ANIMATION_FLAG.WALK] = 8;
        FRAME_COUNT[ANIMATION_FLAG.RISE] = 2;
        FRAME_COUNT[ANIMATION_FLAG.FALL] = 2;
        FRAME_COUNT[ANIMATION_FLAG.CLIMB] = 4;
        FRAME_COUNT[ANIMATION_FLAG.RAM] = 8;
        FRAME_COUNT[ANIMATION_FLAG.PUSH] = 4;
        FRAME_COUNT[ANIMATION_FLAG.HURT] = 1;
        
        // How long to remain on a single frame
        DELAY[ANIMATION_FLAG.STAND] = .25f;
        DELAY[ANIMATION_FLAG.WALK] = .125f;
        DELAY[ANIMATION_FLAG.RISE] = .375f;
        DELAY[ANIMATION_FLAG.FALL] = .25f;
        DELAY[ANIMATION_FLAG.CLIMB] = .25f;
        DELAY[ANIMATION_FLAG.RAM] = .125f;
        DELAY[ANIMATION_FLAG.PUSH] = .25f;
        DELAY[ANIMATION_FLAG.HURT] = .175f;
        
        // Texture width for a single frame on an animation strip
        WIDTH[ANIMATION_FLAG.STAND] = 1f/FRAME_COUNT[ANIMATION_FLAG.STAND];
        WIDTH[ANIMATION_FLAG.WALK] = 1f/FRAME_COUNT[ANIMATION_FLAG.WALK];
        WIDTH[ANIMATION_FLAG.RISE] = 1f/FRAME_COUNT[ANIMATION_FLAG.RISE];
        WIDTH[ANIMATION_FLAG.FALL] = 1f/FRAME_COUNT[ANIMATION_FLAG.FALL];
        WIDTH[ANIMATION_FLAG.CLIMB] = 1f/FRAME_COUNT[ANIMATION_FLAG.CLIMB];
        WIDTH[ANIMATION_FLAG.RAM] = 1f/FRAME_COUNT[ANIMATION_FLAG.RAM];
        WIDTH[ANIMATION_FLAG.PUSH] = 1f/FRAME_COUNT[ANIMATION_FLAG.PUSH];
        WIDTH[ANIMATION_FLAG.HURT] = 1f/FRAME_COUNT[ANIMATION_FLAG.HURT];
        
        // Texture scale for single frame on an animation strip
        SCALE[ANIMATION_FLAG.STAND] = new Vector2(WIDTH[ANIMATION_FLAG.STAND], 1f);
        SCALE[ANIMATION_FLAG.WALK] = new Vector2(WIDTH[ANIMATION_FLAG.WALK], 1f);
        SCALE[ANIMATION_FLAG.RISE] = new Vector2(WIDTH[ANIMATION_FLAG.RISE], 1f);
        SCALE[ANIMATION_FLAG.FALL] = new Vector2(WIDTH[ANIMATION_FLAG.FALL], 1f);
        SCALE[ANIMATION_FLAG.CLIMB] = new Vector2(WIDTH[ANIMATION_FLAG.CLIMB], 1f);
        SCALE[ANIMATION_FLAG.RAM] = new Vector2(WIDTH[ANIMATION_FLAG.RAM], 1f);
        SCALE[ANIMATION_FLAG.PUSH] = new Vector2(WIDTH[ANIMATION_FLAG.PUSH], 1f);
        SCALE[ANIMATION_FLAG.HURT] = new Vector2(WIDTH[ANIMATION_FLAG.HURT], 1f);
    }
    
    private MeshRenderer _sprite_renderer;
    private MeshRenderer _sfx_renderer;
    private Texture2D[] _texture_array;
    private int _animation_flag;
    private Texture2D _texture;
    private Vector3 _scale;
    private float _width;
    private float _delay;
    private float _offset;
    private float _timer;
    private float _sfx_offset;
    private float _sfx_timer;
    
    public Animator(MeshRenderer sprite_renderer, MeshRenderer sfx_renderer, Texture2D[] texture_array) {
        _sprite_renderer = sprite_renderer;
        _sfx_renderer = sfx_renderer;
        _texture_array = texture_array;
        _animation_flag = ANIMATION_FLAG.STAND;
        _texture = _texture_array[_animation_flag];
        _scale = SCALE[_animation_flag];
        _width = WIDTH[_animation_flag];
        _delay = DELAY[_animation_flag];
        _offset = 0f;
        _timer = 0f;
        _sfx_offset = 1f;
        _sfx_timer = 0f;
    }
    
    public void Set(int animation_flag) {
        // Reset animation if a new flag is set
        if (animation_flag != _animation_flag) {
            _animation_flag = animation_flag;
            _texture = _texture_array[animation_flag];
            _scale = SCALE[animation_flag];
            _width = WIDTH[animation_flag];
            _delay = DELAY[animation_flag];
            _offset = 0f;
            _timer = 0f;
        }
    }
    
    public void ApplySfx() {
        // Reset special effects attributes, which starts animation upon update
        _sfx_offset = 0f;
        _sfx_timer = 0;
    }
    
    public void Update(Observer observer, Character character, bool pause) {
        // Process updates
        _timer += Time.deltaTime;
        if (_timer > _delay) {
            _timer -= _delay;
            // Increment frame unless climbing 'passively' (aka, holding onto ladder w/o moving up or down)
            if (!pause) {
                switch (_animation_flag) {
                    case ANIMATION_FLAG.STAND:
                        // Random chance of blinking
                        _offset = ((_offset == 0f) && (Random.value > BLINK_CHANCE)) ? _width : 0f;
                        break;
                    case ANIMATION_FLAG.RISE:
                        // Stop on 2nd frame
                        _offset = _width;
                        break;
                    case ANIMATION_FLAG.RAM:
                        // Repeat last 3 frames
                        _offset += _width;
                        _offset = (_offset < 1f) ? _offset : _offset - (3 * _width);
                        break;
                    case ANIMATION_FLAG.HURT:
                        character.health.AttackProcessed();
                        break;
                    default:
                        // Cycle animation
                        _offset += _width;
                        _offset = (_offset < 1f) ? _offset : 0f;
                        break;
                }
            }
        }
        
        // Apply updates
        _sprite_renderer.material.mainTexture = _texture;
        _sprite_renderer.material.mainTextureScale = _scale;
        _sprite_renderer.material.mainTextureOffset = new Vector2(_offset, 0f);
        
        // Apply special effects healing, stops after animation finishes
        if (_sfx_offset < 1f) {
            _sfx_timer += Time.deltaTime;
            if (_sfx_timer > SFX_DELAY) {
                _sfx_offset += SFX_WIDTH;
                _sfx_timer -= SFX_DELAY;
                _sfx_renderer.material.mainTextureOffset = new Vector2(_sfx_offset, 0f);
            }
        }
    }
}
