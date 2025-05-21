// See https://aka.ms/new-console-template for more information


const decimal pi = 3.14159265358979323846264338327950288419716M;
Console.WriteLine($"PI is {pi}");


var bestR = -1.0M;
var bestD = -1.0M;
var bestDp = -1;

for (decimal d = 1; d < 100000; d++)
{
    for (var r = d*3; r < d*4; r++)
    {
        var attempt = r / d;
        var score = CountCommonDecimalPlaces(attempt);
        if (score > bestDp)
        {
            bestDp = score;
            bestR = r;
            bestD = d;
            Console.WriteLine($"{bestR}/{bestD} == {attempt} is {bestDp}dp");
        }
    }
}

Console.WriteLine("Done");

return;

static int CountCommonDecimalPlaces(decimal a)
{
    if (a is < 3 or > 4) return 0;
    
    a = Math.Abs(a);
    var b = pi;

    a -= Math.Floor(a); // Get fractional part
    b -= Math.Floor(b);

    var count = 0;
    for (var i = 0; i < 100; i++)
    {
        a *= 10;
        b *= 10;

        var digitA = (int)Math.Floor(a);
        var digitB = (int)Math.Floor(b);

        if (digitA != digitB)
            break;

        count++;

        a -= digitA;
        b -= digitB;
    }

    return count;
}

// int Measure(decimal attempt)
// {
//   
//     var difference = Math.Abs(attempt - pi);
//     var matches = -Math.Floor(Math.Log10((double)Math.Abs(difference))) - 1;
//     return (int)matches;
// }

// int Measure(string attempt)
// {
//     var actual = File.ReadAllText("/Users/davidbetteridge/pi/CalculatePI/CalculatePI/pi.txt");
//     var matches = 0;
//     for (var i = 0; i < attempt.Length; i++)
//     {
//         if (actual[i] == attempt[i])
//             matches++;
//         else
//             break;
//     }
//
//     return matches;
// }