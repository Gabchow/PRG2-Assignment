//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;

public class OrderedFoodItem
{
    private int QtyOrdered;
    private double SubTotal;
    private FoodItem FoodItem;

    public int qtyOrdered
    {
        get { return QtyOrdered; }
        set { QtyOrdered = value; }
    }

    public double subTotal
    {
        get { return SubTotal; }
        set { SubTotal = value; }
    }

    public FoodItem foodItem
    {
        get { return FoodItem; }
        set { FoodItem = value; }
    }


    public OrderedFoodItem() { }

    public OrderedFoodItem(FoodItem foodItem, int qtyOrdered)
    {
        FoodItem = foodItem;
        QtyOrdered = qtyOrdered;
        CalculateSubtotal();
    }

    public double CalculateSubtotal()
    {
        SubTotal = FoodItem.ItemPrice * QtyOrdered;
        return SubTotal;
    }

    public override string ToString()
    {
        return $"{FoodItem.ItemName} - {QtyOrdered}";
    }
}
