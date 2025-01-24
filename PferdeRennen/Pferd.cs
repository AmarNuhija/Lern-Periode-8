using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Pferd
{
    public string Name { get; set; }
    public decimal Quote { get; set; }

    public Pferd(string name, decimal quote)
    {
        Name = name;
        Quote = quote;
    }
}