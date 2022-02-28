(**
[![Binder](img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=fundamentals.ipynb)&emsp;
[![Script](img/badge-script.svg)](/Teaching//fundamentals.fsx)&emsp;
[![Notebook](img/badge-notebook.svg)](/Teaching//fundamentals.ipynb)

# Fundamentals

A good place to start is to define a one-period return calculation.

Objectives:

* [Interactive programming](#Interactive-programming)

* [How to calculate returns](#Calculating-returns).

* [Working with data.](#Working-with-data)

* [How to calculate return volatility](#Volatility)

## Interactive programming

We are going to focus on interactive programming. This is the most productive (and most common) type of analytic programming. In constrast to compiled programs (e.g, C, C++, Fortran, Java), interactive programs:

* Allow rapid iterative development.

* You can quickly quickly write and rewrite sections of code, evaluating the output, without having to rerun the entire program.

* This is especially useful for financial analysis, because we often evaluate large datasets that take a long time to process.

Interactive programming typically involves a [REPL](https://en.wikipedia.org/wiki/Read%E2%80%93eval%E2%80%93print_loop) (Read, Evaluate, Print, Loop). It is common for scripting langauges such as R, Python, Julia, Ruby, and Perl.

### The terminal

The most basic way that you can run interactive code is at the command line using an interpreter. We can start the F# interactive interpreter by opening a terminal (e.g., terminal.app, cmd, powershell) and running `dotnet fsi`.

Once fsi is open, we can type a code snippet in the prompt followed by ";;" to terminate it and it will run.

![fsi](img/fsi.png)

It is fine to run code this way, but we can do better using an IDE (Integrated development environment) that incorportes syntax highlighting, intellisense tooltips, and execution. We will use two common IDE's: Visual Studio Code with the Ionide extension and Jupyter Notebooks.

## Calculating returns

### Basic calculations in fsi

Let's assume that you have $120.00 today and that you had $100.00 a year ago. Your annual return is then:

*)
(120.0 / 100.0) - 1.0(* output: 
val it : float = 0.2*)
(**
### Basic numerical types: float, int, and decimal

Notice that I included a decimal point "." in the numbers. The decimal point makes it a [floating point](https://en.wikipedia.org/wiki/Floating-point_arithmetic) number. Floating point numbers (floats) are the most commonly used numerical type for mathematical calculations.

If we left the decimal point off and wrote "120" without the ".0" at the end it would be an integer and we would get the wrong answer because integers cannot represent fractions.

*)
(120/100) - 1(* output: 
val it : int = 0*)
(**
The other main numerical data type is [decimal](https://en.wikipedia.org/wiki/Decimal_data_type).

*)
(120m/100m) - 1m(* output: 
val it : decimal = 0.2M*)
(**
Decimals are used when you need an exact fractional amount. Floats are insufficient in these circumstances because **"... such representations typically restrict the denominator to a power of two ... 0.3 (3/10) might be represented as 5404319552844595/18014398509481984 (0.299999999999999988897769...)"** ([see wiki](https://en.wikipedia.org/wiki/Decimal_data_type)).

### Static type checking

Finally, since F# is staticly typed, we must do arithmetic using numbers that are all the same type. If we mix floats and integers we will get an error:

```fsharp
(120.0 / 100) - 1.0
```
```output
fundamentals.fsx(18,10): error FS0001: The type 'int' does not match the type 'float'
```
Static tying can slow you down a bit writing simple small programs, but as programs get larger and more complex the benefits become apparent. Specifically, static typing as implemented by F#:

* helps you ensure that the code is correct (i.e., the type of the input data matches what the function expects). In the words of Yaron Minksy at Janestreet, you can ["make illegal states unrepresentable"](https://blog.janestreet.com/effective-ml-revisited/) (see [here](https://fsharpforfunandprofit.com/posts/designing-with-types-making-illegal-states-unrepresentable/) for F# examples).

* it also facilitates editor tooling that can check your code without running it and give tooltip errors (you should have seen a tooltip error in your editor if you type `(120.0 / 100) - 1.0` in your program file). It's like clippy on steriods (you are too young, but your parents might get this reference).

![clippy](img/intellisense-example.png)

[Image source](https://blog.codinghorror.com/it-looks-like-youre-writing-a-for-loop/)

### Assigning values

We could also do the same calculations by assigning $120.00 and $100.0 to named values.

*)
let yearAgo = 100.0
let today = 120.0
(today / yearAgo) - 1.0(* output: 
val yearAgo : float = 100.0
val today : float = 120.0
val it : float = 0.2*)
(**
This works for one-off calculations, but if we want to do this more than once, then it makes more sense to define a function to do this calculation.

### Defining functions

Functions map (or transform) inputs into outputs.

Here is a simple function named `addOne`.
It takes an input x and then it adds 1 to whatever x is.

*)
let addOne x = x + 1

addOne 0 // here x is 1
addOne 1 // here x is 2
addOne 2 // here x is 3

// We can also chain them
addOne (addOne (addOne 0)) // = (1 + (1 + (1 + 0)))
(**
We can define a function to calcuate a return.

*)
let calcReturn pv fv = (fv / pv) - 1.0
(**
The type signature tells us that `calcReturn` is a function with two float inputs (pv and fv) and it maps those two inputs into a float output. The program was able to infer that `pv` and `fv` are floats because of the `1.0` float in the calculation.

We can execute it on simple floats:

*)
// here pv = 100., fv = 110.0
calcReturn 100.0 110.0(* output: 
val it : float = 0.1*)
// here pv = 80.0, fv = 60.0
calcReturn 80.0 60.0(* output: 
val it : float = -0.25*)
(**
Or we can execute it on our previously defined `yearAgo` and `today` values:

*)
calcReturn yearAgo today(* output: 
val it : float = 0.2*)
(**
### Handling dividends

Our prior return calculation did not handle cash distributions such as dividends. We can incorporate dividends with a small modificaton:

*)
let simpleReturn beginningPrice endingPrice dividend =
    // This is solving for `r` in FV = PV*(1+r)^t where t=1.
    (endingPrice + dividend) / beginningPrice - 1.0
(**
The examples thus far have used simple (per period) compounding. We can also calculate continuously compounded returns, also known as log returns.

*)
let logReturn beginningPrice endingPrice dividend =
    // This is solving for `r` in FV = PV*e^(rt) where t=1.
    log(endingPrice + dividend) - log(beginningPrice)
(**
These two calculations give slightly different returns.

*)
simpleReturn 100.0 110.0 0.0 (* output: 
val it : float = 0.1*)
logReturn 100.0 110.0 0.0(* output: 
val it : float = 0.0953101798*)
(**
It is typically not important which version of return you use so long as you are consistent and keep track of what type of return it is when you're compounding things.

**Practice:** Can you write a function to compound an initial investment of $100.00 at 6% for 5 years? You can calculate power and exponents using:

*)
2.0**3.0(* output: 
val it : float = 8.0*)
log 2.0(* output: 
val it : float = 0.6931471806*)
exp 0.6931(* output: 
val it : float = 1.999905641*)
exp(log(2.0))(* output: 
val it : float = 2.0*)
(**
### Tuples

Looking at our return functions, we're starting to get several values that we're passing into the functions individaully. It can be useful to group these values together to make it easy to pass them around. Tuples are a simple way to group values.

Further information about tuples can be found in the [F# Language reference](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/tuples) and the [F# for Fun and Profit](https://fsharpforfunandprofit.com/posts/tuples/) websites.

*)
(1,2)(* output: 
val it : int * int = (1, 2)*)
(1,2,3)(* output: 
val it : int * int * int = (1, 2, 3)*)
(**
Tubles can contain mixed types.

*)
(1,"2")(* output: 
val it : int * string = (1, "2")*)
(**
We can also deconstruct tuples. We can use built-in convenience functions for pairs.

*)
fst (1,2)(* output: 
val it : int = 1*)
snd (1,2)(* output: 
val it : int = 2*)
(**
We can also deconstruct tuples using pattern matching.

*)
let (a, b) = (1, 2)(* output: 
val b : int = 2
val a : int = 1*)
let (c, d, e) = (1, "2", 3.0)(* output: 
val e : float = 3.0
val d : string = "2"
val c : int = 1*)
(**
Now redefining our simple return function to take a single tuple as the input parameter.

*)
let simpleReturnTuple (beginningPrice, endingPrice, dividend) =
    // This is solving for `r` in FV = PV*(1+r)^t where t=1.
    (endingPrice + dividend) / beginningPrice - 1.0

simpleReturnTuple (100.0, 110.0, 0.0)(* output: 
val simpleReturnTuple :
  beginningPrice:float * endingPrice:float * dividend:float -> float
val it : float = 0.1*)
let xx = (100.0, 110.0, 0.0)
simpleReturnTuple xx(* output: 
val xx : float * float * float = (100.0, 110.0, 0.0)
val it : float = 0.1*)
(**
### Records

If we want more structure than a tuple, then we can define a record.

For more information on records see the relevant sections of the [F# language reference](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records) or [F# for Fun and Profit](https://fsharpforfunandprofit.com/posts/records/) websites.

You must first define the record type before you use it:

*)
type RecordExample = 
    { BeginningPrice : float 
      EndingPrice : float 
      Dividend : float }      
(**
And construct a value with that record type.

*)
let x = { BeginningPrice = 100.0; EndingPrice = 110.0; Dividend = 0.0}(* output: 
val x : RecordExample = { BeginningPrice = 100.0
                          EndingPrice = 110.0
                          Dividend = 0.0 }*)
(**
Similar to tuples, we can deconstruct our record value `x` using pattern matching.

*)
let { BeginningPrice = aa; EndingPrice = bb; Dividend = cc} = x(* output: 
input.fsx (1,5)-(1,60) typecheck warning The field labels and expected type of this record expression or pattern do not uniquely determine a corresponding record type
val cc : float = 0.0
val bb : float = 110.0
val aa : float = 100.0*)
(**
We can also access individual fields by name.

*)
x.EndingPrice / x.BeginningPrice(* output: 
val it : float = 1.1*)
(**
We can define a return function that operates on the `RecordExample` type explicitly:

*)
let simpleReturnRecord1 { BeginningPrice = beginningPrice; EndingPrice = endingPrice; Dividend = dividend} =
    // This is solving for `r` in FV = PV*(1+r)^t where t=1.
    (endingPrice + dividend) / beginningPrice - 1.0
(**
Or we can let the compiler's type inference figure out the input type.

*)
let simpleReturnRecord2 x =
    // This is solving for `r` in FV = PV*(1+r)^t where t=1.
    (x.EndingPrice + x.Dividend) / x.BeginningPrice - 1.0
(**
Or we can provide a type hint to tell the compiler the type of the input.

*)
let simpleReturnRecord3 (x : RecordExample) =
    // This is solving for `r` in FV = PV*(1+r)^t where t=1.
    (x.EndingPrice + x.Dividend) / x.BeginningPrice - 1.0
(**
All 3 can be used interchangably, but when you have many similar types a type hint may be necessary to make the particular type that you want explicit.

*)
simpleReturnRecord1 x(* output: 
val it : float = 0.1*)
simpleReturnRecord2 x(* output: 
val it : float = 0.1*)
simpleReturnRecord3 x(* output: 
val it : float = 0.1*)
(**
### Pipelines and lambda expressions

This download code used pipelining and lambda functions, which are two important language features. Pipelines are created using the pipe operator (`|>`) and allow you to pipe the output of one function to the input of another. Lambda expressions allow you to create functions on the fly.

*)
1.0 |> fun x -> x + 1.0 |> fun x -> x ** 2.0(* output: 
val it : float = 4.0*)
(**
### Collections: Arrays, Lists, Sequences

A simple int array.

*)
let ar = [| 0 .. 10 |] 
ar |> Array.take 5(* output: 
val it : int [] = [|0; 1; 2; 3; 4|]*)
(**
When we look at the type signature of the elements in the array `val ar : int []`, it tells us that we have a integer array, meaning an array in which each element of the array is an integer. Arrays are "zero indexed", meaning the 0th item is the first in the array. We can access the elements individually or use a range to access multiple together.

*)
ar.[0] // or ar[0] in F# 6(* output: 
val it : int = 0*)
ar.[0 .. 2] // or ar[0 .. 2] in F# 6(* output: 
val it : int [] = [|0; 1; 2|]*)
(**
A simple float array.

*)
let arr = [| 1.0 .. 10.0 |]
arr.[0]
arr.[0 .. 5](* output: 
val arr : float [] = [|1.0; 2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0|]
val it : float [] = [|1.0; 2.0; 3.0; 4.0; 5.0; 6.0|]*)
(**
Lists and sequences are similar.

*)
// List
[ 1.0 .. 10.0 ]
// Sequence
seq { 1.0 .. 10.0 }
(**
Arrays, lists, and sequences have different properties that can make one data structure preferable to the others in a given setting. We'll discuss these different properties in due time, but for an overview you can see the F# collection language reference [here](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/fsharp-collection-types). Sequences are the most different as they are "lazy", meaning "Sequences are particularly useful when you have a large, ordered collection of data but don't necessarily expect to use all the elements. Individual sequence elements are computed only as required, so a sequence can perform better than a list if not all the elements are used" (see F# language reference).

These collections have several built-in functions for operating on them such as map, filter, groupBy, etc.

*)
arr
|> Array.map(fun x -> x + 1.0)(* output: 
val it : float [] = [|2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0; 11.0|]*)
arr
|> Array.filter(fun x -> x < 5.0)(* output: 
val it : float [] = [|1.0; 2.0; 3.0; 4.0|]*)
arr
|> Array.groupBy(fun x -> x < 5.0)
|> Array.map(fun (group, xs) -> Array.min xs, Array.max xs)(* output: 
val it : (float * float) [] = [|(1.0, 4.0); (5.0, 10.0)|]*)
(**
## Working with data

With this foundation, let's now try loading some data. We are going to obtain and process the data using an external F# library called [FSharp.Data](https://github.com/fsprojects/FSharp.Data) that makes the processing easier.

### Namespaces

First, let's create a file directory to hold data. We are going to use built-in dotnet IO (input-output) libraries to do so.

*)
// Set working directory to the this code file's directory
System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)
// Now create cache directory one level above the working directory
System.IO.File.WriteAllLines("test.txt",["first";"second"]) 
(**
This illustrates the library namespace hierarchy. If we want to access the function within the hierarchy without typing the full namespace repetitively, we can open it. The following code is equivalent.

```
open System.IO
Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)
File.WriteAllLines("test.txt",["first";"second"]) 
```

It is common to open the System namespace

*)
open System
(**
### API keys

We are going to request the data from the provider [tiingo](https://api.tiingo.com/). Make sure that you are signed up and have your [API token](https://api.tiingo.com/documentation/general/connecting). An [API](https://en.wikipedia.org/wiki/API) (application programming interface) allows you to write code to communicate with another program. In this case we are goig to write code that requests stock price data from tiingo's web servers.

Once you have your api key, create a file called `secrets.fsx` and save it at the root/top level of your project folder. In `secrets.fsx`, assign your key to a value named `tiingoKey`. If you are using git, make sure to add `secrets.fsx` to your `.gitignore` file.

```fsharp
let tiingoKey = "yourSuperSecretApiKey"
```

We can load this in our interactive session as follows, assuming that `secrets.fsx` is located one folder above the current one in the file system.

    #load "secrets.fsx"

and we can access the value by typing

    Secrets.tiingoKey

### FSharp.Data Csv Type Provider

We're now going to process our downloaded data using the **FSharp.Data** [Csv Type Provider](http://fsprojects.github.io/FSharp.Data/library/CsvProvider.html). This is code that automatically defines the types of input data based on a sample. We have already reference the nuget packaged and opened the namespace, so we can just use it now.

*)
#load "Common.fsx"
open Common


let aapl = 
    "AAPL"
    |> Tiingo.request
    |> Tiingo.get  

aapl
|> Array.take 5(* output: 
[|{ Date = 3/1/2021 12:00:00 AM
    Close = 127.79M
    High = 127.93M
    Low = 122.79M
    Open = 123.75M
    Volume = 116307892
    AdjClose = 127.03559237302M
    AdjHigh = 127.174765883719M
    AdjLow = 122.065109848056M
    AdjOpen = 123.019442492849M
    AdjVolume = 116307892M
    DivCash = 0.0M
    SplitFactor = 1.0M }; { Date = 3/2/2021 12:00:00 AM
                            Close = 125.12M
                            High = 128.72M
                            Low = 125.01M
                            Open = 128.41M
                            Volume = 102260945
                            AdjClose = 124.381354704689M
                            AdjHigh = 127.960102122663M
                            AdjLow = 124.27200408914M
                            AdjOpen = 127.651932206116M
                            AdjVolume = 102260945M
                            DivCash = 0.0M
                            SplitFactor = 1.0M }; { Date = 3/3/2021 12:00:00 AM
                                                    Close = 122.06M
                                                    High = 125.71M
                                                    Low = 121.84M
                                                    Open = 124.81M
                                                    Volume = 112966340
                                                    AdjClose = 121.339419399412M
                                                    AdjHigh = 124.967871642635M
                                                    AdjLow = 121.120718168313M
                                                    AdjOpen = 124.073184788142M
                                                    AdjVolume = 112966340M
                                                    DivCash = 0.0M
                                                    SplitFactor = 1.0M };
  { Date = 3/4/2021 12:00:00 AM
    Close = 120.13M
    High = 123.6M
    Low = 118.62M
    Open = 121.75M
    Volume = 178154975
    AdjClose = 119.420813144776M
    AdjHigh = 122.870328017101M
    AdjLow = 117.919727422237M
    AdjOpen = 121.031249482864M
    AdjVolume = 178154975M
    DivCash = 0.0M
    SplitFactor = 1.0M }; { Date = 3/5/2021 12:00:00 AM
                            Close = 121.42M
                            High = 121.935M
                            Low = 117.57M
                            Open = 120.98M
                            Volume = 153766601
                            AdjClose = 120.703197636216M
                            AdjHigh = 121.215157336288M
                            AdjLow = 116.875926091994M
                            AdjOpen = 120.26579517402M
                            AdjVolume = 153766601M
                            DivCash = 0.0M
                            SplitFactor = 1.0M }|]*)
(**
### Plotting

Now let's plot the stock price using [Plotly.NET](https://plotly.github.io/Plotly.NET/).

*)
#r "nuget: Plotly.NET, 2.0.0-preview.17"
#r "nuget: Plotly.NET.Interactive, 2.0.0-preview.17"
open Plotly.NET


let sampleChart =
    aapl
    |> Seq.map(fun x -> x.Date, x.AdjClose)
    |> Chart.Line
sampleChart |> Chart.show   (* output: 
<div id="2c1b2b4c-43eb-465e-b895-bcceeb3612e7"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_2c1b2b4c43eb465eb895bcceeb3612e7 = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.6.3.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"scatter","mode":"lines","x":["2021-03-01T00:00:00+00:00","2021-03-02T00:00:00+00:00","2021-03-03T00:00:00+00:00","2021-03-04T00:00:00+00:00","2021-03-05T00:00:00+00:00","2021-03-08T00:00:00+00:00","2021-03-09T00:00:00+00:00","2021-03-10T00:00:00+00:00","2021-03-11T00:00:00+00:00","2021-03-12T00:00:00+00:00","2021-03-15T00:00:00+00:00","2021-03-16T00:00:00+00:00","2021-03-17T00:00:00+00:00","2021-03-18T00:00:00+00:00","2021-03-19T00:00:00+00:00","2021-03-22T00:00:00+00:00","2021-03-23T00:00:00+00:00","2021-03-24T00:00:00+00:00","2021-03-25T00:00:00+00:00","2021-03-26T00:00:00+00:00","2021-03-29T00:00:00+00:00","2021-03-30T00:00:00+00:00","2021-03-31T00:00:00+00:00","2021-04-01T00:00:00+00:00","2021-04-05T00:00:00+00:00","2021-04-06T00:00:00+00:00","2021-04-07T00:00:00+00:00","2021-04-08T00:00:00+00:00","2021-04-09T00:00:00+00:00","2021-04-12T00:00:00+00:00","2021-04-13T00:00:00+00:00","2021-04-14T00:00:00+00:00","2021-04-15T00:00:00+00:00","2021-04-16T00:00:00+00:00","2021-04-19T00:00:00+00:00","2021-04-20T00:00:00+00:00","2021-04-21T00:00:00+00:00","2021-04-22T00:00:00+00:00","2021-04-23T00:00:00+00:00","2021-04-26T00:00:00+00:00","2021-04-27T00:00:00+00:00","2021-04-28T00:00:00+00:00","2021-04-29T00:00:00+00:00","2021-04-30T00:00:00+00:00","2021-05-03T00:00:00+00:00","2021-05-04T00:00:00+00:00","2021-05-05T00:00:00+00:00","2021-05-06T00:00:00+00:00","2021-05-07T00:00:00+00:00","2021-05-10T00:00:00+00:00","2021-05-11T00:00:00+00:00","2021-05-12T00:00:00+00:00","2021-05-13T00:00:00+00:00","2021-05-14T00:00:00+00:00","2021-05-17T00:00:00+00:00","2021-05-18T00:00:00+00:00","2021-05-19T00:00:00+00:00","2021-05-20T00:00:00+00:00","2021-05-21T00:00:00+00:00","2021-05-24T00:00:00+00:00","2021-05-25T00:00:00+00:00","2021-05-26T00:00:00+00:00","2021-05-27T00:00:00+00:00","2021-05-28T00:00:00+00:00","2021-06-01T00:00:00+00:00","2021-06-02T00:00:00+00:00","2021-06-03T00:00:00+00:00","2021-06-04T00:00:00+00:00","2021-06-07T00:00:00+00:00","2021-06-08T00:00:00+00:00","2021-06-09T00:00:00+00:00","2021-06-10T00:00:00+00:00","2021-06-11T00:00:00+00:00","2021-06-14T00:00:00+00:00","2021-06-15T00:00:00+00:00","2021-06-16T00:00:00+00:00","2021-06-17T00:00:00+00:00","2021-06-18T00:00:00+00:00","2021-06-21T00:00:00+00:00","2021-06-22T00:00:00+00:00","2021-06-23T00:00:00+00:00","2021-06-24T00:00:00+00:00","2021-06-25T00:00:00+00:00","2021-06-28T00:00:00+00:00","2021-06-29T00:00:00+00:00","2021-06-30T00:00:00+00:00","2021-07-01T00:00:00+00:00","2021-07-02T00:00:00+00:00","2021-07-06T00:00:00+00:00","2021-07-07T00:00:00+00:00","2021-07-08T00:00:00+00:00","2021-07-09T00:00:00+00:00","2021-07-12T00:00:00+00:00","2021-07-13T00:00:00+00:00","2021-07-14T00:00:00+00:00","2021-07-15T00:00:00+00:00","2021-07-16T00:00:00+00:00","2021-07-19T00:00:00+00:00","2021-07-20T00:00:00+00:00","2021-07-21T00:00:00+00:00","2021-07-22T00:00:00+00:00","2021-07-23T00:00:00+00:00","2021-07-26T00:00:00+00:00","2021-07-27T00:00:00+00:00","2021-07-28T00:00:00+00:00","2021-07-29T00:00:00+00:00","2021-07-30T00:00:00+00:00","2021-08-02T00:00:00+00:00","2021-08-03T00:00:00+00:00","2021-08-04T00:00:00+00:00","2021-08-05T00:00:00+00:00","2021-08-06T00:00:00+00:00","2021-08-09T00:00:00+00:00","2021-08-10T00:00:00+00:00","2021-08-11T00:00:00+00:00","2021-08-12T00:00:00+00:00","2021-08-13T00:00:00+00:00","2021-08-16T00:00:00+00:00","2021-08-17T00:00:00+00:00","2021-08-18T00:00:00+00:00","2021-08-19T00:00:00+00:00","2021-08-20T00:00:00+00:00","2021-08-23T00:00:00+00:00","2021-08-24T00:00:00+00:00","2021-08-25T00:00:00+00:00","2021-08-26T00:00:00+00:00","2021-08-27T00:00:00+00:00","2021-08-30T00:00:00+00:00","2021-08-31T00:00:00+00:00","2021-09-01T00:00:00+00:00","2021-09-02T00:00:00+00:00","2021-09-03T00:00:00+00:00","2021-09-07T00:00:00+00:00","2021-09-08T00:00:00+00:00","2021-09-09T00:00:00+00:00","2021-09-10T00:00:00+00:00","2021-09-13T00:00:00+00:00","2021-09-14T00:00:00+00:00","2021-09-15T00:00:00+00:00","2021-09-16T00:00:00+00:00","2021-09-17T00:00:00+00:00","2021-09-20T00:00:00+00:00","2021-09-21T00:00:00+00:00","2021-09-22T00:00:00+00:00","2021-09-23T00:00:00+00:00","2021-09-24T00:00:00+00:00","2021-09-27T00:00:00+00:00","2021-09-28T00:00:00+00:00","2021-09-29T00:00:00+00:00","2021-09-30T00:00:00+00:00","2021-10-01T00:00:00+00:00","2021-10-04T00:00:00+00:00","2021-10-05T00:00:00+00:00","2021-10-06T00:00:00+00:00","2021-10-07T00:00:00+00:00","2021-10-08T00:00:00+00:00","2021-10-11T00:00:00+00:00","2021-10-12T00:00:00+00:00","2021-10-13T00:00:00+00:00","2021-10-14T00:00:00+00:00","2021-10-15T00:00:00+00:00","2021-10-18T00:00:00+00:00","2021-10-19T00:00:00+00:00","2021-10-20T00:00:00+00:00","2021-10-21T00:00:00+00:00","2021-10-22T00:00:00+00:00","2021-10-25T00:00:00+00:00","2021-10-26T00:00:00+00:00","2021-10-27T00:00:00+00:00","2021-10-28T00:00:00+00:00","2021-10-29T00:00:00+00:00","2021-11-01T00:00:00+00:00","2021-11-02T00:00:00+00:00","2021-11-03T00:00:00+00:00","2021-11-04T00:00:00+00:00","2021-11-05T00:00:00+00:00","2021-11-08T00:00:00+00:00","2021-11-09T00:00:00+00:00","2021-11-10T00:00:00+00:00","2021-11-11T00:00:00+00:00","2021-11-12T00:00:00+00:00","2021-11-15T00:00:00+00:00","2021-11-16T00:00:00+00:00","2021-11-17T00:00:00+00:00","2021-11-18T00:00:00+00:00","2021-11-19T00:00:00+00:00","2021-11-22T00:00:00+00:00","2021-11-23T00:00:00+00:00","2021-11-24T00:00:00+00:00","2021-11-26T00:00:00+00:00","2021-11-29T00:00:00+00:00","2021-11-30T00:00:00+00:00","2021-12-01T00:00:00+00:00","2021-12-02T00:00:00+00:00","2021-12-03T00:00:00+00:00","2021-12-06T00:00:00+00:00","2021-12-07T00:00:00+00:00","2021-12-08T00:00:00+00:00","2021-12-09T00:00:00+00:00","2021-12-10T00:00:00+00:00","2021-12-13T00:00:00+00:00","2021-12-14T00:00:00+00:00","2021-12-15T00:00:00+00:00","2021-12-16T00:00:00+00:00","2021-12-17T00:00:00+00:00","2021-12-20T00:00:00+00:00","2021-12-21T00:00:00+00:00","2021-12-22T00:00:00+00:00","2021-12-23T00:00:00+00:00","2021-12-27T00:00:00+00:00","2021-12-28T00:00:00+00:00","2021-12-29T00:00:00+00:00","2021-12-30T00:00:00+00:00","2021-12-31T00:00:00+00:00","2022-01-03T00:00:00+00:00","2022-01-04T00:00:00+00:00","2022-01-05T00:00:00+00:00","2022-01-06T00:00:00+00:00","2022-01-07T00:00:00+00:00","2022-01-10T00:00:00+00:00","2022-01-11T00:00:00+00:00","2022-01-12T00:00:00+00:00","2022-01-13T00:00:00+00:00","2022-01-14T00:00:00+00:00","2022-01-18T00:00:00+00:00","2022-01-19T00:00:00+00:00","2022-01-20T00:00:00+00:00","2022-01-21T00:00:00+00:00","2022-01-24T00:00:00+00:00","2022-01-25T00:00:00+00:00","2022-01-26T00:00:00+00:00","2022-01-27T00:00:00+00:00","2022-01-28T00:00:00+00:00","2022-01-31T00:00:00+00:00","2022-02-01T00:00:00+00:00","2022-02-02T00:00:00+00:00","2022-02-03T00:00:00+00:00","2022-02-04T00:00:00+00:00","2022-02-07T00:00:00+00:00","2022-02-08T00:00:00+00:00","2022-02-09T00:00:00+00:00","2022-02-10T00:00:00+00:00","2022-02-11T00:00:00+00:00","2022-02-14T00:00:00+00:00","2022-02-15T00:00:00+00:00","2022-02-16T00:00:00+00:00","2022-02-17T00:00:00+00:00","2022-02-18T00:00:00+00:00","2022-02-22T00:00:00+00:00","2022-02-23T00:00:00+00:00","2022-02-24T00:00:00+00:00","2022-02-25T00:00:00+00:00"],"y":[127.03559237302,124.381354704689,121.339419399412,119.420813144776,120.703197636216,115.673069320953,120.370175307044,119.271698669027,121.240009748912,120.315499999269,123.258025654048,124.828698131936,124.023479962892,119.818451746773,119.281639634077,122.661567751052,121.816585721808,119.381049284576,119.878097537072,120.494437370168,120.673374741067,119.192170948627,121.428888084861,122.273870114105,125.156749978584,125.464919895132,127.144942988569,129.590420390851,132.209864681507,130.465225315245,133.636393166172,131.250561554189,133.705979921521,133.367987109824,134.043972733219,132.324185779581,132.711883416528,131.16109286874,133.527042550622,133.92468115262,133.596629305972,132.791411136928,132.692001486429,130.683926546343,131.757550771735,127.09523816332,127.343762289568,128.974080557756,129.660007146201,126.314199420133,125.378169877722,122.251432895623,124.44214033531,126.911665085503,125.736649276943,124.322647202236,124.163323024804,126.77225643025,124.900197345426,126.563143447371,126.363988225581,126.314199420133,124.750830929084,124.083660936089,123.755054820135,124.531760185115,123.018180499513,125.358254355543,125.368212116632,126.204664048149,126.593016730639,125.577325099511,126.812087474608,129.928866695617,129.0924147641,129.600260579664,131.23333339834,129.908951173438,131.741179213903,133.414083076937,133.135265766432,132.846490694836,132.547757862152,134.210703964096,135.754156932966,136.381495881604,136.690186475378,139.36882420845,141.420122992884,143.959352070703,142.634969845801,144.497071169535,143.889647743077,145.024832507278,148.520006649688,147.852836656692,145.77166458899,141.848306719732,145.532678322842,144.78584624113,146.179932793659,147.932498745408,148.360682472256,146.15005951039,144.367620275372,145.024832507278,145.243903251247,144.905339374204,146.73756741467,146.329299210001,146.438834581985,145.741791305721,145.691927547918,145.203262721452,145.462554262026,148.484297984869,148.693725767641,150.70822158287,149.78075568774,145.961191840053,146.300265393111,147.786205375632,149.302063612834,149.212308848789,147.955742152161,147.137976524197,148.195088189614,152.702771894978,151.416286943668,152.094434049785,153.231327727686,153.879556579121,156.26304420209,154.687349455525,153.650183293229,148.564079997354,149.142499587865,147.716396114708,148.623916506717,148.384570469264,145.662009293237,142.550510806349,143.039175632815,145.452581510466,146.429911163398,146.519665927443,144.97388943556,141.523317395613,142.440810539183,141.114434581631,142.261301011093,138.760865213344,140.72549727077,141.613072159658,142.899557110967,142.510619800106,142.420865036062,141.124407333191,140.526042239559,143.368276434313,144.445333602851,146.150674119703,148.354652214582,148.853289792609,149.072690326941,148.284842953659,148.234979195856,148.913126301973,148.444406978627,152.154270559148,149.391818376879,148.554107245793,149.61121891121,151.07721339061,150.548657557901,151.08718614217,150.248256763803,150.617785180465,147.731468628701,147.681532356179,149.798830311106,149.80881756561,150.807543016048,153.294369387637,157.668786860553,160.345371067725,160.814772029431,161.204274955101,161.733599443833,156.610137883089,160.035766178089,165.089316957303,164.559992468571,163.551279763629,161.633726898789,165.109291466311,170.961822605875,174.85685186258,174.337514628353,179.221282080992,175.516010659869,174.107807774752,179.071473263426,172.040446092347,170.921873587857,169.533645211749,172.769515671166,175.416138114825,176.055322403105,180.100160477377,179.061486008922,179.151371299461,177.972875267945,177.34367823417,181.778019234112,179.470963443601,174.69705579051,171.780777475233,171.950560801808,171.970535310816,174.85685186258,175.306278315277,171.970535310816,172.849413707201,169.583581484271,166.018131626209,164.300323851457,162.203000405538,161.414007299693,159.576352470888,159.486467180349,159.017066218643,170.112905973003,174.557234227449,174.387450900875,175.615883204913,172.679630380627,172.39,171.66,174.83,176.28,172.12,168.64,168.88,172.79,172.55,168.88,167.3,164.32,160.07,162.74,164.85],"marker":{},"line":{}}];
            var layout = {"width":600,"height":600,"template":{"layout":{"title":{"x":0.05},"font":{"color":"rgba(42, 63, 95, 1.0)"},"paper_bgcolor":"rgba(255, 255, 255, 1.0)","plot_bgcolor":"rgba(229, 236, 246, 1.0)","autotypenumbers":"strict","colorscale":{"diverging":[[0.0,"#8e0152"],[0.1,"#c51b7d"],[0.2,"#de77ae"],[0.3,"#f1b6da"],[0.4,"#fde0ef"],[0.5,"#f7f7f7"],[0.6,"#e6f5d0"],[0.7,"#b8e186"],[0.8,"#7fbc41"],[0.9,"#4d9221"],[1.0,"#276419"]],"sequential":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"sequentialminus":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]},"hovermode":"closest","hoverlabel":{"align":"left"},"coloraxis":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"geo":{"showland":true,"landcolor":"rgba(229, 236, 246, 1.0)","showlakes":true,"lakecolor":"rgba(255, 255, 255, 1.0)","subunitcolor":"rgba(255, 255, 255, 1.0)","bgcolor":"rgba(255, 255, 255, 1.0)"},"mapbox":{"style":"light"},"polar":{"bgcolor":"rgba(229, 236, 246, 1.0)","radialaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""},"angularaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""}},"scene":{"xaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"yaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"zaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true}},"ternary":{"aaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"baxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"caxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"bgcolor":"rgba(229, 236, 246, 1.0)"},"xaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"yaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"annotationdefaults":{"arrowcolor":"#2a3f5f","arrowhead":0,"arrowwidth":1},"shapedefaults":{"line":{"color":"rgba(42, 63, 95, 1.0)"}},"colorway":["rgba(99, 110, 250, 1.0)","rgba(239, 85, 59, 1.0)","rgba(0, 204, 150, 1.0)","rgba(171, 99, 250, 1.0)","rgba(255, 161, 90, 1.0)","rgba(25, 211, 243, 1.0)","rgba(255, 102, 146, 1.0)","rgba(182, 232, 128, 1.0)","rgba(255, 151, 255, 1.0)","rgba(254, 203, 82, 1.0)"]},"data":{"bar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}},"error_x":{"color":"rgba(42, 63, 95, 1.0)"},"error_y":{"color":"rgba(42, 63, 95, 1.0)"}}],"barpolar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"carpet":[{"aaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"},"baxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"}}],"choropleth":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contourcarpet":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"heatmap":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"heatmapgl":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram":[{"marker":{"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"histogram2d":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram2dcontour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"mesh3d":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"parcoords":[{"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"pie":[{"automargin":true}],"scatter":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatter3d":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattercarpet":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergeo":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattermapbox":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolar":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolargl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterternary":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"surface":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"table":[{"cells":{"fill":{"color":"rgba(235, 240, 248, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}},"header":{"fill":{"color":"rgba(200, 212, 227, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}}}]}}};
            var config = {"responsive":true};
            Plotly.newPlot('2c1b2b4c-43eb-465e-b895-bcceeb3612e7', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_2c1b2b4c43eb465eb895bcceeb3612e7();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_2c1b2b4c43eb465eb895bcceeb3612e7();
            }
</script>
*)
(**
Let's calculate returns for this data. Typically we calculate close-close returns. Looking at the data, we could use the `close`, `divCash`, and `splitFacor` columns to calculate returns accounting for stock splits and dividends (a good at home exercise). But there is also an `adjClose` column that accounts for both those things. So we we can use this

*)
// Returns
let returns = 
    aapl
    |> Seq.sortBy(fun x -> x.Date)
    |> Seq.pairwise
    |> Seq.map(fun (a,b) -> b.Date, calcReturn (float a.AdjClose) (float b.AdjClose))

let avgReturnEachMonth = 
    returns
    |> Seq.groupBy(fun (date, ret) -> DateTime(date.Year, date.Month,1))
    |> Seq.map(fun (month, xs) -> month, Seq.length xs, xs |> Seq.averageBy snd)
(**
We can look at a few of these

*)
avgReturnEachMonth |> Seq.take 3 |> Seq.toList(* output: 
val it : (DateTime * int * float) list =
  [(3/1/2021 12:00:00 AM {Date = 3/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Monday;
                          DayOfYear = 60;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 3;
                          Second = 0;
                          Ticks = 637501536000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 22, -0.001847299936);
   (4/1/2021 12:00:00 AM {Date = 4/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Thursday;
                          DayOfYear = 91;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 4;
                          Second = 0;
                          Ticks = 637528320000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 21, 0.003591135423);
   (5/1/2021 12:00:00 AM {Date = 5/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Saturday;
                          DayOfYear = 121;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 5;
                          Second = 0;
                          Ticks = 637554240000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 20, -0.002470084796)]*)
(**
The default DateTime printing is too verbose if we don't care about time. We can simplify the printing:

    fsi.AddPrinter<DateTime>(fun dt -> dt.ToString("s"))
 
*)
avgReturnEachMonth |> Seq.take 3 |> Seq.toList(* output: 
val it : (DateTime * int * float) list =
  [(3/1/2021 12:00:00 AM {Date = 3/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Monday;
                          DayOfYear = 60;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 3;
                          Second = 0;
                          Ticks = 637501536000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 22, -0.001847299936);
   (4/1/2021 12:00:00 AM {Date = 4/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Thursday;
                          DayOfYear = 91;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 4;
                          Second = 0;
                          Ticks = 637528320000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 21, 0.003591135423);
   (5/1/2021 12:00:00 AM {Date = 5/1/2021 12:00:00 AM;
                          Day = 1;
                          DayOfWeek = Saturday;
                          DayOfYear = 121;
                          Hour = 0;
                          Kind = Unspecified;
                          Millisecond = 0;
                          Minute = 0;
                          Month = 5;
                          Second = 0;
                          Ticks = 637554240000000000L;
                          TimeOfDay = 00:00:00;
                          Year = 2021;}, 20, -0.002470084796)]*)
let monthlyReturnChart =
    avgReturnEachMonth
    |> Seq.map(fun (month, cnt, ret) -> month, ret)
    |> Chart.Bar
monthlyReturnChart |> Chart.show(* output: 
<div id="0c27b9d6-279a-42ae-a7cc-338499e5759e"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_0c27b9d6279a42aea7cc338499e5759e = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.6.3.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"bar","x":[-0.001847299935700057,0.003591135422937198,-0.002470084795557481,0.004349052687061024,0.0030958850599136604,0.0019568190098952043,-0.0032670973797818506,0.0027846923099910316,0.004869983333998869,0.003422019754716722,-0.0005661315187370896,-0.0030603219658519557],"y":["2021-03-01T00:00:00","2021-04-01T00:00:00","2021-05-01T00:00:00","2021-06-01T00:00:00","2021-07-01T00:00:00","2021-08-01T00:00:00","2021-09-01T00:00:00","2021-10-01T00:00:00","2021-11-01T00:00:00","2021-12-01T00:00:00","2022-01-01T00:00:00","2022-02-01T00:00:00"],"orientation":"h","marker":{"pattern":{}}}];
            var layout = {"width":600,"height":600,"template":{"layout":{"title":{"x":0.05},"font":{"color":"rgba(42, 63, 95, 1.0)"},"paper_bgcolor":"rgba(255, 255, 255, 1.0)","plot_bgcolor":"rgba(229, 236, 246, 1.0)","autotypenumbers":"strict","colorscale":{"diverging":[[0.0,"#8e0152"],[0.1,"#c51b7d"],[0.2,"#de77ae"],[0.3,"#f1b6da"],[0.4,"#fde0ef"],[0.5,"#f7f7f7"],[0.6,"#e6f5d0"],[0.7,"#b8e186"],[0.8,"#7fbc41"],[0.9,"#4d9221"],[1.0,"#276419"]],"sequential":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"sequentialminus":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]},"hovermode":"closest","hoverlabel":{"align":"left"},"coloraxis":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"geo":{"showland":true,"landcolor":"rgba(229, 236, 246, 1.0)","showlakes":true,"lakecolor":"rgba(255, 255, 255, 1.0)","subunitcolor":"rgba(255, 255, 255, 1.0)","bgcolor":"rgba(255, 255, 255, 1.0)"},"mapbox":{"style":"light"},"polar":{"bgcolor":"rgba(229, 236, 246, 1.0)","radialaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""},"angularaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""}},"scene":{"xaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"yaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"zaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true}},"ternary":{"aaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"baxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"caxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"bgcolor":"rgba(229, 236, 246, 1.0)"},"xaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"yaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"annotationdefaults":{"arrowcolor":"#2a3f5f","arrowhead":0,"arrowwidth":1},"shapedefaults":{"line":{"color":"rgba(42, 63, 95, 1.0)"}},"colorway":["rgba(99, 110, 250, 1.0)","rgba(239, 85, 59, 1.0)","rgba(0, 204, 150, 1.0)","rgba(171, 99, 250, 1.0)","rgba(255, 161, 90, 1.0)","rgba(25, 211, 243, 1.0)","rgba(255, 102, 146, 1.0)","rgba(182, 232, 128, 1.0)","rgba(255, 151, 255, 1.0)","rgba(254, 203, 82, 1.0)"]},"data":{"bar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}},"error_x":{"color":"rgba(42, 63, 95, 1.0)"},"error_y":{"color":"rgba(42, 63, 95, 1.0)"}}],"barpolar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"carpet":[{"aaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"},"baxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"}}],"choropleth":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contourcarpet":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"heatmap":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"heatmapgl":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram":[{"marker":{"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"histogram2d":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram2dcontour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"mesh3d":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"parcoords":[{"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"pie":[{"automargin":true}],"scatter":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatter3d":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattercarpet":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergeo":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattermapbox":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolar":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolargl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterternary":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"surface":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"table":[{"cells":{"fill":{"color":"rgba(235, 240, 248, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}},"header":{"fill":{"color":"rgba(200, 212, 227, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}}}]}}};
            var config = {"responsive":true};
            Plotly.newPlot('0c27b9d6-279a-42ae-a7cc-338499e5759e', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_0c27b9d6279a42aea7cc338499e5759e();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_0c27b9d6279a42aea7cc338499e5759e();
            }
</script>
*)
(**
## Volatility

We represent volatility by the standard deviation of returns. We can define a standard deviation function ourself.

*)
let stddev xs =
    let mu = xs |> Seq.average
    let sse = xs |> Seq.sumBy(fun x -> (x - mu)**2.0)
    let n = xs |> Seq.length |> float
    sqrt (sse / (n - 1.0))

[1.0 .. 10.0 ] |> stddev    (* output: 
val stddev : xs:seq<float> -> float
val it : float = 3.027650354*)
(**
But it is also convenient to use the [FSharp.Stats](https://fslab.org/FSharp.Stats/)

*)
#r "nuget: FSharp.Stats"

open FSharp.Stats
[1.0 .. 10.0 ] |> Seq.stDev(* output: 
[Loading C:\Users\runneradmin\AppData\Local\Temp\nuget\1516--846daabe-40b7-491d-824e-f9ed3f9047ef\Project.fsproj.fsx]
namespace FSI_2123.Project

val it : float = 3.027650354*)
(**
Now let's look at 5-day rolling volatilities.

*)
let rollingVols =
    returns
    // Sort by date again because you never can be too careful
    // about making sure that you have the right sort order.
    |> Seq.sortBy fst 
    |> Seq.windowed 5
    |> Seq.map(fun xs ->
        let maxWindowDate = xs |> Seq.map fst |> Seq.max
        let dailyVol = xs |> Seq.stDevBy snd
        let annualizedVolInPct = dailyVol * sqrt(252.0) * 100.0
        maxWindowDate, annualizedVolInPct)

let volChart = 
    rollingVols
    |> Chart.Line
volChart |> Chart.show    (* output: 
<div id="31b3fc50-8691-49ea-bdd5-d3276be386fe"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_31b3fc50869149eabdd5d3276be386fe = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.6.3.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"scatter","mode":"lines","x":["2021-03-08T00:00:00+00:00","2021-03-09T00:00:00+00:00","2021-03-10T00:00:00+00:00","2021-03-11T00:00:00+00:00","2021-03-12T00:00:00+00:00","2021-03-15T00:00:00+00:00","2021-03-16T00:00:00+00:00","2021-03-17T00:00:00+00:00","2021-03-18T00:00:00+00:00","2021-03-19T00:00:00+00:00","2021-03-22T00:00:00+00:00","2021-03-23T00:00:00+00:00","2021-03-24T00:00:00+00:00","2021-03-25T00:00:00+00:00","2021-03-26T00:00:00+00:00","2021-03-29T00:00:00+00:00","2021-03-30T00:00:00+00:00","2021-03-31T00:00:00+00:00","2021-04-01T00:00:00+00:00","2021-04-05T00:00:00+00:00","2021-04-06T00:00:00+00:00","2021-04-07T00:00:00+00:00","2021-04-08T00:00:00+00:00","2021-04-09T00:00:00+00:00","2021-04-12T00:00:00+00:00","2021-04-13T00:00:00+00:00","2021-04-14T00:00:00+00:00","2021-04-15T00:00:00+00:00","2021-04-16T00:00:00+00:00","2021-04-19T00:00:00+00:00","2021-04-20T00:00:00+00:00","2021-04-21T00:00:00+00:00","2021-04-22T00:00:00+00:00","2021-04-23T00:00:00+00:00","2021-04-26T00:00:00+00:00","2021-04-27T00:00:00+00:00","2021-04-28T00:00:00+00:00","2021-04-29T00:00:00+00:00","2021-04-30T00:00:00+00:00","2021-05-03T00:00:00+00:00","2021-05-04T00:00:00+00:00","2021-05-05T00:00:00+00:00","2021-05-06T00:00:00+00:00","2021-05-07T00:00:00+00:00","2021-05-10T00:00:00+00:00","2021-05-11T00:00:00+00:00","2021-05-12T00:00:00+00:00","2021-05-13T00:00:00+00:00","2021-05-14T00:00:00+00:00","2021-05-17T00:00:00+00:00","2021-05-18T00:00:00+00:00","2021-05-19T00:00:00+00:00","2021-05-20T00:00:00+00:00","2021-05-21T00:00:00+00:00","2021-05-24T00:00:00+00:00","2021-05-25T00:00:00+00:00","2021-05-26T00:00:00+00:00","2021-05-27T00:00:00+00:00","2021-05-28T00:00:00+00:00","2021-06-01T00:00:00+00:00","2021-06-02T00:00:00+00:00","2021-06-03T00:00:00+00:00","2021-06-04T00:00:00+00:00","2021-06-07T00:00:00+00:00","2021-06-08T00:00:00+00:00","2021-06-09T00:00:00+00:00","2021-06-10T00:00:00+00:00","2021-06-11T00:00:00+00:00","2021-06-14T00:00:00+00:00","2021-06-15T00:00:00+00:00","2021-06-16T00:00:00+00:00","2021-06-17T00:00:00+00:00","2021-06-18T00:00:00+00:00","2021-06-21T00:00:00+00:00","2021-06-22T00:00:00+00:00","2021-06-23T00:00:00+00:00","2021-06-24T00:00:00+00:00","2021-06-25T00:00:00+00:00","2021-06-28T00:00:00+00:00","2021-06-29T00:00:00+00:00","2021-06-30T00:00:00+00:00","2021-07-01T00:00:00+00:00","2021-07-02T00:00:00+00:00","2021-07-06T00:00:00+00:00","2021-07-07T00:00:00+00:00","2021-07-08T00:00:00+00:00","2021-07-09T00:00:00+00:00","2021-07-12T00:00:00+00:00","2021-07-13T00:00:00+00:00","2021-07-14T00:00:00+00:00","2021-07-15T00:00:00+00:00","2021-07-16T00:00:00+00:00","2021-07-19T00:00:00+00:00","2021-07-20T00:00:00+00:00","2021-07-21T00:00:00+00:00","2021-07-22T00:00:00+00:00","2021-07-23T00:00:00+00:00","2021-07-26T00:00:00+00:00","2021-07-27T00:00:00+00:00","2021-07-28T00:00:00+00:00","2021-07-29T00:00:00+00:00","2021-07-30T00:00:00+00:00","2021-08-02T00:00:00+00:00","2021-08-03T00:00:00+00:00","2021-08-04T00:00:00+00:00","2021-08-05T00:00:00+00:00","2021-08-06T00:00:00+00:00","2021-08-09T00:00:00+00:00","2021-08-10T00:00:00+00:00","2021-08-11T00:00:00+00:00","2021-08-12T00:00:00+00:00","2021-08-13T00:00:00+00:00","2021-08-16T00:00:00+00:00","2021-08-17T00:00:00+00:00","2021-08-18T00:00:00+00:00","2021-08-19T00:00:00+00:00","2021-08-20T00:00:00+00:00","2021-08-23T00:00:00+00:00","2021-08-24T00:00:00+00:00","2021-08-25T00:00:00+00:00","2021-08-26T00:00:00+00:00","2021-08-27T00:00:00+00:00","2021-08-30T00:00:00+00:00","2021-08-31T00:00:00+00:00","2021-09-01T00:00:00+00:00","2021-09-02T00:00:00+00:00","2021-09-03T00:00:00+00:00","2021-09-07T00:00:00+00:00","2021-09-08T00:00:00+00:00","2021-09-09T00:00:00+00:00","2021-09-10T00:00:00+00:00","2021-09-13T00:00:00+00:00","2021-09-14T00:00:00+00:00","2021-09-15T00:00:00+00:00","2021-09-16T00:00:00+00:00","2021-09-17T00:00:00+00:00","2021-09-20T00:00:00+00:00","2021-09-21T00:00:00+00:00","2021-09-22T00:00:00+00:00","2021-09-23T00:00:00+00:00","2021-09-24T00:00:00+00:00","2021-09-27T00:00:00+00:00","2021-09-28T00:00:00+00:00","2021-09-29T00:00:00+00:00","2021-09-30T00:00:00+00:00","2021-10-01T00:00:00+00:00","2021-10-04T00:00:00+00:00","2021-10-05T00:00:00+00:00","2021-10-06T00:00:00+00:00","2021-10-07T00:00:00+00:00","2021-10-08T00:00:00+00:00","2021-10-11T00:00:00+00:00","2021-10-12T00:00:00+00:00","2021-10-13T00:00:00+00:00","2021-10-14T00:00:00+00:00","2021-10-15T00:00:00+00:00","2021-10-18T00:00:00+00:00","2021-10-19T00:00:00+00:00","2021-10-20T00:00:00+00:00","2021-10-21T00:00:00+00:00","2021-10-22T00:00:00+00:00","2021-10-25T00:00:00+00:00","2021-10-26T00:00:00+00:00","2021-10-27T00:00:00+00:00","2021-10-28T00:00:00+00:00","2021-10-29T00:00:00+00:00","2021-11-01T00:00:00+00:00","2021-11-02T00:00:00+00:00","2021-11-03T00:00:00+00:00","2021-11-04T00:00:00+00:00","2021-11-05T00:00:00+00:00","2021-11-08T00:00:00+00:00","2021-11-09T00:00:00+00:00","2021-11-10T00:00:00+00:00","2021-11-11T00:00:00+00:00","2021-11-12T00:00:00+00:00","2021-11-15T00:00:00+00:00","2021-11-16T00:00:00+00:00","2021-11-17T00:00:00+00:00","2021-11-18T00:00:00+00:00","2021-11-19T00:00:00+00:00","2021-11-22T00:00:00+00:00","2021-11-23T00:00:00+00:00","2021-11-24T00:00:00+00:00","2021-11-26T00:00:00+00:00","2021-11-29T00:00:00+00:00","2021-11-30T00:00:00+00:00","2021-12-01T00:00:00+00:00","2021-12-02T00:00:00+00:00","2021-12-03T00:00:00+00:00","2021-12-06T00:00:00+00:00","2021-12-07T00:00:00+00:00","2021-12-08T00:00:00+00:00","2021-12-09T00:00:00+00:00","2021-12-10T00:00:00+00:00","2021-12-13T00:00:00+00:00","2021-12-14T00:00:00+00:00","2021-12-15T00:00:00+00:00","2021-12-16T00:00:00+00:00","2021-12-17T00:00:00+00:00","2021-12-20T00:00:00+00:00","2021-12-21T00:00:00+00:00","2021-12-22T00:00:00+00:00","2021-12-23T00:00:00+00:00","2021-12-27T00:00:00+00:00","2021-12-28T00:00:00+00:00","2021-12-29T00:00:00+00:00","2021-12-30T00:00:00+00:00","2021-12-31T00:00:00+00:00","2022-01-03T00:00:00+00:00","2022-01-04T00:00:00+00:00","2022-01-05T00:00:00+00:00","2022-01-06T00:00:00+00:00","2022-01-07T00:00:00+00:00","2022-01-10T00:00:00+00:00","2022-01-11T00:00:00+00:00","2022-01-12T00:00:00+00:00","2022-01-13T00:00:00+00:00","2022-01-14T00:00:00+00:00","2022-01-18T00:00:00+00:00","2022-01-19T00:00:00+00:00","2022-01-20T00:00:00+00:00","2022-01-21T00:00:00+00:00","2022-01-24T00:00:00+00:00","2022-01-25T00:00:00+00:00","2022-01-26T00:00:00+00:00","2022-01-27T00:00:00+00:00","2022-01-28T00:00:00+00:00","2022-01-31T00:00:00+00:00","2022-02-01T00:00:00+00:00","2022-02-02T00:00:00+00:00","2022-02-03T00:00:00+00:00","2022-02-04T00:00:00+00:00","2022-02-07T00:00:00+00:00","2022-02-08T00:00:00+00:00","2022-02-09T00:00:00+00:00","2022-02-10T00:00:00+00:00","2022-02-11T00:00:00+00:00","2022-02-14T00:00:00+00:00","2022-02-15T00:00:00+00:00","2022-02-16T00:00:00+00:00","2022-02-17T00:00:00+00:00","2022-02-18T00:00:00+00:00","2022-02-22T00:00:00+00:00","2022-02-23T00:00:00+00:00","2022-02-24T00:00:00+00:00","2022-02-25T00:00:00+00:00"],"y":[30.122874930983624,51.20486648024972,48.84854849276489,48.91817779005703,48.92216531901166,33.86718069410324,23.827715154799424,22.70891468928603,35.37719587464932,35.140539446999846,37.00590340011678,35.06969254724308,36.77734327250256,28.477535106978614,28.29411453341149,16.675331716328714,17.821129199386483,17.552984203271297,17.735160335856463,22.627307693285864,22.4682158744891,13.581976988558392,13.709968987161986,13.173241886607434,22.216778552289934,23.876379087524192,32.22444632150733,32.09584854316622,29.933468970745537,26.777280564362783,23.108279797865563,18.264332559778225,13.024882860283164,20.396691625785408,20.14144891013215,17.136673672466884,17.97724062349414,14.83519289740334,10.921943418518435,13.453084318419561,26.370090012097968,27.669558402889127,31.396072769710198,30.797952494962633,33.44920405299151,23.57606961494443,27.659701243112238,30.206387274974748,35.269453824417695,30.478805593006232,31.120230548947863,23.51485584757865,24.80200499181336,22.799439587683615,24.529370436283386,22.240704682936784,22.133161522208187,17.770258320726377,14.92435889890991,7.597013842391161,10.83509458544485,12.24731162911001,19.103431770837474,18.335042393843054,18.000457149114936,17.88617963564295,15.744000934430732,10.881760173103249,18.7484737068794,21.14942888875023,21.119151283842925,18.104463260951288,22.460541023737438,17.3289418708316,16.17374328264231,17.33453522373861,16.650820134212445,13.561429321778466,12.861472088164374,12.354248816857663,11.313440373467454,9.9345673948208,10.927931011980945,11.334857785049321,12.541507206363464,19.47086259151042,18.582297246688928,19.48717060775347,18.227349461024144,21.232399067361637,19.232243311586032,23.304522888237393,31.242573661266622,37.17792433178971,30.96804970288314,32.66911520332155,31.882529772924407,18.34871562731556,17.554911462428027,19.671776976062656,18.30505437952585,14.55660161958654,13.51716294654536,14.496394415360085,10.011646311409866,9.936674006439317,11.04942747973987,10.792499230189454,3.5777572663690034,4.402101697887022,16.439252935469288,15.18228712996301,15.836363860456046,17.049338979109265,28.6392948043582,22.9740316153271,24.770660859640074,23.626275252660214,23.305863321755737,12.485178048038094,13.823576045878125,12.723925574579404,24.750663969552782,26.359420595786464,24.337304766017446,22.243999649413535,22.4292412201698,13.670191899784632,14.682931729476659,16.624083831142155,28.817016951465558,28.74229075034329,21.468729755160155,24.792026934826012,25.2027908090724,16.025743870598124,18.1764919444592,20.133885373331626,25.145002099589796,26.352589780807378,22.314565373001557,15.772777655304731,24.941341350987894,20.713214594866315,18.43576566119761,21.056637242969266,25.037757964247653,25.04184391994602,25.008897658676588,24.59378689406125,24.26552253972261,11.035383542358602,11.536416674328615,10.636881391263199,18.016533846104938,18.253225010389208,18.923997295126778,14.69077978554928,10.383645980242308,9.00955240303703,13.00404137146876,11.986619713449992,6.126201442994702,6.126262946485411,19.392792498182875,24.753394078504893,25.28828951118836,25.606633294135463,25.920529420121248,17.722480256100717,10.565925567982895,10.540175348078845,9.679738872309166,14.435609194015111,14.736101848683832,19.379460129562883,19.07503132145539,19.713263488125516,12.422061013337734,17.06931875349675,17.279707173083846,15.918575391839381,17.41863980377046,18.467902128010504,28.723166029725988,30.811669797725383,38.4343236156976,38.922493036684365,39.65484152081504,30.23549319282037,30.11743383475184,32.207395533536996,32.21335240778734,31.12970075443662,22.947739992718937,37.33712235288134,33.02554455655352,35.270198653525576,47.65520097728037,39.41656665969302,38.097957806973284,42.14263987719213,36.99946396606826,19.615462057396684,20.178905397144362,18.82795269343201,18.450105999251885,19.020345168752232,19.532521950139483,20.939397596435942,23.016020162259963,30.02059626698378,31.216103336252676,31.68525446240792,18.545331638984493,26.79720575870128,18.86871446946414,20.250901919125017,20.550210903640625,25.065315416175878,20.517116778063148,17.214401333983613,16.34767500013013,10.371525963242714,9.25741527720129,8.12586643940464,8.462923721315017,53.42621336562991,52.51484062592403,49.50278445109432,47.94815571968164,52.81646060747269,24.81450196543306,13.656137498313653,20.83856031653634,21.10014485141813,24.922769602691623,28.64858886367673,28.931704230449196,31.234592094703896,29.915767818125698,28.99189063376355,25.94933262190538,28.094506914349243,15.518587897712354,26.80313785840116,29.811864951473588],"marker":{},"line":{}}];
            var layout = {"width":600,"height":600,"template":{"layout":{"title":{"x":0.05},"font":{"color":"rgba(42, 63, 95, 1.0)"},"paper_bgcolor":"rgba(255, 255, 255, 1.0)","plot_bgcolor":"rgba(229, 236, 246, 1.0)","autotypenumbers":"strict","colorscale":{"diverging":[[0.0,"#8e0152"],[0.1,"#c51b7d"],[0.2,"#de77ae"],[0.3,"#f1b6da"],[0.4,"#fde0ef"],[0.5,"#f7f7f7"],[0.6,"#e6f5d0"],[0.7,"#b8e186"],[0.8,"#7fbc41"],[0.9,"#4d9221"],[1.0,"#276419"]],"sequential":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"sequentialminus":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]},"hovermode":"closest","hoverlabel":{"align":"left"},"coloraxis":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"geo":{"showland":true,"landcolor":"rgba(229, 236, 246, 1.0)","showlakes":true,"lakecolor":"rgba(255, 255, 255, 1.0)","subunitcolor":"rgba(255, 255, 255, 1.0)","bgcolor":"rgba(255, 255, 255, 1.0)"},"mapbox":{"style":"light"},"polar":{"bgcolor":"rgba(229, 236, 246, 1.0)","radialaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""},"angularaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""}},"scene":{"xaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"yaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"zaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true}},"ternary":{"aaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"baxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"caxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"bgcolor":"rgba(229, 236, 246, 1.0)"},"xaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"yaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"annotationdefaults":{"arrowcolor":"#2a3f5f","arrowhead":0,"arrowwidth":1},"shapedefaults":{"line":{"color":"rgba(42, 63, 95, 1.0)"}},"colorway":["rgba(99, 110, 250, 1.0)","rgba(239, 85, 59, 1.0)","rgba(0, 204, 150, 1.0)","rgba(171, 99, 250, 1.0)","rgba(255, 161, 90, 1.0)","rgba(25, 211, 243, 1.0)","rgba(255, 102, 146, 1.0)","rgba(182, 232, 128, 1.0)","rgba(255, 151, 255, 1.0)","rgba(254, 203, 82, 1.0)"]},"data":{"bar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}},"error_x":{"color":"rgba(42, 63, 95, 1.0)"},"error_y":{"color":"rgba(42, 63, 95, 1.0)"}}],"barpolar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"carpet":[{"aaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"},"baxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"}}],"choropleth":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contourcarpet":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"heatmap":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"heatmapgl":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram":[{"marker":{"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"histogram2d":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram2dcontour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"mesh3d":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"parcoords":[{"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"pie":[{"automargin":true}],"scatter":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatter3d":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattercarpet":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergeo":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattermapbox":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolar":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolargl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterternary":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"surface":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"table":[{"cells":{"fill":{"color":"rgba(235, 240, 248, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}},"header":{"fill":{"color":"rgba(200, 212, 227, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}}}]}}};
            var config = {"responsive":true};
            Plotly.newPlot('31b3fc50-8691-49ea-bdd5-d3276be386fe', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_31b3fc50869149eabdd5d3276be386fe();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_31b3fc50869149eabdd5d3276be386fe();
            }
</script>
*)

