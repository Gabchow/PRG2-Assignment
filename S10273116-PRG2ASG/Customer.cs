//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;
using System.Collections.Generic;

public class Customer
{
    private string EmailAddress;
    private string CustomerName;
    private List<Order> OrderList;

    public string emailAddress
    {
        get { return EmailAddress; }
        set { EmailAddress = value; }
    }

    public string customerName
    {
        get { return CustomerName; }
        set { CustomerName = value; }
    }

    public List<Order> orderList
    {
        get { return OrderList; }
        set { OrderList = value; }
    }




    public Customer()
    {
        OrderList = new List<Order>();
    }

    public Customer(string customerName, string emailAddress)
    {
        CustomerName = customerName;
        EmailAddress = emailAddress;
        OrderList = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        OrderList.Add(order);
    }


    public void DisplayAllOrders()
    {
        Console.WriteLine($"Orders for {CustomerName}:");
        foreach (Order order in OrderList)
        {
            Console.WriteLine($"  {order}");
        }
    }

    public bool RemoveOrder(Order order)
    {
        return OrderList.Remove(order);
    }

    public List<Order> GetPendingOrders()
    {
        List<Order> pendingOrders = new List<Order>();
        foreach (Order order in OrderList)
        {
            if (order.orderStatus == "Pending")
            {
                pendingOrders.Add(order);
            }
        }
        return pendingOrders;
    }

    public Order FindOrder(int orderId)
    {
        foreach (Order order in OrderList)
        {
            if (order.orderId == orderId)
            {
                return order;
            }
        }
        return null;
    }

    public override string ToString()
    {
        return $"{CustomerName} ({EmailAddress})";
    }
}
