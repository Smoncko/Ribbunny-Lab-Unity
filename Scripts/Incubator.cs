using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Incubator : MonoBehaviour {
    public List<GameObject> breedingList = new List<GameObject>(2);
    public GameObject prefab;
    private GameManager gameManagerScript;
    public TextMeshProUGUI countdownText;
    public int startCountdown = 60;
    private bool occupied = false;

    // Start is called before the first frame update
    void Start() {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnMouseDown() {
        if (breedingList.Count == 2 && !occupied) {
            GameObject parent1 = breedingList[0];
            GameObject parent2 = breedingList[1];

            Color c1 = parent1.GetComponent<Renderer>().material.color;
            Color c2 = parent2.GetComponent<Renderer>().material.color;
            Color c3 = (c1 + c2) / 2;

            StartCoroutine(CountToZero(startCountdown, c3, 0));

            StartCoroutine(MoveTo(parent1, Vector3.up * 2));
            StartCoroutine(MoveTo(parent2, Vector3.up * 3));
            breedingList.Clear();
        }
    }

    public List<GameObject> GetBreedList() {
        return breedingList;
    }

    IEnumerator MoveTo(GameObject go, Vector3 end) {
        int frameCount = 60;
        int elapsed = 0;
        Vector3 start = go.transform.position;
        while (elapsed < frameCount) {
            yield return new WaitForFixedUpdate();
            float interpolationRatio = (float) elapsed / frameCount;
            go.transform.position = Vector3.Lerp(start, end, interpolationRatio);
            elapsed++;
        }
    }

    IEnumerator CountToZero(int start, Color c, int cost) {
        int cur = start;

        countdownText.gameObject.SetActive(true);
        occupied = true;

        while (cur >= 0) {
            countdownText.text = cur.ToString();
            cur--;
            yield return new WaitForSecondsRealtime(1);
        }

        gameManagerScript.Spawn(c, cost);

        countdownText.gameObject.SetActive(false);
        occupied = false;
    }
}
