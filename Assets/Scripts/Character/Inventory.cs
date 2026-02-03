using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    public const int MAX_ITEM_CAPACITY = 4;
    
    private int _food;
    private int _keys;
    
    public int food { get { return _food; } }
    public int keys { get { return _keys; } }
    
    public Inventory() {
        _food = 0;
        _keys = 0;
    }
    
    public void Reset() {
        _food = 0;
        _keys = 0;
    }
    
    public void Add(GameObject gameObject) {
        if ((_food + _keys) < MAX_ITEM_CAPACITY) {
            switch (gameObject.tag) {
                case Environment.FOOD_TAG:
                    _food += 1;
                    break;
                case Environment.KEY_TAG:
                    _keys += 1;
                    break;
                default:
                    throw new Exception("Missing item tag in Inventory.Add(GameObject)!");
            }
            Debug.Log("Added " + gameObject.tag + " to inventory (keys:" + _keys.ToString() + ",food:" + _food.ToString() + ")");
            GameObject.Destroy(gameObject);
        } else {
            Debug.Log("Inventory is full, can't add " + gameObject.tag + " to inventory (keys:" + _keys.ToString() + ",food:" + _food.ToString() + ")");
        }
    }
    
    public void Remove(string item_tag) {
        switch (item_tag) {
            case Environment.FOOD_TAG:
                if (_food > 0) {
                    _food -= 1;
                    Debug.Log("Removed food from inventory (keys:" + _keys.ToString() + ",food:" + _food.ToString() + ")");
                } else {
                    throw new Exception("Only use Inventory.Remove(Environment.FOOD_TAG) if Observer.FoodAvailable()!");
                }
                break;
            case Environment.KEY_TAG:
                if (_keys > 0) {
                    _keys -= 1;
                    Debug.Log("Removed key from inventory (keys:" + _keys.ToString() + ",food:" + _food.ToString() + ")");
                } else {
                    throw new Exception("Only use Inventory.Remove(Environment.FOOD_TAG) if Observer.KeyAvailable()!");
                }
                break;
            default:
                throw new Exception("Missing item tag in Inventory.Remove(GameObject)!");
        }
    }
}
