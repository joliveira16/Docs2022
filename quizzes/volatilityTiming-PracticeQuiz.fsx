(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=quizzes/volatilityTiming-PracticeQuiz.ipynb)&emsp;
[![Script](../img/badge-script.svg)](/Teaching//quizzes/volatilityTiming-PracticeQuiz.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](/Teaching//quizzes/volatilityTiming-PracticeQuiz.ipynb)

## Question 1

Given the list below, filter the list so that only numbers greater than `2` remain.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list = [7]
```

</details>
</span>
</p>
</div>

## Question 2

Given the list below, take elements until you find one that is greater than `4`.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list = [1; -4]
```

</details>
</span>
</p>
</div>

## Question 3

Given the list below, take elements until you find one that is greater than `4`.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list = [7; 2; -10]
```

</details>
</span>
</p>
</div>

## Question 4

Given the list below, return tuples of all consecutive pairs.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : (int * int) list = [(1, -4); (-4, 7); (7, 2); (2, -10)]
```

</details>
</span>
</p>
</div>

## Question 5

Given the list below, return sliding windows of 3 consecutive observations.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list list = [[1; -4; 7]; [-4; 7; 2]; [7; 2; -10]]
```

</details>
</span>
</p>
</div>

## Question 6

Given the list below, use `scan` to return the intermediate and final cumulative sums.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list = [0; 1; -3; 4; 6; -4]
```

</details>
</span>
</p>
</div>

## Question 7

Given the list below, use `fold` to return the final sum.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int = -4
```

</details>
</span>
</p>
</div>

## Question 8

Given the list below, use `mapFold` to return the intermediate and final cumulative sums.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int list * int = ([1; -3; 4; 6; -4], -4)
```

</details>
</span>
</p>
</div>

## Question 9

Given the list below, use `mapFold` to return a tuple of

0 A new list in which each element of the original list is transformed by adding `1` to it and then converted into a `string`.

1 The final cumulative sums of the list elements.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : string list * int = (["2"; "-3"; "8"; "3"; "-9"], -4)
```

</details>
</span>
</p>
</div>

## Question 10

Given the list below, use `mapFold` to return a tuple of

0 The list of records with the `Y` field in each record updated to `Y+1`

1 The sum of the `Y` fields.

*)
type R1 = { X : string; Y : int }

let r1xs =
    [ { X = "a"; Y = 1 }
      { X = "b"; Y = -4 }
      { X = "c"; Y = 7 } 
      { X = "d"; Y = 2 }
      { X = "e"; Y = -10 }](* output: 

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>


Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : R1 list * int =
  ([{ X = "a"
      Y = 2 }; { X = "b"
                 Y = -3 }; { X = "c"
                             Y = 8 }; { X = "d"
                                        Y = 3 }; { X = "e"
                                                   Y = -9 }], -4)


</details>
</span>
</p>
</div>
*)
(**
## Question 11

Given the list below, sum all the elements.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int = -4
```

</details>
</span>
</p>
</div>

## Question 12

Given the list below, add `1` to all the elements and then calculate the sum.

```fsharp
[ 1; -4; 7; 2; -10]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int = 1
```

</details>
</span>
</p>
</div>

## Question 13

Given the list below, calculate the `average` of the elements in the list.

```fsharp
[ 1.0; -4.0; 7.0; 2.0; -10.0]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float = -0.8
```

</details>
</span>
</p>
</div>

## Question 14

Given the list below, convert each element to a `decimal` and then calculate the `average` of the elements in the list.

```fsharp
[ 1.0; -4.0; 7.0; 2.0; -10.0]
```
<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : decimal = -0.8M
```

Since `decimal` is a function that converts to
the `decimal` type, you could also do.
The FSharp linter shouLd show you a blue squiggly
in the above code telling you this.

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : decimal = -0.8M
```

</details>
</span>
</p>
</div>

*)

