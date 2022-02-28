(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=quizzes/fundamentals-PracticeQuiz.ipynb)&emsp;
[![Script](../img/badge-script.svg)](/Teaching//quizzes/fundamentals-PracticeQuiz.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](/Teaching//quizzes/fundamentals-PracticeQuiz.ipynb)

## Question 1

Calculate `3.0` to the power of `4.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float = 81.0
```

</details>
</span>
</p>
</div>

## Question 2

Assign the integer `1` to a value called `a`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val a : int = 1
```

</details>
</span>
</p>
</div>

## Question 3

Write a function named `add3` that adds `3.0` to any `float` input.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val add3 : x:float -> float
```

</details>
</span>
</p>
</div>

## Question 4

Given a tuple `(1.0,2.0)`, assign the second element to a value named `b`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val b : float = 2.0
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val b1 : float = 2.0
```

</details>
</span>
</p>
</div>

## Question 5

Create a tuple where the first, second, and third elements are `"a"`, `1`, and `2.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : string * int * float = ("a", 1, 2.0)
```

</details>
</span>
</p>
</div>

## Question 6

Define a record type named `Record1` that has a `string` `Id` field and a `float Y` field.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
type Record1 =
  { Id: string
    Y: float }
```

</details>
</span>
</p>
</div>

## Question 7

Given the type signature `val a : float = 2.0`, what is the type of value a?

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

float

</details>
</span>
</p>
</div>

## Question 8

Create a record type named `Record2`. It should have two integer fields `X` and `Y`. Create an instance of the record where `X = 4` and `Y = 2`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
type Record2 =
  { X: int
    Y: int }
val it : Record2 = { X = 4
                     Y = 2 }
```

</details>
</span>
</p>
</div>

## Question 9

Explain why this expression gives an error when you try to run it: `4 + 7.0`

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

Because 4 is an integer and 7.0 is a float. Addition is defined on values with the same type.
The two values need to either both be integers or both be floats.

</details>
</span>
</p>
</div>

## Question 10

Create an `array` where the elements are `1`, `2`, and `3`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int [] = [|1; 2; 3|]
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int [] = [|1; 2; 3|]
```

or

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int [] = [|1; 2; 3|]
```

</details>
</span>
</p>
</div>

## Question 11

Take a `list` containing floats `1.0 .. 10.0`. Pass it to `List.map` and use an anonymous function to divide each number by `3.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float list =
  [0.3333333333; 0.6666666667; 1.0; 1.333333333; 1.666666667; 2.0; 2.333333333;
   2.666666667; 3.0; 3.333333333]
```

</details>
</span>
</p>
</div>

## Question 12

Take a `list` containing floats `1.0 .. 10.0`. Group the elements based on whether the elements are greater than or equal to `4.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : (bool * float list) list =
  [(false, [1.0; 2.0; 3.0]); (true, [4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0])]
```

</details>
</span>
</p>
</div>

## Question 13

Take a `list` containing floats `1.0 .. 10.0`. Filter it so that you are left with the elements `> 5.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float list = [6.0; 7.0; 8.0; 9.0; 10.0]
```

</details>
</span>
</p>
</div>

## Question 14

Take a `list` containing floats `1.0 .. 10.0`. Use `List.groupBy` to group the elements based on if they're `>= 5.0`. Then use `List.map` to get the maxiumum element that is `< 5.0` and the minimum value that is `>= 5.0`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float list = [4.0; 5.0]
```

</details>
</span>
</p>
</div>

## Question 15

Take a `list` containing floats `1.0 .. 10.0`. Use functions from the List module to sort it in descending order. Then take the 3rd element of the reversed list and add `7.0` to it.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float = 15.0
```

</details>
</span>
</p>
</div>

*)

