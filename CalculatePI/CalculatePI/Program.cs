// See https://aka.ms/new-console-template for more information


const decimal pi = 3.14159265358979323846264338327950288419716M;
Console.WriteLine($"PI is {pi}");

var rnd = new Random();
var inCircle = 1.0;
var bestDP = 0;
var i = 1;
while (true)
{
    var x = (rnd.NextDouble()*2)-1;
    var y = (rnd.NextDouble()*2)-1;
    var d = Math.Abs(Math.Sqrt((x * x) + (y * y)));

    if (d < 1)
        inCircle++;

    var attempt = (decimal)(inCircle / i) * 4;
    var score = CountCommonDecimalPlaces(attempt);
    if (score > bestDP)
    {
        Console.WriteLine($"{attempt} matches {score}dp with {i} iterations");
        bestDP = score;
    }

    i++;
}


//GregoryLeibniz();
//Fractions();
// Nilakantha();

Console.WriteLine("Done");

return;

void Nilakantha()
{
    var sign = 1;
    var d = 2;
    decimal attempt = 3;
    var bestDP = -1;
    for (var n = 1; n < 50000; n++)
    {
        attempt += ((4 / (decimal)(d * (d+1) * (d+2))) * sign);
        sign = -sign;
        d += 2;
        var score = CountCommonDecimalPlaces(attempt);
        if (score > bestDP)
        {
            Console.WriteLine($"{attempt} matches {score}dp with {n} iterations");
            bestDP = score;
        }
    } 
}

void GregoryLeibniz()
{
    var sign = 1;
    var d = 1;
    decimal attempt = 0;
    var bestDP = -1;
    for (var n = 1; n < 50000000; n++)
    {
        attempt += ((4 / (decimal)d) * sign);
        sign = -sign;
        d += 2;
        var score = CountCommonDecimalPlaces(attempt);
        if (score > bestDP)
        {
            Console.WriteLine($"{attempt} matches {score}dp with {n} iterations");
            bestDP = score;
        }
    }
}

void Fractions()
{
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
}

static int CountCommonDecimalPlaces(decimal a)
{
    if (a is < 3 or > 4) return 0;

    // if (a == 3.14159265358979323846264338327950288419716M) return 1000;
    // if (a == 3.14159265358979323846264338327950288419716M) return 1000;
    // if (a == 3.14159265358979323846264338327950288419716M) return 1000;
    // if (a == 3.1M) return 1000;

    
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