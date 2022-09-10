using AtoLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    private List<NewCustomer> customers;

    public List<NewCustomer> Customers { get => customers; set => customers = value; }

    protected override void Initialize()
    {
        base.Initialize();
        customers = new List<NewCustomer>();
    }
}
