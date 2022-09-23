using AtoLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSelectionCollector : CollectionDisplayer<OptionSelect>
{
    [SerializeField] private OptionSelectionDisplayer prefab;
    [SerializeField] private Transform layout;

    private Action<OptionSelectionDisplayer> onSelect;
    protected readonly List<OptionSelectionDisplayer> displayers = new List<OptionSelectionDisplayer>();
    public override int DisplayerCount => displayers.Count;

    public OptionSelectionDisplayer GetDisplayer(int index)
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

            OptionSelectionDisplayer displayer = GetDisplayer(i);
            if (displayer)
            {
                displayer.gameObject.SetActive(true);
                SetupDisplayer(displayer, GetItem(i));
            }
        }

        for (int i = Capacity; i < DisplayerCount; i++)
        {
            OptionSelectionDisplayer displayer = GetDisplayer(i);
            if (displayer)
            {
                displayer.gameObject.SetActive(false);
            }
        }
    }

    public void SetupDisplayer(OptionSelectionDisplayer displayer, OptionSelect item)
    {
        if (displayer == null)
        {
            return;
        }
        displayer.SetOnSelect(onSelect).SetModel(item).Show();
    }

    protected OptionSelectionDisplayer CreateDisplayer()
    {
        OptionSelectionDisplayer viewItem = Instantiate(prefab, layout);
        return viewItem;
    }

    public OptionSelectionCollector AddOnSelect(Action<OptionSelectionDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }


}
