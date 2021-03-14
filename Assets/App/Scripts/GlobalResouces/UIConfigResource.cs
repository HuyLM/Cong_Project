using UnityEngine;

[CreateAssetMenu(fileName = "UIConfigResource", menuName = "Data/Resources/UIConfigResource" )]
public class UIConfigResource : ScriptableObject {
    [Header( "State Bill Color" )]
    [SerializeField, ColorUsage( true )] private Color defaultBillColor;
    [SerializeField, ColorUsage( true )] private Color doneBillColor;
    [SerializeField, ColorUsage( true )] private Color debtBillColor;
    [SerializeField, ColorUsage( true )] private Color wattingBillColor;
    [SerializeField, ColorUsage( true )] private Color cancelBillColor;

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
}
