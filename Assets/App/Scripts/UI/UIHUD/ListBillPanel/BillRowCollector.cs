using AtoLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BillRowCollector : CollectionDisplayer<Bill>
{
    [SerializeField] private BillRowViewDisplayer prefab;
    [SerializeField] private Transform layout;

    private Action<BillRowViewDisplayer> onSelect;
    protected readonly List<BillRowViewDisplayer> displayers = new List<BillRowViewDisplayer>();
    public override int DisplayerCount => displayers.Count;

    public BillRowViewDisplayer GetDisplayer(int index)
    {
        if ( index < 0 || index >= DisplayerCount ) {
            return null;
        }
        return displayers[ index ];
    }

    public override void Show()
    {
        for ( int i = 0; i < Capacity; i++ ) {
            if ( DisplayerCount == i ) {
                displayers.Add( CreateDisplayer() );
            }

            BillRowViewDisplayer displayer = GetDisplayer( i );
            if ( displayer ) {
                displayer.gameObject.SetActive( true );
                SetupDisplayer( displayer, GetItem( i ) );
            }
        }

        for ( int i = Capacity; i < DisplayerCount; i++ ) {
            BillRowViewDisplayer displayer = GetDisplayer( i );
            if ( displayer ) {
                displayer.gameObject.SetActive( false );
            }
        }
    }

    public BillRowViewDisplayer GetItemView(Bill data)
    {
        foreach ( var displayer in displayers ) {
            if ( displayer.Model == data ) {
                return displayer;
            }
        }
        return null;
    }

    public void SetupDisplayer(BillRowViewDisplayer displayer, Bill item)
    {
        if ( displayer == null ) {
            return;
        }
        displayer.SetModel( item ).Show();
    }

    protected BillRowViewDisplayer CreateDisplayer()
    {
        BillRowViewDisplayer viewItem = Instantiate( prefab, layout );
        return viewItem;
    }

    public BillRowCollector AddOnSelect(Action<BillRowViewDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }


}