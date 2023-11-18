string products = Console.ReadLine();

Dictionary<string, List<double>>  items = new Dictionary<string, List<double>>();

while (products!="buy")
{
    List<string> products1 = products.Split(' ').ToList();
    string product = products1[0];
    double price = double.Parse(products1[1]);
    double quantity = double.Parse(products1[2]);
    
   if (!items.ContainsKey(product))
    {
        items.Add(product,new List<double>()); 
        items[product].Add(price);
        items[product].Add(quantity);

     }
    else
    {
        items[product][0] = price;
        items[product][1] += quantity;
    }
    products = Console.ReadLine();
}
foreach (KeyValuePair<string, List<double>> currentproduct in items)
{
    string currentProductName = currentproduct.Key;
    double currentProductPrice = currentproduct.Value[0];
    double currentProductQuantity = currentproduct.Value[1];

    double totalPrice=currentProductPrice*currentProductQuantity;

    Console.WriteLine($"{currentProductName} -> {totalPrice:f2}");
}
