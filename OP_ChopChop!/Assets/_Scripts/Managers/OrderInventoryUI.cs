using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrderInventoryUI : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject salmonSlabPrefab;
    [SerializeField] private GameObject tunaSlabPrefab;
    [SerializeField] private GameObject spawnPoint;

    [Header("Variables")]
    [SerializeField] private int startPlayerMoney;
    [SerializeField] private int currentPlayerMoney;
    [SerializeField] private int salmonSlabOrderCount;
    [SerializeField] private int tunaSlabOrderCount;

    [SerializeField] private int salmonSlabPrice;
    [SerializeField] private int tunaSlabPrice;
    [SerializeField] private int totalPrice;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtCurrentPlayerMoney;
    [SerializeField] private TextMeshProUGUI txtSalmonOrderCount;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonSlabPrice;
    //[SerializeField] private TextMeshProUGUI txtTunaSlabPrice;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMoney = startPlayerMoney;
    }

    // Update is called once per frame
    void Update()
    {
        txtTotalPrice.text = CalculateTotalPrice().ToString();
        txtSalmonOrderCount.text = salmonSlabOrderCount.ToString();
        txtTotalPrice.text = totalPrice.ToString();
        txtSalmonSlabPrice.text = salmonSlabPrice.ToString();
        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();
    }

    public void DoOrderSupplies()
    {
        if(currentPlayerMoney >= totalPrice) 
        { 
            ToOrderSupplies();
            currentPlayerMoney -= totalPrice;
        }
    }

    public void ToOrderSupplies()
    {
        if (salmonSlabOrderCount > 0)
        {
            for (int i = 0; i < salmonSlabOrderCount; i++)
            {
                Instantiate(salmonSlabPrefab, spawnPoint.transform.localPosition, spawnPoint.transform.rotation);
            }
        }

        if (tunaSlabOrderCount > 0)
        {
            for (int i = 0; i < tunaSlabOrderCount; i++)
            {
                Instantiate(tunaSlabPrefab, spawnPoint.transform.localPosition, spawnPoint.transform.rotation);
            }
        }

        // Reset order counts after spawning
        salmonSlabOrderCount = 0;
        tunaSlabOrderCount = 0;
    }

    private int CalculateTotalPrice()
    {
        totalPrice = (salmonSlabOrderCount * salmonSlabPrice) + (tunaSlabOrderCount * tunaSlabPrice);
        return totalPrice;
    }

    void UpdateSalmonOrderTxt()
    {
        txtSalmonOrderCount.text = salmonSlabOrderCount.ToString();
    }

    /*
    void UpdateTunaOrderTxt()
    {
        txtTunaOrderCount.text = tunaSlabOrderCount.ToString();
    }
    */

    public void IncreaseSalmonSlabOrder()
    {
        salmonSlabOrderCount++;
        UpdateSalmonOrderTxt();

        Mathf.Clamp(salmonSlabOrderCount, 0, 99);
    }

    public void DecreaseSalmonSlabOrder()
    {
        if (salmonSlabOrderCount > 0)
        {
            salmonSlabOrderCount--;
            UpdateSalmonOrderTxt();
        }

        Mathf.Clamp(salmonSlabOrderCount, 0, salmonSlabOrderCount);
    }

    public void IncreaseTunaSlabOrder()
    {
        tunaSlabOrderCount++;

        Mathf.Clamp(tunaSlabOrderCount, 0, 99);
    }

    public void DecreaseTunaSlabOrder()
    {
        if (tunaSlabOrderCount > 0)
        {
            tunaSlabOrderCount--;
        }

        Mathf.Clamp(tunaSlabOrderCount, 0, salmonSlabOrderCount);
    }
}
