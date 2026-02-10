//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    // Global lists to store all data
    static List<Restaurant> restaurantList = new List<Restaurant>();
    static List<Customer> customerList = new List<Customer>();
    static Stack<Order> refundStack = new Stack<Order>();
    static int nextOrderId = 1001; // Track next order ID

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Gruberoo Food Delivery System");

        // Load all data files
        LoadRestaurants();
        LoadFoodItems();
        LoadCustomers();
        LoadOrders();

        // Main menu loop
        bool exit = false;
        while (!exit)
        {
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllRestaurantsAndMenuItems();
                    break;
                case "2":
                    ListAllOrders();
                    break;
                case "3":
                    CreateNewOrder();
                    break;
                case "4":
                    ProcessOrder();
                    break;
                case "5":
                    ModifyOrder();
                    break;
                case "6":
                    DeleteOrder();
                    break;
                case "7":
                    BulkProcessOrders();
                    break;
                case "8":
                    DisplayTotalOrderAmounts();
                    break;
                case "0":
                    exit = true;
                    SaveQueueAndStack();
                    Console.WriteLine("Thank you for using Gruberoo!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("===== Gruberoo Food Delivery System =====");
        Console.WriteLine("1. List all restaurants and menu items");
        Console.WriteLine("2. List all orders");
        Console.WriteLine("3. Create a new order");
        Console.WriteLine("4. Process an order");
        Console.WriteLine("5. Modify an existing order");
        Console.WriteLine("6. Delete an existing order");
        Console.WriteLine("7. Bulk process unprocessed orders");
        Console.WriteLine("8. Display total order amounts");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");
    }

    // Feature 1: Load Restaurants
    static void LoadRestaurants()
    {
        try
        {
            string[] lines = File.ReadAllLines("restaurants.csv");
            int count = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] data = line.Split(',');
                if (data.Length >= 3)
                {
                    Restaurant restaurant = new Restaurant(data[0], data[1], data[2]);
                    restaurantList.Add(restaurant);
                    count++;
                }
            }
            Console.WriteLine($"{count} restaurants loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurants: {ex.Message}");
        }
    }

    // Feature 1: Load Food Items
    static void LoadFoodItems()
    {
        try
        {
            string[] lines = File.ReadAllLines("fooditems.csv");
            int count = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] data = line.Split(',');
                if (data.Length >= 4)
                {
                    string restaurantId = data[0];
                    string itemName = data[1];
                    string itemDesc = data[2];
                    double itemPrice = double.Parse(data[3]);

                    // Find the restaurant and add food item
                    Restaurant restaurant = FindRestaurantById(restaurantId);
                    if (restaurant != null)
                    {
                        FoodItem item = new FoodItem(itemName, itemDesc, itemPrice);
                        restaurant.Menu.AddFoodItem(item);
                        count++;
                    }
                }
            }
            Console.WriteLine($"{count} food items loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading food items: {ex.Message}");
        }
    }

    // Feature 2: Load Customers
    static void LoadCustomers()
    {
        try
        {
            string[] lines = File.ReadAllLines("customers.csv");
            int count = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] data = line.Split(',');
                if (data.Length >= 2)
                {
                    Customer customer = new Customer(data[0], data[1]);
                    customerList.Add(customer);
                    count++;
                }
            }
            Console.WriteLine($"{count} customers loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading customers: {ex.Message}");
        }
    }

    // Feature 2: Load Orders
    static void LoadOrders()
    {
        try
        {
            string[] lines = File.ReadAllLines("orders.csv");
            int count = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // Parse CSV line carefully (items field contains commas)
                string[] data = ParseOrderLine(line);
                if (data.Length >= 10)
                {
                    int orderId = int.Parse(data[0]);
                    string customerEmail = data[1];
                    string restaurantId = data[2];
                    DateTime deliveryDate = ParseDate(data[3]);
                    DateTime deliveryTime = ParseTime(data[4]);
                    DateTime deliveryDateTime = deliveryDate.Date + deliveryTime.TimeOfDay;
                    string deliveryAddress = data[5];
                    DateTime orderDateTime = ParseDateTime(data[6]);
                    double orderTotal = double.Parse(data[7]);
                    string orderStatus = data[8];
                    string itemsString = data[9];

                    // Find customer and restaurant
                    Customer customer = FindCustomerByEmail(customerEmail);
                    Restaurant restaurant = FindRestaurantById(restaurantId);

                    if (customer != null && restaurant != null)
                    {
                        Order order = new Order(orderId, customer, restaurant,
                                              deliveryDateTime, deliveryAddress);
                        order.OrderDateTime = orderDateTime;
                        order.OrderStatus = orderStatus;
                        order.OrderPaid = true;

                        // Parse and add ordered food items
                        ParseOrderedFoodItems(order, restaurant, itemsString);

                        order.OrderTotal = orderTotal;

                        // Add to customer's order list
                        customer.AddOrder(order);

                        // Add to restaurant's queue if not delivered
                        if (orderStatus != "Delivered")
                        {
                            restaurant.OrderQueue.Enqueue(order);
                        }

                        // Track highest order ID
                        if (orderId >= nextOrderId)
                        {
                            nextOrderId = orderId + 1;
                        }

                        count++;
                    }
                }
            }
            Console.WriteLine($"{count} orders loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading orders: {ex.Message}");
        }
    }

    // Helper: Parse order line with items containing commas
    static string[] ParseOrderLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }
        result.Add(current);

        return result.ToArray();
    }

    // Helper: Parse ordered food items from string
    static void ParseOrderedFoodItems(Order order, Restaurant restaurant, string itemsString)
    {
        // Remove quotes if present
        itemsString = itemsString.Trim('"');

        // Split by pipe |
        string[] items = itemsString.Split('|');

        foreach (string item in items)
        {
            string[] parts = item.Split(',');
            if (parts.Length >= 2)
            {
                string itemName = parts[0].Trim();
                int quantity = int.Parse(parts[1].Trim());

                // Find food item in restaurant menu
                FoodItem foodItem = FindFoodItemByName(restaurant, itemName);
                if (foodItem != null)
                {
                    OrderedFoodItem orderedItem = new OrderedFoodItem(foodItem, quantity);
                    order.OrderedFoodItems.Add(orderedItem);
                }
            }
        }
    }

    // Helper: Parse date (dd/MM/yyyy)
    static DateTime ParseDate(string dateStr)
    {
        string[] parts = dateStr.Split('/');
        int day = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int year = int.Parse(parts[2]);
        return new DateTime(year, month, day);
    }

    // Helper: Parse time (HH:mm)
    static DateTime ParseTime(string timeStr)
    {
        string[] parts = timeStr.Split(':');
        int hour = int.Parse(parts[0]);
        int minute = int.Parse(parts[1]);
        return new DateTime(1, 1, 1, hour, minute, 0);
    }

    // Helper: Parse datetime (dd/MM/yyyy HH:mm)
    static DateTime ParseDateTime(string dateTimeStr)
    {
        string[] parts = dateTimeStr.Split(' ');
        DateTime date = ParseDate(parts[0]);
        DateTime time = ParseTime(parts[1]);
        return date.Date + time.TimeOfDay;
    }

    // Helper: Find restaurant by ID
    static Restaurant FindRestaurantById(string restaurantId)
    {
        foreach (Restaurant r in restaurantList)
        {
            if (r.RestaurantId == restaurantId)
            {
                return r;
            }
        }
        return null;
    }

    // Helper: Find customer by email
    static Customer FindCustomerByEmail(string email)
    {
        foreach (Customer c in customerList)
        {
            if (c.EmailAddress.ToLower() == email.ToLower())
            {
                return c;
            }
        }
        return null;
    }

    // Helper: Find food item by name in restaurant
    static FoodItem FindFoodItemByName(Restaurant restaurant, string itemName)
    {
        foreach (FoodItem item in restaurant.Menu.FoodItems)
        {
            if (item.ItemName.ToLower() == itemName.ToLower())
            {
                return item;
            }
        }
        return null;
    }

    // Feature 3: List all restaurants and menu items
    static void ListAllRestaurantsAndMenuItems()
    {
        Console.WriteLine("\nAll Restaurants and Menu Items");
        Console.WriteLine("==============================");

        foreach (Restaurant restaurant in restaurantList)
        {
            restaurant.DisplayMenu();
        }
    }

    // Feature 4: List all orders
    static void ListAllOrders()
    {
        Console.WriteLine("\nAll Orders");
        Console.WriteLine("==========");
        Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-10} {"Status",-15}");
        Console.WriteLine(new string('-', 95));

        // Collect all orders from all customers
        List<Order> allOrders = new List<Order>();
        foreach (Customer customer in customerList)
        {
            allOrders.AddRange(customer.OrderList);
        }

        // Sort by order ID
        allOrders.Sort((a, b) => a.OrderId.CompareTo(b.OrderId));

        foreach (Order order in allOrders)
        {
            Console.WriteLine($"{order.OrderId,-10} {order.Customer.CustomerName,-15} {order.Restaurant.RestaurantName,-15} " +
                            $"{order.DeliveryDateTime:dd/MM/yyyy HH:mm}    ${order.OrderTotal,-9:F2} {order.OrderStatus,-15}");
        }
    }

    // Feature 5: Create new order
    static void CreateNewOrder()
    {
        Console.WriteLine("\nCreate New Order");
        Console.WriteLine("================");

        // Get and validate customer email
        Console.Write("Enter Customer Email: ");
        string customerEmail = Console.ReadLine();
        Customer customer = FindCustomerByEmail(customerEmail);

        if (customer == null)
        {
            Console.WriteLine("Error: Customer not found.");
            return;
        }

        // Get and validate restaurant ID
        Console.Write("Enter Restaurant ID: ");
        string restaurantId = Console.ReadLine().ToUpper();
        Restaurant restaurant = FindRestaurantById(restaurantId);

        if (restaurant == null)
        {
            Console.WriteLine("Error: Restaurant not found.");
            return;
        }

        if (restaurant.Menu.FoodItems.Count == 0)
        {
            Console.WriteLine("Error: Restaurant has no menu items.");
            return;
        }

        // Get and validate delivery date
        DateTime deliveryDate;
        while (true)
        {
            Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
            string dateInput = Console.ReadLine();

            try
            {
                deliveryDate = ParseDate(dateInput);
                if (deliveryDate < DateTime.Now.Date)
                {
                    Console.WriteLine("Error: Delivery date cannot be in the past.");
                    continue;
                }
                break;
            }
            catch
            {
                Console.WriteLine("Error: Invalid date format. Use dd/mm/yyyy");
            }
        }

        // Get and validate delivery time
        DateTime deliveryTime;
        while (true)
        {
            Console.Write("Enter Delivery Time (hh:mm): ");
            string timeInput = Console.ReadLine();

            try
            {
                deliveryTime = ParseTime(timeInput);
                break;
            }
            catch
            {
                Console.WriteLine("Error: Invalid time format. Use hh:mm");
            }
        }

        DateTime deliveryDateTime = deliveryDate.Date + deliveryTime.TimeOfDay;

        // Validate delivery time is not in the past
        if (deliveryDateTime < DateTime.Now)
        {
            Console.WriteLine("Error: Delivery date/time cannot be in the past.");
            return;
        }

        // Get delivery address
        Console.Write("Enter Delivery Address: ");
        string deliveryAddress = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(deliveryAddress))
        {
            Console.WriteLine("Error: Delivery address cannot be empty.");
            return;
        }

        // Display available food items
        Console.WriteLine("\nAvailable Food Items:");
        for (int i = 0; i < restaurant.Menu.FoodItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {restaurant.Menu.FoodItems[i].ItemName} - ${restaurant.Menu.FoodItems[i].ItemPrice:F2}");
        }

        // Create new order
        Order newOrder = new Order(nextOrderId, customer, restaurant, deliveryDateTime, deliveryAddress);

        // Select food items
        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int itemNum))
            {
                Console.WriteLine("Error: Please enter a valid number.");
                continue;
            }

            if (itemNum == 0)
            {
                if (newOrder.OrderedFoodItems.Count == 0)
                {
                    Console.WriteLine("Error: Order must contain at least one item.");
                    continue;
                }
                break;
            }

            if (itemNum < 1 || itemNum > restaurant.Menu.FoodItems.Count)
            {
                Console.WriteLine($"Error: Please enter a number between 1 and {restaurant.Menu.FoodItems.Count}");
                continue;
            }

            // Get quantity
            Console.Write("Enter quantity: ");
            string qtyInput = Console.ReadLine();

            if (!int.TryParse(qtyInput, out int quantity) || quantity < 1)
            {
                Console.WriteLine("Error: Quantity must be a positive number.");
                continue;
            }

            FoodItem selectedItem = restaurant.Menu.FoodItems[itemNum - 1];
            OrderedFoodItem orderedItem = new OrderedFoodItem(selectedItem, quantity);
            newOrder.AddOrderedFoodItem(orderedItem);
            Console.WriteLine($"Added: {selectedItem.ItemName} x{quantity}");
        }

        // Ask for special request
        Console.Write("Add special request? [Y/N]: ");
        string specialRequestChoice = Console.ReadLine().ToUpper();

        string specialRequest = "";
        if (specialRequestChoice == "Y")
        {
            Console.Write("Enter special request: ");
            specialRequest = Console.ReadLine();
            // Store in first item's Customise field
            if (newOrder.OrderedFoodItems.Count > 0)
            {
                newOrder.OrderedFoodItems[0].FoodItem.Customise = specialRequest;
            }
        }

        // Display order total
        double subtotal = newOrder.OrderTotal - Order.DELIVERY_FEE;
        Console.WriteLine($"\nOrder Total: ${subtotal:F2} + ${Order.DELIVERY_FEE:F2} (delivery) = ${newOrder.OrderTotal:F2}");

        // Ask for payment
        Console.Write("Proceed to payment? [Y/N]: ");
        string paymentChoice = Console.ReadLine().ToUpper();

        if (paymentChoice != "Y")
        {
            Console.WriteLine("Order cancelled.");
            return;
        }

        // Get payment method
        string paymentMethod = "";
        while (true)
        {
            Console.Write("Payment method:\n[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            paymentMethod = Console.ReadLine().ToUpper();

            if (paymentMethod == "CC" || paymentMethod == "PP" || paymentMethod == "CD")
            {
                break;
            }
            Console.WriteLine("Error: Invalid payment method. Choose CC, PP, or CD.");
        }

        newOrder.OrderPaymentMethod = paymentMethod;
        newOrder.OrderStatus = "Pending";
        newOrder.OrderPaid = true;

        // Add order to customer's list
        customer.AddOrder(newOrder);

        // Add order to restaurant's queue
        restaurant.OrderQueue.Enqueue(newOrder);

        // Append to orders.csv
        try
        {
            string itemsString = "";
            for (int i = 0; i < newOrder.OrderedFoodItems.Count; i++)
            {
                OrderedFoodItem oi = newOrder.OrderedFoodItems[i];
                itemsString += $"{oi.FoodItem.ItemName},{oi.QtyOrdered}";
                if (i < newOrder.OrderedFoodItems.Count - 1)
                {
                    itemsString += "|";
                }
            }

            string orderLine = $"{newOrder.OrderId},{customer.EmailAddress},{restaurant.RestaurantId}," +
                             $"{newOrder.DeliveryDateTime:dd/MM/yyyy},{newOrder.DeliveryDateTime:HH:mm}," +
                             $"{newOrder.DeliveryAddress},{newOrder.OrderDateTime:dd/MM/yyyy HH:mm}," +
                             $"{newOrder.OrderTotal:F1},{newOrder.OrderStatus},\"{itemsString}\"";

            File.AppendAllText("orders.csv", orderLine + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not save to file: {ex.Message}");
        }

        // Increment order ID for next order
        nextOrderId++;

        Console.WriteLine($"\nOrder {newOrder.OrderId} created successfully! Status: {newOrder.OrderStatus}");
    }

    // Feature 6: Process order
    static void ProcessOrder()
    {
        Console.WriteLine("\nProcess Order");
        Console.WriteLine("=============");

        // Get restaurant ID
        Console.Write("Enter Restaurant ID: ");
        string restaurantId = Console.ReadLine().ToUpper();
        Restaurant restaurant = FindRestaurantById(restaurantId);

        if (restaurant == null)
        {
            Console.WriteLine("Error: Restaurant not found.");
            return;
        }

        if (restaurant.OrderQueue.Count == 0)
        {
            Console.WriteLine("No orders to process for this restaurant.");
            return;
        }

        // Process orders in the queue
        int ordersProcessed = 0;
        Queue<Order> tempQueue = new Queue<Order>();

        while (restaurant.OrderQueue.Count > 0)
        {
            Order order = restaurant.OrderQueue.Dequeue();

            // Display order details
            Console.WriteLine($"\nOrder {order.OrderId}:");
            Console.WriteLine($"Customer: {order.Customer.CustomerName}");
            Console.WriteLine("Ordered Items:");

            for (int i = 0; i < order.OrderedFoodItems.Count; i++)
            {
                OrderedFoodItem item = order.OrderedFoodItems[i];
                Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
            }

            Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
            Console.WriteLine($"Order Status: {order.OrderStatus}");

            // Get action
            string action = "";
            while (true)
            {
                Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
                action = Console.ReadLine().ToUpper();

                if (action == "C" || action == "R" || action == "S" || action == "D")
                {
                    break;
                }
                Console.WriteLine("Error: Invalid choice. Enter C, R, S, or D.");
            }

            bool requeue = true;

            switch (action)
            {
                case "C":
                    // Confirm - can only confirm pending orders
                    if (order.OrderStatus == "Pending")
                    {
                        order.UpdateStatus("Preparing");
                        Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                    }
                    else
                    {
                        Console.WriteLine($"Error: Cannot confirm order with status '{order.OrderStatus}'. Only 'Pending' orders can be confirmed.");
                    }
                    break;

                case "R":
                    // Reject - can only reject pending orders
                    if (order.OrderStatus == "Pending")
                    {
                        order.UpdateStatus("Rejected");
                        refundStack.Push(order);
                        Console.WriteLine($"Order {order.OrderId} rejected. Refund of ${order.OrderTotal:F2} processed.");
                        requeue = false; // Don't put rejected orders back in queue
                    }
                    else
                    {
                        Console.WriteLine($"Error: Cannot reject order with status '{order.OrderStatus}'. Only 'Pending' orders can be rejected.");
                    }
                    break;

                case "S":
                    // Skip - only skip cancelled orders
                    if (order.OrderStatus == "Cancelled")
                    {
                        Console.WriteLine($"Order {order.OrderId} skipped.");
                        requeue = false; // Don't requeue cancelled orders
                    }
                    else
                    {
                        Console.WriteLine($"Skipped order {order.OrderId}.");
                    }
                    break;

                case "D":
                    // Deliver - can only deliver preparing orders
                    if (order.OrderStatus == "Preparing")
                    {
                        order.UpdateStatus("Delivered");
                        Console.WriteLine($"Order {order.OrderId} delivered. Status: Delivered");
                        requeue = false; // Don't requeue delivered orders
                    }
                    else
                    {
                        Console.WriteLine($"Error: Cannot deliver order with status '{order.OrderStatus}'. Only 'Preparing' orders can be delivered.");
                    }
                    break;
            }

            // Requeue if needed
            if (requeue)
            {
                tempQueue.Enqueue(order);
            }

            ordersProcessed++;
        }

        // Put remaining orders back in queue
        while (tempQueue.Count > 0)
        {
            restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
        }

        Console.WriteLine($"\n{ordersProcessed} order(s) processed.");
    }

    // Feature 7: Modify order
    static void ModifyOrder()
    {
        Console.WriteLine("\nModify Order");
        Console.WriteLine("============");

        // Get customer email
        Console.Write("Enter Customer Email: ");
        string customerEmail = Console.ReadLine();
        Customer customer = FindCustomerByEmail(customerEmail);

        if (customer == null)
        {
            Console.WriteLine("Error: Customer not found.");
            return;
        }

        // Get pending orders
        List<Order> pendingOrders = customer.GetPendingOrders();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found for this customer.");
            return;
        }

        // Display pending orders
        Console.WriteLine("Pending Orders:");
        foreach (Order order in pendingOrders)
        {
            Console.WriteLine(order.OrderId);
        }

        // Get order ID
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Error: Invalid Order ID.");
            return;
        }

        Order selectedOrder = customer.FindOrder(orderId);

        if (selectedOrder == null)
        {
            Console.WriteLine("Error: Order not found.");
            return;
        }

        if (selectedOrder.OrderStatus != "Pending")
        {
            Console.WriteLine($"Error: Cannot modify order with status '{selectedOrder.OrderStatus}'. Only 'Pending' orders can be modified.");
            return;
        }

        // Display current order details
        Console.WriteLine("\nOrder Items:");
        for (int i = 0; i < selectedOrder.OrderedFoodItems.Count; i++)
        {
            OrderedFoodItem item = selectedOrder.OrderedFoodItems[i];
            Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
        }

        Console.WriteLine($"\nAddress:");
        Console.WriteLine(selectedOrder.DeliveryAddress);

        Console.WriteLine($"\nDelivery Date/Time:");
        Console.WriteLine($"{selectedOrder.DeliveryDateTime:dd/MM/yyyy, HH:mm}");

        // Get modification choice
        Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                // Modify Items
                double oldTotal = selectedOrder.OrderTotal;
                selectedOrder.OrderedFoodItems.Clear();

                Console.WriteLine("\nAvailable Food Items:");
                for (int i = 0; i < selectedOrder.Restaurant.Menu.FoodItems.Count; i++)
                {
                    FoodItem item = selectedOrder.Restaurant.Menu.FoodItems[i];
                    Console.WriteLine($"{i + 1}. {item.ItemName} - ${item.ItemPrice:F2}");
                }

                while (true)
                {
                    Console.Write("Enter item number (0 to finish): ");
                    if (!int.TryParse(Console.ReadLine(), out int itemNum))
                    {
                        Console.WriteLine("Error: Invalid input.");
                        continue;
                    }

                    if (itemNum == 0)
                    {
                        if (selectedOrder.OrderedFoodItems.Count == 0)
                        {
                            Console.WriteLine("Error: Order must contain at least one item.");
                            continue;
                        }
                        break;
                    }

                    if (itemNum < 1 || itemNum > selectedOrder.Restaurant.Menu.FoodItems.Count)
                    {
                        Console.WriteLine($"Error: Please enter a number between 1 and {selectedOrder.Restaurant.Menu.FoodItems.Count}");
                        continue;
                    }

                    Console.Write("Enter quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
                    {
                        Console.WriteLine("Error: Quantity must be a positive number.");
                        continue;
                    }

                    FoodItem selectedItem = selectedOrder.Restaurant.Menu.FoodItems[itemNum - 1];
                    OrderedFoodItem orderedItem = new OrderedFoodItem(selectedItem, quantity);
                    selectedOrder.AddOrderedFoodItem(orderedItem);
                    Console.WriteLine($"Added: {selectedItem.ItemName} x{quantity}");
                }

                selectedOrder.CalculateOrderTotal();

                // Check if payment needed
                if (selectedOrder.OrderTotal > oldTotal)
                {
                    double difference = selectedOrder.OrderTotal - oldTotal;
                    Console.WriteLine($"\nAdditional payment required: ${difference:F2}");
                    Console.Write("Proceed with payment? [Y/N]: ");
                    string payChoice = Console.ReadLine().ToUpper();

                    if (payChoice != "Y")
                    {
                        Console.WriteLine("Order modification cancelled.");
                        return;
                    }
                }

                Console.WriteLine($"Order {orderId} updated. New Total: ${selectedOrder.OrderTotal:F2}");
                break;

            case "2":
                // Modify Address
                Console.Write("Enter new Delivery Address: ");
                string newAddress = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newAddress))
                {
                    Console.WriteLine("Error: Address cannot be empty.");
                    return;
                }

                selectedOrder.ModifyDeliveryAddress(newAddress);
                Console.WriteLine($"Order {orderId} updated. New Address: {newAddress}");
                break;

            case "3":
                // Modify Delivery Time
                DateTime newTime;
                while (true)
                {
                    Console.Write("Enter new Delivery Time (hh:mm): ");
                    string timeInput = Console.ReadLine();

                    try
                    {
                        newTime = ParseTime(timeInput);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Error: Invalid time format. Use hh:mm");
                    }
                }

                DateTime newDateTime = selectedOrder.DeliveryDateTime.Date + newTime.TimeOfDay;

                // Validate not in the past
                if (newDateTime < DateTime.Now)
                {
                    Console.WriteLine("Error: Delivery time cannot be in the past.");
                    return;
                }

                selectedOrder.ModifyDeliveryTime(newDateTime);
                Console.WriteLine($"Order {orderId} updated. New Delivery Time: {newDateTime:HH:mm}");
                break;

            default:
                Console.WriteLine("Error: Invalid choice.");
                break;
        }
    }

    // Feature 8: Delete order
    static void DeleteOrder()
    {
        Console.WriteLine("\nDelete Order");
        Console.WriteLine("============");

        // Get customer email
        Console.Write("Enter Customer Email: ");
        string customerEmail = Console.ReadLine();
        Customer customer = FindCustomerByEmail(customerEmail);

        if (customer == null)
        {
            Console.WriteLine("Error: Customer not found.");
            return;
        }

        // Get pending orders
        List<Order> pendingOrders = customer.GetPendingOrders();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found for this customer.");
            return;
        }

        // Display pending orders
        Console.WriteLine("Pending Orders:");
        foreach (Order order in pendingOrders)
        {
            Console.WriteLine(order.OrderId);
        }

        // Get order ID
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Error: Invalid Order ID.");
            return;
        }

        Order selectedOrder = customer.FindOrder(orderId);

        if (selectedOrder == null)
        {
            Console.WriteLine("Error: Order not found.");
            return;
        }

        if (selectedOrder.OrderStatus != "Pending")
        {
            Console.WriteLine($"Error: Cannot delete order with status '{selectedOrder.OrderStatus}'. Only 'Pending' orders can be deleted.");
            return;
        }

        // Display order details
        Console.WriteLine($"\nCustomer: {selectedOrder.Customer.CustomerName}");
        Console.WriteLine("Ordered Items:");

        for (int i = 0; i < selectedOrder.OrderedFoodItems.Count; i++)
        {
            OrderedFoodItem item = selectedOrder.OrderedFoodItems[i];
            Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
        }

        Console.WriteLine($"Delivery date/time: {selectedOrder.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${selectedOrder.OrderTotal:F2}");
        Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");

        // Confirm deletion
        Console.Write("\nConfirm deletion? [Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();

        if (confirm != "Y")
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        // Update order status to Cancelled
        selectedOrder.UpdateStatus("Cancelled");

        // Add to refund stack
        refundStack.Push(selectedOrder);

        // Remove from restaurant's queue
        Queue<Order> tempQueue = new Queue<Order>();
        while (selectedOrder.Restaurant.OrderQueue.Count > 0)
        {
            Order order = selectedOrder.Restaurant.OrderQueue.Dequeue();
            if (order.OrderId != orderId)
            {
                tempQueue.Enqueue(order);
            }
        }

        // Put remaining orders back
        while (tempQueue.Count > 0)
        {
            selectedOrder.Restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
        }

        Console.WriteLine($"Order {orderId} cancelled. Refund of ${selectedOrder.OrderTotal:F2} processed.");
    }

    // Advanced Feature (a): Bulk process unprocessed orders
    static void BulkProcessOrders()
    {
        Console.WriteLine("\nBulk Process Unprocessed Orders");
        Console.WriteLine("================================");

        // Count total pending orders across all restaurants
        int totalPendingOrders = 0;
        foreach (Restaurant restaurant in restaurantList)
        {
            foreach (Order order in restaurant.OrderQueue)
            {
                if (order.OrderStatus == "Pending")
                {
                    totalPendingOrders++;
                }
            }
        }

        Console.WriteLine($"Total pending orders found: {totalPendingOrders}");

        if (totalPendingOrders == 0)
        {
            Console.WriteLine("No pending orders to process.");
            return;
        }

        int ordersProcessed = 0;
        int ordersPreparing = 0;
        int ordersRejected = 0;
        DateTime now = DateTime.Now;

        // Process each restaurant's queue
        foreach (Restaurant restaurant in restaurantList)
        {
            Queue<Order> tempQueue = new Queue<Order>();

            while (restaurant.OrderQueue.Count > 0)
            {
                Order order = restaurant.OrderQueue.Dequeue();

                if (order.OrderStatus == "Pending")
                {
                    // Calculate time until delivery
                    TimeSpan timeUntilDelivery = order.DeliveryDateTime - now;

                    if (timeUntilDelivery.TotalHours < 1)
                    {
                        // Reject if less than 1 hour
                        order.UpdateStatus("Rejected");
                        refundStack.Push(order);
                        ordersRejected++;
                        Console.WriteLine($"Order {order.OrderId} rejected (delivery time < 1 hour). Refund: ${order.OrderTotal:F2}");
                    }
                    else
                    {
                        // Set to Preparing
                        order.UpdateStatus("Preparing");
                        ordersPreparing++;
                        Console.WriteLine($"Order {order.OrderId} set to Preparing.");
                        tempQueue.Enqueue(order);
                    }

                    ordersProcessed++;
                }
                else
                {
                    // Keep non-pending orders in queue
                    tempQueue.Enqueue(order);
                }
            }

            // Put orders back in queue
            while (tempQueue.Count > 0)
            {
                restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
            }
        }

        // Calculate total orders across all customers
        int totalOrders = 0;
        foreach (Customer customer in customerList)
        {
            totalOrders += customer.OrderList.Count;
        }

        double percentageProcessed = totalOrders > 0 ? (ordersProcessed * 100.0 / totalOrders) : 0;

        // Display summary
        Console.WriteLine("\n===== Summary Statistics =====");
        Console.WriteLine($"Orders processed: {ordersProcessed}");
        Console.WriteLine($"Orders set to Preparing: {ordersPreparing}");
        Console.WriteLine($"Orders rejected: {ordersRejected}");
        Console.WriteLine($"Percentage of automatically processed orders: {percentageProcessed:F2}%");
    }

    // Advanced Feature (b): Display total order amounts
    static void DisplayTotalOrderAmounts()
    {
        Console.WriteLine("\nTotal Order Amounts");
        Console.WriteLine("===================");

        double grandTotalOrderAmount = 0;
        double grandTotalRefunds = 0;
        const double GRUBEROO_COMMISSION = 0.30; // 30%

        foreach (Restaurant restaurant in restaurantList)
        {
            double restaurantTotal = 0;
            double restaurantRefunds = 0;
            int deliveredCount = 0;
            int refundedCount = 0;

            // Get all orders for this restaurant from all customers
            foreach (Customer customer in customerList)
            {
                foreach (Order order in customer.OrderList)
                {
                    if (order.Restaurant.RestaurantId == restaurant.RestaurantId)
                    {
                        if (order.OrderStatus == "Delivered")
                        {
                            // Subtract delivery fee from order total
                            double orderAmountWithoutDelivery = order.OrderTotal - Order.DELIVERY_FEE;
                            restaurantTotal += orderAmountWithoutDelivery;
                            deliveredCount++;
                        }
                        else if (order.OrderStatus == "Cancelled" || order.OrderStatus == "Rejected")
                        {
                            restaurantRefunds += order.OrderTotal;
                            refundedCount++;
                        }
                    }
                }
            }

            if (deliveredCount > 0 || refundedCount > 0)
            {
                Console.WriteLine($"\n{restaurant.RestaurantName} ({restaurant.RestaurantId})");
                Console.WriteLine($"  Delivered orders: {deliveredCount}");
                Console.WriteLine($"  Total order amount: ${restaurantTotal:F2}");
                Console.WriteLine($"  Refunded orders: {refundedCount}");
                Console.WriteLine($"  Total refunds: ${restaurantRefunds:F2}");

                grandTotalOrderAmount += restaurantTotal;
                grandTotalRefunds += restaurantRefunds;
            }
        }

        // Calculate Gruberoo's earnings (30% commission on delivered orders)
        double gruberooEarnings = grandTotalOrderAmount * GRUBEROO_COMMISSION;

        Console.WriteLine("\n===== Overall Summary =====");
        Console.WriteLine($"Total order amount (all restaurants): ${grandTotalOrderAmount:F2}");
        Console.WriteLine($"Total refunds: ${grandTotalRefunds:F2}");
        Console.WriteLine($"Gruberoo commission (30%): ${gruberooEarnings:F2}");
        Console.WriteLine($"Net amount (after refunds): ${grandTotalOrderAmount - grandTotalRefunds:F2}");
    }

    // Save queue and stack on exit
    static void SaveQueueAndStack()
    {
        try
        {
            // Save Queue data
            using (StreamWriter writer = new StreamWriter("queue.csv"))
            {
                writer.WriteLine("RestaurantId,RestaurantName,OrderId,CustomerEmail,Status,TotalAmount");

                foreach (Restaurant restaurant in restaurantList)
                {
                    foreach (Order order in restaurant.OrderQueue)
                    {
                        writer.WriteLine($"{restaurant.RestaurantId},{restaurant.RestaurantName}," +
                                       $"{order.OrderId},{order.Customer.EmailAddress}," +
                                       $"{order.OrderStatus},{order.OrderTotal:F2}");
                    }
                }
            }

            // Save Stack data
            using (StreamWriter writer = new StreamWriter("stack.csv"))
            {
                writer.WriteLine("OrderId,CustomerEmail,RestaurantId,Status,TotalAmount,RefundAmount");

                foreach (Order order in refundStack)
                {
                    writer.WriteLine($"{order.OrderId},{order.Customer.EmailAddress}," +
                                   $"{order.Restaurant.RestaurantId},{order.OrderStatus}," +
                                   $"{order.OrderTotal:F2},{order.OrderTotal:F2}");
                }
            }

            Console.WriteLine("Queue and stack data saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
}
