using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour
{


    public GameObject inventory;
    public GameObject characterSystem;
    public GameObject craftSystem;
    private Inventory craftSystemInventory;
    private CraftSystem cS;
    private Inventory mainInventory;
    private Inventory characterSystemInventory;
    private Tooltip toolTip;

    private InputManager inputManagerDatabase;

    Image hpImage;
    Image manaImage;
    Image barreXp;
    private Text level;
    private Text txtHP;
    private Text txtMana;
    private Text txtExp;

    public int lvlPlayer = 1;
    
    float maxHealth = 100;
    float maxMana = 100;
    float maxDamage = 0;
    float maxArmor = 0;

    public float currentHealth = 60;
    public float currentMana = 40;
    public float currentDamage = 0;
    public float currentArmor = 0;
    public float currentXp = 0;
    public float rateXp = 1.2F;
    public float rateDam = 1.4F;
    public float rateArm = 1.2F;
    public float rateHealth = 1.4F;
    public float rateMana = 1.2F;
    public float maxXp = 100;

    int normalSize = 3;

    private CharacterMotor characterMotor;
    private Animation playerAnimation;
    private EnemyAI enemyAi;
    private EnemyAILancer enemyAiLancer;

    public void OnEnable()
    {
        Inventory.ItemEquip += OnBackpack;
        Inventory.UnEquipItem += UnEquipBackpack;

        Inventory.ItemEquip += OnGearItem;
        Inventory.ItemConsumed += OnConsumeItem;
        Inventory.UnEquipItem += OnUnEquipItem;

        Inventory.ItemEquip += EquipWeapon;
        Inventory.UnEquipItem += UnEquipWeapon;
    }

    public void OnDisable()
    {
        Inventory.ItemEquip -= OnBackpack;
        Inventory.UnEquipItem -= UnEquipBackpack;

        Inventory.ItemEquip -= OnGearItem;
        Inventory.ItemConsumed -= OnConsumeItem;
        Inventory.UnEquipItem -= OnUnEquipItem;

        Inventory.UnEquipItem -= UnEquipWeapon;
        Inventory.ItemEquip -= EquipWeapon;
    }

    void EquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //add the weapon if you unequip the weapon
        }
    }

    void UnEquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //delete the weapon if you unequip the weapon
        }
    }

    void OnBackpack(Item item)
    {
        if (item.itemType == ItemType.Backpack)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (mainInventory == null)
                    mainInventory = inventory.GetComponent<Inventory>();
                mainInventory.sortItems();
                if (item.itemAttributes[i].attributeName == "Slots")
                    changeInventorySize(item.itemAttributes[i].attributeValue);
            }
        }
    }

    void UnEquipBackpack(Item item)
    {
        if (item.itemType == ItemType.Backpack)
            changeInventorySize(normalSize);
    }

    void changeInventorySize(int size)
    {
        dropTheRestItems(size);

        if (mainInventory == null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (size == 3)
        {
            mainInventory.width = 3;
            mainInventory.height = 1;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        if (size == 6)
        {
            mainInventory.width = 3;
            mainInventory.height = 2;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 12)
        {
            mainInventory.width = 4;
            mainInventory.height = 3;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 16)
        {
            mainInventory.width = 4;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 24)
        {
            mainInventory.width = 6;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
    }

    void dropTheRestItems(int size)
    {
        if (size < mainInventory.ItemsInInventory.Count)
        {
            for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
            {
                GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
                dropItem.AddComponent<PickUpItem>();
                dropItem.GetComponent<PickUpItem>().item = mainInventory.ItemsInInventory[i];
                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            }
        }
    }

    void Start()
    {
        hpImage = GameObject.Find("currentHP").GetComponent<Image>();
        manaImage = GameObject.Find("currentMana").GetComponent<Image>();
        barreXp = GameObject.Find("currentEXP").GetComponent<Image>();
        level = GameObject.Find("Level").GetComponent<Text>();
        txtHP = GameObject.Find("txtHP").GetComponent<Text>();
        txtMana = GameObject.Find("txtMana").GetComponent<Text>();
        txtExp = GameObject.Find("txtEXP").GetComponent<Text>();
        enemyAi = gameObject.GetComponent<EnemyAI>();
        enemyAiLancer = gameObject.GetComponent<EnemyAILancer>();
        characterMotor = gameObject.GetComponent<CharacterMotor>();
        playerAnimation = gameObject.GetComponent<Animation>();

        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        if (craftSystem != null)
            cS = craftSystem.GetComponent<CraftSystem>();

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (inventory != null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (characterSystem != null)
            characterSystemInventory = characterSystem.GetComponent<Inventory>();
        if (craftSystem != null)
            craftSystemInventory = craftSystem.GetComponent<Inventory>();
        SetLevelText();
    }

    public void ApplyDamage(float Damage)
    {
        if (!characterMotor.isDead)
        {
            currentHealth -= (Damage - ((currentArmor * Damage) / 100));
            if (currentHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        characterMotor.isDead = true; //Désactive la possibilité du perso à se déplacer
        playerAnimation.Play("die"); 
    }


    public void OnConsumeItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
            {
                if ((currentHealth + item.itemAttributes[i].attributeValue) > maxHealth)
                    currentHealth = maxHealth;
                else
                    currentHealth += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Mana")
            {
                if ((currentMana + item.itemAttributes[i].attributeValue) > maxMana)
                    currentMana = maxMana;
                else
                    currentMana += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Armor")
            {
                if ((currentArmor + item.itemAttributes[i].attributeValue) > maxArmor)
                    currentArmor = maxArmor;
                else
                    currentArmor += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Damage")
            {
                if ((currentDamage + item.itemAttributes[i].attributeValue) > maxDamage)
                    currentDamage = maxDamage;
                else
                    currentDamage += item.itemAttributes[i].attributeValue;
            }
        }

    }

    public void OnGearItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
                currentHealth += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Mana")
                currentMana += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Armor")
                currentArmor += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
                currentDamage += item.itemAttributes[i].attributeValue;
        }

    }

    public void OnUnEquipItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
                currentHealth -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Mana")
                currentMana -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Armor")
                currentArmor -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
                currentDamage -= item.itemAttributes[i].attributeValue;
        }
 
    }

    public void GainExp(float exp)
    {
        currentXp = currentXp + exp;
        if (currentXp >= maxXp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        float rest = currentXp - maxXp; // On récupère l'Xp qui dépasse le seuil maximum
        lvlPlayer++;
        currentXp = 0 + rest; // Pour le restituer ici
        maxXp = maxXp * rateXp;
        currentArmor = currentArmor * rateArm;
        currentDamage = currentDamage * rateDam;
        maxHealth = maxHealth * rateHealth;
        maxMana = maxMana * rateMana;
        currentHealth = maxHealth;
        currentMana = maxMana;
        SetLevelText();
    }

    void SetLevelText()
    {
        level.text = "Level : " + lvlPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        float percentageHP = ((currentHealth) * 100 / maxHealth / 100);
        hpImage.fillAmount = percentageHP;
        txtHP.text = currentHealth + "/" + maxHealth;

        float percentageMana = ((currentMana) * 100 / maxMana/ 100);
        manaImage.fillAmount = percentageMana;
        txtMana.text = currentMana + "/" + maxMana;

        float pourcentageXp = ((currentXp) * 100 / maxXp / 100);
        barreXp.fillAmount = pourcentageXp;
        txtExp.text = currentXp+ "/" + maxXp;

        if (Input.GetKeyDown(KeyCode.L))  //Test si la le gain d'exp marche
		{
            GainExp(120);
		}
        
        if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
        {
            if (!characterSystem.activeSelf)
            {
                characterSystemInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                characterSystemInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            if (!inventory.activeSelf)
            {
                mainInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                mainInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
                craftSystemInventory.openInventory();
            else
            {
                if (cS != null)
                    cS.backToInventory();
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                craftSystemInventory.closeInventory();
            }
        }

    }


}
