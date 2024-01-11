using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopToMoney : MonoBehaviour {
    private GameManager gameManagerScript;
    private Rigidbody rb;
    public GameObject cleanSand;
    public GameObject dirtySand;
    public int poopCount = 0;
    // Start is called before the first frame update
    void Start() {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (poopCount >= 10) {
            cleanSand.SetActive(false);
            dirtySand.SetActive(true);
        }
        else {
            cleanSand.SetActive(true);
            dirtySand.SetActive(false);
        }
    }

    void OnMouseDown() {
        gameManagerScript.addMoney(10 * poopCount);
        poopCount = 0;
    }
    public void incrementPoop() {
        poopCount++;
    }
}
