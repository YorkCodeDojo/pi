# Calculating PI

## Exercise 1
Calculate PI using 22/7.   How many decimal places is this correct to?

## Exercise 2
Can you find a different division which is accurate to more decimal places?

## Exercise 3
Try using the Gregory-Leibniz series

π = $\dfrac{4}{1} - \dfrac{4}{3} + \dfrac{4}{5} - \dfrac{4}{7} + \dfrac{4}{9} - \dfrac{4}{11} + \dfrac{4}{13} - \dfrac{4}{15}$ ....

How many iterations do you need to get a number accurate to 5 decimal places?

## Exercise 4
Try using the Nilakantha series.

π = $3 + \dfrac{4}{2\times3\times4} - \dfrac{4}{4\times5\times6} + \dfrac{4}{6\times7\times8} - \dfrac{4}{8\times9\times10} + \dfrac{4}{10\times11\times12} - \dfrac{4}{12\times13\times44}$ ...

## Exercise 5
Using random numbers!  

Imagine you have a circle inscribed inside a square.
You then throw darts at random at the square
The number of darts inside the circle divided the number of darts will then give you a value for PI.

Steps are...

* Assume the coordinates for your square run from -1,-1 (bottom-left) to 1,1 (top-right)
* Generate a random number for X which is between -1.0 and 1.0
* Generate a random number for Y which is between -1.0 and 1.0
* If the point X,Y is inside the unit circle then increase the HITS count by 1
* Increase the number of THROWS by 1
* The estimate for PI can be found by (HITS / THROWS) * 4
* Repeat the process

The following formula can be used to test if X,Y is inside the circle.

$Distance = |\sqrt{X^2 + Y^2}|$

If Distance is < 1 then the dart is with the circle.



