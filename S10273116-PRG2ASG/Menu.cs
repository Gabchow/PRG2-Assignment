// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;
using System.Collections.Generic;

public class Menu
{
    private string MenuId;
    private string MenuName;
    private List<FoodItem> FoodItems;

    public string menuId
    {
        get { return MenuId; }
        set { MenuId = value; }
    }
    public string menuName
    {
        get { return MenuName; }
        set { MenuName = value; }
    }

    public List<FoodItem> foodItems
    {
        get { return FoodItems; }
        set { FoodItems = value; }
    }




    public Menu()
    {
        FoodItems = new List<FoodItem>();
    }

    public Menu(string menuId, string menuName)
    {
        MenuId = menuId;
        MenuName = menuName;
        FoodItems = new List<FoodItem>();
    }

    public void AddFoodItem(FoodItem foodItem)
    {
        FoodItems.Add(foodItem);
    }

    public bool RemoveFoodItem(FoodItem foodItem)
    {
        return FoodItems.Remove(foodItem);
    }

    public void DisplayFoodItems()
    {
        Console.WriteLine($"Menu: {MenuName} ({MenuId})");
        foreach (FoodItem item in FoodItems)
        {
            Console.WriteLine($" - {item}");
        }
    }

    public override string ToString()
    {
        return $"Menu: {MenuName} ({MenuId}) - {FoodItems.Count} items";
    }
}
