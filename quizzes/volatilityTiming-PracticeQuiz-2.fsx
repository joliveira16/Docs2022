(**
[![Binder](../img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=quizzes/volatilityTiming-PracticeQuiz-2.ipynb)&emsp;
[![Script](../img/badge-script.svg)](/Teaching//quizzes/volatilityTiming-PracticeQuiz-2.fsx)&emsp;
[![Notebook](../img/badge-notebook.svg)](/Teaching//quizzes/volatilityTiming-PracticeQuiz-2.ipynb)

We're going to use the following in the questions

*)
#r "nuget: FSharp.Stats"

open System
open FSharp.Stats

type ReturnOb = { Symbol : string; Date : DateTime; Return : float }
type ValueOb = { Symbol : string; Date : DateTime; Value : float }

let seed = 1
Random.SetSampleGenerator(Random.RandBasic(seed))   
let normal = Distributions.Continuous.normal 0.0 0.1

let returns =
    [| 
        for symbol in ["AAPL"; "TSLA"] do
        for month in [1..2] do
        for day in [1..3] do
            { Symbol = symbol 
              Date = DateTime(2021, month, day)
              Return = normal.Sample()}
    |]
(**
## Question 1

Take this array of arrays, add `1.0` to each element of the "inner" arrays,
and then concatenate all the inner arrays together.

*)
[| [| 1.0; 2.0|]
   [| 3.0; 4.0|] |](* output: 

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>


Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float [] = [|2.0; 3.0; 4.0; 5.0|]*)
(**
or

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float [] = [|2.0; 3.0; 4.0; 5.0|]
```

</details>
</span>
</p>
</div>

## Question 2

Take the following two-parameter function:

*)
let add x y = x + y
(**
Use the above function and [partial application](https://fsharpforfunandprofit.com/posts/partial-application/)
to define a new function called
`add2` that adds 2
to it's input.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val add2 : (int -> int)
```

</details>
</span>
</p>
</div>

## Question 3

Given `returns : ReturnOb []`, use `printfn` to print the whole
array to standard output using the [structured plaintext formatter](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/plaintext-formatting).

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
[|{ Symbol = "AAPL"
    Date = 1/1/2021 12:00:00 AM
    Return = -0.02993466474 }; { Symbol = "AAPL"
                                 Date = 1/2/2021 12:00:00 AM
                                 Return = -0.01872509079 };
  { Symbol = "AAPL"
    Date = 1/3/2021 12:00:00 AM
    Return = 0.1904072042 }; { Symbol = "AAPL"
                               Date = 2/1/2021 12:00:00 AM
                               Return = -0.01626157984 };
  { Symbol = "AAPL"
    Date = 2/2/2021 12:00:00 AM
    Return = -0.0767937252 }; { Symbol = "AAPL"
                                Date = 2/3/2021 12:00:00 AM
                                Return = 0.1308654699 };
  { Symbol = "TSLA"
    Date = 1/1/2021 12:00:00 AM
    Return = -0.1487757845 }; { Symbol = "TSLA"
                                Date = 1/2/2021 12:00:00 AM
                                Return = 0.1059620976 };
  { Symbol = "TSLA"
    Date = 1/3/2021 12:00:00 AM
    Return = -0.108695795 }; { Symbol = "TSLA"
                               Date = 2/1/2021 12:00:00 AM
                               Return = 0.04571273462 };
  { Symbol = "TSLA"
    Date = 2/2/2021 12:00:00 AM
    Return = 0.099311576 }; { Symbol = "TSLA"
                              Date = 2/3/2021 12:00:00 AM
                              Return = 0.04506957545 }|]
```

</details>
</span>
</p>
</div>

## Question 4

Given the tuple `("hi", false, 20.321, 4)`,
use `printfn` and the tuple to print the following string
to standard output:
`"hi teacher, my False knowledge implies that 4%=0020.1"`

[String formatting](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/plaintext-formatting#format-specifiers-for-printf) documentation will be useful.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val xString : string = "hi"
val xInt : int = 4
val xFloat : float = 20.321
val xBool : bool = false
```

Using string interpolation

```
hi teacher, my False knowledge implies that 4%=0020.3
```

Using old-style printfn

```
hi teacher, my false knowledge implies that 4%=0020.3
```

</details>
</span>
</p>
</div>

## Question 5

Given `returns : ReturnOb []`, calculate the arithmetic average return
for every symbol each month.
Give the result as a `ReturnOb []` where the date is the last date for the symbol
each month.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.002, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.04724914955 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.01260338828 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = -0.05050316065 };
    { Symbol = "TSLA"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.06336462869 }|]
```

</details>
</span>
</p>
</div>

## Question 6

Given `returns : ReturnOb []`, calculate the monthly return
for every symbol each month.
Give the result as a `ReturnOb []` where the date is the last date for the symbol
each month.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.1331495388 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.02704464906 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = -0.1609068633 };
    { Symbol = "TSLA"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Return = 0.2013744809 }|]
```

</details>
</span>
</p>
</div>

## Question 7

Given `returns : ReturnOb []`, calculate the standard deviation of daily returns
for every symbol each month.
Give the result as a `ValueOb []` where the date in each `ValueOb` is the last date for the symbol
each month.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.002, CPU: 00:00:00.015, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1241051372 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.106796419 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.136976765 };
    { Symbol = "TSLA"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.03113263046 }|]
```

</details>
</span>
</p>
</div>

## Question 8

Given `returns : ReturnOb []`, calculate the standard deviation of daily returns
for every symbol using rolling 3 day windows.
Give the result as a `ValueOb []` where the date in each `ValueOb` is the last date for the symbol
in the window.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1241051372 };
    { Symbol = "AAPL"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1401026193 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.106796419 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.136976765 };
    { Symbol = "TSLA"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1107173508 };
    { Symbol = "TSLA"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1079983767 };
    { Symbol = "TSLA"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.03113263046 }|]
```

Breaking this answer down,
If you're unsure, it's helpful to work through things step by step.
then build up from there.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val groups : (string * ReturnOb []) [] =
  [|("AAPL",
     [|{ Symbol = "AAPL"
         Date = 1/1/2021 12:00:00 AM
         Return = -0.02993466474 }; { Symbol = "AAPL"
                                      Date = 1/2/2021 12:00:00 AM
                                      Return = -0.01872509079 };
       { Symbol = "AAPL"
         Date = 1/3/2021 12:00:00 AM
         Return = 0.1904072042 }; { Symbol = "AAPL"
                                    Date = 2/1/2021 12:00:00 AM
                                    Return = -0.01626157984 };
       { Symbol = "AAPL"
         Date = 2/2/2021 12:00:00 AM
         Return = -0.0767937252 }; { Symbol = "AAPL"
                                     Date = 2/3/2021 12:00:00 AM
                                     Return = 0.1308654699 }|]);
    ("TSLA",
     [|{ Symbol = "TSLA"
         Date = 1/1/2021 12:00:00 AM
         Return = -0.1487757845 }; { Symbol = "TSLA"
                                     Date = 1/2/2021 12:00:00 AM
                                     Return = 0.1059620976 };
       { Symbol = "TSLA"
         Date = 1/3/2021 12:00:00 AM
         Return = -0.108695795 }; { Symbol = "TSLA"
                                    Date = 2/1/2021 12:00:00 AM
                                    Return = 0.04571273462 };
       { Symbol = "TSLA"
         Date = 2/2/2021 12:00:00 AM
         Return = 0.099311576 }; { Symbol = "TSLA"
                                   Date = 2/3/2021 12:00:00 AM
                                   Return = 0.04506957545 }|])|]
```

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val firstGroup : string * ReturnOb [] =
  ("AAPL",
   [|{ Symbol = "AAPL"
       Date = 1/1/2021 12:00:00 AM
       Return = -0.02993466474 }; { Symbol = "AAPL"
                                    Date = 1/2/2021 12:00:00 AM
                                    Return = -0.01872509079 };
     { Symbol = "AAPL"
       Date = 1/3/2021 12:00:00 AM
       Return = 0.1904072042 }; { Symbol = "AAPL"
                                  Date = 2/1/2021 12:00:00 AM
                                  Return = -0.01626157984 };
     { Symbol = "AAPL"
       Date = 2/2/2021 12:00:00 AM
       Return = -0.0767937252 }; { Symbol = "AAPL"
                                   Date = 2/3/2021 12:00:00 AM
                                   Return = 0.1308654699 }|])
val firstSymbol : string = "AAPL"
val firstObs : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/1/2021 12:00:00 AM
      Return = -0.02993466474 }; { Symbol = "AAPL"
                                   Date = 1/2/2021 12:00:00 AM
                                   Return = -0.01872509079 };
    { Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Return = 0.1904072042 }; { Symbol = "AAPL"
                                 Date = 2/1/2021 12:00:00 AM
                                 Return = -0.01626157984 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM
      Return = -0.0767937252 }; { Symbol = "AAPL"
                                  Date = 2/3/2021 12:00:00 AM
                                  Return = 0.1308654699 }|]
val windowedFirstObs : ReturnOb [] [] =
  [|[|{ Symbol = "AAPL"
        Date = 1/1/2021 12:00:00 AM
        Return = -0.02993466474 }; { Symbol = "AAPL"
                                     Date = 1/2/2021 12:00:00 AM
                                     Return = -0.01872509079 };
      { Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }|];
    [|{ Symbol = "AAPL"
        Date = 1/2/2021 12:00:00 AM
        Return = -0.01872509079 }; { Symbol = "AAPL"
                                     Date = 1/3/2021 12:00:00 AM
                                     Return = 0.1904072042 };
      { Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }|];
    [|{ Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }; { Symbol = "AAPL"
                                   Date = 2/1/2021 12:00:00 AM
                                   Return = -0.01626157984 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM
        Return = -0.0767937252 }|];
    [|{ Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }; { Symbol = "AAPL"
                                     Date = 2/2/2021 12:00:00 AM
                                     Return = -0.0767937252 };
      { Symbol = "AAPL"
        Date = 2/3/2021 12:00:00 AM
        Return = 0.1308654699 }|]|]
val firstWindow : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/1/2021 12:00:00 AM
      Return = -0.02993466474 }; { Symbol = "AAPL"
                                   Date = 1/2/2021 12:00:00 AM
                                   Return = -0.01872509079 };
    { Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Return = 0.1904072042 }|]
val lastDayOfFirstWindow : ReturnOb = { Symbol = "AAPL"
                                        Date = 1/3/2021 12:00:00 AM
                                        Return = 0.1904072042 }
val firstWindowReturnStdDev : float = 0.1241051372
val firstWindowResult : ValueOb = { Symbol = "AAPL"
                                    Date = 1/3/2021 12:00:00 AM
                                    Value = 0.1241051372 }
```

Now take the inner-most code operating on a single window
and make a function by copying and pasting inside a function.
often using more general variable names

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val resultForWindow : window:ReturnOb [] -> ValueOb
```

test it on your window

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val firstWindowFunctionResult : ValueOb = { Symbol = "AAPL"
                                            Date = 1/3/2021 12:00:00 AM
                                            Value = 0.1241051372 }
```

check

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```

now a function to create the windows

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val createWindows : days:ReturnOb array -> ReturnOb [] []
```

check

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```

so now we can do

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1241051372 };
    { Symbol = "AAPL"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1401026193 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.106796419 }|]
```

Cool, now first obs was the obs from the first group.
we could do function to operate on a group.
our group is a tuple of `(string,ReturnObs array)`.
We're not going to use the `string` variable, so we'll preface it
with ** to let the compiler know we're leaving it out o purpose.
the ** is not necessary but it's good practice

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val resultsForGroup : _symbol:'a * xs:ReturnOb array -> ValueOb []
```

test it on the first group

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1241051372 };
    { Symbol = "AAPL"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1401026193 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.106796419 }|]
```

now make the group and apply my
group function to each group

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val resultsForEachGroup : ValueOb [] [] =
  [|[|{ Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Value = 0.1241051372 }; { Symbol = "AAPL"
                                  Date = 2/1/2021 12:00:00 AM
                                  Value = 0.1200377524 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM
        Value = 0.1401026193 }; { Symbol = "AAPL"
                                  Date = 2/3/2021 12:00:00 AM
                                  Value = 0.106796419 }|];
    [|{ Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM
        Value = 0.136976765 }; { Symbol = "TSLA"
                                 Date = 2/1/2021 12:00:00 AM
                                 Value = 0.1107173508 };
      { Symbol = "TSLA"
        Date = 2/2/2021 12:00:00 AM
        Value = 0.1079983767 }; { Symbol = "TSLA"
                                  Date = 2/3/2021 12:00:00 AM
                                  Value = 0.03113263046 }|]|]
```

Okay, but this is an array of `ValueOb arrays` (that's what `ValueOb [ ][ ]` means).
What happened is that I had an array of groups, and then I transformed each group.
so it's still one result per group. For instance

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1241051372 };
    { Symbol = "AAPL"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1401026193 };
    { Symbol = "AAPL"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.106796419 }|]
```

is the first group of results

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb [] =
  [|{ Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Sunday;
                                   DayOfYear = 3;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 1;
                                   Second = 0;
                                   Ticks = 637452288000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.136976765 };
    { Symbol = "TSLA"
      Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                   Day = 1;
                                   DayOfWeek = Monday;
                                   DayOfYear = 32;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637477344000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1107173508 };
    { Symbol = "TSLA"
      Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                   Day = 2;
                                   DayOfWeek = Tuesday;
                                   DayOfYear = 33;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637478208000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.1079983767 };
    { Symbol = "TSLA"
      Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                   Day = 3;
                                   DayOfWeek = Wednesday;
                                   DayOfYear = 34;
                                   Hour = 0;
                                   Kind = Unspecified;
                                   Millisecond = 0;
                                   Minute = 0;
                                   Month = 2;
                                   Second = 0;
                                   Ticks = 637479072000000000L;
                                   TimeOfDay = 00:00:00;
                                   Year = 2021;}
      Value = 0.03113263046 }|]
```

is the second group. I don't want an array of arrays.
I just want one array of value obs. So `concat` them.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val resultsForEachGroupConcatenated : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.1241051372 }; { Symbol = "AAPL"
                                Date = 2/1/2021 12:00:00 AM
                                Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1401026193 }; { Symbol = "AAPL"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.106796419 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.136976765 }; { Symbol = "TSLA"
                               Date = 2/1/2021 12:00:00 AM
                               Value = 0.1107173508 };
    { Symbol = "TSLA"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1079983767 }; { Symbol = "TSLA"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.03113263046 }|]
```

what's the first thing in the array?

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ValueOb = { Symbol = "AAPL"
                     Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                                  Day = 3;
                                                  DayOfWeek = Sunday;
                                                  DayOfYear = 3;
                                                  Hour = 0;
                                                  Kind = Unspecified;
                                                  Millisecond = 0;
                                                  Minute = 0;
                                                  Month = 1;
                                                  Second = 0;
                                                  Ticks = 637452288000000000L;
                                                  TimeOfDay = 00:00:00;
                                                  Year = 2021;}
                     Value = 0.1241051372 }
```

`Collect` does the `map` and `concat` in one step.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val resultsForEachGroupCollected : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.1241051372 }; { Symbol = "AAPL"
                                Date = 2/1/2021 12:00:00 AM
                                Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1401026193 }; { Symbol = "AAPL"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.106796419 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.136976765 }; { Symbol = "TSLA"
                               Date = 2/1/2021 12:00:00 AM
                               Value = 0.1107173508 };
    { Symbol = "TSLA"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1079983767 }; { Symbol = "TSLA"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.03113263046 }|]
```

check, this should evaluate to `true`

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```

why did I write the answer using an anonymous function instead of functions like this?
I use reusable functions for something I'm going to use multiple times.
If it's something I'll do once, and it's not too many lines, then I use
the anonymous lambda function. As you get more experience, you can code using
the type signatures to tell you what everything is. And I don't actually
have to running it step by step.
however, starting out especially, I think you'll find it helpful
to kinda break things down like I did here.
Another way you can do it, similar to the first answer using
an anonymous lambda function, but now we'll do it with fewer
nested arrays by concatenating/collecting the windows
into the parent array before doing the standard deviations.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val m2Groups : (string * ReturnOb []) [] =
  [|("AAPL",
     [|{ Symbol = "AAPL"
         Date = 1/1/2021 12:00:00 AM
         Return = -0.02993466474 }; { Symbol = "AAPL"
                                      Date = 1/2/2021 12:00:00 AM
                                      Return = -0.01872509079 };
       { Symbol = "AAPL"
         Date = 1/3/2021 12:00:00 AM
         Return = 0.1904072042 }; { Symbol = "AAPL"
                                    Date = 2/1/2021 12:00:00 AM
                                    Return = -0.01626157984 };
       { Symbol = "AAPL"
         Date = 2/2/2021 12:00:00 AM
         Return = -0.0767937252 }; { Symbol = "AAPL"
                                     Date = 2/3/2021 12:00:00 AM
                                     Return = 0.1308654699 }|]);
    ("TSLA",
     [|{ Symbol = "TSLA"
         Date = 1/1/2021 12:00:00 AM
         Return = -0.1487757845 }; { Symbol = "TSLA"
                                     Date = 1/2/2021 12:00:00 AM
                                     Return = 0.1059620976 };
       { Symbol = "TSLA"
         Date = 1/3/2021 12:00:00 AM
         Return = -0.108695795 }; { Symbol = "TSLA"
                                    Date = 2/1/2021 12:00:00 AM
                                    Return = 0.04571273462 };
       { Symbol = "TSLA"
         Date = 2/2/2021 12:00:00 AM
         Return = 0.099311576 }; { Symbol = "TSLA"
                                   Date = 2/3/2021 12:00:00 AM
                                   Return = 0.04506957545 }|])|]
val m2GroupsOfWindows : ReturnOb [] [] [] =
  [|[|[|{ Symbol = "AAPL"
          Date = 1/1/2021 12:00:00 AM
          Return = -0.02993466474 }; { Symbol = "AAPL"
                                       Date = 1/2/2021 12:00:00 AM
                                       Return = -0.01872509079 };
        { Symbol = "AAPL"
          Date = 1/3/2021 12:00:00 AM
          Return = 0.1904072042 }|];
      [|{ Symbol = "AAPL"
          Date = 1/2/2021 12:00:00 AM
          Return = -0.01872509079 }; { Symbol = "AAPL"
                                       Date = 1/3/2021 12:00:00 AM
                                       Return = 0.1904072042 };
        { Symbol = "AAPL"
          Date = 2/1/2021 12:00:00 AM
          Return = -0.01626157984 }|];
      [|{ Symbol = "AAPL"
          Date = 1/3/2021 12:00:00 AM
          Return = 0.1904072042 }; { Symbol = "AAPL"
                                     Date = 2/1/2021 12:00:00 AM
                                     Return = -0.01626157984 };
        { Symbol = "AAPL"
          Date = 2/2/2021 12:00:00 AM
          Return = -0.0767937252 }|];
      [|{ Symbol = "AAPL"
          Date = 2/1/2021 12:00:00 AM
          Return = -0.01626157984 }; { Symbol = "AAPL"
                                       Date = 2/2/2021 12:00:00 AM
                                       Return = -0.0767937252 };
        { Symbol = "AAPL"
          Date = 2/3/2021 12:00:00 AM
          Return = 0.1308654699 }|]|];
    [|[|{ Symbol = "TSLA"
          Date = 1/1/2021 12:00:00 AM
          Return = -0.1487757845 }; { Symbol = "TSLA"
                                      Date = 1/2/2021 12:00:00 AM
                                      Return = 0.1059620976 };
        { Symbol = "TSLA"
          Date = 1/3/2021 12:00:00 AM
          Return = -0.108695795 }|];
      [|{ Symbol = "TSLA"
          Date = 1/2/2021 12:00:00 AM
          Return = 0.1059620976 }; { Symbol = "TSLA"
                                     Date = 1/3/2021 12:00:00 AM
                                     Return = -0.108695795 };
        { Symbol = "TSLA"
          Date = 2/1/2021 12:00:00 AM
          Return = 0.04571273462 }|];
      [|{ Symbol = "TSLA"
          Date = 1/3/2021 12:00:00 AM
          Return = -0.108695795 }; { Symbol = "TSLA"
                                     Date = 2/1/2021 12:00:00 AM
                                     Return = 0.04571273462 };
        { Symbol = "TSLA"
          Date = 2/2/2021 12:00:00 AM
          Return = 0.099311576 }|];
      [|{ Symbol = "TSLA"
          Date = 2/1/2021 12:00:00 AM
          Return = 0.04571273462 }; { Symbol = "TSLA"
                                      Date = 2/2/2021 12:00:00 AM
                                      Return = 0.099311576 };
        { Symbol = "TSLA"
          Date = 2/3/2021 12:00:00 AM
          Return = 0.04506957545 }|]|]|]
```

first group of windows

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ReturnOb [] [] =
  [|[|{ Symbol = "AAPL"
        Date = 1/1/2021 12:00:00 AM {Date = 1/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Friday;
                                     DayOfYear = 1;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637450560000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.02993466474 };
      { Symbol = "AAPL"
        Date = 1/2/2021 12:00:00 AM {Date = 1/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Saturday;
                                     DayOfYear = 2;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637451424000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.01872509079 };
      { Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1904072042 }|];
    [|{ Symbol = "AAPL"
        Date = 1/2/2021 12:00:00 AM {Date = 1/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Saturday;
                                     DayOfYear = 2;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637451424000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.01872509079 };
      { Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1904072042 };
      { Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.01626157984 }|];
    [|{ Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1904072042 };
      { Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.01626157984 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Tuesday;
                                     DayOfYear = 33;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637478208000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.0767937252 }|];
    [|{ Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.01626157984 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Tuesday;
                                     DayOfYear = 33;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637478208000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.0767937252 };
      { Symbol = "AAPL"
        Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Wednesday;
                                     DayOfYear = 34;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637479072000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1308654699 }|]|]
```

second group of windows

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : ReturnOb [] [] =
  [|[|{ Symbol = "TSLA"
        Date = 1/1/2021 12:00:00 AM {Date = 1/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Friday;
                                     DayOfYear = 1;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637450560000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.1487757845 };
      { Symbol = "TSLA"
        Date = 1/2/2021 12:00:00 AM {Date = 1/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Saturday;
                                     DayOfYear = 2;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637451424000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1059620976 };
      { Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.108695795 }|];
    [|{ Symbol = "TSLA"
        Date = 1/2/2021 12:00:00 AM {Date = 1/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Saturday;
                                     DayOfYear = 2;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637451424000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.1059620976 };
      { Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.108695795 };
      { Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.04571273462 }|];
    [|{ Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM {Date = 1/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Sunday;
                                     DayOfYear = 3;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 1;
                                     Second = 0;
                                     Ticks = 637452288000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = -0.108695795 };
      { Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.04571273462 };
      { Symbol = "TSLA"
        Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Tuesday;
                                     DayOfYear = 33;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637478208000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.099311576 }|];
    [|{ Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM {Date = 2/1/2021 12:00:00 AM;
                                     Day = 1;
                                     DayOfWeek = Monday;
                                     DayOfYear = 32;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637477344000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.04571273462 };
      { Symbol = "TSLA"
        Date = 2/2/2021 12:00:00 AM {Date = 2/2/2021 12:00:00 AM;
                                     Day = 2;
                                     DayOfWeek = Tuesday;
                                     DayOfYear = 33;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637478208000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.099311576 };
      { Symbol = "TSLA"
        Date = 2/3/2021 12:00:00 AM {Date = 2/3/2021 12:00:00 AM;
                                     Day = 3;
                                     DayOfWeek = Wednesday;
                                     DayOfYear = 34;
                                     Hour = 0;
                                     Kind = Unspecified;
                                     Millisecond = 0;
                                     Minute = 0;
                                     Month = 2;
                                     Second = 0;
                                     Ticks = 637479072000000000L;
                                     TimeOfDay = 00:00:00;
                                     Year = 2021;}
        Return = 0.04506957545 }|]|]
```

Now concatenate the windows.

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val m2GroupsOfWindowsConcatenated : ReturnOb [] [] =
  [|[|{ Symbol = "AAPL"
        Date = 1/1/2021 12:00:00 AM
        Return = -0.02993466474 }; { Symbol = "AAPL"
                                     Date = 1/2/2021 12:00:00 AM
                                     Return = -0.01872509079 };
      { Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }|];
    [|{ Symbol = "AAPL"
        Date = 1/2/2021 12:00:00 AM
        Return = -0.01872509079 }; { Symbol = "AAPL"
                                     Date = 1/3/2021 12:00:00 AM
                                     Return = 0.1904072042 };
      { Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }|];
    [|{ Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }; { Symbol = "AAPL"
                                   Date = 2/1/2021 12:00:00 AM
                                   Return = -0.01626157984 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM
        Return = -0.0767937252 }|];
    [|{ Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }; { Symbol = "AAPL"
                                     Date = 2/2/2021 12:00:00 AM
                                     Return = -0.0767937252 };
      { Symbol = "AAPL"
        Date = 2/3/2021 12:00:00 AM
        Return = 0.1308654699 }|];
    [|{ Symbol = "TSLA"
        Date = 1/1/2021 12:00:00 AM
        Return = -0.1487757845 }; { Symbol = "TSLA"
                                    Date = 1/2/2021 12:00:00 AM
                                    Return = 0.1059620976 };
      { Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM
        Return = -0.108695795 }|];
    [|{ Symbol = "TSLA"
        Date = 1/2/2021 12:00:00 AM
        Return = 0.1059620976 }; { Symbol = "TSLA"
                                   Date = 1/3/2021 12:00:00 AM
                                   Return = -0.108695795 };
      { Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM
        Return = 0.04571273462 }|];
    [|{ Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM
        Return = -0.108695795 }; { Symbol = "TSLA"
                                   Date = 2/1/2021 12:00:00 AM
                                   Return = 0.04571273462 };
      { Symbol = "TSLA"
        Date = 2/2/2021 12:00:00 AM
        Return = 0.099311576 }|];
    [|{ Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM
        Return = 0.04571273462 }; { Symbol = "TSLA"
                                    Date = 2/2/2021 12:00:00 AM
                                    Return = 0.099311576 };
      { Symbol = "TSLA"
        Date = 2/3/2021 12:00:00 AM
        Return = 0.04506957545 }|]|]
```

same as if I'd used collect instead of map and then concat

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val m2GroupsOfWindowsCollected : ReturnOb [] [] =
  [|[|{ Symbol = "AAPL"
        Date = 1/1/2021 12:00:00 AM
        Return = -0.02993466474 }; { Symbol = "AAPL"
                                     Date = 1/2/2021 12:00:00 AM
                                     Return = -0.01872509079 };
      { Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }|];
    [|{ Symbol = "AAPL"
        Date = 1/2/2021 12:00:00 AM
        Return = -0.01872509079 }; { Symbol = "AAPL"
                                     Date = 1/3/2021 12:00:00 AM
                                     Return = 0.1904072042 };
      { Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }|];
    [|{ Symbol = "AAPL"
        Date = 1/3/2021 12:00:00 AM
        Return = 0.1904072042 }; { Symbol = "AAPL"
                                   Date = 2/1/2021 12:00:00 AM
                                   Return = -0.01626157984 };
      { Symbol = "AAPL"
        Date = 2/2/2021 12:00:00 AM
        Return = -0.0767937252 }|];
    [|{ Symbol = "AAPL"
        Date = 2/1/2021 12:00:00 AM
        Return = -0.01626157984 }; { Symbol = "AAPL"
                                     Date = 2/2/2021 12:00:00 AM
                                     Return = -0.0767937252 };
      { Symbol = "AAPL"
        Date = 2/3/2021 12:00:00 AM
        Return = 0.1308654699 }|];
    [|{ Symbol = "TSLA"
        Date = 1/1/2021 12:00:00 AM
        Return = -0.1487757845 }; { Symbol = "TSLA"
                                    Date = 1/2/2021 12:00:00 AM
                                    Return = 0.1059620976 };
      { Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM
        Return = -0.108695795 }|];
    [|{ Symbol = "TSLA"
        Date = 1/2/2021 12:00:00 AM
        Return = 0.1059620976 }; { Symbol = "TSLA"
                                   Date = 1/3/2021 12:00:00 AM
                                   Return = -0.108695795 };
      { Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM
        Return = 0.04571273462 }|];
    [|{ Symbol = "TSLA"
        Date = 1/3/2021 12:00:00 AM
        Return = -0.108695795 }; { Symbol = "TSLA"
                                   Date = 2/1/2021 12:00:00 AM
                                   Return = 0.04571273462 };
      { Symbol = "TSLA"
        Date = 2/2/2021 12:00:00 AM
        Return = 0.099311576 }|];
    [|{ Symbol = "TSLA"
        Date = 2/1/2021 12:00:00 AM
        Return = 0.04571273462 }; { Symbol = "TSLA"
                                    Date = 2/2/2021 12:00:00 AM
                                    Return = 0.099311576 };
      { Symbol = "TSLA"
        Date = 2/3/2021 12:00:00 AM
        Return = 0.04506957545 }|]|]
```

compare them

```
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val m2FirstConcatenated : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/1/2021 12:00:00 AM
      Return = -0.02993466474 }; { Symbol = "AAPL"
                                   Date = 1/2/2021 12:00:00 AM
                                   Return = -0.01872509079 };
    { Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Return = 0.1904072042 }|]
val m2FirstCollected : ReturnOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/1/2021 12:00:00 AM
      Return = -0.02993466474 }; { Symbol = "AAPL"
                                   Date = 1/2/2021 12:00:00 AM
                                   Return = -0.01872509079 };
    { Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Return = 0.1904072042 }|]
val it : bool = true
```

If they're not true, make sure they're sorted the same before you take the first obs.

Now, standard deviations of the windows' returns

```
Real: 00:00:00.001, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val m2Result : ValueOb [] =
  [|{ Symbol = "AAPL"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.1241051372 }; { Symbol = "AAPL"
                                Date = 2/1/2021 12:00:00 AM
                                Value = 0.1200377524 };
    { Symbol = "AAPL"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1401026193 }; { Symbol = "AAPL"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.106796419 };
    { Symbol = "TSLA"
      Date = 1/3/2021 12:00:00 AM
      Value = 0.136976765 }; { Symbol = "TSLA"
                               Date = 2/1/2021 12:00:00 AM
                               Value = 0.1107173508 };
    { Symbol = "TSLA"
      Date = 2/2/2021 12:00:00 AM
      Value = 0.1079983767 }; { Symbol = "TSLA"
                                Date = 2/3/2021 12:00:00 AM
                                Value = 0.03113263046 }|]
```

</details>
</span>
</p>
</div>

*)

