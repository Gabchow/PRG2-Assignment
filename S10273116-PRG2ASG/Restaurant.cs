//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;
using System.Collections.Generic;

public class Restaurant
{
    private string RestaurantId;
    private string RestaurantName;
    private string RestaurantEmail;
    private Menu Menu;
    private List<SpecialOffer> SpecialOffers;
    private Queue<Order> OrderQueue;

    public string restaurantId
    {
        get { return RestaurantId; }
        set { RestaurantId = value; }
    }

    public string restaurantName
    {
        get { return RestaurantName; }
        set { RestaurantName = value; }
    }

    public string restaurantEmail
    {
        get { return RestaurantEmail; }
        set { RestaurantEmail = value; }
    }

    public Menu menu
    {
        get { return Menu; }
        set { Menu = value; }
    }

    public List<SpecialOffer> specialOffers
    {
        get { return SpecialOffers; }
        set { SpecialOffers = value; }
    }

    public Queue<Order> orderQueue
    {
        get { return OrderQueue; }
        set { OrderQueue = value; }
    }

    public Restaurant()
    {
        Menu = new Menu();
        SpecialOffers = new List<SpecialOffer>();
        OrderQueue = new Queue<Order>();
    }

    public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
    {
        RestaurantId = restaurantId;
        RestaurantName = restaurantName;
        RestaurantEmail = restaurantEmail;
        Menu = new Menu(restaurantId + "_MENU", restaurantName + " Menu");
        SpecialOffers = new List<SpecialOffer>();
        OrderQueue = new Queue<Order>();
    }

    public void DisplayOrders()
    {
        Console.WriteLine($"\nOrders for {RestaurantName}:");
        foreach (Order order in OrderQueue)
        {
            Console.WriteLine($"  {order}");
        }
    }

    public void DisplaySpecialOffers()
    {
        Console.WriteLine($"Special Offers for {RestaurantName}:");
        foreach (SpecialOffer offer in SpecialOffers)
        {
            Console.WriteLine($"  {offer}");
        }
    }

    public void DisplayMenu()
    {
        Console.WriteLine($"\n{RestaurantName} ({RestaurantId})");
        foreach (FoodItem item in Menu.FoodItems)
        {
            Console.WriteLine($" - {item}");
        }
    }

    public void AddMenu(Menu menu)
    {
        Menu = menu;
    }

    public bool RemoveMenu(Menu menu)
    {
        if (Menu == menu)
        {
            Menu = new Menu();
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"Restaurant: {RestaurantName} ({RestaurantId})";
    }
}
