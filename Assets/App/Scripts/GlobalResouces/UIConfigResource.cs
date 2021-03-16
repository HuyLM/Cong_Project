using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[CreateAssetMenu(fileName = "UIConfigResource", menuName = "Data/Resources/UIConfigResource" )]
public class UIConfigResource : ScriptableObject {
    [Header( "State Bill Color" )]
    [SerializeField, ColorUsage( true )] private Color defaultBillColor;
    [SerializeField, ColorUsage( true )] private Color doneBillColor;
    [SerializeField, ColorUsage( true )] private Color debtBillColor;
    [SerializeField, ColorUsage( true )] private Color wattingBillColor;
    [SerializeField, ColorUsage( true )] private Color cancelBillColor;

    [Header( "State Bill Config" )]
    [SerializeField] private List<DropDownListItem> dropDownListItems;

    public Color GetBillColor(BillState state)
    {
        switch ( state ) {
            case BillState.None: {
                    return defaultBillColor;
                }
            case BillState.Done: {
                    return doneBillColor;
                }
            case BillState.Debt: {
                    return debtBillColor;
                }
            case BillState.Waiting: {
                    return wattingBillColor;
                }
            case BillState.Cancel: {
                    return cancelBillColor;
                }
            default: {
                    return defaultBillColor;
                }
        }
    }

    public List<DropDownListItem> GetListItemStateBill(List<DropDownListItem> items, BillState state)
    {
        items = dropDownListItems;
        switch ( state ) {
            case BillState.None: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = false;
                    items[ 2 ].IsDisabled = false;
                    items[ 3 ].IsDisabled = false;
                    items[ 4 ].IsDisabled = false;
                    break;
                }
            case BillState.Done: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = true;
                    items[ 2 ].IsDisabled = true;
                    items[ 3 ].IsDisabled = true;
                    items[ 4 ].IsDisabled = true;
                    break;
                }
            case BillState.Debt: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = false;
                    items[ 2 ].IsDisabled = true;
                    items[ 3 ].IsDisabled = true;
                    items[ 4 ].IsDisabled = true;
                    break;
                }
            case BillState.Waiting: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = false;
                    items[ 2 ].IsDisabled = false;
                    items[ 3 ].IsDisabled = true;
                    items[ 4 ].IsDisabled = true;
                    break;
                }
            case BillState.Cancel: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = false;
                    items[ 2 ].IsDisabled = false;
                    items[ 3 ].IsDisabled = false;
                    items[ 4 ].IsDisabled = true;
                    break;
                }
            default: {
                    items[ 0 ].IsDisabled = true;
                    items[ 1 ].IsDisabled = false;
                    items[ 2 ].IsDisabled = false;
                    items[ 3 ].IsDisabled = false;
                    items[ 4 ].IsDisabled = false;
                    break;
                }
        }
        items[ 0 ].IsDisabled = false;
        items[ 1 ].IsDisabled = false;
        items[ 2 ].IsDisabled = false;
        items[ 3 ].IsDisabled = false;
        items[ 4 ].IsDisabled = false;
        return items;
    }

}
