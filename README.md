Gruberoo Food Delivery System
PRG2 Assignment
Student Number: S10273116H
Student Name: Gabriel Chow
Partner Name: V Raghav Raj

Team Contributions
Gabriel Chow

Feature 1: Load Restaurants and Food Items
Feature 4: List All Orders
Feature 6: Process Orders
Feature 8: Delete Orders
Advanced Feature (b): Display Total Order Amounts
Bonus Feature: Favourite Orders Statistics

V Raghav Raj

Feature 2: Load Customers and Orders
Feature 3: List All Restaurants and Menu Items
Feature 5: Create New Order
Feature 7: Modify Existing Order
Advanced Feature (a): Bulk Process Unprocessed Orders
Bonus Feature: Special Offers Management


 Class Structure

FoodItem.cs - Menu item with name, description, price, and customisation
Menu.cs - Collection of food items for a restaurant
SpecialOffer.cs - Promotional offers with discount codes and amounts
OrderedFoodItem.cs - Food item in an order with quantity and subtotal
Restaurant.cs - Restaurant with menu, special offers, and order queue
Customer.cs - Customer with email, name, and order history
Order.cs - Order with items, payment details, and delivery information
Program.cs - Main program with all features


Features Implemented
Basic Features (50%)
Feature 1: Load Restaurants and Food Items (Gabriel)
Feature 2: Load Customers and Orders (Raghav)
Feature 3: List All Restaurants and Menu Items (Raghav)
Feature 4: List All Orders (Gabriel)
Feature 5: Create New Order (Raghav)
Feature 6: Process Orders (Gabriel)
Feature 7: Modify Existing Order (Raghav)
Feature 8: Delete Order (Gabriel)
Advanced Features (20%)
Feature (a): Bulk Process Unprocessed Orders (Raghav)

Automatically processes all pending orders
Confirms orders with >1 hour until delivery (status → Preparing)
Rejects orders with <1 hour until delivery (status → Rejected, adds to refund stack)
Displays processing statistics and percentage

Feature (b): Display Total Order Amounts (Gabriel)

Shows delivered and refunded orders per restaurant
Calculates total order amounts excluding delivery fees
Displays Gruberoo's 30% commission earnings
Shows net amounts after refunds

Bonus Features
Special Offers Management (Raghav)

Loads special offers from CSV (by restaurant name)
Displays available offers when creating orders
Applies percentage discounts or free delivery
Tracks offer usage count and actual savings
Displays comprehensive usage statistics showing:

Offers used per restaurant
Actual customer savings (not estimated)
Most popular offer across all restaurants



Favourite Orders (Gabriel)

Mark/unmark delivered orders as favourites
Display favourite orders with star (★) indicator
Statistics per customer:

Total favourite orders
Total amount spent on favourites
Favourite orders by restaurant
Most frequently ordered restaurant


System-wide statistics:

Total favourite orders
Total amount from favourites
Most popular restaurant across all favourites




Data Files
Input Files:

restaurants.csv - 15 restaurants
fooditems.csv - 169 menu items
specialoffers.csv - 19 promotional offers
customers.csv - 20 customers
orders.csv - 35 historical orders

Output Files (Generated on Exit):

queue.csv - Restaurant order queues
stack.csv - Refunded orders


Key Implementation Details
Special Features in Create Order:

Date/time validation (prevents past dates)
Special offer application with discount calculation
Special request handling (stored in first item's customise field)
Payment method selection (CC/PP/CD)
Order appended to CSV file

Enhanced Modify Order:

Interactive menu with three sub-options:

[1] Add new item (merges if item exists)
[2] Delete item (prevents deleting last item)
[3] Edit quantity (enter 0 to delete)


Real-time total updates
Transparent payment calculation (shows old vs new total with explanation)
Handles refunds for decreased totals

Process Order Features:

Four actions: Confirm, Reject, Skip, Deliver
Status validation (only Pending can be confirmed/rejected, only Preparing can be delivered)
Proper queue management (removes completed/cancelled orders)
Refund stack for rejected/cancelled orders

Program Flow

System loads all data files on startup
Main menu displays 11 options + Exit
User selects features to execute
Data structures updated in memory
New orders appended to orders.csv
Queue and stack saved to CSV files on exit

Testing Notes

All 8 basic features fully implemented and tested
Both advanced features (a) and (b) working correctly
Special Offers bonus feature tracks actual savings
Favourite Orders bonus feature with complete statistics
CSV parsing handles quoted fields and special characters
Input validation implemented throughout
Error handling with try-catch blocks


Additional Notes

Property Naming: Uses camelCase properties (e.g., restaurantId, customerName) as per class diagram
Order ID Tracking: Automatically increments from highest existing order ID
Queue Management: Non-delivered orders loaded into restaurant queues on startup
Payment Transparency: ModifyOrder clearly explains additional payment or refund amounts
Discount Tracking: Special offers track actual savings, not estimates