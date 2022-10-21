using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6;

public struct Weather
{
    public string? Country;
    public string? Name;
    public float Temp;
    public string Description;

    public override string ToString()
    {
        return $"Weather Description:\nCountry:{Country}\nName:{Name}\nTemp:{Temp}\nDescription:{Description}\n";
    }
}