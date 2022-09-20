using AtoLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerRowCollector : CollectionDisplayer<NewCustomer>
{
    [SerializeField] private CustomerRowDisplayer prefab;
    [SerializeField] private Transform layout;

    private Action<CustomerRowDisplayer> onSelect;
    protected readonly List<CustomerRowDisplayer> displayers = new List<CustomerRowDisplayer>();
    public override int DisplayerCount => displayers.Count;

    public CustomerRowDisplayer GetDisplayer(int index)
    {
        if (index < 0 || index >= DisplayerCount)
        {
            return null;
        }
        return displayers[index];
    }

    public override void Show()
    {
        for (int i = 0; i < Capacity; i++)
        {
            if (DisplayerCount == i)
            {
                displayers.Add(CreateDisplayer());
            }

            CustomerRowDisplayer displayer = GetDisplayer(i);
            if (displayer)
            {
                displayer.gameObject.SetActive(true);
                SetupDisplayer(displayer, GetItem(i));
            }
        }

        for (int i = Capacity; i < DisplayerCount; i++)
        {
            CustomerRowDisplayer displayer = GetDisplayer(i);
            if (displayer)
            {
                displayer.gameObject.SetActive(false);
            }
        }
    }

    public CustomerRowDisplayer GetItemView(NewCustomer data)
    {
        foreach (var displayer in displayers)
        {
            if (displayer.Model == data)
            {
                return displayer;
            }
        }
        return null;
    }

    public void SetupDisplayer(CustomerRowDisplayer displayer, NewCustomer item)
    {
        if (displayer == null)
        {
            return;
        }
        displayer.AddOnSelect(onSelect).SetModel(item).Show();
    }

    protected CustomerRowDisplayer CreateDisplayer()
    {
        CustomerRowDisplayer viewItem = Instantiate(prefab, layout);
        return viewItem;
    }

    public CustomerRowCollector AddOnSelect(Action<CustomerRowDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }


}
