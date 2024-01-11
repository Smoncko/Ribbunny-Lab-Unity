using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    // Start is called before the first frame update
    private Rigidbody creatureRb;
    private Incubator incubatorScript;
    private PoopToMoney poopManagerScript;
    private FoodManager foodManagerScript;
    private GameObject incubator;
    private GameObject food;
    private GameObject puddle;
    private GameObject toilet;
    public bool inBounds;
    public float boundX;
    public float boundZ;
    public float hungerLevel = 100;
    public float waterLevel = 100;
    public float toiletLevel = 0;
    public GameObject pellet;
    void Start() {
        creatureRb = GetComponent<Rigidbody>();

        food = GameObject.FindWithTag("Food Bowl");
        puddle = GameObject.FindWithTag("Water Puddle");
        toilet = GameObject.FindWithTag("Litter Box");

        InvokeRepeating("Move", 1.0f, 1.5f);

        float hungerTick = UnityEngine.Random.Range(0.2f, 1.2f);
        InvokeRepeating("Hunger", 1.0f, hungerTick);

        float waterTick = UnityEngine.Random.Range(0.2f, 1.2f);
        InvokeRepeating("Water", 1.0f, waterTick);

        incubator = GameObject.Find("Incubator");
        incubatorScript = incubator.GetComponent<Incubator>();
        poopManagerScript = toilet.GetComponent<PoopToMoney>();
        foodManagerScript = food.GetComponent<FoodManager>();
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.x >= -boundX && transform.position.x <= boundX && transform.position.z >= -boundZ && transform.position.z <= boundZ) {
            inBounds = true;
        }
        else {
            inBounds = false;
        }
        if (transform.position.y < 0) {
            transform.position += Vector3.up;
        }
    }

    void OnMouseDown() {
        List<GameObject> bl = incubatorScript.GetBreedList();
        if (!inBounds) {
            StartCoroutine(MoveTo(Vector3.up * 4));
            if (bl.IndexOf(gameObject) == 0 && bl.Count == 2) {
                bl[1].transform.position += Vector3.left * 2;
            }
            bl.Remove(gameObject);
        }
        else {
            if (bl.Count < 2) {
                StartCoroutine(MoveTo(incubator.transform.position + Vector3.left + Vector3.right * 2 * bl.Count));
                bl.Add(gameObject);
            }
        }
    }

    IEnumerator MoveTo(Vector3 end) {
        int frameCount = 15;
        int elapsed = 0;
        Quaternion s = transform.rotation;
        // Debug.Log("Start:" + start);
        Quaternion e = new Quaternion(0, Mathf.Atan(end.x / end.z) + ((end.z >= 0) ? 0 : ((end.x >= 0) ? 180 : -180)), 0, 1);
        // Debug.Log("End: " + end);
        while (elapsed < frameCount) {
            yield return new WaitForFixedUpdate();
            float interpolationRatio = (float) elapsed / frameCount;
            transform.rotation = Quaternion.Lerp(s, e, interpolationRatio);
            // Debug.Log(transform.rotation);
            elapsed++;
        }

        frameCount = 75;
        elapsed = 0;
        Vector3 start = transform.position;
        while (elapsed < frameCount) {
            yield return new WaitForFixedUpdate();
            float interpolationRatio = (float) elapsed / frameCount;
            transform.position = Vector3.Lerp(start, end, interpolationRatio);
            elapsed++;
        }
    }

    private void Move() {
        if (inBounds) {
            if (hungerLevel < 10) { //Move to food bowl
                StartCoroutine(MoveTo(food.transform.position + new Vector3(1, 0.5f, -1)));
                if (foodManagerScript.GetFood() > 0) {
                    hungerLevel = 100;
                    Litter(10);
                    foodManagerScript.DecFood();
                }
            }
            else if (waterLevel < 10) { //Move to water puddle
                StartCoroutine(MoveTo(puddle.transform.position));
                waterLevel = 100;
                Litter(5);
            }
            else if (toiletLevel >= 20) { //Move to litter box
                StartCoroutine(Poop(toilet.transform.position));
            }
            else {  //Normal movement
                StartCoroutine(Jump());
            }
        }
    }

    private void Hunger() {
        if (hungerLevel > 0) {
            hungerLevel--;
        }
    }
    private void Water() {
        if (waterLevel > 0) {
            waterLevel--;
        }
    }
    private void Litter(int increase) {
        toiletLevel += increase;
    }

    IEnumerator Poop(Vector3 end) {
        toiletLevel = 0;

        // IEnumerator enum1;
        // while ((enum1 = MoveTo(end)) != null) yield return enum1;

        int frameCount = 90;
        int elapsed = 0;
        Vector3 start = transform.position;
        while (elapsed < frameCount) {
            yield return new WaitForFixedUpdate();
            float interpolationRatio = (float) elapsed / frameCount;
            transform.position = Vector3.Lerp(start, end, interpolationRatio);
            elapsed++;
        }

        // creatureRb.AddForce(Vector3.up * 2, ForceMode.Impulse);
        // GameObject poop = Instantiate(pellet, gameObject.transform.position + new Vector3(0.1f, 0, 0.1f), gameObject.transform.rotation);
        // poop.GetComponent<Rigidbody>().AddForce(Vector3.up * 2, ForceMode.Impulse);

        poopManagerScript.incrementPoop();
    }

    IEnumerator Jump() {
        float dirX = UnityEngine.Random.Range(-2, 2);
        float dirY = UnityEngine.Random.Range(1, 5);
        float dirZ = UnityEngine.Random.Range(-2, 2);

        int frameCount = 15;
        int elapsed = 0;
        Quaternion start = transform.rotation;
        // Debug.Log("Start:" + start);
        Quaternion end = new Quaternion(0, Mathf.Atan(dirX / dirZ) + ((dirZ >= 0) ? 0 : ((dirX >= 0) ? 180 : -180)), 0, 1);
        // Debug.Log("End: " + end);
        while (elapsed < frameCount) {
            yield return new WaitForFixedUpdate();
            float interpolationRatio = (float) elapsed / frameCount;
            transform.rotation = Quaternion.Lerp(start, end, interpolationRatio);
            // Debug.Log(transform.rotation);
            elapsed++;
        }

        creatureRb.AddForce(new Vector3(dirX, dirY, dirZ), ForceMode.Impulse);
    }
}
