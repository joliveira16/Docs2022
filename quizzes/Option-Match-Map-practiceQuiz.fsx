(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=quizzes/Option-Match-Map-practiceQuiz.ipynb)&emsp;
[![Script](../img/badge-script.svg)](/Teaching//quizzes/Option-Match-Map-practiceQuiz.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](/Teaching//quizzes/Option-Match-Map-practiceQuiz.ipynb)

This practice quiz emphasizes `Optional types`, `Match expressions`, and `Map Collections`. These are some features that we will use in building a portfolio.

Here are some references to these topics. Please read the F# language reference links before proceeding with the questions. The other links (F# tour and F# for fun and profit) provide additional background and examples but are not necessary:

* Option types
  

  * The F# language reference for [options](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/options)
    
  
  * The tour of F# section on [options](https://docs.microsoft.com/en-us/dotnet/fsharp/tour#optional-types)
    
  
  * If you want more a more in depth discussion, see F# for fun and profit's section on [options](https://fsharpforfunandprofit.com/posts/the-option-type/)
    
  

* Pattern matching using match expressions.
  

  * [F# Language reference](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/match-expressions)
    
  
  * [Tour of F#](https://docs.microsoft.com/en-us/dotnet/fsharp/tour#pattern-matching)
    
  
  * [F# for fun and profit](https://fsharpforfunandprofit.com/posts/match-expression/)
    
  

* Map collections.
  

  * [F# Wikibook](https://en.wikibooks.org/wiki/F_Sharp_Programming/Sets_and_Maps#Maps)
    
  

# Options

## Question 1

Create a value named `a` and assign `Some 4` to it.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val a : int option = Some 4
```

</details>
</span>
</p>
</div>

## Question 2

Create a value name `b` and assign `None` to it.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val b : 'a option
```

</details>
</span>
</p>
</div>

## Question 3

Create a tuple named `c` and assign `(Some 4, None)` to it.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val c : int option * 'a option
```

</details>
</span>
</p>
</div>

## Question 4

Write a function named d that takes `x: float` as an input and outputs
`Some x` if x < 0 and `None` if x >= 0. Test it by mapping each element of
`[0.0; 1.4; -7.0]` by function d.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val d : x:float -> float option
val it : float option list = [None; None; Some -7.0]
```

or, we don't actually have to tell it that `x` is a `float`
because type inference can tell that `x` must be a `float`
because the function does `x < 0.0` and `0.0` is a `float`.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val d2 : x:float -> float option
val it : float option list = [None; None; Some -7.0]
```

</details>
</span>
</p>
</div>

## Question 5

Consider this array of trading days for a stock and it's price and dividends:

*)
type StockDays = { Day : int; Price : decimal; Dividend : decimal Option }
let stockDays = 
    [| for day = 0 to 5 do 
        let dividend = if day % 2 = 0 then None else Some 1m
        { Day = day
          Price = 100m + decimal day
          Dividend = dividend } |]   
(**
0 create a new array called `stockDaysWithDividends` that is a filtered
version of `stockDays` that only contains days with dividends.

1 Then create an array called `stockDaysWithoutDividends` that is a filtered
version of `stockDays` that only contains days that do not have dividends.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockDaysWithDivideds : StockDays [] =
  [|{ Day = 1
      Price = 101M
      Dividend = Some 1M }; { Day = 3
                              Price = 103M
                              Dividend = Some 1M }; { Day = 5
                                                      Price = 105M
                                                      Dividend = Some 1M }|]
```

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockDaysWithoutDividends : StockDays [] =
  [|{ Day = 0
      Price = 100M
      Dividend = None }; { Day = 2
                           Price = 102M
                           Dividend = None }; { Day = 4
                                                Price = 104M
                                                Dividend = None }|]
```

</details>
</span>
</p>
</div>

## Question 6

Consider the value `let nestedOption = (Some (Some 4))`. Pipe
it to `Option.flatten` so that you are left with `Some 4`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val nestedOption : int option option = Some (Some 4)
val it : int option = Some 4
```

</details>
</span>
</p>
</div>

## Question 7

Consider this list `let listOfNestedOptions = [(Some (Some 4)); Some (None); None]`.
Show how to transform it into `[Some 4; None; None]` by mapping a function to each
element of the list.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val listOfNestedOptions : int option option list =
  [Some (Some 4); Some None; None]
val it : int option list = [Some 4; None; None]
```

</details>
</span>
</p>
</div>

# Match Expressions

## Question 1

Write a function named `ma` that takes `x: float Option` as an input.
Use a match expression to output the `float` if `x` is something and
`0.0` if the `float` is nothing. Provide a test case for both cases to show
that the function works.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val ma : x:float option -> float
val ma2Some7 : float = 7.0
val ma2None : float = 0.0
```

or, see the `x` in the (`Some x`) part of the match expression
is the `float`, not the original (`x: float Option`)
To see this, hover your cursor over the first two xs. it says `x is float Option`.
Then hover over the second two xs. It says `x is float`. Two different xs!

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val ma2 : x:float option -> float
val ma2Some7Other : float = 7.0
val ma2NoneOther : float = 0.0
```

</details>
</span>
</p>
</div>

## Question 2

Write a function named `mb` that takes `x: float` as an input.
Use a match expression to output `1.0` if `x` is `1.0`, `4.0` if `x` is `2.0`,
and `x^3.0` if `x` is anything else. Provide 3 tests for the 3 test cases
to show that the function works.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mb : x:float -> float
val mb1 : float = 1.0
val mb2 : float = 4.0
val mb7 : float = 343.0
```

</details>
</span>
</p>
</div>

## Question 3

Write a function named `mc` that takes a tuple pair of ints  `x: int * int`
as an input. Handle these cases in the following order:

0 if the first `int` is `7`, return `"a"`.

1 if the second int is `7`, return `"b"`.

2 For everything else, return `"c"`.

Finally, test the function on `(7,6)`, `(6,7)`, `(7, 7)`, and `(6,6)`.
Make sure that you understand how those 4 examples are handled.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mc : int * int -> string
val mc76 : string = "a"
val mc67 : string = "b"
val mc77 : string = "a"
val mc66 : string = "c"
```

</details>
</span>
</p>
</div>

## Question 4

Consider this array of trading days for a stock and it's price and dividends:

*)
type StockDays2 = { Day : int; Price : decimal; Dividend : decimal Option }
let stockDays2 = 
    [| for day = 0 to 5 do 
        let dividend = if day % 2 = 0 then None else Some 1m
        { Day = day
          Price = 100m + decimal day
          Dividend = dividend } |]   
(**
0 create a new array called `daysWithDividends` that is a filtered
version of `stockDays` that only contains days with dividends. For
each day with a dividend, you should return a `(int * decimal)` tuple
where the int is the day  and the decimal is the dividend.
Thus the result is an `(int * decimal) array`.

1 Then create an array called `daysWithoutDividends` that is a filtered
version of `stockDays` that only contains days that do not have dividends.
For each day without a dividend, you should return the day as an `int`.
Thus the result is an `int array`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

Days With Dividends:

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val daysWithDividends1 : (int * decimal) [] = [|(1, 1M); (3, 1M); (5, 1M)|]
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val daysWithDividends2 : (int * decimal) [] = [|(1, 1M); (3, 1M); (5, 1M)|]
```

Days Without Dividends:

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val daysWithoutDividends : int [] = [|0; 2; 4|]
```

</details>
</span>
</p>
</div>

# Map Collections

## Question 1

Create a Map collection named `mapA`
from the list `[("a",1);("b",2)]` where the first thing
in the tuple is the key and the second thing is the value.

0 Use `Map.tryFind` to retrieve the value for key `"a"`

1 Use `Map.tryFind` to retrieve the value for key `"c"`

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

Create Map Collection:

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapA : Map<string,int> = map [("a", 1); ("b", 2)]
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapA2 : Map<string,int> = map [("a", 1); ("b", 2)]
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapA3 : Map<string,int> = map [("a", 1); ("b", 2)]
```

Use `Map.tryFind` to retrieve the value for key `"a"`:

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int option = Some 1
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int option = Some 1
```

Use `Map.tryFind` to retrieve the value for key `"c"`:

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int option = None
```

or

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : int option = None
```

</details>
</span>
</p>
</div>

## Question 2

Create a Map collection named `mapB`
from the list `[(1,"a");(2,"b")]` where the first thing
in the tuple is the key and the second thing is the value.

0 Use `Map.tryFind` to retrieve the value for key `1`

1 Use `Map.tryFind` to retrieve the value for key `3`

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapB : Map<int,string> = map [(1, "a"); (2, "b")]
val tryFindMapB1 : string option = Some "a"
val tryFindMapB3 : string option = None
```

</details>
</span>
</p>
</div>

## Question 3

Use this array

*)
type StockDays3 = { Day : int; Price : decimal; Dividend : decimal Option }
let stockDays3 = 
    [| for day = 0 to 5 do 
        let dividend = if day % 2 = 0 then None else Some 1m
        { Day = day
          Price = 100m + decimal day
          Dividend = dividend } |]     
(**
0 Create a Map collection named `mapC`. The key should be the day field, 
and the value should be the full `StockDays3` record.

1 Create a Map collection named `mapD`. The key should be the full
`StockDay3` record. The value should be the day field.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapC : Map<int,StockDays3> =
  map
    [(0, { Day = 0
           Price = 100M
           Dividend = None }); (1, { Day = 1
                                     Price = 101M
                                     Dividend = Some 1M });
     (2, { Day = 2
           Price = 102M
           Dividend = None }); (3, { Day = 3
                                     Price = 103M
                                     Dividend = Some 1M });
     (4, { Day = 4
           Price = 104M
           Dividend = None }); (5, { Day = 5
                                     Price = 105M
                                     Dividend = Some 1M })]
```

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val mapD : Map<StockDays3,int> =
  map
    [({ Day = 0
        Price = 100M
        Dividend = None }, 0); ({ Day = 1
                                  Price = 101M
                                  Dividend = Some 1M }, 1);
     ({ Day = 2
        Price = 102M
        Dividend = None }, 2); ({ Day = 3
                                  Price = 103M
                                  Dividend = Some 1M }, 3);
     ({ Day = 4
        Price = 104M
        Dividend = None }, 4); ({ Day = 5
                                  Price = 105M
                                  Dividend = Some 1M }, 5)]
```

</details>
</span>
</p>
</div>

## Question 4

Consider a the following Map collection:

*)
let mapp = [("a", 1); ("d",7)] |> Map.ofList
(**
Write a function named `lookFor` that takes `x: string` as an input and
looks up the value in `mapp`. If it finds `Some y`, print
`"I found y"` to standard output where `y` is the actual integer found.
If it finds `None`, print `"I did not find x"` to standard output
where `x` is the actual key that was looked up. Test it by looking
up `"a"`,`"b"`,"`c"`,`"d"`

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
I found 1
I did not find b
I did not find c
I found 7
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val lookFor : x:string -> unit
val it : unit = ()
```

or iterate it
we use iter instead of map
because the result of iter has type `unit`,
and iter is for when your function has type `unit`.
Basically, unit type means the function did something
(in this case, printed to standard output) but
it doesn't actually return any output.
You could use map, but then we get `unit list` which
isn't really what we want. We just want to iterate
through the list and print to output.

```
I found 1
I did not find b
I did not find c
I found 7
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : unit = ()
```

or loop it

```
I found 1

I did not find b

I did not find c

I found 7

Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : unit = ()
```

</details>
</span>
</p>
</div>

# Joins

For the following questions use this data:

*)
type StockPriceOb =
    { Stock : string 
      Time : int
      Price : int }
type TwoStocksPriceOb =
    { Time : int 
      PriceA : int option 
      PriceB : int option }
let stockA = 
    [{ Stock = "A"; Time = 1; Price = 5}
     { Stock = "A"; Time = 2; Price = 6}]
let stockB =     
    [{ Stock = "B"; Time = 2; Price = 5}
     { Stock = "B"; Time = 3; Price = 4}]
(**
Hint: remember that Map collections are useful for lookups.

## Question 1

Create a `TwoStocksPriceOb list` named `tslA` that has prices for
every observation of `stockA`. If there is a price for `stockB`
at the same time as `stockA`, then include the `stockB` price. Otherwise,
the `stockB` price should be `None`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.003, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockbByTime : Map<int,StockPriceOb> =
  map [(2, { Stock = "B"
             Time = 2
             Price = 5 }); (3, { Stock = "B"
                                 Time = 3
                                 Price = 4 })]
val tslA1 : TwoStocksPriceOb list = [{ Time = 1
                                       PriceA = Some 5
                                       PriceB = None }; { Time = 2
                                                          PriceA = Some 6
                                                          PriceB = Some 5 }]
val tslA2 : TwoStocksPriceOb list = [{ Time = 1
                                       PriceA = Some 5
                                       PriceB = None }; { Time = 2
                                                          PriceA = Some 6
                                                          PriceB = Some 5 }]
val tryFindBforA : dayA:StockPriceOb -> TwoStocksPriceOb
val tslA3 : TwoStocksPriceOb list = [{ Time = 1
                                       PriceA = Some 5
                                       PriceB = None }; { Time = 2
                                                          PriceA = Some 6
                                                          PriceB = Some 5 }]
```

</details>
</span>
</p>
</div>

## Question 2

Create a `TwoStocksPriceOb list` named `tslB` that has prices for
every observation of stockB. If there is a price for `stockA`
at the same time as `stockB`, then include the `stockA` price. Otherwise,
the `stockA` price should be `None`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockaByTime : Map<int,StockPriceOb> =
  map [(1, { Stock = "A"
             Time = 1
             Price = 5 }); (2, { Stock = "A"
                                 Time = 2
                                 Price = 6 })]
val tslB : TwoStocksPriceOb list = [{ Time = 2
                                      PriceA = Some 6
                                      PriceB = Some 5 }; { Time = 3
                                                           PriceA = None
                                                           PriceB = Some 4 }]
```

</details>
</span>
</p>
</div>

## Question 3

Create a `TwoStocksPriceOb list` named `tslC` that only includes times
when there is a price for both `stockA` and `stockB`. The prices for stocks
A and B should always be something.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockaByTime2 : Map<int,StockPriceOb> =
  map [(1, { Stock = "A"
             Time = 1
             Price = 5 }); (2, { Stock = "A"
                                 Time = 2
                                 Price = 6 })]
val tslC1 : TwoStocksPriceOb list = [{ Time = 2
                                       PriceA = Some 6
                                       PriceB = Some 5 }]
val timesA : Set<int> = set [1; 2]
val timesB : Set<int> = set [2; 3]
val timesAandB : Set<int> = set [2]
val tslC2 : TwoStocksPriceOb list = [{ Time = 2
                                       PriceA = Some 6
                                       PriceB = Some 5 }]
```

</details>
</span>
</p>
</div>

## Question 4

Create a `TwoStocksPriceOb list` named `tslD` that includes available
stock prices for `stockA` and `stockB` at all possible times. If a price for
one of the stocks is missing for a given time, it should be None.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val stockATimes : int list = [1; 2]
val stockBTimes : int list = [2; 3]
val allTimes : int list = [1; 2; 3]
val tslD : TwoStocksPriceOb list =
  [{ Time = 1
     PriceA = Some 5
     PriceB = None }; { Time = 2
                        PriceA = Some 6
                        PriceB = Some 5 }; { Time = 3
                                             PriceA = None
                                             PriceB = Some 4 }]
val testTime : int = 1
val time1A : StockPriceOb option = Some { Stock = "A"
                                          Time = 1
                                          Price = 5 }
val time1B : StockPriceOb option = None
val time1Aprice : int option = Some 5
val time1Bprice : int option = None
val testOutput : TwoStocksPriceOb = { Time = 1
                                      PriceA = Some 5
                                      PriceB = None }
val getTheMatch : time:int -> TwoStocksPriceOb
val tslD2 : TwoStocksPriceOb list =
  [{ Time = 1
     PriceA = Some 5
     PriceB = None }; { Time = 2
                        PriceA = Some 6
                        PriceB = Some 5 }; { Time = 3
                                             PriceA = None
                                             PriceB = Some 4 }]
```

</details>
</span>
</p>
</div>

*)

