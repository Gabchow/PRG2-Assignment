//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;
using System.Collections.Generic;

public class Order
{
    // Properties - matching class diagram exactly
    private int OrderId;
    private DateTime OrderDateTime;
    private double OrderTotal;
    private string OrderStatus;
    private DateTime DeliveryDateTime;
    private string DeliveryAddress;
    private string OrderPaymentMethod;
    private bool OrderPaid;
    private List<OrderedFoodItem> OrderedFoodItems;
    private Customer Customer;
    private Restaurant Restaurant;
    public bool IsFavourite;

    public int orderId
    {
        get { return OrderId; }
        set { OrderId = value; }
    }

    public DateTime orderDateTime
    {
        get { return OrderDateTime; }
        set { OrderDateTime = value; }
    }

    public double orderTotal
    {
        get { return OrderTotal; }
        set { OrderTotal = value; }
    }

    public string orderStatus
    {
        get { return OrderStatus; }
        set { OrderStatus = value; }
    }

    public DateTime deliveryDateTime
    {
        get { return DeliveryDateTime; }
        set { DeliveryDateTime = value; }
    }

    public string deliveryAddress
    {
        get { return DeliveryAddress; }
        set { DeliveryAddress = value; }
    }

    public string orderPaymentMethod
    {
        get { return OrderPaymentMethod; }
        set { OrderPaymentMethod = value; }
    }

    public bool orderPaid
    {
        get { return OrderPaid; }
        set { OrderPaid = value; }
    }

    public List<OrderedFoodItem> orderedFoodItems
    {
        get { return OrderedFoodItems; }
        set { OrderedFoodItems = value; }
    }

    public Customer customer
    {
        get { return Customer; }
        set { Customer = value; }
    }

    public Restaurant restaurant
    {
        get { return Restaurant; }
        set { Restaurant = value; }
    }

    public bool isFavourite
    {
        get { return IsFavourite; }
        set { IsFavourite = value; }
    }

    // Constant for delivery fee
    public const double DELIVERY_FEE = 5.00;

    // Default Constructor
    public Order()
    {
        OrderedFoodItems = new List<OrderedFoodItem>();
        OrderDateTime = DateTime.Now;
        OrderStatus = "Pending";
        OrderPaid = false;
        OrderTotal = DELIVERY_FEE;
    }

    // Parameterized Constructor
    public Order(int orderId, Customer customer, Restaurant restaurant,
                 DateTime deliveryDateTime, string deliveryAddress)
    {
        OrderId = orderId;
        Customer = customer;
        Restaurant = restaurant;
        DeliveryDateTime = deliveryDateTime;
        DeliveryAddress = deliveryAddress;
        OrderedFoodItems = new List<OrderedFoodItem>();
        OrderDateTime = DateTime.Now;
        OrderStatus = "Pending";
        OrderTotal = DELIVERY_FEE;
        OrderPaid = false;
    }

    public double CalculateOrderTotal()
    {
        double subtotal = 0;
        foreach (OrderedFoodItem item in OrderedFoodItems)
        {
            subtotal += item.CalculateSubtotal();
        }
        OrderTotal = subtotal + DELIVERY_FEE;
        return OrderTotal;
    }

    public void AddOrderedFoodItem(OrderedFoodItem orderedFoodItem)
    {
        OrderedFoodItems.Add(orderedFoodItem);
        CalculateOrderTotal();
    }

    public bool RemoveOrderedFoodItem(OrderedFoodItem orderedFoodItem)
    {
        bool removed = OrderedFoodItems.Remove(orderedFoodItem);
        if (removed)
        {
            CalculateOrderTotal();
        }
        return removed;
    }

    public void DisplayOrderedFoodItems()
    {
        Console.WriteLine("Ordered Items:");
        for (int i = 0; i < OrderedFoodItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {OrderedFoodItems[i]}");
        }
    }

    public void UpdateStatus(string newStatus)
    {
        OrderStatus = newStatus;
    }

    public void ModifyDeliveryAddress(string newAddress)
    {
        DeliveryAddress = newAddress;
    }

    public void ModifyDeliveryTime(DateTime newDateTime)
    {
        DeliveryDateTime = newDateTime;
    }

    public override string ToString()
    {
        return $"Order {OrderId}: {Customer.customerName} - {Restaurant.restaurantName} - ${OrderTotal:F2} - {OrderStatus}";
    }

    public string DisplayFullDetails()
    {
        string result = $"Order ID: {OrderId}\n";
        result += $"Customer: {Customer.customerName}\n";
        result += $"Restaurant: {Restaurant.restaurantName}\n";
        result += "Ordered Items:\n";

        for (int i = 0; i < OrderedFoodItems.Count; i++)
        {
            result += $"{i + 1}. {OrderedFoodItems[i]}\n";
        }

        result += $"Delivery date/time: {DeliveryDateTime:dd/MM/yyyy HH:mm}\n";
        result += $"Delivery address: {DeliveryAddress}\n";
        result += $"Total Amount: ${OrderTotal:F2}\n";
        result += $"Order Status: {OrderStatus}\n";
        result += $"Payment Method: {OrderPaymentMethod}\n";
        result += $"Paid: {(OrderPaid ? "Yes" : "No")}\n";

        return result;
    }
}
