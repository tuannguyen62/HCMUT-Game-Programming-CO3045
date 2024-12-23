using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;

    int plantStage = 0;
    float timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;
    SpriteRenderer plot;

    PlantObject selectedPlant;

    FarmManager fm;

    //tuoi nuoc
    bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    //phan bon
    float speed = 1f;
    public bool isBought = true;

    public AudioClip plantSound; // Clip âm thanh trồng cây
    public AudioClip collectSound; // am thanh thu hoach
    public AudioClip waterSound; // File âm thanh tưới nước
    public AudioClip fertilizerSound; // File âm thanh phân bón
    public AudioClip unlockSound; // am thanh mo khoa o dat moi
    private AudioSource audioSource; // Audio Source để phát âm thanh


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //khoi tao Audio Source  
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        if (plantCollider == null)
        {
            plantCollider = transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("BoxCollider2D was missing and has been added to the Plant object.");
        }
        fm = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted && !isDry)
        {
            timer -= speed*Time.deltaTime;

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length-1) 
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted) 
        {
            if (plantStage == selectedPlant.plantStages.Length-1 && !fm.isPlanting)
            {
            //harvest
            Harvest();

            //am thanh thu hoach
            if (audioSource != null && collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            }
        }
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money && isBought)
        {
            Plant(fm.selectPlant.plant);

            // Phát âm thanh trồng cây
            if (audioSource != null && plantSound != null)
            {
                audioSource.PlayOneShot(plantSound);
            }
        }

        if (fm.isSelecting)
        {
            switch(fm.selectedTool)
            {
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted) UpdatePlant();
                        // Phát âm thanh tưới nước
                        if (audioSource != null && waterSound != null)
                        {
                            audioSource.PlayOneShot(waterSound);
                        }
                    }
                    break;

                case 2:
                    if (fm.money >= 10 && isBought)
                    {
                        fm.Transaction(-10);
                        if (speed < 2)  speed += .2f;
                        // Phát âm thanh phân bón
                        if (audioSource != null && fertilizerSound != null)
                        {
                            audioSource.PlayOneShot(fertilizerSound);
                        }
                    }
                    break;
                case 3:
                    if (fm.money > 100 && !isBought) 
                    {
                        fm.Transaction(-100);
                        isBought = true;
                        plot.sprite = drySprite;
                        if (audioSource != null && unlockSound != null)
                        {
                            audioSource.PlayOneShot(unlockSound);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (fm.isPlanting) 
        {
            if (isPlanted || fm.selectPlant.plant.buyPrice > fm.money || !isBought && !fm.isSelecting)
            {
                //can't buy
                plot.color = unavailableColor;
            }
            else
            {
                //can buy
                plot.color = availableColor;
            }

        }

        if (fm.isSelecting) 
        {
            switch (fm.selectedTool)
            {
                case 1:
                case 2:
                    if (isBought && fm.money >= (fm.selectedTool - 1)*10) 
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                case 3: 
                    if (!isBought && fm.money >= 100) 
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                default:
                    plot.color = unavailableColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        fm.Transaction(selectedPlant.sellPrice);
        isDry = true;
        plot.sprite = drySprite;
        speed = 1f;
        // Phát âm thanh trồng cây
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;

        fm.Transaction(-selectedPlant.buyPrice);

        plantStage = 0;
        UpdatePlant();
        timer = selectedPlant.timeBtwStages;
        plant.gameObject.SetActive(true);
        // Phát âm thanh trồng cây
        if (audioSource != null && plantSound != null)
        {
            audioSource.PlayOneShot(plantSound);
        }
    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0,plant.bounds.size.y/2);   
    }

}
