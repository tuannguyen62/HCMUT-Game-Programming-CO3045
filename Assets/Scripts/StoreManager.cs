using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject plantItem;
    List<PlantObject> plantObjects = new List<PlantObject>();
    
    private void Awake()
    {
        //Assets/Resources/Plants
        var loadPlants = Resources.LoadAll("Plants", typeof(PlantObject));
        foreach (var plant in loadPlants)
        {
            plantObjects.Add((PlantObject)plant);

            // PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            // newPlant.plant = (PlantObject)plant;
        }
        plantObjects.Sort(SortByPrice);

        foreach (var plant in plantObjects)
        {
            PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            newPlant.plant = plant;
        }
    }

    int SortByPrice(PlantObject plantObject1, PlantObject plantObject2)
    {
        return plantObject1.buyPrice.CompareTo(plantObject2.buyPrice);
    }

}
