using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject prefab;
    public TextMeshProUGUI moneyDisplay;
    public Button shopButton;
    public Button redButton, greenButton, blueButton, cyanButton, magentaButton, yellowButton, whiteButton, blackButton;
    public Button capacityButton;
    public Button wallButton;
    public List<Button> buttonList;
    private float spawnRange = 4.5f;
    public int money = 20;
    public int poopCount = 0;
    private FoodManager foodManagerScript;
    private int capacityCost = 25;
    private int wallUpgradeCost = 100;
    private bool wallUpgraded = false;
    

    void Start() {
        blueButton.onClick.AddListener(delegate {Spawn(new Color(0, 0, 1, 1), 10);});
        greenButton.onClick.AddListener(delegate {Spawn(new Color(0, 1, 0, 1), 20);});
        redButton.onClick.AddListener(delegate {Spawn(new Color(1, 0, 0, 1), 30);});
        cyanButton.onClick.AddListener(delegate {Spawn(new Color(0, 1, 1, 1), 30);});
        magentaButton.onClick.AddListener(delegate {Spawn(new Color(1, 0, 1, 1), 40);});
        yellowButton.onClick.AddListener(delegate {Spawn(new Color(1, 1, 0, 1), 50);});
        whiteButton.onClick.AddListener(delegate {Spawn(new Color(1, 1, 1, 1), 60);});
        blackButton.onClick.AddListener(delegate {Spawn(new Color(0, 0, 0, 1), 60);});

        shopButton.onClick.AddListener(ShopSwitch);

        foodManagerScript = GameObject.Find("Food Bowl").GetComponent<FoodManager>();
        capacityButton.onClick.AddListener(CapacityUp);

        wallButton.onClick.AddListener(WallUpgrade);
    }

    // Update is called once per frame
    void Update() {
        moneyDisplay.text = "Money:\n" + money;
        capacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade Capacity\nCost:" + capacityCost;
    }

    public void Spawn(Color color, int cost) {
        if (money >= cost) {
            money -= cost;
            float spawnPosX = Random.Range(-spawnRange, spawnRange);
            float spawnPosZ = Random.Range(-spawnRange, spawnRange);
            GameObject creature = Instantiate(prefab, new Vector3(spawnPosX, 1, spawnPosZ), prefab.transform.rotation);
            AssignColor(creature, color);
        }
    }
    public void AssignColor(GameObject creature, Color color) {
        Material creatureMat = creature.GetComponent<Renderer>().material;
        creatureMat.color = color;
    }

    public void addMoney(int add) {
        money += add;
    }
    public void ShopSwitch() {
        foreach (Button b in buttonList) {
            if (b.gameObject.activeSelf == true) {
                b.gameObject.SetActive(false);
            }
            else {
                b.gameObject.SetActive(true);
                if (b == wallButton && wallUpgraded == true) {
                    b.gameObject.SetActive(false);
                }
            }
        }
    }
    public void CapacityUp() {
        if (money >= capacityCost) {
            money -= capacityCost;
            capacityCost *= 2;
            foodManagerScript.capacity *= 2;
        }
    }
    public void WallUpgrade() {
        if (money >= wallUpgradeCost) {
            money -= wallUpgradeCost;
            wallUpgraded = true;
            wallButton.gameObject.SetActive(false);

            Transform groundScale = GameObject.Find("Floor").GetComponent<Transform>();
            groundScale.localScale = new Vector3(groundScale.localScale.x, groundScale.localScale.y * 2, groundScale.localScale.z);
        }
    }
}
