string resource = Console.ReadLine();

Dictionary<string,int> resourses= new Dictionary<string,int>(); 

while(resource!="stop")
{
    int quantity=int.Parse(Console.ReadLine()); 

    if(resourses.ContainsKey(resource))
    {
        resourses[resource] += quantity;
    }
    else
    {
        resourses.Add(resource, quantity);
    }
    resource= Console.ReadLine();
}

foreach (var res in resourses)
{
    Console.WriteLine($"{res.Key} -> {res.Value}");
}