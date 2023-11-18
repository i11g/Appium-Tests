string input = Console.ReadLine();

Dictionary<char,int> occuarences= new Dictionary<char,int>();

foreach (char ch in input)
{
    if (ch == ' ')
    {
        continue;
    }
    if (occuarences.ContainsKey(ch))
    {
        occuarences[ch]++;
    }
    else
    {
        occuarences.Add(ch, 1);
    }

}
foreach (KeyValuePair<char,int> ch in occuarences)
{
    Console.WriteLine($"{ch.Key} - {ch.Value}");
}
