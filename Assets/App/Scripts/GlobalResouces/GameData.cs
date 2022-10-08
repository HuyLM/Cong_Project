using AtoLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    private List<NewCustomer> customers;
    private int showMode;

    public List<NewCustomer> Customers { get => customers; set => customers = value; }
    public int ShowMode { get => showMode; set => showMode = value; }

    protected override void Initialize()
    {
        base.Initialize();
        customers = new List<NewCustomer>();
    }
}
