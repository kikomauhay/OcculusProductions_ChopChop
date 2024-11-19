using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    [SerializeField]
    public GameObject Knife;
    [SerializeField]
    public GameObject MeatBoard;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        //gg
    }
}
