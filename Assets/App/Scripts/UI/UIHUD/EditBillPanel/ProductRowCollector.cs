using AtoLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductRowCollector : CollectionDisplayer<NewProduct>
{
    [SerializeField] private ProductRowViewDisplayer prefab;
    [SerializeField] private Transform layout;

    private Action<ProductRowViewDisplayer> onSelect;
    protected readonly List<ProductRowViewDisplayer> displayers = new List<ProductRowViewDisplayer>();
    public override int DisplayerCount => displayers.Count;

    public ProductRowViewDisplayer GetDisplayer(int index)
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

            ProductRowViewDisplayer displayer = GetDisplayer( i );
            if ( displayer ) {
                displayer.gameObject.SetActive( true );
                displayer.SetNumber( i + 1 );
                SetupDisplayer( displayer, GetItem( i ) );
            }
        }

        for ( int i = Capacity; i < DisplayerCount; i++ ) {
            ProductRowViewDisplayer displayer = GetDisplayer( i );
            if ( displayer ) {
                displayer.gameObject.SetActive( false );
            }
        }
    }

    public ProductRowViewDisplayer GetItemView(NewProduct data)
    {
        foreach ( var displayer in displayers ) {
            if ( displayer.Model == data ) {
                return displayer;
            }
        }
        return null;
    }

    public void SetupDisplayer(ProductRowViewDisplayer displayer, NewProduct item)
    {
        if ( displayer == null ) {
            return;
        }
        displayer.AddOnSelect(onSelect).SetModel( item ).Show();
    }

    protected ProductRowViewDisplayer CreateDisplayer()
    {
        ProductRowViewDisplayer viewItem = Instantiate( prefab, layout );
        return viewItem;
    }

    public ProductRowCollector AddOnSelect(Action<ProductRowViewDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }


}