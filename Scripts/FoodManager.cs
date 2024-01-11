using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour {
    public int filled;
    public int capacity = 10;
    public GameObject foodFull;
    public GameObject foodHalf;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // 0
        // 1, 2, 3, 4, 5
        // 6, 7, 8, 9, 10
        if (filled >= capacity / 2) {
            foodFull.SetActive(true);
            foodHalf.SetActive(false);
        }
        else if (filled >= 1) {
            foodFull.SetActive(false);
            foodHalf.SetActive(true);
        }
        else if (filled == 0) {
            foodFull.SetActive(false);
            foodHalf.SetActive(false);
        }
    }

    void OnMouseDown() {
        filled = capacity;
    }

    public void DecFood() {
        filled--;
    }
    public int GetFood() {
        return filled;
    }
}
