//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;

public class FoodItem
{
    private string ItemName;
    private string ItemDesc;
    private double ItemPrice;
    private string Customise;

    public string itemName
    {
        get { return ItemName; }
        set { ItemName = value; }
    }

    public string itemDesc
    {
        get { return ItemDesc; }
        set { ItemDesc = value; }
    }

    public double itemPrice
    {
        get { return ItemPrice; }
        set { ItemPrice = value; }
    }

    public string customise
    {
        get { return Customise; }
        set { Customise = value; }
    }

    public FoodItem()
    {
        Customise = "";
    }

    public FoodItem(string itemName, string itemDesc, double itemPrice)
    {
        ItemName = itemName;
        ItemDesc = itemDesc;
        ItemPrice = itemPrice;
        Customise = "";
    }

    public override string ToString()
    {
        return $"{ItemName}: {ItemDesc} - ${ItemPrice:F2}";
    }
}