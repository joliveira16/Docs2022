(**
[![Binder](img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=portfolio-statistics.ipynb)&emsp;
[![Script](img/badge-script.svg)](/Teaching//portfolio-statistics.fsx)&emsp;
[![Notebook](img/badge-notebook.svg)](/Teaching//portfolio-statistics.ipynb)

This page covers important fundamentals for building portfolios.

* [Portfolio Weights](#Portfolio-Weights)

* [Mean and Variance of Portfolio Returns](#Mean-and-Variance-of-Portfolio-Returns)

* [Leverage](#Leverage)

## Portfolio Weights

An investment portfolio consists of positions in assets.
It is common to refer to a position's size as its share of
the portfolio's total value.
This is known as the asset's portfolio weight.

The portfolio weight of asset $i$ in portfolio $p$ is

\begin{equation}
w_i=(\text{positionValue}_i)/(\text{portfolioValue}_p)
\end{equation}

### Long positions

When an investor buys a long position,
they pay for the position now and hope to sell it later at a higher price.

Let's look at cash flows for long positions.

* At time 0: investor has $100 cash and no shares.

* At time 1: investor takes their $100 of cash and buys 4 shares 
of stock at a price of $25 per share. They have no cash and are long 4 shares.

* At time 2: the stock has risen to $27, the investor sells their 4 shares.
They have $108 of cash and no shares.

We can define some functions to update an account given these trades.

*)
// A record type that defines an account
type AccountBalances =
    { Time: int
      Cash: float 
      Shares: float }

// A record type that defines a trade
type Trade = 
    { Shares: float 
      Price : float }

let accountAt0 = { Time = 0; Cash = 100.0; Shares = 0.0 }
let tradeAt1 = { Shares = 4.0; Price = 25.0 }

// `updateAccount` is a function that updates an account after a trade is made.
// 
// (trade: Trade) restricts the `trade` parameter to data of type `Trade`.
//
// (inAccount: AccountBalances) restricts the `inAccount` parameter 
// to data of type `AccountBalances`
//
let updateAccount (trade: Trade) (inAccount: AccountBalances) =
    let tradeValue = trade.Price * trade.Shares
    let newCash = inAccount.Cash - tradeValue
    let newShares = inAccount.Shares + trade.Shares
    let newTime = inAccount.Time + 1
    { Time = newTime 
      Cash = newCash 
      Shares = newShares }
(**
You can make names with spaces using "``" before and after.

*)
let ``expected account at t1`` = { Time = 1; Cash = 0.0; Shares = 4.0}
let ``actual account at t1`` = updateAccount tradeAt1 accountAt0 

``actual account at t1``(* output: 
{ Time = 1
  Cash = 0.0
  Shares = 4.0 }*)
if ``actual account at t1`` <> ``expected account at t1`` then
    failwith "You are not updating account correctly after a trade"
(**
Now we can calculate how the account value changes over time.

*)
let accountAt1 = updateAccount tradeAt1 accountAt0

accountAt1(* output: 
{ Time = 1
  Cash = 0.0
  Shares = 4.0 }*)
let tradeAt2 = { Shares = -4.0; Price = 27.0 }
let accountAt2 = updateAccount tradeAt2 accountAt1

accountAt2(* output: 
{ Time = 2
  Cash = 108.0
  Shares = 0.0 }*)
(**
We could have also written this code using the pipe operator.

*)
let accountAt1' = accountAt0 |> updateAccount tradeAt1 // same as "updateAccount tradeAt1 accountAt0"
let accountAt2' = accountAt1 |> updateAccount tradeAt2 // same as "updateAccount tradeAt2 accountAt1"

accountAt1'(* output: 
{ Time = 1
  Cash = 0.0
  Shares = 4.0 }*)
accountAt2'(* output: 
{ Time = 2
  Cash = 108.0
  Shares = 0.0 }*)
(**
The pipe operator does not look very useful above because
we are only doing one operation.
It is more useful when you're doing a series of multiple operations.
This example recomputes the account value at time 2 by
chaining together updates for the trades at times 1 and 2.

*)
let accountAt2'' =
    accountAt0
    |> updateAccount tradeAt1
    |> updateAccount tradeAt2

accountAt2''(* output: 
{ Time = 2
  Cash = 108.0
  Shares = 0.0 }*)
(**
This code is closer to how you would describe it in English:
"Start with the account at time 0,
update it for the trade at time 1,
then update it for the trade at time 2."

> Practice: complete the code for the `accountValue` function below.
It should calculate total account value of
the stock and cash positiions.
If it is correct then the account value test below should evaluate to `true`
> 

*)
let accountValue (stockPrice: float) (account: AccountBalances) =
    failwith "unimplemented"
// simple account value test
(accountValue 27.0 accountAt2) = 108.0
(**
### Portfolio weights of long positions

Now that we understand long positions we can calculate portfolio weights for them.
Let's calculate weights for an example **Portfolio A** consisting of

* $100 invested in AAPL

* $300 invested in GOOG

* $500 invested in TSLA

These are all long positions, meaning that they have positive costs.

*)
let aaplPositionValue = 100.0
let googPositionValue = 300.0
let tslaPositionValue = 500.0

// This implies:

let portfolioValueA = aaplPositionValue + googPositionValue + tslaPositionValue

portfolioValueA(* output: 
900.0*)
(**
The portfolio weights are then

*)
let aaplWeight = aaplPositionValue / portfolioValueA

aaplWeight(* output: 
0.1111111111*)
let googWeight = googPositionValue / portfolioValueA

googWeight(* output: 
0.3333333333*)
let tslaWeight = tslaPositionValue / portfolioValueA

tslaWeight(* output: 
0.5555555556*)
(**
These weights for AAPL, GOOG, and TSLA are all positive.
Long positions always have positive weights.

Another thing to notice is that the portfolio weights add up to one (or 100%):

*)
aaplWeight + googWeight + tslaWeight(* output: 
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float = 1.0*)
(**
This portfolio is a net long portfolio,
meaning that it costs you money to purchase it.
Net long portfolios such as this one must
have portfolio weights that add up to one.
Due to margin requirements, real-world
portfolios are generally net long--you must
put up capital to acquire the portfolio.

The other type of portfolio is a zero-cost portfolio.
As the name implies, zero-cost portfolios do not require any investment to purchase.
There is no cost because long positions are funded by offsetting short positions.
To see how this works we need to examine how short positions work.

### Short positions

When an investor buys a long position,
they pay for the position now and hope to sell it later at a higher price.
A short sale reverses this.
The investor sells the position now and hopes to buy it back later at a lower price.

We now go through an example to see how the cash flows work.

* At time 0: investor has $100 cash and no shares.

* At time 1: investor borrows 4 shares of stock and sells them
for a price of $25 per share. They have $200 cash and are short 4 shares.

* At time 2: the stock has risen to $27, the investor buys back the
4 shares that they sold short and returns them to the person that
they borrowed them from. They have $92 of cash and 0 shares.

The investor's cash and stock balances at the end of each period will look something like

*)
let shortAt1 = { Shares = -4.0; Price = 25.0 }
let shortCoverAt2 = { Shares = 4.0; Price = 27.0 }

// positions at t1
accountAt0 
|> updateAccount shortAt1(* output: 
{ Time = 1
  Cash = 200.0
  Shares = -4.0 }*)
// positions at t2
accountAt0 
|> updateAccount shortAt1 
|> updateAccount shortCoverAt2(* output: 
{ Time = 2
  Cash = 92.0
  Shares = 0.0 }*)
(**
### Portfolio weights for short positions

Let's create a new portfolio, **Portfolio B**, that includes short sales and calculate weights. Assume that you start with **Portfolio A** and short $150 of AMZN stock. This generates $150 of cash that you have to put somewhere. For individual investors, often your broker puts it in bonds and gives you none of the interest. Institutional investors can get some of the interest or even reinvest the proceeds in something else. We will assume that we are an institution and can reinvest all of the short proceeds. We will take the $150 and add $50 to each of our AAPL, GOOG, and TLSA positions.

Short positions have negative portfolio weights.

*)
let amznPositionValueB = -150.0
let aaplPositionValueB = aaplPositionValue + 50.0
let googPositionValueB = googPositionValue + 50.0
let tslaPositionValueB = tslaPositionValue + 50.0

let portfolioValueB = 
    amznPositionValueB +
    aaplPositionValueB +
    googPositionValueB +
    tslaPositionValueB

portfolioValueB(* output: 
900.0*)
(**
Compare to **Portfolio A**

*)
portfolioValueA = portfolioValueB (* output: 
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true*)
(**
The weights in **Portfolio B**:

*)
let amznWeightB = amznPositionValueB / portfolioValueB

amznWeightB(* output: 
-0.1666666667*)
let aaplWeightB = aaplPositionValueB / portfolioValueB

aaplWeightB(* output: 
0.1666666667*)
let googWeightB = googPositionValueB / portfolioValueB

googWeightB(* output: 
0.3888888889*)
let tslaWeightB = tslaPositionValueB / portfolioValueB

tslaWeightB(* output: 
0.6111111111*)
(**
The weights of **Portfolio B** also add up to one.

*)
amznWeightB + aaplWeightB + googWeightB + tslaWeightB(* output: 
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float = 1.0*)
(**
### Zero-cost portfolios

Another type of portfolio that you will see is zero-cost portfolios.
They are called self funding because the short sale proceeds
fund the long investments. The portfolio weights add up to 0.
You can scale weights relative to what they would be per $ long or short.

An example:

*)
// Portfolio C
let koPositionValue = -50.0
let hogPositionValue = 40.0
let yumPositionValue = 10.0

let dollarsLong = 50.0
let koWeight = koPositionValue / dollarsLong
let hogWeight = hogPositionValue / dollarsLong
let yumWeight = yumPositionValue / dollarsLong

printfn $"koWeight = {koWeight}"
printfn $"hogWeight= {hogWeight}"
printfn $"yumWeight= {yumWeight}"(* output: 
koWeight = -1
hogWeight= 0.8
yumWeight= 0.2*)
koWeight + hogWeight + yumWeight(* output: 
5.551115123e-17*)
(**
### Calculating weights using a list of positions

The calculations that we did thus far required code for each position.
We did the same thing to each position, so there was some repetition.
We can reduce the repetition by putting the positions into a list
and then operating on the elements of the list via iteration.

*)
// defining a position record type
type Position = { Id: string; PositionValue: float }

// assigning a list of positions to a value named portfolio
let portfolio =
    [ { Id = "AMZN"; PositionValue = amznPositionValueB }
      { Id = "AAPL"; PositionValue = aaplPositionValueB }
      { Id = "GOOG"; PositionValue = googPositionValueB }
      { Id = "TSLA"; PositionValue = tslaPositionValueB } ]

// This is called a list comprehension
let positionValues = [ for p in portfolio -> p.PositionValue ](* output: 
[-150.0; 150.0; 350.0; 550.0]*)
(**
The list module has many different functions for operating on lists.
If you type `List.` you should see many different functions pop up.
These functions are very useful. We will explore them in more detail later.

For now, let's see what `List.map` does.

*)
portfolio |> List.map (fun p -> p.PositionValue)(* output: 
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : float list = [-150.0; 150.0; 350.0; 550.0]*)
(**
This is the same result as the `positionValues` value that we calculated
using the list comprehension.
`List.map` "maps" each element of the list to an output using a function.
In this case, our function `(fun p -> p.PositionValue)` was an anonymous function.

Another useful function from the list module is `List.sum`.
We can use it to calculate the total value of the portfolio by
summing position values.

*)
let portfolioValue = positionValues |> List.sum(* output: 
900.0*)
(**
And with this we can calculate portfolio weights.

*)
let portfolioWeights =
    [ for p in portfolio -> 
        let weight = p.PositionValue / portfolioValue 
        p.Id, weight ]
portfolioWeights(* output: 
[("AMZN", -0.1666666667); ("AAPL", 0.1666666667); ("GOOG", 0.3888888889);
 ("TSLA", 0.6111111111)]*)
(**
## Mean and Variance of Portfolio Returns

### A portfolio's return.

A portfolio's return is the weighted average return of the portfolio's positions.

\begin{equation}
 r_p = \Sigma^N_{i=1} w_i r_i,
\end{equation}

where $r$ is return, $i$ indexes stocks, and $w$ is portfolio weights.

*)
type PositionsWithReturn =
    { Id: string 
      Weight: float 
      Return: float }

let exPortfolio =
    [ { Id = "A"; Weight = 0.25; Return = 0.1 }
      { Id = "B"; Weight = 0.75; Return = 0.2 } ]

let weightsXreturn = [ for pos in exPortfolio -> pos.Weight * pos.Return ]
weightsXreturn(* output: 
[0.025; 0.15]*)
let exPortfolioReturn = weightsXreturn |> List.sum 
exPortfolioReturn(* output: 
0.175*)
(**
We are now going to look at returns of actual stock and bond portfolios.
The two portfolios are [VTI](https://investor.vanguard.com/etf/profile/VTI) and
[BND](https://investor.vanguard.com/etf/profile/BND).
These are value-weighted exchange traded funds (ETFs).
VTI tracks a stock market index and BND tracks a bond market index.
They are good proxies for the return of the overall US stock and bond markets.

We are going to load some helper code that allows us to download and plot this data.
This will introduce using `#load` to load scripts with external code,
the `nuget` package manager for loading external libraries,
and how to open namespaces.

When you type `#load "Script.fsx"` in the REPL,
F# interactive compiles the code in `Script.fsx` and puts it into
a code module with the same name as the script.

We are going to use a helper script called `YahooFinance.fsx` that includes
code for requesting price histories from yahoo. To download it,
go to the [YahooFinance](YahooFinance.html) page and click the "download script"
button at the top. Make sure that you have saved it in
the same directory as this file.

If you have downloaded it correctly then the following code will evaluate to `true`.

*)
System.IO.File.Exists("YahooFinance.fsx")(* output: 
true*)
(**
Assuming that the above code evaluated to `true` we can now load it into our session.

*)
#load "YahooFinance.fsx"
(**
Namespaces are a hierarchical way of organizing code.
In the above checking for the existence of a file we have a hierarchy of
`System.IO` where the period `.` separates the `System` and `IO` namespaces.
If we `open` a namespace, then we have access to the code inside the namespace directly.

It is common to open the `System` namespace.

*)
open System
(**
Now we can leave `System` off when accessing code in the `System` namespace.

*)
IO.File.Exists("YahooFinance.fsx")(* output: 
true*)
(**
We also want to open the `YahooFinance` module from `YahooFinance.fsx`,
which is similar to a namespace.

*)
open YahooFinance
(**
We are ready to request some data. Let's define our start and end dates.
`DateTime` is a type in the `System` namespace.
We have opened that namespace so we can access the type directly.

*)
let myStart = DateTime(2010,1,1)
let myEnd = DateTime.UtcNow
myEnd(* output: 
2/28/2022 2:49:36 PM*)
(**
Our `YahooFinance` module has code for requesting price histories of stocks.

*)
let bnd = YahooFinance.PriceHistory("BND",startDate=myStart,endDate=myEnd,interval = Interval.Daily)
let vti = YahooFinance.PriceHistory("VTI",startDate=myStart,endDate=myEnd,interval = Interval.Daily)
(**
This returns several data items for each point in time.

*)
vti[0..3](* output: 
[{ Symbol = "VTI"
   Date = 1/4/2010 12:00:00 AM
   Open = 56.860001
   High = 57.380001
   Low = 56.84
   Close = 57.310001
   AdjustedClose = 45.772934
   Volume = 2251500.0 }; { Symbol = "VTI"
                           Date = 1/5/2010 12:00:00 AM
                           Open = 57.34
                           High = 57.540001
                           Low = 57.110001
                           Close = 57.529999
                           AdjustedClose = 45.948635
                           Volume = 1597700.0 }; { Symbol = "VTI"
                                                   Date = 1/6/2010 12:00:00 AM
                                                   Open = 57.5
                                                   High = 57.720001
                                                   Low = 57.41
                                                   Close = 57.610001
                                                   AdjustedClose = 46.012543
                                                   Volume = 2120300.0 };
 { Symbol = "VTI"
   Date = 1/7/2010 12:00:00 AM
   Open = 57.549999
   High = 57.889999
   Low = 57.290001
   Close = 57.849998
   AdjustedClose = 46.204227
   Volume = 1656700.0 }]*)
(**
The adjusted close is adjusted for stock splits and dividends.
This adjustment is done so that you can calculate returns from the price changes.

Let's see what it looks like to plot it.
We're going to use the [plotly.NET](https://plotly.net) library for this.
We download the code from the [nuget.org](http://www.nuget.org) package manager.

This is equivalent to loading libraries with `pip` or `conda` in python
or `install.packages` in R.

*)
#r "nuget: Plotly.NET, 2.0.0-preview.17"
#r "nuget: Plotly.NET.Interactive, 2.0.0-preview.17"


open Plotly.NET
(**
Above we are loading an exact version by using a "," and version number.

Plot prices as a line chart.

*)
let vtiAdjPrices = [ for period in vti -> period.Date, period.AdjustedClose ]
Chart.Line(vtiAdjPrices)(* output: 
<div id="2556851b-7e13-433e-8b2b-da75f16bfd02"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_2556851b7e13433e8b2bda75f16bfd02 = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.6.3.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"scatter","mode":"lines","x":["2010-01-04T00:00:00+00:00","2010-01-05T00:00:00+00:00","2010-01-06T00:00:00+00:00","2010-01-07T00:00:00+00:00","2010-01-08T00:00:00+00:00","2010-01-11T00:00:00+00:00","2010-01-12T00:00:00+00:00","2010-01-13T00:00:00+00:00","2010-01-14T00:00:00+00:00","2010-01-15T00:00:00+00:00","2010-01-19T00:00:00+00:00","2010-01-20T00:00:00+00:00","2010-01-21T00:00:00+00:00","2010-01-22T00:00:00+00:00","2010-01-25T00:00:00+00:00","2010-01-26T00:00:00+00:00","2010-01-27T00:00:00+00:00","2010-01-28T00:00:00+00:00","2010-01-29T00:00:00+00:00","2010-02-01T00:00:00+00:00","2010-02-02T00:00:00+00:00","2010-02-03T00:00:00+00:00","2010-02-04T00:00:00+00:00","2010-02-05T00:00:00+00:00","2010-02-08T00:00:00+00:00","2010-02-09T00:00:00+00:00","2010-02-10T00:00:00+00:00","2010-02-11T00:00:00+00:00","2010-02-12T00:00:00+00:00","2010-02-16T00:00:00+00:00","2010-02-17T00:00:00+00:00","2010-02-18T00:00:00+00:00","2010-02-19T00:00:00+00:00","2010-02-22T00:00:00+00:00","2010-02-23T00:00:00+00:00","2010-02-24T00:00:00+00:00","2010-02-25T00:00:00+00:00","2010-02-26T00:00:00+00:00","2010-03-01T00:00:00+00:00","2010-03-02T00:00:00+00:00","2010-03-03T00:00:00+00:00","2010-03-04T00:00:00+00:00","2010-03-05T00:00:00+00:00","2010-03-08T00:00:00+00:00","2010-03-09T00:00:00+00:00","2010-03-10T00:00:00+00:00","2010-03-11T00:00:00+00:00","2010-03-12T00:00:00+00:00","2010-03-15T00:00:00+00:00","2010-03-16T00:00:00+00:00","2010-03-17T00:00:00+00:00","2010-03-18T00:00:00+00:00","2010-03-19T00:00:00+00:00","2010-03-22T00:00:00+00:00","2010-03-23T00:00:00+00:00","2010-03-24T00:00:00+00:00","2010-03-25T00:00:00+00:00","2010-03-26T00:00:00+00:00","2010-03-29T00:00:00+00:00","2010-03-30T00:00:00+00:00","2010-03-31T00:00:00+00:00","2010-04-01T00:00:00+00:00","2010-04-05T00:00:00+00:00","2010-04-06T00:00:00+00:00","2010-04-07T00:00:00+00:00","2010-04-08T00:00:00+00:00","2010-04-09T00:00:00+00:00","2010-04-12T00:00:00+00:00","2010-04-13T00:00:00+00:00","2010-04-14T00:00:00+00:00","2010-04-15T00:00:00+00:00","2010-04-16T00:00:00+00:00","2010-04-19T00:00:00+00:00","2010-04-20T00:00:00+00:00","2010-04-21T00:00:00+00:00","2010-04-22T00:00:00+00:00","2010-04-23T00:00:00+00:00","2010-04-26T00:00:00+00:00","2010-04-27T00:00:00+00:00","2010-04-28T00:00:00+00:00","2010-04-29T00:00:00+00:00","2010-04-30T00:00:00+00:00","2010-05-03T00:00:00+00:00","2010-05-04T00:00:00+00:00","2010-05-05T00:00:00+00:00","2010-05-06T00:00:00+00:00","2010-05-07T00:00:00+00:00","2010-05-10T00:00:00+00:00","2010-05-11T00:00:00+00:00","2010-05-12T00:00:00+00:00","2010-05-13T00:00:00+00:00","2010-05-14T00:00:00+00:00","2010-05-17T00:00:00+00:00","2010-05-18T00:00:00+00:00","2010-05-19T00:00:00+00:00","2010-05-20T00:00:00+00:00","2010-05-21T00:00:00+00:00","2010-05-24T00:00:00+00:00","2010-05-25T00:00:00+00:00","2010-05-26T00:00:00+00:00","2010-05-27T00:00:00+00:00","2010-05-28T00:00:00+00:00","2010-06-01T00:00:00+00:00","2010-06-02T00:00:00+00:00","2010-06-03T00:00:00+00:00","2010-06-04T00:00:00+00:00","2010-06-07T00:00:00+00:00","2010-06-08T00:00:00+00:00","2010-06-09T00:00:00+00:00","2010-06-10T00:00:00+00:00","2010-06-11T00:00:00+00:00","2010-06-14T00:00:00+00:00","2010-06-15T00:00:00+00:00","2010-06-16T00:00:00+00:00","2010-06-17T00:00:00+00:00","2010-06-18T00:00:00+00:00","2010-06-21T00:00:00+00:00","2010-06-22T00:00:00+00:00","2010-06-23T00:00:00+00:00","2010-06-24T00:00:00+00:00","2010-06-25T00:00:00+00:00","2010-06-28T00:00:00+00:00","2010-06-29T00:00:00+00:00","2010-06-30T00:00:00+00:00","2010-07-01T00:00:00+00:00","2010-07-02T00:00:00+00:00","2010-07-06T00:00:00+00:00","2010-07-07T00:00:00+00:00","2010-07-08T00:00:00+00:00","2010-07-09T00:00:00+00:00","2010-07-12T00:00:00+00:00","2010-07-13T00:00:00+00:00","2010-07-14T00:00:00+00:00","2010-07-15T00:00:00+00:00","2010-07-16T00:00:00+00:00","2010-07-19T00:00:00+00:00","2010-07-20T00:00:00+00:00","2010-07-21T00:00:00+00:00","2010-07-22T00:00:00+00:00","2010-07-23T00:00:00+00:00","2010-07-26T00:00:00+00:00","2010-07-27T00:00:00+00:00","2010-07-28T00:00:00+00:00","2010-07-29T00:00:00+00:00","2010-07-30T00:00:00+00:00","2010-08-02T00:00:00+00:00","2010-08-03T00:00:00+00:00","2010-08-04T00:00:00+00:00","2010-08-05T00:00:00+00:00","2010-08-06T00:00:00+00:00","2010-08-09T00:00:00+00:00","2010-08-10T00:00:00+00:00","2010-08-11T00:00:00+00:00","2010-08-12T00:00:00+00:00","2010-08-13T00:00:00+00:00","2010-08-16T00:00:00+00:00","2010-08-17T00:00:00+00:00","2010-08-18T00:00:00+00:00","2010-08-19T00:00:00+00:00","2010-08-20T00:00:00+00:00","2010-08-23T00:00:00+00:00","2010-08-24T00:00:00+00:00","2010-08-25T00:00:00+00:00","2010-08-26T00:00:00+00:00","2010-08-27T00:00:00+00:00","2010-08-30T00:00:00+00:00","2010-08-31T00:00:00+00:00","2010-09-01T00:00:00+00:00","2010-09-02T00:00:00+00:00","2010-09-03T00:00:00+00:00","2010-09-07T00:00:00+00:00","2010-09-08T00:00:00+00:00","2010-09-09T00:00:00+00:00","2010-09-10T00:00:00+00:00","2010-09-13T00:00:00+00:00","2010-09-14T00:00:00+00:00","2010-09-15T00:00:00+00:00","2010-09-16T00:00:00+00:00","2010-09-17T00:00:00+00:00","2010-09-20T00:00:00+00:00","2010-09-21T00:00:00+00:00","2010-09-22T00:00:00+00:00","2010-09-23T00:00:00+00:00","2010-09-24T00:00:00+00:00","2010-09-27T00:00:00+00:00","2010-09-28T00:00:00+00:00","2010-09-29T00:00:00+00:00","2010-09-30T00:00:00+00:00","2010-10-01T00:00:00+00:00","2010-10-04T00:00:00+00:00","2010-10-05T00:00:00+00:00","2010-10-06T00:00:00+00:00","2010-10-07T00:00:00+00:00","2010-10-08T00:00:00+00:00","2010-10-11T00:00:00+00:00","2010-10-12T00:00:00+00:00","2010-10-13T00:00:00+00:00","2010-10-14T00:00:00+00:00","2010-10-15T00:00:00+00:00","2010-10-18T00:00:00+00:00","2010-10-19T00:00:00+00:00","2010-10-20T00:00:00+00:00","2010-10-21T00:00:00+00:00","2010-10-22T00:00:00+00:00","2010-10-25T00:00:00+00:00","2010-10-26T00:00:00+00:00","2010-10-27T00:00:00+00:00","2010-10-28T00:00:00+00:00","2010-10-29T00:00:00+00:00","2010-11-01T00:00:00+00:00","2010-11-02T00:00:00+00:00","2010-11-03T00:00:00+00:00","2010-11-04T00:00:00+00:00","2010-11-05T00:00:00+00:00","2010-11-08T00:00:00+00:00","2010-11-09T00:00:00+00:00","2010-11-10T00:00:00+00:00","2010-11-11T00:00:00+00:00","2010-11-12T00:00:00+00:00","2010-11-15T00:00:00+00:00","2010-11-16T00:00:00+00:00","2010-11-17T00:00:00+00:00","2010-11-18T00:00:00+00:00","2010-11-19T00:00:00+00:00","2010-11-22T00:00:00+00:00","2010-11-23T00:00:00+00:00","2010-11-24T00:00:00+00:00","2010-11-26T00:00:00+00:00","2010-11-29T00:00:00+00:00","2010-11-30T00:00:00+00:00","2010-12-01T00:00:00+00:00","2010-12-02T00:00:00+00:00","2010-12-03T00:00:00+00:00","2010-12-06T00:00:00+00:00","2010-12-07T00:00:00+00:00","2010-12-08T00:00:00+00:00","2010-12-09T00:00:00+00:00","2010-12-10T00:00:00+00:00","2010-12-13T00:00:00+00:00","2010-12-14T00:00:00+00:00","2010-12-15T00:00:00+00:00","2010-12-16T00:00:00+00:00","2010-12-17T00:00:00+00:00","2010-12-20T00:00:00+00:00","2010-12-21T00:00:00+00:00","2010-12-22T00:00:00+00:00","2010-12-23T00:00:00+00:00","2010-12-27T00:00:00+00:00","2010-12-28T00:00:00+00:00","2010-12-29T00:00:00+00:00","2010-12-30T00:00:00+00:00","2010-12-31T00:00:00+00:00","2011-01-03T00:00:00+00:00","2011-01-04T00:00:00+00:00","2011-01-05T00:00:00+00:00","2011-01-06T00:00:00+00:00","2011-01-07T00:00:00+00:00","2011-01-10T00:00:00+00:00","2011-01-11T00:00:00+00:00","2011-01-12T00:00:00+00:00","2011-01-13T00:00:00+00:00","2011-01-14T00:00:00+00:00","2011-01-18T00:00:00+00:00","2011-01-19T00:00:00+00:00","2011-01-20T00:00:00+00:00","2011-01-21T00:00:00+00:00","2011-01-24T00:00:00+00:00","2011-01-25T00:00:00+00:00","2011-01-26T00:00:00+00:00","2011-01-27T00:00:00+00:00","2011-01-28T00:00:00+00:00","2011-01-31T00:00:00+00:00","2011-02-01T00:00:00+00:00","2011-02-02T00:00:00+00:00","2011-02-03T00:00:00+00:00","2011-02-04T00:00:00+00:00","2011-02-07T00:00:00+00:00","2011-02-08T00:00:00+00:00","2011-02-09T00:00:00+00:00","2011-02-10T00:00:00+00:00","2011-02-11T00:00:00+00:00","2011-02-14T00:00:00+00:00","2011-02-15T00:00:00+00:00","2011-02-16T00:00:00+00:00","2011-02-17T00:00:00+00:00","2011-02-18T00:00:00+00:00","2011-02-22T00:00:00+00:00","2011-02-23T00:00:00+00:00","2011-02-24T00:00:00+00:00","2011-02-25T00:00:00+00:00","2011-02-28T00:00:00+00:00","2011-03-01T00:00:00+00:00","2011-03-02T00:00:00+00:00","2011-03-03T00:00:00+00:00","2011-03-04T00:00:00+00:00","2011-03-07T00:00:00+00:00","2011-03-08T00:00:00+00:00","2011-03-09T00:00:00+00:00","2011-03-10T00:00:00+00:00","2011-03-11T00:00:00+00:00","2011-03-14T00:00:00+00:00","2011-03-15T00:00:00+00:00","2011-03-16T00:00:00+00:00","2011-03-17T00:00:00+00:00","2011-03-18T00:00:00+00:00","2011-03-21T00:00:00+00:00","2011-03-22T00:00:00+00:00","2011-03-23T00:00:00+00:00","2011-03-24T00:00:00+00:00","2011-03-25T00:00:00+00:00","2011-03-28T00:00:00+00:00","2011-03-29T00:00:00+00:00","2011-03-30T00:00:00+00:00","2011-03-31T00:00:00+00:00","2011-04-01T00:00:00+00:00","2011-04-04T00:00:00+00:00","2011-04-05T00:00:00+00:00","2011-04-06T00:00:00+00:00","2011-04-07T00:00:00+00:00","2011-04-08T00:00:00+00:00","2011-04-11T00:00:00+00:00","2011-04-12T00:00:00+00:00","2011-04-13T00:00:00+00:00","2011-04-14T00:00:00+00:00","2011-04-15T00:00:00+00:00","2011-04-18T00:00:00+00:00","2011-04-19T00:00:00+00:00","2011-04-20T00:00:00+00:00","2011-04-21T00:00:00+00:00","2011-04-25T00:00:00+00:00","2011-04-26T00:00:00+00:00","2011-04-27T00:00:00+00:00","2011-04-28T00:00:00+00:00","2011-04-29T00:00:00+00:00","2011-05-02T00:00:00+00:00","2011-05-03T00:00:00+00:00","2011-05-04T00:00:00+00:00","2011-05-05T00:00:00+00:00","2011-05-06T00:00:00+00:00","2011-05-09T00:00:00+00:00","2011-05-10T00:00:00+00:00","2011-05-11T00:00:00+00:00","2011-05-12T00:00:00+00:00","2011-05-13T00:00:00+00:00","2011-05-16T00:00:00+00:00","2011-05-17T00:00:00+00:00","2011-05-18T00:00:00+00:00","2011-05-19T00:00:00+00:00","2011-05-20T00:00:00+00:00","2011-05-23T00:00:00+00:00","2011-05-24T00:00:00+00:00","2011-05-25T00:00:00+00:00","2011-05-26T00:00:00+00:00","2011-05-27T00:00:00+00:00","2011-05-31T00:00:00+00:00","2011-06-01T00:00:00+00:00","2011-06-02T00:00:00+00:00","2011-06-03T00:00:00+00:00","2011-06-06T00:00:00+00:00","2011-06-07T00:00:00+00:00","2011-06-08T00:00:00+00:00","2011-06-09T00:00:00+00:00","2011-06-10T00:00:00+00:00","2011-06-13T00:00:00+00:00","2011-06-14T00:00:00+00:00","2011-06-15T00:00:00+00:00","2011-06-16T00:00:00+00:00","2011-06-17T00:00:00+00:00","2011-06-20T00:00:00+00:00","2011-06-21T00:00:00+00:00","2011-06-22T00:00:00+00:00","2011-06-23T00:00:00+00:00","2011-06-24T00:00:00+00:00","2011-06-27T00:00:00+00:00","2011-06-28T00:00:00+00:00","2011-06-29T00:00:00+00:00","2011-06-30T00:00:00+00:00","2011-07-01T00:00:00+00:00","2011-07-05T00:00:00+00:00","2011-07-06T00:00:00+00:00","2011-07-07T00:00:00+00:00","2011-07-08T00:00:00+00:00","2011-07-11T00:00:00+00:00","2011-07-12T00:00:00+00:00","2011-07-13T00:00:00+00:00","2011-07-14T00:00:00+00:00","2011-07-15T00:00:00+00:00","2011-07-18T00:00:00+00:00","2011-07-19T00:00:00+00:00","2011-07-20T00:00:00+00:00","2011-07-21T00:00:00+00:00","2011-07-22T00:00:00+00:00","2011-07-25T00:00:00+00:00","2011-07-26T00:00:00+00:00","2011-07-27T00:00:00+00:00","2011-07-28T00:00:00+00:00","2011-07-29T00:00:00+00:00","2011-08-01T00:00:00+00:00","2011-08-02T00:00:00+00:00","2011-08-03T00:00:00+00:00","2011-08-04T00:00:00+00:00","2011-08-05T00:00:00+00:00","2011-08-08T00:00:00+00:00","2011-08-09T00:00:00+00:00","2011-08-10T00:00:00+00:00","2011-08-11T00:00:00+00:00","2011-08-12T00:00:00+00:00","2011-08-15T00:00:00+00:00","2011-08-16T00:00:00+00:00","2011-08-17T00:00:00+00:00","2011-08-18T00:00:00+00:00","2011-08-19T00:00:00+00:00","2011-08-22T00:00:00+00:00","2011-08-23T00:00:00+00:00","2011-08-24T00:00:00+00:00","2011-08-25T00:00:00+00:00","2011-08-26T00:00:00+00:00","2011-08-29T00:00:00+00:00","2011-08-30T00:00:00+00:00","2011-08-31T00:00:00+00:00","2011-09-01T00:00:00+00:00","2011-09-02T00:00:00+00:00","2011-09-06T00:00:00+00:00","2011-09-07T00:00:00+00:00","2011-09-08T00:00:00+00:00","2011-09-09T00:00:00+00:00","2011-09-12T00:00:00+00:00","2011-09-13T00:00:00+00:00","2011-09-14T00:00:00+00:00","2011-09-15T00:00:00+00:00","2011-09-16T00:00:00+00:00","2011-09-19T00:00:00+00:00","2011-09-20T00:00:00+00:00","2011-09-21T00:00:00+00:00","2011-09-22T00:00:00+00:00","2011-09-23T00:00:00+00:00","2011-09-26T00:00:00+00:00","2011-09-27T00:00:00+00:00","2011-09-28T00:00:00+00:00","2011-09-29T00:00:00+00:00","2011-09-30T00:00:00+00:00","2011-10-03T00:00:00+00:00","2011-10-04T00:00:00+00:00","2011-10-05T00:00:00+00:00","2011-10-06T00:00:00+00:00","2011-10-07T00:00:00+00:00","2011-10-10T00:00:00+00:00","2011-10-11T00:00:00+00:00","2011-10-12T00:00:00+00:00","2011-10-13T00:00:00+00:00","2011-10-14T00:00:00+00:00","2011-10-17T00:00:00+00:00","2011-10-18T00:00:00+00:00","2011-10-19T00:00:00+00:00","2011-10-20T00:00:00+00:00","2011-10-21T00:00:00+00:00","2011-10-24T00:00:00+00:00","2011-10-25T00:00:00+00:00","2011-10-26T00:00:00+00:00","2011-10-27T00:00:00+00:00","2011-10-28T00:00:00+00:00","2011-10-31T00:00:00+00:00","2011-11-01T00:00:00+00:00","2011-11-02T00:00:00+00:00","2011-11-03T00:00:00+00:00","2011-11-04T00:00:00+00:00","2011-11-07T00:00:00+00:00","2011-11-08T00:00:00+00:00","2011-11-09T00:00:00+00:00","2011-11-10T00:00:00+00:00","2011-11-11T00:00:00+00:00","2011-11-14T00:00:00+00:00","2011-11-15T00:00:00+00:00","2011-11-16T00:00:00+00:00","2011-11-17T00:00:00+00:00","2011-11-18T00:00:00+00:00","2011-11-21T00:00:00+00:00","2011-11-22T00:00:00+00:00","2011-11-23T00:00:00+00:00","2011-11-25T00:00:00+00:00","2011-11-28T00:00:00+00:00","2011-11-29T00:00:00+00:00","2011-11-30T00:00:00+00:00","2011-12-01T00:00:00+00:00","2011-12-02T00:00:00+00:00","2011-12-05T00:00:00+00:00","2011-12-06T00:00:00+00:00","2011-12-07T00:00:00+00:00","2011-12-08T00:00:00+00:00","2011-12-09T00:00:00+00:00","2011-12-12T00:00:00+00:00","2011-12-13T00:00:00+00:00","2011-12-14T00:00:00+00:00","2011-12-15T00:00:00+00:00","2011-12-16T00:00:00+00:00","2011-12-19T00:00:00+00:00","2011-12-20T00:00:00+00:00","2011-12-21T00:00:00+00:00","2011-12-22T00:00:00+00:00","2011-12-23T00:00:00+00:00","2011-12-27T00:00:00+00:00","2011-12-28T00:00:00+00:00","2011-12-29T00:00:00+00:00","2011-12-30T00:00:00+00:00","2012-01-03T00:00:00+00:00","2012-01-04T00:00:00+00:00","2012-01-05T00:00:00+00:00","2012-01-06T00:00:00+00:00","2012-01-09T00:00:00+00:00","2012-01-10T00:00:00+00:00","2012-01-11T00:00:00+00:00","2012-01-12T00:00:00+00:00","2012-01-13T00:00:00+00:00","2012-01-17T00:00:00+00:00","2012-01-18T00:00:00+00:00","2012-01-19T00:00:00+00:00","2012-01-20T00:00:00+00:00","2012-01-23T00:00:00+00:00","2012-01-24T00:00:00+00:00","2012-01-25T00:00:00+00:00","2012-01-26T00:00:00+00:00","2012-01-27T00:00:00+00:00","2012-01-30T00:00:00+00:00","2012-01-31T00:00:00+00:00","2012-02-01T00:00:00+00:00","2012-02-02T00:00:00+00:00","2012-02-03T00:00:00+00:00","2012-02-06T00:00:00+00:00","2012-02-07T00:00:00+00:00","2012-02-08T00:00:00+00:00","2012-02-09T00:00:00+00:00","2012-02-10T00:00:00+00:00","2012-02-13T00:00:00+00:00","2012-02-14T00:00:00+00:00","2012-02-15T00:00:00+00:00","2012-02-16T00:00:00+00:00","2012-02-17T00:00:00+00:00","2012-02-21T00:00:00+00:00","2012-02-22T00:00:00+00:00","2012-02-23T00:00:00+00:00","2012-02-24T00:00:00+00:00","2012-02-27T00:00:00+00:00","2012-02-28T00:00:00+00:00","2012-02-29T00:00:00+00:00","2012-03-01T00:00:00+00:00","2012-03-02T00:00:00+00:00","2012-03-05T00:00:00+00:00","2012-03-06T00:00:00+00:00","2012-03-07T00:00:00+00:00","2012-03-08T00:00:00+00:00","2012-03-09T00:00:00+00:00","2012-03-12T00:00:00+00:00","2012-03-13T00:00:00+00:00","2012-03-14T00:00:00+00:00","2012-03-15T00:00:00+00:00","2012-03-16T00:00:00+00:00","2012-03-19T00:00:00+00:00","2012-03-20T00:00:00+00:00","2012-03-21T00:00:00+00:00","2012-03-22T00:00:00+00:00","2012-03-23T00:00:00+00:00","2012-03-26T00:00:00+00:00","2012-03-27T00:00:00+00:00","2012-03-28T00:00:00+00:00","2012-03-29T00:00:00+00:00","2012-03-30T00:00:00+00:00","2012-04-02T00:00:00+00:00","2012-04-03T00:00:00+00:00","2012-04-04T00:00:00+00:00","2012-04-05T00:00:00+00:00","2012-04-09T00:00:00+00:00","2012-04-10T00:00:00+00:00","2012-04-11T00:00:00+00:00","2012-04-12T00:00:00+00:00","2012-04-13T00:00:00+00:00","2012-04-16T00:00:00+00:00","2012-04-17T00:00:00+00:00","2012-04-18T00:00:00+00:00","2012-04-19T00:00:00+00:00","2012-04-20T00:00:00+00:00","2012-04-23T00:00:00+00:00","2012-04-24T00:00:00+00:00","2012-04-25T00:00:00+00:00","2012-04-26T00:00:00+00:00","2012-04-27T00:00:00+00:00","2012-04-30T00:00:00+00:00","2012-05-01T00:00:00+00:00","2012-05-02T00:00:00+00:00","2012-05-03T00:00:00+00:00","2012-05-04T00:00:00+00:00","2012-05-07T00:00:00+00:00","2012-05-08T00:00:00+00:00","2012-05-09T00:00:00+00:00","2012-05-10T00:00:00+00:00","2012-05-11T00:00:00+00:00","2012-05-14T00:00:00+00:00","2012-05-15T00:00:00+00:00","2012-05-16T00:00:00+00:00","2012-05-17T00:00:00+00:00","2012-05-18T00:00:00+00:00","2012-05-21T00:00:00+00:00","2012-05-22T00:00:00+00:00","2012-05-23T00:00:00+00:00","2012-05-24T00:00:00+00:00","2012-05-25T00:00:00+00:00","2012-05-29T00:00:00+00:00","2012-05-30T00:00:00+00:00","2012-05-31T00:00:00+00:00","2012-06-01T00:00:00+00:00","2012-06-04T00:00:00+00:00","2012-06-05T00:00:00+00:00","2012-06-06T00:00:00+00:00","2012-06-07T00:00:00+00:00","2012-06-08T00:00:00+00:00","2012-06-11T00:00:00+00:00","2012-06-12T00:00:00+00:00","2012-06-13T00:00:00+00:00","2012-06-14T00:00:00+00:00","2012-06-15T00:00:00+00:00","2012-06-18T00:00:00+00:00","2012-06-19T00:00:00+00:00","2012-06-20T00:00:00+00:00","2012-06-21T00:00:00+00:00","2012-06-22T00:00:00+00:00","2012-06-25T00:00:00+00:00","2012-06-26T00:00:00+00:00","2012-06-27T00:00:00+00:00","2012-06-28T00:00:00+00:00","2012-06-29T00:00:00+00:00","2012-07-02T00:00:00+00:00","2012-07-03T00:00:00+00:00","2012-07-05T00:00:00+00:00","2012-07-06T00:00:00+00:00","2012-07-09T00:00:00+00:00","2012-07-10T00:00:00+00:00","2012-07-11T00:00:00+00:00","2012-07-12T00:00:00+00:00","2012-07-13T00:00:00+00:00","2012-07-16T00:00:00+00:00","2012-07-17T00:00:00+00:00","2012-07-18T00:00:00+00:00","2012-07-19T00:00:00+00:00","2012-07-20T00:00:00+00:00","2012-07-23T00:00:00+00:00","2012-07-24T00:00:00+00:00","2012-07-25T00:00:00+00:00","2012-07-26T00:00:00+00:00","2012-07-27T00:00:00+00:00","2012-07-30T00:00:00+00:00","2012-07-31T00:00:00+00:00","2012-08-01T00:00:00+00:00","2012-08-02T00:00:00+00:00","2012-08-03T00:00:00+00:00","2012-08-06T00:00:00+00:00","2012-08-07T00:00:00+00:00","2012-08-08T00:00:00+00:00","2012-08-09T00:00:00+00:00","2012-08-10T00:00:00+00:00","2012-08-13T00:00:00+00:00","2012-08-14T00:00:00+00:00","2012-08-15T00:00:00+00:00","2012-08-16T00:00:00+00:00","2012-08-17T00:00:00+00:00","2012-08-20T00:00:00+00:00","2012-08-21T00:00:00+00:00","2012-08-22T00:00:00+00:00","2012-08-23T00:00:00+00:00","2012-08-24T00:00:00+00:00","2012-08-27T00:00:00+00:00","2012-08-28T00:00:00+00:00","2012-08-29T00:00:00+00:00","2012-08-30T00:00:00+00:00","2012-08-31T00:00:00+00:00","2012-09-04T00:00:00+00:00","2012-09-05T00:00:00+00:00","2012-09-06T00:00:00+00:00","2012-09-07T00:00:00+00:00","2012-09-10T00:00:00+00:00","2012-09-11T00:00:00+00:00","2012-09-12T00:00:00+00:00","2012-09-13T00:00:00+00:00","2012-09-14T00:00:00+00:00","2012-09-17T00:00:00+00:00","2012-09-18T00:00:00+00:00","2012-09-19T00:00:00+00:00","2012-09-20T00:00:00+00:00","2012-09-21T00:00:00+00:00","2012-09-24T00:00:00+00:00","2012-09-25T00:00:00+00:00","2012-09-26T00:00:00+00:00","2012-09-27T00:00:00+00:00","2012-09-28T00:00:00+00:00","2012-10-01T00:00:00+00:00","2012-10-02T00:00:00+00:00","2012-10-03T00:00:00+00:00","2012-10-04T00:00:00+00:00","2012-10-05T00:00:00+00:00","2012-10-08T00:00:00+00:00","2012-10-09T00:00:00+00:00","2012-10-10T00:00:00+00:00","2012-10-11T00:00:00+00:00","2012-10-12T00:00:00+00:00","2012-10-15T00:00:00+00:00","2012-10-16T00:00:00+00:00","2012-10-17T00:00:00+00:00","2012-10-18T00:00:00+00:00","2012-10-19T00:00:00+00:00","2012-10-22T00:00:00+00:00","2012-10-23T00:00:00+00:00","2012-10-24T00:00:00+00:00","2012-10-25T00:00:00+00:00","2012-10-26T00:00:00+00:00","2012-10-31T00:00:00+00:00","2012-11-01T00:00:00+00:00","2012-11-02T00:00:00+00:00","2012-11-05T00:00:00+00:00","2012-11-06T00:00:00+00:00","2012-11-07T00:00:00+00:00","2012-11-08T00:00:00+00:00","2012-11-09T00:00:00+00:00","2012-11-12T00:00:00+00:00","2012-11-13T00:00:00+00:00","2012-11-14T00:00:00+00:00","2012-11-15T00:00:00+00:00","2012-11-16T00:00:00+00:00","2012-11-19T00:00:00+00:00","2012-11-20T00:00:00+00:00","2012-11-21T00:00:00+00:00","2012-11-23T00:00:00+00:00","2012-11-26T00:00:00+00:00","2012-11-27T00:00:00+00:00","2012-11-28T00:00:00+00:00","2012-11-29T00:00:00+00:00","2012-11-30T00:00:00+00:00","2012-12-03T00:00:00+00:00","2012-12-04T00:00:00+00:00","2012-12-05T00:00:00+00:00","2012-12-06T00:00:00+00:00","2012-12-07T00:00:00+00:00","2012-12-10T00:00:00+00:00","2012-12-11T00:00:00+00:00","2012-12-12T00:00:00+00:00","2012-12-13T00:00:00+00:00","2012-12-14T00:00:00+00:00","2012-12-17T00:00:00+00:00","2012-12-18T00:00:00+00:00","2012-12-19T00:00:00+00:00","2012-12-20T00:00:00+00:00","2012-12-21T00:00:00+00:00","2012-12-24T00:00:00+00:00","2012-12-26T00:00:00+00:00","2012-12-27T00:00:00+00:00","2012-12-28T00:00:00+00:00","2012-12-31T00:00:00+00:00","2013-01-02T00:00:00+00:00","2013-01-03T00:00:00+00:00","2013-01-04T00:00:00+00:00","2013-01-07T00:00:00+00:00","2013-01-08T00:00:00+00:00","2013-01-09T00:00:00+00:00","2013-01-10T00:00:00+00:00","2013-01-11T00:00:00+00:00","2013-01-14T00:00:00+00:00","2013-01-15T00:00:00+00:00","2013-01-16T00:00:00+00:00","2013-01-17T00:00:00+00:00","2013-01-18T00:00:00+00:00","2013-01-22T00:00:00+00:00","2013-01-23T00:00:00+00:00","2013-01-24T00:00:00+00:00","2013-01-25T00:00:00+00:00","2013-01-28T00:00:00+00:00","2013-01-29T00:00:00+00:00","2013-01-30T00:00:00+00:00","2013-01-31T00:00:00+00:00","2013-02-01T00:00:00+00:00","2013-02-04T00:00:00+00:00","2013-02-05T00:00:00+00:00","2013-02-06T00:00:00+00:00","2013-02-07T00:00:00+00:00","2013-02-08T00:00:00+00:00","2013-02-11T00:00:00+00:00","2013-02-12T00:00:00+00:00","2013-02-13T00:00:00+00:00","2013-02-14T00:00:00+00:00","2013-02-15T00:00:00+00:00","2013-02-19T00:00:00+00:00","2013-02-20T00:00:00+00:00","2013-02-21T00:00:00+00:00","2013-02-22T00:00:00+00:00","2013-02-25T00:00:00+00:00","2013-02-26T00:00:00+00:00","2013-02-27T00:00:00+00:00","2013-02-28T00:00:00+00:00","2013-03-01T00:00:00+00:00","2013-03-04T00:00:00+00:00","2013-03-05T00:00:00+00:00","2013-03-06T00:00:00+00:00","2013-03-07T00:00:00+00:00","2013-03-08T00:00:00+00:00","2013-03-11T00:00:00+00:00","2013-03-12T00:00:00+00:00","2013-03-13T00:00:00+00:00","2013-03-14T00:00:00+00:00","2013-03-15T00:00:00+00:00","2013-03-18T00:00:00+00:00","2013-03-19T00:00:00+00:00","2013-03-20T00:00:00+00:00","2013-03-21T00:00:00+00:00","2013-03-22T00:00:00+00:00","2013-03-25T00:00:00+00:00","2013-03-26T00:00:00+00:00","2013-03-27T00:00:00+00:00","2013-03-28T00:00:00+00:00","2013-04-01T00:00:00+00:00","2013-04-02T00:00:00+00:00","2013-04-03T00:00:00+00:00","2013-04-04T00:00:00+00:00","2013-04-05T00:00:00+00:00","2013-04-08T00:00:00+00:00","2013-04-09T00:00:00+00:00","2013-04-10T00:00:00+00:00","2013-04-11T00:00:00+00:00","2013-04-12T00:00:00+00:00","2013-04-15T00:00:00+00:00","2013-04-16T00:00:00+00:00","2013-04-17T00:00:00+00:00","2013-04-18T00:00:00+00:00","2013-04-19T00:00:00+00:00","2013-04-22T00:00:00+00:00","2013-04-23T00:00:00+00:00","2013-04-24T00:00:00+00:00","2013-04-25T00:00:00+00:00","2013-04-26T00:00:00+00:00","2013-04-29T00:00:00+00:00","2013-04-30T00:00:00+00:00","2013-05-01T00:00:00+00:00","2013-05-02T00:00:00+00:00","2013-05-03T00:00:00+00:00","2013-05-06T00:00:00+00:00","2013-05-07T00:00:00+00:00","2013-05-08T00:00:00+00:00","2013-05-09T00:00:00+00:00","2013-05-10T00:00:00+00:00","2013-05-13T00:00:00+00:00","2013-05-14T00:00:00+00:00","2013-05-15T00:00:00+00:00","2013-05-16T00:00:00+00:00","2013-05-17T00:00:00+00:00","2013-05-20T00:00:00+00:00","2013-05-21T00:00:00+00:00","2013-05-22T00:00:00+00:00","2013-05-23T00:00:00+00:00","2013-05-24T00:00:00+00:00","2013-05-28T00:00:00+00:00","2013-05-29T00:00:00+00:00","2013-05-30T00:00:00+00:00","2013-05-31T00:00:00+00:00","2013-06-03T00:00:00+00:00","2013-06-04T00:00:00+00:00","2013-06-05T00:00:00+00:00","2013-06-06T00:00:00+00:00","2013-06-07T00:00:00+00:00","2013-06-10T00:00:00+00:00","2013-06-11T00:00:00+00:00","2013-06-12T00:00:00+00:00","2013-06-13T00:00:00+00:00","2013-06-14T00:00:00+00:00","2013-06-17T00:00:00+00:00","2013-06-18T00:00:00+00:00","2013-06-19T00:00:00+00:00","2013-06-20T00:00:00+00:00","2013-06-21T00:00:00+00:00","2013-06-24T00:00:00+00:00","2013-06-25T00:00:00+00:00","2013-06-26T00:00:00+00:00","2013-06-27T00:00:00+00:00","2013-06-28T00:00:00+00:00","2013-07-01T00:00:00+00:00","2013-07-02T00:00:00+00:00","2013-07-03T00:00:00+00:00","2013-07-05T00:00:00+00:00","2013-07-08T00:00:00+00:00","2013-07-09T00:00:00+00:00","2013-07-10T00:00:00+00:00","2013-07-11T00:00:00+00:00","2013-07-12T00:00:00+00:00","2013-07-15T00:00:00+00:00","2013-07-16T00:00:00+00:00","2013-07-17T00:00:00+00:00","2013-07-18T00:00:00+00:00","2013-07-19T00:00:00+00:00","2013-07-22T00:00:00+00:00","2013-07-23T00:00:00+00:00","2013-07-24T00:00:00+00:00","2013-07-25T00:00:00+00:00","2013-07-26T00:00:00+00:00","2013-07-29T00:00:00+00:00","2013-07-30T00:00:00+00:00","2013-07-31T00:00:00+00:00","2013-08-01T00:00:00+00:00","2013-08-02T00:00:00+00:00","2013-08-05T00:00:00+00:00","2013-08-06T00:00:00+00:00","2013-08-07T00:00:00+00:00","2013-08-08T00:00:00+00:00","2013-08-09T00:00:00+00:00","2013-08-12T00:00:00+00:00","2013-08-13T00:00:00+00:00","2013-08-14T00:00:00+00:00","2013-08-15T00:00:00+00:00","2013-08-16T00:00:00+00:00","2013-08-19T00:00:00+00:00","2013-08-20T00:00:00+00:00","2013-08-21T00:00:00+00:00","2013-08-22T00:00:00+00:00","2013-08-23T00:00:00+00:00","2013-08-26T00:00:00+00:00","2013-08-27T00:00:00+00:00","2013-08-28T00:00:00+00:00","2013-08-29T00:00:00+00:00","2013-08-30T00:00:00+00:00","2013-09-03T00:00:00+00:00","2013-09-04T00:00:00+00:00","2013-09-05T00:00:00+00:00","2013-09-06T00:00:00+00:00","2013-09-09T00:00:00+00:00","2013-09-10T00:00:00+00:00","2013-09-11T00:00:00+00:00","2013-09-12T00:00:00+00:00","2013-09-13T00:00:00+00:00","2013-09-16T00:00:00+00:00","2013-09-17T00:00:00+00:00","2013-09-18T00:00:00+00:00","2013-09-19T00:00:00+00:00","2013-09-20T00:00:00+00:00","2013-09-23T00:00:00+00:00","2013-09-24T00:00:00+00:00","2013-09-25T00:00:00+00:00","2013-09-26T00:00:00+00:00","2013-09-27T00:00:00+00:00","2013-09-30T00:00:00+00:00","2013-10-01T00:00:00+00:00","2013-10-02T00:00:00+00:00","2013-10-03T00:00:00+00:00","2013-10-04T00:00:00+00:00","2013-10-07T00:00:00+00:00","2013-10-08T00:00:00+00:00","2013-10-09T00:00:00+00:00","2013-10-10T00:00:00+00:00","2013-10-11T00:00:00+00:00","2013-10-14T00:00:00+00:00","2013-10-15T00:00:00+00:00","2013-10-16T00:00:00+00:00","2013-10-17T00:00:00+00:00","2013-10-18T00:00:00+00:00","2013-10-21T00:00:00+00:00","2013-10-22T00:00:00+00:00","2013-10-23T00:00:00+00:00","2013-10-24T00:00:00+00:00","2013-10-25T00:00:00+00:00","2013-10-28T00:00:00+00:00","2013-10-29T00:00:00+00:00","2013-10-30T00:00:00+00:00","2013-10-31T00:00:00+00:00","2013-11-01T00:00:00+00:00","2013-11-04T00:00:00+00:00","2013-11-05T00:00:00+00:00","2013-11-06T00:00:00+00:00","2013-11-07T00:00:00+00:00","2013-11-08T00:00:00+00:00","2013-11-11T00:00:00+00:00","2013-11-12T00:00:00+00:00","2013-11-13T00:00:00+00:00","2013-11-14T00:00:00+00:00","2013-11-15T00:00:00+00:00","2013-11-18T00:00:00+00:00","2013-11-19T00:00:00+00:00","2013-11-20T00:00:00+00:00","2013-11-21T00:00:00+00:00","2013-11-22T00:00:00+00:00","2013-11-25T00:00:00+00:00","2013-11-26T00:00:00+00:00","2013-11-27T00:00:00+00:00","2013-11-29T00:00:00+00:00","2013-12-02T00:00:00+00:00","2013-12-03T00:00:00+00:00","2013-12-04T00:00:00+00:00","2013-12-05T00:00:00+00:00","2013-12-06T00:00:00+00:00","2013-12-09T00:00:00+00:00","2013-12-10T00:00:00+00:00","2013-12-11T00:00:00+00:00","2013-12-12T00:00:00+00:00","2013-12-13T00:00:00+00:00","2013-12-16T00:00:00+00:00","2013-12-17T00:00:00+00:00","2013-12-18T00:00:00+00:00","2013-12-19T00:00:00+00:00","2013-12-20T00:00:00+00:00","2013-12-23T00:00:00+00:00","2013-12-24T00:00:00+00:00","2013-12-26T00:00:00+00:00","2013-12-27T00:00:00+00:00","2013-12-30T00:00:00+00:00","2013-12-31T00:00:00+00:00","2014-01-02T00:00:00+00:00","2014-01-03T00:00:00+00:00","2014-01-06T00:00:00+00:00","2014-01-07T00:00:00+00:00","2014-01-08T00:00:00+00:00","2014-01-09T00:00:00+00:00","2014-01-10T00:00:00+00:00","2014-01-13T00:00:00+00:00","2014-01-14T00:00:00+00:00","2014-01-15T00:00:00+00:00","2014-01-16T00:00:00+00:00","2014-01-17T00:00:00+00:00","2014-01-21T00:00:00+00:00","2014-01-22T00:00:00+00:00","2014-01-23T00:00:00+00:00","2014-01-24T00:00:00+00:00","2014-01-27T00:00:00+00:00","2014-01-28T00:00:00+00:00","2014-01-29T00:00:00+00:00","2014-01-30T00:00:00+00:00","2014-01-31T00:00:00+00:00","2014-02-03T00:00:00+00:00","2014-02-04T00:00:00+00:00","2014-02-05T00:00:00+00:00","2014-02-06T00:00:00+00:00","2014-02-07T00:00:00+00:00","2014-02-10T00:00:00+00:00","2014-02-11T00:00:00+00:00","2014-02-12T00:00:00+00:00","2014-02-13T00:00:00+00:00","2014-02-14T00:00:00+00:00","2014-02-18T00:00:00+00:00","2014-02-19T00:00:00+00:00","2014-02-20T00:00:00+00:00","2014-02-21T00:00:00+00:00","2014-02-24T00:00:00+00:00","2014-02-25T00:00:00+00:00","2014-02-26T00:00:00+00:00","2014-02-27T00:00:00+00:00","2014-02-28T00:00:00+00:00","2014-03-03T00:00:00+00:00","2014-03-04T00:00:00+00:00","2014-03-05T00:00:00+00:00","2014-03-06T00:00:00+00:00","2014-03-07T00:00:00+00:00","2014-03-10T00:00:00+00:00","2014-03-11T00:00:00+00:00","2014-03-12T00:00:00+00:00","2014-03-13T00:00:00+00:00","2014-03-14T00:00:00+00:00","2014-03-17T00:00:00+00:00","2014-03-18T00:00:00+00:00","2014-03-19T00:00:00+00:00","2014-03-20T00:00:00+00:00","2014-03-21T00:00:00+00:00","2014-03-24T00:00:00+00:00","2014-03-25T00:00:00+00:00","2014-03-26T00:00:00+00:00","2014-03-27T00:00:00+00:00","2014-03-28T00:00:00+00:00","2014-03-31T00:00:00+00:00","2014-04-01T00:00:00+00:00","2014-04-02T00:00:00+00:00","2014-04-03T00:00:00+00:00","2014-04-04T00:00:00+00:00","2014-04-07T00:00:00+00:00","2014-04-08T00:00:00+00:00","2014-04-09T00:00:00+00:00","2014-04-10T00:00:00+00:00","2014-04-11T00:00:00+00:00","2014-04-14T00:00:00+00:00","2014-04-15T00:00:00+00:00","2014-04-16T00:00:00+00:00","2014-04-17T00:00:00+00:00","2014-04-21T00:00:00+00:00","2014-04-22T00:00:00+00:00","2014-04-23T00:00:00+00:00","2014-04-24T00:00:00+00:00","2014-04-25T00:00:00+00:00","2014-04-28T00:00:00+00:00","2014-04-29T00:00:00+00:00","2014-04-30T00:00:00+00:00","2014-05-01T00:00:00+00:00","2014-05-02T00:00:00+00:00","2014-05-05T00:00:00+00:00","2014-05-06T00:00:00+00:00","2014-05-07T00:00:00+00:00","2014-05-08T00:00:00+00:00","2014-05-09T00:00:00+00:00","2014-05-12T00:00:00+00:00","2014-05-13T00:00:00+00:00","2014-05-14T00:00:00+00:00","2014-05-15T00:00:00+00:00","2014-05-16T00:00:00+00:00","2014-05-19T00:00:00+00:00","2014-05-20T00:00:00+00:00","2014-05-21T00:00:00+00:00","2014-05-22T00:00:00+00:00","2014-05-23T00:00:00+00:00","2014-05-27T00:00:00+00:00","2014-05-28T00:00:00+00:00","2014-05-29T00:00:00+00:00","2014-05-30T00:00:00+00:00","2014-06-02T00:00:00+00:00","2014-06-03T00:00:00+00:00","2014-06-04T00:00:00+00:00","2014-06-05T00:00:00+00:00","2014-06-06T00:00:00+00:00","2014-06-09T00:00:00+00:00","2014-06-10T00:00:00+00:00","2014-06-11T00:00:00+00:00","2014-06-12T00:00:00+00:00","2014-06-13T00:00:00+00:00","2014-06-16T00:00:00+00:00","2014-06-17T00:00:00+00:00","2014-06-18T00:00:00+00:00","2014-06-19T00:00:00+00:00","2014-06-20T00:00:00+00:00","2014-06-23T00:00:00+00:00","2014-06-24T00:00:00+00:00","2014-06-25T00:00:00+00:00","2014-06-26T00:00:00+00:00","2014-06-27T00:00:00+00:00","2014-06-30T00:00:00+00:00","2014-07-01T00:00:00+00:00","2014-07-02T00:00:00+00:00","2014-07-03T00:00:00+00:00","2014-07-07T00:00:00+00:00","2014-07-08T00:00:00+00:00","2014-07-09T00:00:00+00:00","2014-07-10T00:00:00+00:00","2014-07-11T00:00:00+00:00","2014-07-14T00:00:00+00:00","2014-07-15T00:00:00+00:00","2014-07-16T00:00:00+00:00","2014-07-17T00:00:00+00:00","2014-07-18T00:00:00+00:00","2014-07-21T00:00:00+00:00","2014-07-22T00:00:00+00:00","2014-07-23T00:00:00+00:00","2014-07-24T00:00:00+00:00","2014-07-25T00:00:00+00:00","2014-07-28T00:00:00+00:00","2014-07-29T00:00:00+00:00","2014-07-30T00:00:00+00:00","2014-07-31T00:00:00+00:00","2014-08-01T00:00:00+00:00","2014-08-04T00:00:00+00:00","2014-08-05T00:00:00+00:00","2014-08-06T00:00:00+00:00","2014-08-07T00:00:00+00:00","2014-08-08T00:00:00+00:00","2014-08-11T00:00:00+00:00","2014-08-12T00:00:00+00:00","2014-08-13T00:00:00+00:00","2014-08-14T00:00:00+00:00","2014-08-15T00:00:00+00:00","2014-08-18T00:00:00+00:00","2014-08-19T00:00:00+00:00","2014-08-20T00:00:00+00:00","2014-08-21T00:00:00+00:00","2014-08-22T00:00:00+00:00","2014-08-25T00:00:00+00:00","2014-08-26T00:00:00+00:00","2014-08-27T00:00:00+00:00","2014-08-28T00:00:00+00:00","2014-08-29T00:00:00+00:00","2014-09-02T00:00:00+00:00","2014-09-03T00:00:00+00:00","2014-09-04T00:00:00+00:00","2014-09-05T00:00:00+00:00","2014-09-08T00:00:00+00:00","2014-09-09T00:00:00+00:00","2014-09-10T00:00:00+00:00","2014-09-11T00:00:00+00:00","2014-09-12T00:00:00+00:00","2014-09-15T00:00:00+00:00","2014-09-16T00:00:00+00:00","2014-09-17T00:00:00+00:00","2014-09-18T00:00:00+00:00","2014-09-19T00:00:00+00:00","2014-09-22T00:00:00+00:00","2014-09-23T00:00:00+00:00","2014-09-24T00:00:00+00:00","2014-09-25T00:00:00+00:00","2014-09-26T00:00:00+00:00","2014-09-29T00:00:00+00:00","2014-09-30T00:00:00+00:00","2014-10-01T00:00:00+00:00","2014-10-02T00:00:00+00:00","2014-10-03T00:00:00+00:00","2014-10-06T00:00:00+00:00","2014-10-07T00:00:00+00:00","2014-10-08T00:00:00+00:00","2014-10-09T00:00:00+00:00","2014-10-10T00:00:00+00:00","2014-10-13T00:00:00+00:00","2014-10-14T00:00:00+00:00","2014-10-15T00:00:00+00:00","2014-10-16T00:00:00+00:00","2014-10-17T00:00:00+00:00","2014-10-20T00:00:00+00:00","2014-10-21T00:00:00+00:00","2014-10-22T00:00:00+00:00","2014-10-23T00:00:00+00:00","2014-10-24T00:00:00+00:00","2014-10-27T00:00:00+00:00","2014-10-28T00:00:00+00:00","2014-10-29T00:00:00+00:00","2014-10-30T00:00:00+00:00","2014-10-31T00:00:00+00:00","2014-11-03T00:00:00+00:00","2014-11-04T00:00:00+00:00","2014-11-05T00:00:00+00:00","2014-11-06T00:00:00+00:00","2014-11-07T00:00:00+00:00","2014-11-10T00:00:00+00:00","2014-11-11T00:00:00+00:00","2014-11-12T00:00:00+00:00","2014-11-13T00:00:00+00:00","2014-11-14T00:00:00+00:00","2014-11-17T00:00:00+00:00","2014-11-18T00:00:00+00:00","2014-11-19T00:00:00+00:00","2014-11-20T00:00:00+00:00","2014-11-21T00:00:00+00:00","2014-11-24T00:00:00+00:00","2014-11-25T00:00:00+00:00","2014-11-26T00:00:00+00:00","2014-11-28T00:00:00+00:00","2014-12-01T00:00:00+00:00","2014-12-02T00:00:00+00:00","2014-12-03T00:00:00+00:00","2014-12-04T00:00:00+00:00","2014-12-05T00:00:00+00:00","2014-12-08T00:00:00+00:00","2014-12-09T00:00:00+00:00","2014-12-10T00:00:00+00:00","2014-12-11T00:00:00+00:00","2014-12-12T00:00:00+00:00","2014-12-15T00:00:00+00:00","2014-12-16T00:00:00+00:00","2014-12-17T00:00:00+00:00","2014-12-18T00:00:00+00:00","2014-12-19T00:00:00+00:00","2014-12-22T00:00:00+00:00","2014-12-23T00:00:00+00:00","2014-12-24T00:00:00+00:00","2014-12-26T00:00:00+00:00","2014-12-29T00:00:00+00:00","2014-12-30T00:00:00+00:00","2014-12-31T00:00:00+00:00","2015-01-02T00:00:00+00:00","2015-01-05T00:00:00+00:00","2015-01-06T00:00:00+00:00","2015-01-07T00:00:00+00:00","2015-01-08T00:00:00+00:00","2015-01-09T00:00:00+00:00","2015-01-12T00:00:00+00:00","2015-01-13T00:00:00+00:00","2015-01-14T00:00:00+00:00","2015-01-15T00:00:00+00:00","2015-01-16T00:00:00+00:00","2015-01-20T00:00:00+00:00","2015-01-21T00:00:00+00:00","2015-01-22T00:00:00+00:00","2015-01-23T00:00:00+00:00","2015-01-26T00:00:00+00:00","2015-01-27T00:00:00+00:00","2015-01-28T00:00:00+00:00","2015-01-29T00:00:00+00:00","2015-01-30T00:00:00+00:00","2015-02-02T00:00:00+00:00","2015-02-03T00:00:00+00:00","2015-02-04T00:00:00+00:00","2015-02-05T00:00:00+00:00","2015-02-06T00:00:00+00:00","2015-02-09T00:00:00+00:00","2015-02-10T00:00:00+00:00","2015-02-11T00:00:00+00:00","2015-02-12T00:00:00+00:00","2015-02-13T00:00:00+00:00","2015-02-17T00:00:00+00:00","2015-02-18T00:00:00+00:00","2015-02-19T00:00:00+00:00","2015-02-20T00:00:00+00:00","2015-02-23T00:00:00+00:00","2015-02-24T00:00:00+00:00","2015-02-25T00:00:00+00:00","2015-02-26T00:00:00+00:00","2015-02-27T00:00:00+00:00","2015-03-02T00:00:00+00:00","2015-03-03T00:00:00+00:00","2015-03-04T00:00:00+00:00","2015-03-05T00:00:00+00:00","2015-03-06T00:00:00+00:00","2015-03-09T00:00:00+00:00","2015-03-10T00:00:00+00:00","2015-03-11T00:00:00+00:00","2015-03-12T00:00:00+00:00","2015-03-13T00:00:00+00:00","2015-03-16T00:00:00+00:00","2015-03-17T00:00:00+00:00","2015-03-18T00:00:00+00:00","2015-03-19T00:00:00+00:00","2015-03-20T00:00:00+00:00","2015-03-23T00:00:00+00:00","2015-03-24T00:00:00+00:00","2015-03-25T00:00:00+00:00","2015-03-26T00:00:00+00:00","2015-03-27T00:00:00+00:00","2015-03-30T00:00:00+00:00","2015-03-31T00:00:00+00:00","2015-04-01T00:00:00+00:00","2015-04-02T00:00:00+00:00","2015-04-06T00:00:00+00:00","2015-04-07T00:00:00+00:00","2015-04-08T00:00:00+00:00","2015-04-09T00:00:00+00:00","2015-04-10T00:00:00+00:00","2015-04-13T00:00:00+00:00","2015-04-14T00:00:00+00:00","2015-04-15T00:00:00+00:00","2015-04-16T00:00:00+00:00","2015-04-17T00:00:00+00:00","2015-04-20T00:00:00+00:00","2015-04-21T00:00:00+00:00","2015-04-22T00:00:00+00:00","2015-04-23T00:00:00+00:00","2015-04-24T00:00:00+00:00","2015-04-27T00:00:00+00:00","2015-04-28T00:00:00+00:00","2015-04-29T00:00:00+00:00","2015-04-30T00:00:00+00:00","2015-05-01T00:00:00+00:00","2015-05-04T00:00:00+00:00","2015-05-05T00:00:00+00:00","2015-05-06T00:00:00+00:00","2015-05-07T00:00:00+00:00","2015-05-08T00:00:00+00:00","2015-05-11T00:00:00+00:00","2015-05-12T00:00:00+00:00","2015-05-13T00:00:00+00:00","2015-05-14T00:00:00+00:00","2015-05-15T00:00:00+00:00","2015-05-18T00:00:00+00:00","2015-05-19T00:00:00+00:00","2015-05-20T00:00:00+00:00","2015-05-21T00:00:00+00:00","2015-05-22T00:00:00+00:00","2015-05-26T00:00:00+00:00","2015-05-27T00:00:00+00:00","2015-05-28T00:00:00+00:00","2015-05-29T00:00:00+00:00","2015-06-01T00:00:00+00:00","2015-06-02T00:00:00+00:00","2015-06-03T00:00:00+00:00","2015-06-04T00:00:00+00:00","2015-06-05T00:00:00+00:00","2015-06-08T00:00:00+00:00","2015-06-09T00:00:00+00:00","2015-06-10T00:00:00+00:00","2015-06-11T00:00:00+00:00","2015-06-12T00:00:00+00:00","2015-06-15T00:00:00+00:00","2015-06-16T00:00:00+00:00","2015-06-17T00:00:00+00:00","2015-06-18T00:00:00+00:00","2015-06-19T00:00:00+00:00","2015-06-22T00:00:00+00:00","2015-06-23T00:00:00+00:00","2015-06-24T00:00:00+00:00","2015-06-25T00:00:00+00:00","2015-06-26T00:00:00+00:00","2015-06-29T00:00:00+00:00","2015-06-30T00:00:00+00:00","2015-07-01T00:00:00+00:00","2015-07-02T00:00:00+00:00","2015-07-06T00:00:00+00:00","2015-07-07T00:00:00+00:00","2015-07-08T00:00:00+00:00","2015-07-09T00:00:00+00:00","2015-07-10T00:00:00+00:00","2015-07-13T00:00:00+00:00","2015-07-14T00:00:00+00:00","2015-07-15T00:00:00+00:00","2015-07-16T00:00:00+00:00","2015-07-17T00:00:00+00:00","2015-07-20T00:00:00+00:00","2015-07-21T00:00:00+00:00","2015-07-22T00:00:00+00:00","2015-07-23T00:00:00+00:00","2015-07-24T00:00:00+00:00","2015-07-27T00:00:00+00:00","2015-07-28T00:00:00+00:00","2015-07-29T00:00:00+00:00","2015-07-30T00:00:00+00:00","2015-07-31T00:00:00+00:00","2015-08-03T00:00:00+00:00","2015-08-04T00:00:00+00:00","2015-08-05T00:00:00+00:00","2015-08-06T00:00:00+00:00","2015-08-07T00:00:00+00:00","2015-08-10T00:00:00+00:00","2015-08-11T00:00:00+00:00","2015-08-12T00:00:00+00:00","2015-08-13T00:00:00+00:00","2015-08-14T00:00:00+00:00","2015-08-17T00:00:00+00:00","2015-08-18T00:00:00+00:00","2015-08-19T00:00:00+00:00","2015-08-20T00:00:00+00:00","2015-08-21T00:00:00+00:00","2015-08-24T00:00:00+00:00","2015-08-25T00:00:00+00:00","2015-08-26T00:00:00+00:00","2015-08-27T00:00:00+00:00","2015-08-28T00:00:00+00:00","2015-08-31T00:00:00+00:00","2015-09-01T00:00:00+00:00","2015-09-02T00:00:00+00:00","2015-09-03T00:00:00+00:00","2015-09-04T00:00:00+00:00","2015-09-08T00:00:00+00:00","2015-09-09T00:00:00+00:00","2015-09-10T00:00:00+00:00","2015-09-11T00:00:00+00:00","2015-09-14T00:00:00+00:00","2015-09-15T00:00:00+00:00","2015-09-16T00:00:00+00:00","2015-09-17T00:00:00+00:00","2015-09-18T00:00:00+00:00","2015-09-21T00:00:00+00:00","2015-09-22T00:00:00+00:00","2015-09-23T00:00:00+00:00","2015-09-24T00:00:00+00:00","2015-09-25T00:00:00+00:00","2015-09-28T00:00:00+00:00","2015-09-29T00:00:00+00:00","2015-09-30T00:00:00+00:00","2015-10-01T00:00:00+00:00","2015-10-02T00:00:00+00:00","2015-10-05T00:00:00+00:00","2015-10-06T00:00:00+00:00","2015-10-07T00:00:00+00:00","2015-10-08T00:00:00+00:00","2015-10-09T00:00:00+00:00","2015-10-12T00:00:00+00:00","2015-10-13T00:00:00+00:00","2015-10-14T00:00:00+00:00","2015-10-15T00:00:00+00:00","2015-10-16T00:00:00+00:00","2015-10-19T00:00:00+00:00","2015-10-20T00:00:00+00:00","2015-10-21T00:00:00+00:00","2015-10-22T00:00:00+00:00","2015-10-23T00:00:00+00:00","2015-10-26T00:00:00+00:00","2015-10-27T00:00:00+00:00","2015-10-28T00:00:00+00:00","2015-10-29T00:00:00+00:00","2015-10-30T00:00:00+00:00","2015-11-02T00:00:00+00:00","2015-11-03T00:00:00+00:00","2015-11-04T00:00:00+00:00","2015-11-05T00:00:00+00:00","2015-11-06T00:00:00+00:00","2015-11-09T00:00:00+00:00","2015-11-10T00:00:00+00:00","2015-11-11T00:00:00+00:00","2015-11-12T00:00:00+00:00","2015-11-13T00:00:00+00:00","2015-11-16T00:00:00+00:00","2015-11-17T00:00:00+00:00","2015-11-18T00:00:00+00:00","2015-11-19T00:00:00+00:00","2015-11-20T00:00:00+00:00","2015-11-23T00:00:00+00:00","2015-11-24T00:00:00+00:00","2015-11-25T00:00:00+00:00","2015-11-27T00:00:00+00:00","2015-11-30T00:00:00+00:00","2015-12-01T00:00:00+00:00","2015-12-02T00:00:00+00:00","2015-12-03T00:00:00+00:00","2015-12-04T00:00:00+00:00","2015-12-07T00:00:00+00:00","2015-12-08T00:00:00+00:00","2015-12-09T00:00:00+00:00","2015-12-10T00:00:00+00:00","2015-12-11T00:00:00+00:00","2015-12-14T00:00:00+00:00","2015-12-15T00:00:00+00:00","2015-12-16T00:00:00+00:00","2015-12-17T00:00:00+00:00","2015-12-18T00:00:00+00:00","2015-12-21T00:00:00+00:00","2015-12-22T00:00:00+00:00","2015-12-23T00:00:00+00:00","2015-12-24T00:00:00+00:00","2015-12-28T00:00:00+00:00","2015-12-29T00:00:00+00:00","2015-12-30T00:00:00+00:00","2015-12-31T00:00:00+00:00","2016-01-04T00:00:00+00:00","2016-01-05T00:00:00+00:00","2016-01-06T00:00:00+00:00","2016-01-07T00:00:00+00:00","2016-01-08T00:00:00+00:00","2016-01-11T00:00:00+00:00","2016-01-12T00:00:00+00:00","2016-01-13T00:00:00+00:00","2016-01-14T00:00:00+00:00","2016-01-15T00:00:00+00:00","2016-01-19T00:00:00+00:00","2016-01-20T00:00:00+00:00","2016-01-21T00:00:00+00:00","2016-01-22T00:00:00+00:00","2016-01-25T00:00:00+00:00","2016-01-26T00:00:00+00:00","2016-01-27T00:00:00+00:00","2016-01-28T00:00:00+00:00","2016-01-29T00:00:00+00:00","2016-02-01T00:00:00+00:00","2016-02-02T00:00:00+00:00","2016-02-03T00:00:00+00:00","2016-02-04T00:00:00+00:00","2016-02-05T00:00:00+00:00","2016-02-08T00:00:00+00:00","2016-02-09T00:00:00+00:00","2016-02-10T00:00:00+00:00","2016-02-11T00:00:00+00:00","2016-02-12T00:00:00+00:00","2016-02-16T00:00:00+00:00","2016-02-17T00:00:00+00:00","2016-02-18T00:00:00+00:00","2016-02-19T00:00:00+00:00","2016-02-22T00:00:00+00:00","2016-02-23T00:00:00+00:00","2016-02-24T00:00:00+00:00","2016-02-25T00:00:00+00:00","2016-02-26T00:00:00+00:00","2016-02-29T00:00:00+00:00","2016-03-01T00:00:00+00:00","2016-03-02T00:00:00+00:00","2016-03-03T00:00:00+00:00","2016-03-04T00:00:00+00:00","2016-03-07T00:00:00+00:00","2016-03-08T00:00:00+00:00","2016-03-09T00:00:00+00:00","2016-03-10T00:00:00+00:00","2016-03-11T00:00:00+00:00","2016-03-14T00:00:00+00:00","2016-03-15T00:00:00+00:00","2016-03-16T00:00:00+00:00","2016-03-17T00:00:00+00:00","2016-03-18T00:00:00+00:00","2016-03-21T00:00:00+00:00","2016-03-22T00:00:00+00:00","2016-03-23T00:00:00+00:00","2016-03-24T00:00:00+00:00","2016-03-28T00:00:00+00:00","2016-03-29T00:00:00+00:00","2016-03-30T00:00:00+00:00","2016-03-31T00:00:00+00:00","2016-04-01T00:00:00+00:00","2016-04-04T00:00:00+00:00","2016-04-05T00:00:00+00:00","2016-04-06T00:00:00+00:00","2016-04-07T00:00:00+00:00","2016-04-08T00:00:00+00:00","2016-04-11T00:00:00+00:00","2016-04-12T00:00:00+00:00","2016-04-13T00:00:00+00:00","2016-04-14T00:00:00+00:00","2016-04-15T00:00:00+00:00","2016-04-18T00:00:00+00:00","2016-04-19T00:00:00+00:00","2016-04-20T00:00:00+00:00","2016-04-21T00:00:00+00:00","2016-04-22T00:00:00+00:00","2016-04-25T00:00:00+00:00","2016-04-26T00:00:00+00:00","2016-04-27T00:00:00+00:00","2016-04-28T00:00:00+00:00","2016-04-29T00:00:00+00:00","2016-05-02T00:00:00+00:00","2016-05-03T00:00:00+00:00","2016-05-04T00:00:00+00:00","2016-05-05T00:00:00+00:00","2016-05-06T00:00:00+00:00","2016-05-09T00:00:00+00:00","2016-05-10T00:00:00+00:00","2016-05-11T00:00:00+00:00","2016-05-12T00:00:00+00:00","2016-05-13T00:00:00+00:00","2016-05-16T00:00:00+00:00","2016-05-17T00:00:00+00:00","2016-05-18T00:00:00+00:00","2016-05-19T00:00:00+00:00","2016-05-20T00:00:00+00:00","2016-05-23T00:00:00+00:00","2016-05-24T00:00:00+00:00","2016-05-25T00:00:00+00:00","2016-05-26T00:00:00+00:00","2016-05-27T00:00:00+00:00","2016-05-31T00:00:00+00:00","2016-06-01T00:00:00+00:00","2016-06-02T00:00:00+00:00","2016-06-03T00:00:00+00:00","2016-06-06T00:00:00+00:00","2016-06-07T00:00:00+00:00","2016-06-08T00:00:00+00:00","2016-06-09T00:00:00+00:00","2016-06-10T00:00:00+00:00","2016-06-13T00:00:00+00:00","2016-06-14T00:00:00+00:00","2016-06-15T00:00:00+00:00","2016-06-16T00:00:00+00:00","2016-06-17T00:00:00+00:00","2016-06-20T00:00:00+00:00","2016-06-21T00:00:00+00:00","2016-06-22T00:00:00+00:00","2016-06-23T00:00:00+00:00","2016-06-24T00:00:00+00:00","2016-06-27T00:00:00+00:00","2016-06-28T00:00:00+00:00","2016-06-29T00:00:00+00:00","2016-06-30T00:00:00+00:00","2016-07-01T00:00:00+00:00","2016-07-05T00:00:00+00:00","2016-07-06T00:00:00+00:00","2016-07-07T00:00:00+00:00","2016-07-08T00:00:00+00:00","2016-07-11T00:00:00+00:00","2016-07-12T00:00:00+00:00","2016-07-13T00:00:00+00:00","2016-07-14T00:00:00+00:00","2016-07-15T00:00:00+00:00","2016-07-18T00:00:00+00:00","2016-07-19T00:00:00+00:00","2016-07-20T00:00:00+00:00","2016-07-21T00:00:00+00:00","2016-07-22T00:00:00+00:00","2016-07-25T00:00:00+00:00","2016-07-26T00:00:00+00:00","2016-07-27T00:00:00+00:00","2016-07-28T00:00:00+00:00","2016-07-29T00:00:00+00:00","2016-08-01T00:00:00+00:00","2016-08-02T00:00:00+00:00","2016-08-03T00:00:00+00:00","2016-08-04T00:00:00+00:00","2016-08-05T00:00:00+00:00","2016-08-08T00:00:00+00:00","2016-08-09T00:00:00+00:00","2016-08-10T00:00:00+00:00","2016-08-11T00:00:00+00:00","2016-08-12T00:00:00+00:00","2016-08-15T00:00:00+00:00","2016-08-16T00:00:00+00:00","2016-08-17T00:00:00+00:00","2016-08-18T00:00:00+00:00","2016-08-19T00:00:00+00:00","2016-08-22T00:00:00+00:00","2016-08-23T00:00:00+00:00","2016-08-24T00:00:00+00:00","2016-08-25T00:00:00+00:00","2016-08-26T00:00:00+00:00","2016-08-29T00:00:00+00:00","2016-08-30T00:00:00+00:00","2016-08-31T00:00:00+00:00","2016-09-01T00:00:00+00:00","2016-09-02T00:00:00+00:00","2016-09-06T00:00:00+00:00","2016-09-07T00:00:00+00:00","2016-09-08T00:00:00+00:00","2016-09-09T00:00:00+00:00","2016-09-12T00:00:00+00:00","2016-09-13T00:00:00+00:00","2016-09-14T00:00:00+00:00","2016-09-15T00:00:00+00:00","2016-09-16T00:00:00+00:00","2016-09-19T00:00:00+00:00","2016-09-20T00:00:00+00:00","2016-09-21T00:00:00+00:00","2016-09-22T00:00:00+00:00","2016-09-23T00:00:00+00:00","2016-09-26T00:00:00+00:00","2016-09-27T00:00:00+00:00","2016-09-28T00:00:00+00:00","2016-09-29T00:00:00+00:00","2016-09-30T00:00:00+00:00","2016-10-03T00:00:00+00:00","2016-10-04T00:00:00+00:00","2016-10-05T00:00:00+00:00","2016-10-06T00:00:00+00:00","2016-10-07T00:00:00+00:00","2016-10-10T00:00:00+00:00","2016-10-11T00:00:00+00:00","2016-10-12T00:00:00+00:00","2016-10-13T00:00:00+00:00","2016-10-14T00:00:00+00:00","2016-10-17T00:00:00+00:00","2016-10-18T00:00:00+00:00","2016-10-19T00:00:00+00:00","2016-10-20T00:00:00+00:00","2016-10-21T00:00:00+00:00","2016-10-24T00:00:00+00:00","2016-10-25T00:00:00+00:00","2016-10-26T00:00:00+00:00","2016-10-27T00:00:00+00:00","2016-10-28T00:00:00+00:00","2016-10-31T00:00:00+00:00","2016-11-01T00:00:00+00:00","2016-11-02T00:00:00+00:00","2016-11-03T00:00:00+00:00","2016-11-04T00:00:00+00:00","2016-11-07T00:00:00+00:00","2016-11-08T00:00:00+00:00","2016-11-09T00:00:00+00:00","2016-11-10T00:00:00+00:00","2016-11-11T00:00:00+00:00","2016-11-14T00:00:00+00:00","2016-11-15T00:00:00+00:00","2016-11-16T00:00:00+00:00","2016-11-17T00:00:00+00:00","2016-11-18T00:00:00+00:00","2016-11-21T00:00:00+00:00","2016-11-22T00:00:00+00:00","2016-11-23T00:00:00+00:00","2016-11-25T00:00:00+00:00","2016-11-28T00:00:00+00:00","2016-11-29T00:00:00+00:00","2016-11-30T00:00:00+00:00","2016-12-01T00:00:00+00:00","2016-12-02T00:00:00+00:00","2016-12-05T00:00:00+00:00","2016-12-06T00:00:00+00:00","2016-12-07T00:00:00+00:00","2016-12-08T00:00:00+00:00","2016-12-09T00:00:00+00:00","2016-12-12T00:00:00+00:00","2016-12-13T00:00:00+00:00","2016-12-14T00:00:00+00:00","2016-12-15T00:00:00+00:00","2016-12-16T00:00:00+00:00","2016-12-19T00:00:00+00:00","2016-12-20T00:00:00+00:00","2016-12-21T00:00:00+00:00","2016-12-22T00:00:00+00:00","2016-12-23T00:00:00+00:00","2016-12-27T00:00:00+00:00","2016-12-28T00:00:00+00:00","2016-12-29T00:00:00+00:00","2016-12-30T00:00:00+00:00","2017-01-03T00:00:00+00:00","2017-01-04T00:00:00+00:00","2017-01-05T00:00:00+00:00","2017-01-06T00:00:00+00:00","2017-01-09T00:00:00+00:00","2017-01-10T00:00:00+00:00","2017-01-11T00:00:00+00:00","2017-01-12T00:00:00+00:00","2017-01-13T00:00:00+00:00","2017-01-17T00:00:00+00:00","2017-01-18T00:00:00+00:00","2017-01-19T00:00:00+00:00","2017-01-20T00:00:00+00:00","2017-01-23T00:00:00+00:00","2017-01-24T00:00:00+00:00","2017-01-25T00:00:00+00:00","2017-01-26T00:00:00+00:00","2017-01-27T00:00:00+00:00","2017-01-30T00:00:00+00:00","2017-01-31T00:00:00+00:00","2017-02-01T00:00:00+00:00","2017-02-02T00:00:00+00:00","2017-02-03T00:00:00+00:00","2017-02-06T00:00:00+00:00","2017-02-07T00:00:00+00:00","2017-02-08T00:00:00+00:00","2017-02-09T00:00:00+00:00","2017-02-10T00:00:00+00:00","2017-02-13T00:00:00+00:00","2017-02-14T00:00:00+00:00","2017-02-15T00:00:00+00:00","2017-02-16T00:00:00+00:00","2017-02-17T00:00:00+00:00","2017-02-21T00:00:00+00:00","2017-02-22T00:00:00+00:00","2017-02-23T00:00:00+00:00","2017-02-24T00:00:00+00:00","2017-02-27T00:00:00+00:00","2017-02-28T00:00:00+00:00","2017-03-01T00:00:00+00:00","2017-03-02T00:00:00+00:00","2017-03-03T00:00:00+00:00","2017-03-06T00:00:00+00:00","2017-03-07T00:00:00+00:00","2017-03-08T00:00:00+00:00","2017-03-09T00:00:00+00:00","2017-03-10T00:00:00+00:00","2017-03-13T00:00:00+00:00","2017-03-14T00:00:00+00:00","2017-03-15T00:00:00+00:00","2017-03-16T00:00:00+00:00","2017-03-17T00:00:00+00:00","2017-03-20T00:00:00+00:00","2017-03-21T00:00:00+00:00","2017-03-22T00:00:00+00:00","2017-03-23T00:00:00+00:00","2017-03-24T00:00:00+00:00","2017-03-27T00:00:00+00:00","2017-03-28T00:00:00+00:00","2017-03-29T00:00:00+00:00","2017-03-30T00:00:00+00:00","2017-03-31T00:00:00+00:00","2017-04-03T00:00:00+00:00","2017-04-04T00:00:00+00:00","2017-04-05T00:00:00+00:00","2017-04-06T00:00:00+00:00","2017-04-07T00:00:00+00:00","2017-04-10T00:00:00+00:00","2017-04-11T00:00:00+00:00","2017-04-12T00:00:00+00:00","2017-04-13T00:00:00+00:00","2017-04-17T00:00:00+00:00","2017-04-18T00:00:00+00:00","2017-04-19T00:00:00+00:00","2017-04-20T00:00:00+00:00","2017-04-21T00:00:00+00:00","2017-04-24T00:00:00+00:00","2017-04-25T00:00:00+00:00","2017-04-26T00:00:00+00:00","2017-04-27T00:00:00+00:00","2017-04-28T00:00:00+00:00","2017-05-01T00:00:00+00:00","2017-05-02T00:00:00+00:00","2017-05-03T00:00:00+00:00","2017-05-04T00:00:00+00:00","2017-05-05T00:00:00+00:00","2017-05-08T00:00:00+00:00","2017-05-09T00:00:00+00:00","2017-05-10T00:00:00+00:00","2017-05-11T00:00:00+00:00","2017-05-12T00:00:00+00:00","2017-05-15T00:00:00+00:00","2017-05-16T00:00:00+00:00","2017-05-17T00:00:00+00:00","2017-05-18T00:00:00+00:00","2017-05-19T00:00:00+00:00","2017-05-22T00:00:00+00:00","2017-05-23T00:00:00+00:00","2017-05-24T00:00:00+00:00","2017-05-25T00:00:00+00:00","2017-05-26T00:00:00+00:00","2017-05-30T00:00:00+00:00","2017-05-31T00:00:00+00:00","2017-06-01T00:00:00+00:00","2017-06-02T00:00:00+00:00","2017-06-05T00:00:00+00:00","2017-06-06T00:00:00+00:00","2017-06-07T00:00:00+00:00","2017-06-08T00:00:00+00:00","2017-06-09T00:00:00+00:00","2017-06-12T00:00:00+00:00","2017-06-13T00:00:00+00:00","2017-06-14T00:00:00+00:00","2017-06-15T00:00:00+00:00","2017-06-16T00:00:00+00:00","2017-06-19T00:00:00+00:00","2017-06-20T00:00:00+00:00","2017-06-21T00:00:00+00:00","2017-06-22T00:00:00+00:00","2017-06-23T00:00:00+00:00","2017-06-26T00:00:00+00:00","2017-06-27T00:00:00+00:00","2017-06-28T00:00:00+00:00","2017-06-29T00:00:00+00:00","2017-06-30T00:00:00+00:00","2017-07-03T00:00:00+00:00","2017-07-05T00:00:00+00:00","2017-07-06T00:00:00+00:00","2017-07-07T00:00:00+00:00","2017-07-10T00:00:00+00:00","2017-07-11T00:00:00+00:00","2017-07-12T00:00:00+00:00","2017-07-13T00:00:00+00:00","2017-07-14T00:00:00+00:00","2017-07-17T00:00:00+00:00","2017-07-18T00:00:00+00:00","2017-07-19T00:00:00+00:00","2017-07-20T00:00:00+00:00","2017-07-21T00:00:00+00:00","2017-07-24T00:00:00+00:00","2017-07-25T00:00:00+00:00","2017-07-26T00:00:00+00:00","2017-07-27T00:00:00+00:00","2017-07-28T00:00:00+00:00","2017-07-31T00:00:00+00:00","2017-08-01T00:00:00+00:00","2017-08-02T00:00:00+00:00","2017-08-03T00:00:00+00:00","2017-08-04T00:00:00+00:00","2017-08-07T00:00:00+00:00","2017-08-08T00:00:00+00:00","2017-08-09T00:00:00+00:00","2017-08-10T00:00:00+00:00","2017-08-11T00:00:00+00:00","2017-08-14T00:00:00+00:00","2017-08-15T00:00:00+00:00","2017-08-16T00:00:00+00:00","2017-08-17T00:00:00+00:00","2017-08-18T00:00:00+00:00","2017-08-21T00:00:00+00:00","2017-08-22T00:00:00+00:00","2017-08-23T00:00:00+00:00","2017-08-24T00:00:00+00:00","2017-08-25T00:00:00+00:00","2017-08-28T00:00:00+00:00","2017-08-29T00:00:00+00:00","2017-08-30T00:00:00+00:00","2017-08-31T00:00:00+00:00","2017-09-01T00:00:00+00:00","2017-09-05T00:00:00+00:00","2017-09-06T00:00:00+00:00","2017-09-07T00:00:00+00:00","2017-09-08T00:00:00+00:00","2017-09-11T00:00:00+00:00","2017-09-12T00:00:00+00:00","2017-09-13T00:00:00+00:00","2017-09-14T00:00:00+00:00","2017-09-15T00:00:00+00:00","2017-09-18T00:00:00+00:00","2017-09-19T00:00:00+00:00","2017-09-20T00:00:00+00:00","2017-09-21T00:00:00+00:00","2017-09-22T00:00:00+00:00","2017-09-25T00:00:00+00:00","2017-09-26T00:00:00+00:00","2017-09-27T00:00:00+00:00","2017-09-28T00:00:00+00:00","2017-09-29T00:00:00+00:00","2017-10-02T00:00:00+00:00","2017-10-03T00:00:00+00:00","2017-10-04T00:00:00+00:00","2017-10-05T00:00:00+00:00","2017-10-06T00:00:00+00:00","2017-10-09T00:00:00+00:00","2017-10-10T00:00:00+00:00","2017-10-11T00:00:00+00:00","2017-10-12T00:00:00+00:00","2017-10-13T00:00:00+00:00","2017-10-16T00:00:00+00:00","2017-10-17T00:00:00+00:00","2017-10-18T00:00:00+00:00","2017-10-19T00:00:00+00:00","2017-10-20T00:00:00+00:00","2017-10-23T00:00:00+00:00","2017-10-24T00:00:00+00:00","2017-10-25T00:00:00+00:00","2017-10-26T00:00:00+00:00","2017-10-27T00:00:00+00:00","2017-10-30T00:00:00+00:00","2017-10-31T00:00:00+00:00","2017-11-01T00:00:00+00:00","2017-11-02T00:00:00+00:00","2017-11-03T00:00:00+00:00","2017-11-06T00:00:00+00:00","2017-11-07T00:00:00+00:00","2017-11-08T00:00:00+00:00","2017-11-09T00:00:00+00:00","2017-11-10T00:00:00+00:00","2017-11-13T00:00:00+00:00","2017-11-14T00:00:00+00:00","2017-11-15T00:00:00+00:00","2017-11-16T00:00:00+00:00","2017-11-17T00:00:00+00:00","2017-11-20T00:00:00+00:00","2017-11-21T00:00:00+00:00","2017-11-22T00:00:00+00:00","2017-11-24T00:00:00+00:00","2017-11-27T00:00:00+00:00","2017-11-28T00:00:00+00:00","2017-11-29T00:00:00+00:00","2017-11-30T00:00:00+00:00","2017-12-01T00:00:00+00:00","2017-12-04T00:00:00+00:00","2017-12-05T00:00:00+00:00","2017-12-06T00:00:00+00:00","2017-12-07T00:00:00+00:00","2017-12-08T00:00:00+00:00","2017-12-11T00:00:00+00:00","2017-12-12T00:00:00+00:00","2017-12-13T00:00:00+00:00","2017-12-14T00:00:00+00:00","2017-12-15T00:00:00+00:00","2017-12-18T00:00:00+00:00","2017-12-19T00:00:00+00:00","2017-12-20T00:00:00+00:00","2017-12-21T00:00:00+00:00","2017-12-22T00:00:00+00:00","2017-12-26T00:00:00+00:00","2017-12-27T00:00:00+00:00","2017-12-28T00:00:00+00:00","2017-12-29T00:00:00+00:00","2018-01-02T00:00:00+00:00","2018-01-03T00:00:00+00:00","2018-01-04T00:00:00+00:00","2018-01-05T00:00:00+00:00","2018-01-08T00:00:00+00:00","2018-01-09T00:00:00+00:00","2018-01-10T00:00:00+00:00","2018-01-11T00:00:00+00:00","2018-01-12T00:00:00+00:00","2018-01-16T00:00:00+00:00","2018-01-17T00:00:00+00:00","2018-01-18T00:00:00+00:00","2018-01-19T00:00:00+00:00","2018-01-22T00:00:00+00:00","2018-01-23T00:00:00+00:00","2018-01-24T00:00:00+00:00","2018-01-25T00:00:00+00:00","2018-01-26T00:00:00+00:00","2018-01-29T00:00:00+00:00","2018-01-30T00:00:00+00:00","2018-01-31T00:00:00+00:00","2018-02-01T00:00:00+00:00","2018-02-02T00:00:00+00:00","2018-02-05T00:00:00+00:00","2018-02-06T00:00:00+00:00","2018-02-07T00:00:00+00:00","2018-02-08T00:00:00+00:00","2018-02-09T00:00:00+00:00","2018-02-12T00:00:00+00:00","2018-02-13T00:00:00+00:00","2018-02-14T00:00:00+00:00","2018-02-15T00:00:00+00:00","2018-02-16T00:00:00+00:00","2018-02-20T00:00:00+00:00","2018-02-21T00:00:00+00:00","2018-02-22T00:00:00+00:00","2018-02-23T00:00:00+00:00","2018-02-26T00:00:00+00:00","2018-02-27T00:00:00+00:00","2018-02-28T00:00:00+00:00","2018-03-01T00:00:00+00:00","2018-03-02T00:00:00+00:00","2018-03-05T00:00:00+00:00","2018-03-06T00:00:00+00:00","2018-03-07T00:00:00+00:00","2018-03-08T00:00:00+00:00","2018-03-09T00:00:00+00:00","2018-03-12T00:00:00+00:00","2018-03-13T00:00:00+00:00","2018-03-14T00:00:00+00:00","2018-03-15T00:00:00+00:00","2018-03-16T00:00:00+00:00","2018-03-19T00:00:00+00:00","2018-03-20T00:00:00+00:00","2018-03-21T00:00:00+00:00","2018-03-22T00:00:00+00:00","2018-03-23T00:00:00+00:00","2018-03-26T00:00:00+00:00","2018-03-27T00:00:00+00:00","2018-03-28T00:00:00+00:00","2018-03-29T00:00:00+00:00","2018-04-02T00:00:00+00:00","2018-04-03T00:00:00+00:00","2018-04-04T00:00:00+00:00","2018-04-05T00:00:00+00:00","2018-04-06T00:00:00+00:00","2018-04-09T00:00:00+00:00","2018-04-10T00:00:00+00:00","2018-04-11T00:00:00+00:00","2018-04-12T00:00:00+00:00","2018-04-13T00:00:00+00:00","2018-04-16T00:00:00+00:00","2018-04-17T00:00:00+00:00","2018-04-18T00:00:00+00:00","2018-04-19T00:00:00+00:00","2018-04-20T00:00:00+00:00","2018-04-23T00:00:00+00:00","2018-04-24T00:00:00+00:00","2018-04-25T00:00:00+00:00","2018-04-26T00:00:00+00:00","2018-04-27T00:00:00+00:00","2018-04-30T00:00:00+00:00","2018-05-01T00:00:00+00:00","2018-05-02T00:00:00+00:00","2018-05-03T00:00:00+00:00","2018-05-04T00:00:00+00:00","2018-05-07T00:00:00+00:00","2018-05-08T00:00:00+00:00","2018-05-09T00:00:00+00:00","2018-05-10T00:00:00+00:00","2018-05-11T00:00:00+00:00","2018-05-14T00:00:00+00:00","2018-05-15T00:00:00+00:00","2018-05-16T00:00:00+00:00","2018-05-17T00:00:00+00:00","2018-05-18T00:00:00+00:00","2018-05-21T00:00:00+00:00","2018-05-22T00:00:00+00:00","2018-05-23T00:00:00+00:00","2018-05-24T00:00:00+00:00","2018-05-25T00:00:00+00:00","2018-05-29T00:00:00+00:00","2018-05-30T00:00:00+00:00","2018-05-31T00:00:00+00:00","2018-06-01T00:00:00+00:00","2018-06-04T00:00:00+00:00","2018-06-05T00:00:00+00:00","2018-06-06T00:00:00+00:00","2018-06-07T00:00:00+00:00","2018-06-08T00:00:00+00:00","2018-06-11T00:00:00+00:00","2018-06-12T00:00:00+00:00","2018-06-13T00:00:00+00:00","2018-06-14T00:00:00+00:00","2018-06-15T00:00:00+00:00","2018-06-18T00:00:00+00:00","2018-06-19T00:00:00+00:00","2018-06-20T00:00:00+00:00","2018-06-21T00:00:00+00:00","2018-06-22T00:00:00+00:00","2018-06-25T00:00:00+00:00","2018-06-26T00:00:00+00:00","2018-06-27T00:00:00+00:00","2018-06-28T00:00:00+00:00","2018-06-29T00:00:00+00:00","2018-07-02T00:00:00+00:00","2018-07-03T00:00:00+00:00","2018-07-05T00:00:00+00:00","2018-07-06T00:00:00+00:00","2018-07-09T00:00:00+00:00","2018-07-10T00:00:00+00:00","2018-07-11T00:00:00+00:00","2018-07-12T00:00:00+00:00","2018-07-13T00:00:00+00:00","2018-07-16T00:00:00+00:00","2018-07-17T00:00:00+00:00","2018-07-18T00:00:00+00:00","2018-07-19T00:00:00+00:00","2018-07-20T00:00:00+00:00","2018-07-23T00:00:00+00:00","2018-07-24T00:00:00+00:00","2018-07-25T00:00:00+00:00","2018-07-26T00:00:00+00:00","2018-07-27T00:00:00+00:00","2018-07-30T00:00:00+00:00","2018-07-31T00:00:00+00:00","2018-08-01T00:00:00+00:00","2018-08-02T00:00:00+00:00","2018-08-03T00:00:00+00:00","2018-08-06T00:00:00+00:00","2018-08-07T00:00:00+00:00","2018-08-08T00:00:00+00:00","2018-08-09T00:00:00+00:00","2018-08-10T00:00:00+00:00","2018-08-13T00:00:00+00:00","2018-08-14T00:00:00+00:00","2018-08-15T00:00:00+00:00","2018-08-16T00:00:00+00:00","2018-08-17T00:00:00+00:00","2018-08-20T00:00:00+00:00","2018-08-21T00:00:00+00:00","2018-08-22T00:00:00+00:00","2018-08-23T00:00:00+00:00","2018-08-24T00:00:00+00:00","2018-08-27T00:00:00+00:00","2018-08-28T00:00:00+00:00","2018-08-29T00:00:00+00:00","2018-08-30T00:00:00+00:00","2018-08-31T00:00:00+00:00","2018-09-04T00:00:00+00:00","2018-09-05T00:00:00+00:00","2018-09-06T00:00:00+00:00","2018-09-07T00:00:00+00:00","2018-09-10T00:00:00+00:00","2018-09-11T00:00:00+00:00","2018-09-12T00:00:00+00:00","2018-09-13T00:00:00+00:00","2018-09-14T00:00:00+00:00","2018-09-17T00:00:00+00:00","2018-09-18T00:00:00+00:00","2018-09-19T00:00:00+00:00","2018-09-20T00:00:00+00:00","2018-09-21T00:00:00+00:00","2018-09-24T00:00:00+00:00","2018-09-25T00:00:00+00:00","2018-09-26T00:00:00+00:00","2018-09-27T00:00:00+00:00","2018-09-28T00:00:00+00:00","2018-10-01T00:00:00+00:00","2018-10-02T00:00:00+00:00","2018-10-03T00:00:00+00:00","2018-10-04T00:00:00+00:00","2018-10-05T00:00:00+00:00","2018-10-08T00:00:00+00:00","2018-10-09T00:00:00+00:00","2018-10-10T00:00:00+00:00","2018-10-11T00:00:00+00:00","2018-10-12T00:00:00+00:00","2018-10-15T00:00:00+00:00","2018-10-16T00:00:00+00:00","2018-10-17T00:00:00+00:00","2018-10-18T00:00:00+00:00","2018-10-19T00:00:00+00:00","2018-10-22T00:00:00+00:00","2018-10-23T00:00:00+00:00","2018-10-24T00:00:00+00:00","2018-10-25T00:00:00+00:00","2018-10-26T00:00:00+00:00","2018-10-29T00:00:00+00:00","2018-10-30T00:00:00+00:00","2018-10-31T00:00:00+00:00","2018-11-01T00:00:00+00:00","2018-11-02T00:00:00+00:00","2018-11-05T00:00:00+00:00","2018-11-06T00:00:00+00:00","2018-11-07T00:00:00+00:00","2018-11-08T00:00:00+00:00","2018-11-09T00:00:00+00:00","2018-11-12T00:00:00+00:00","2018-11-13T00:00:00+00:00","2018-11-14T00:00:00+00:00","2018-11-15T00:00:00+00:00","2018-11-16T00:00:00+00:00","2018-11-19T00:00:00+00:00","2018-11-20T00:00:00+00:00","2018-11-21T00:00:00+00:00","2018-11-23T00:00:00+00:00","2018-11-26T00:00:00+00:00","2018-11-27T00:00:00+00:00","2018-11-28T00:00:00+00:00","2018-11-29T00:00:00+00:00","2018-11-30T00:00:00+00:00","2018-12-03T00:00:00+00:00","2018-12-04T00:00:00+00:00","2018-12-06T00:00:00+00:00","2018-12-07T00:00:00+00:00","2018-12-10T00:00:00+00:00","2018-12-11T00:00:00+00:00","2018-12-12T00:00:00+00:00","2018-12-13T00:00:00+00:00","2018-12-14T00:00:00+00:00","2018-12-17T00:00:00+00:00","2018-12-18T00:00:00+00:00","2018-12-19T00:00:00+00:00","2018-12-20T00:00:00+00:00","2018-12-21T00:00:00+00:00","2018-12-24T00:00:00+00:00","2018-12-26T00:00:00+00:00","2018-12-27T00:00:00+00:00","2018-12-28T00:00:00+00:00","2018-12-31T00:00:00+00:00","2019-01-02T00:00:00+00:00","2019-01-03T00:00:00+00:00","2019-01-04T00:00:00+00:00","2019-01-07T00:00:00+00:00","2019-01-08T00:00:00+00:00","2019-01-09T00:00:00+00:00","2019-01-10T00:00:00+00:00","2019-01-11T00:00:00+00:00","2019-01-14T00:00:00+00:00","2019-01-15T00:00:00+00:00","2019-01-16T00:00:00+00:00","2019-01-17T00:00:00+00:00","2019-01-18T00:00:00+00:00","2019-01-22T00:00:00+00:00","2019-01-23T00:00:00+00:00","2019-01-24T00:00:00+00:00","2019-01-25T00:00:00+00:00","2019-01-28T00:00:00+00:00","2019-01-29T00:00:00+00:00","2019-01-30T00:00:00+00:00","2019-01-31T00:00:00+00:00","2019-02-01T00:00:00+00:00","2019-02-04T00:00:00+00:00","2019-02-05T00:00:00+00:00","2019-02-06T00:00:00+00:00","2019-02-07T00:00:00+00:00","2019-02-08T00:00:00+00:00","2019-02-11T00:00:00+00:00","2019-02-12T00:00:00+00:00","2019-02-13T00:00:00+00:00","2019-02-14T00:00:00+00:00","2019-02-15T00:00:00+00:00","2019-02-19T00:00:00+00:00","2019-02-20T00:00:00+00:00","2019-02-21T00:00:00+00:00","2019-02-22T00:00:00+00:00","2019-02-25T00:00:00+00:00","2019-02-26T00:00:00+00:00","2019-02-27T00:00:00+00:00","2019-02-28T00:00:00+00:00","2019-03-01T00:00:00+00:00","2019-03-04T00:00:00+00:00","2019-03-05T00:00:00+00:00","2019-03-06T00:00:00+00:00","2019-03-07T00:00:00+00:00","2019-03-08T00:00:00+00:00","2019-03-11T00:00:00+00:00","2019-03-12T00:00:00+00:00","2019-03-13T00:00:00+00:00","2019-03-14T00:00:00+00:00","2019-03-15T00:00:00+00:00","2019-03-18T00:00:00+00:00","2019-03-19T00:00:00+00:00","2019-03-20T00:00:00+00:00","2019-03-21T00:00:00+00:00","2019-03-22T00:00:00+00:00","2019-03-25T00:00:00+00:00","2019-03-26T00:00:00+00:00","2019-03-27T00:00:00+00:00","2019-03-28T00:00:00+00:00","2019-03-29T00:00:00+00:00","2019-04-01T00:00:00+00:00","2019-04-02T00:00:00+00:00","2019-04-03T00:00:00+00:00","2019-04-04T00:00:00+00:00","2019-04-05T00:00:00+00:00","2019-04-08T00:00:00+00:00","2019-04-09T00:00:00+00:00","2019-04-10T00:00:00+00:00","2019-04-11T00:00:00+00:00","2019-04-12T00:00:00+00:00","2019-04-15T00:00:00+00:00","2019-04-16T00:00:00+00:00","2019-04-17T00:00:00+00:00","2019-04-18T00:00:00+00:00","2019-04-22T00:00:00+00:00","2019-04-23T00:00:00+00:00","2019-04-24T00:00:00+00:00","2019-04-25T00:00:00+00:00","2019-04-26T00:00:00+00:00","2019-04-29T00:00:00+00:00","2019-04-30T00:00:00+00:00","2019-05-01T00:00:00+00:00","2019-05-02T00:00:00+00:00","2019-05-03T00:00:00+00:00","2019-05-06T00:00:00+00:00","2019-05-07T00:00:00+00:00","2019-05-08T00:00:00+00:00","2019-05-09T00:00:00+00:00","2019-05-10T00:00:00+00:00","2019-05-13T00:00:00+00:00","2019-05-14T00:00:00+00:00","2019-05-15T00:00:00+00:00","2019-05-16T00:00:00+00:00","2019-05-17T00:00:00+00:00","2019-05-20T00:00:00+00:00","2019-05-21T00:00:00+00:00","2019-05-22T00:00:00+00:00","2019-05-23T00:00:00+00:00","2019-05-24T00:00:00+00:00","2019-05-28T00:00:00+00:00","2019-05-29T00:00:00+00:00","2019-05-30T00:00:00+00:00","2019-05-31T00:00:00+00:00","2019-06-03T00:00:00+00:00","2019-06-04T00:00:00+00:00","2019-06-05T00:00:00+00:00","2019-06-06T00:00:00+00:00","2019-06-07T00:00:00+00:00","2019-06-10T00:00:00+00:00","2019-06-11T00:00:00+00:00","2019-06-12T00:00:00+00:00","2019-06-13T00:00:00+00:00","2019-06-14T00:00:00+00:00","2019-06-17T00:00:00+00:00","2019-06-18T00:00:00+00:00","2019-06-19T00:00:00+00:00","2019-06-20T00:00:00+00:00","2019-06-21T00:00:00+00:00","2019-06-24T00:00:00+00:00","2019-06-25T00:00:00+00:00","2019-06-26T00:00:00+00:00","2019-06-27T00:00:00+00:00","2019-06-28T00:00:00+00:00","2019-07-01T00:00:00+00:00","2019-07-02T00:00:00+00:00","2019-07-03T00:00:00+00:00","2019-07-05T00:00:00+00:00","2019-07-08T00:00:00+00:00","2019-07-09T00:00:00+00:00","2019-07-10T00:00:00+00:00","2019-07-11T00:00:00+00:00","2019-07-12T00:00:00+00:00","2019-07-15T00:00:00+00:00","2019-07-16T00:00:00+00:00","2019-07-17T00:00:00+00:00","2019-07-18T00:00:00+00:00","2019-07-19T00:00:00+00:00","2019-07-22T00:00:00+00:00","2019-07-23T00:00:00+00:00","2019-07-24T00:00:00+00:00","2019-07-25T00:00:00+00:00","2019-07-26T00:00:00+00:00","2019-07-29T00:00:00+00:00","2019-07-30T00:00:00+00:00","2019-07-31T00:00:00+00:00","2019-08-01T00:00:00+00:00","2019-08-02T00:00:00+00:00","2019-08-05T00:00:00+00:00","2019-08-06T00:00:00+00:00","2019-08-07T00:00:00+00:00","2019-08-08T00:00:00+00:00","2019-08-09T00:00:00+00:00","2019-08-12T00:00:00+00:00","2019-08-13T00:00:00+00:00","2019-08-14T00:00:00+00:00","2019-08-15T00:00:00+00:00","2019-08-16T00:00:00+00:00","2019-08-19T00:00:00+00:00","2019-08-20T00:00:00+00:00","2019-08-21T00:00:00+00:00","2019-08-22T00:00:00+00:00","2019-08-23T00:00:00+00:00","2019-08-26T00:00:00+00:00","2019-08-27T00:00:00+00:00","2019-08-28T00:00:00+00:00","2019-08-29T00:00:00+00:00","2019-08-30T00:00:00+00:00","2019-09-03T00:00:00+00:00","2019-09-04T00:00:00+00:00","2019-09-05T00:00:00+00:00","2019-09-06T00:00:00+00:00","2019-09-09T00:00:00+00:00","2019-09-10T00:00:00+00:00","2019-09-11T00:00:00+00:00","2019-09-12T00:00:00+00:00","2019-09-13T00:00:00+00:00","2019-09-16T00:00:00+00:00","2019-09-17T00:00:00+00:00","2019-09-18T00:00:00+00:00","2019-09-19T00:00:00+00:00","2019-09-20T00:00:00+00:00","2019-09-23T00:00:00+00:00","2019-09-24T00:00:00+00:00","2019-09-25T00:00:00+00:00","2019-09-26T00:00:00+00:00","2019-09-27T00:00:00+00:00","2019-09-30T00:00:00+00:00","2019-10-01T00:00:00+00:00","2019-10-02T00:00:00+00:00","2019-10-03T00:00:00+00:00","2019-10-04T00:00:00+00:00","2019-10-07T00:00:00+00:00","2019-10-08T00:00:00+00:00","2019-10-09T00:00:00+00:00","2019-10-10T00:00:00+00:00","2019-10-11T00:00:00+00:00","2019-10-14T00:00:00+00:00","2019-10-15T00:00:00+00:00","2019-10-16T00:00:00+00:00","2019-10-17T00:00:00+00:00","2019-10-18T00:00:00+00:00","2019-10-21T00:00:00+00:00","2019-10-22T00:00:00+00:00","2019-10-23T00:00:00+00:00","2019-10-24T00:00:00+00:00","2019-10-25T00:00:00+00:00","2019-10-28T00:00:00+00:00","2019-10-29T00:00:00+00:00","2019-10-30T00:00:00+00:00","2019-10-31T00:00:00+00:00","2019-11-01T00:00:00+00:00","2019-11-04T00:00:00+00:00","2019-11-05T00:00:00+00:00","2019-11-06T00:00:00+00:00","2019-11-07T00:00:00+00:00","2019-11-08T00:00:00+00:00","2019-11-11T00:00:00+00:00","2019-11-12T00:00:00+00:00","2019-11-13T00:00:00+00:00","2019-11-14T00:00:00+00:00","2019-11-15T00:00:00+00:00","2019-11-18T00:00:00+00:00","2019-11-19T00:00:00+00:00","2019-11-20T00:00:00+00:00","2019-11-21T00:00:00+00:00","2019-11-22T00:00:00+00:00","2019-11-25T00:00:00+00:00","2019-11-26T00:00:00+00:00","2019-11-27T00:00:00+00:00","2019-11-29T00:00:00+00:00","2019-12-02T00:00:00+00:00","2019-12-03T00:00:00+00:00","2019-12-04T00:00:00+00:00","2019-12-05T00:00:00+00:00","2019-12-06T00:00:00+00:00","2019-12-09T00:00:00+00:00","2019-12-10T00:00:00+00:00","2019-12-11T00:00:00+00:00","2019-12-12T00:00:00+00:00","2019-12-13T00:00:00+00:00","2019-12-16T00:00:00+00:00","2019-12-17T00:00:00+00:00","2019-12-18T00:00:00+00:00","2019-12-19T00:00:00+00:00","2019-12-20T00:00:00+00:00","2019-12-23T00:00:00+00:00","2019-12-24T00:00:00+00:00","2019-12-26T00:00:00+00:00","2019-12-27T00:00:00+00:00","2019-12-30T00:00:00+00:00","2019-12-31T00:00:00+00:00","2020-01-02T00:00:00+00:00","2020-01-03T00:00:00+00:00","2020-01-06T00:00:00+00:00","2020-01-07T00:00:00+00:00","2020-01-08T00:00:00+00:00","2020-01-09T00:00:00+00:00","2020-01-10T00:00:00+00:00","2020-01-13T00:00:00+00:00","2020-01-14T00:00:00+00:00","2020-01-15T00:00:00+00:00","2020-01-16T00:00:00+00:00","2020-01-17T00:00:00+00:00","2020-01-21T00:00:00+00:00","2020-01-22T00:00:00+00:00","2020-01-23T00:00:00+00:00","2020-01-24T00:00:00+00:00","2020-01-27T00:00:00+00:00","2020-01-28T00:00:00+00:00","2020-01-29T00:00:00+00:00","2020-01-30T00:00:00+00:00","2020-01-31T00:00:00+00:00","2020-02-03T00:00:00+00:00","2020-02-04T00:00:00+00:00","2020-02-05T00:00:00+00:00","2020-02-06T00:00:00+00:00","2020-02-07T00:00:00+00:00","2020-02-10T00:00:00+00:00","2020-02-11T00:00:00+00:00","2020-02-12T00:00:00+00:00","2020-02-13T00:00:00+00:00","2020-02-14T00:00:00+00:00","2020-02-18T00:00:00+00:00","2020-02-19T00:00:00+00:00","2020-02-20T00:00:00+00:00","2020-02-21T00:00:00+00:00","2020-02-24T00:00:00+00:00","2020-02-25T00:00:00+00:00","2020-02-26T00:00:00+00:00","2020-02-27T00:00:00+00:00","2020-02-28T00:00:00+00:00","2020-03-02T00:00:00+00:00","2020-03-03T00:00:00+00:00","2020-03-04T00:00:00+00:00","2020-03-05T00:00:00+00:00","2020-03-06T00:00:00+00:00","2020-03-09T00:00:00+00:00","2020-03-10T00:00:00+00:00","2020-03-11T00:00:00+00:00","2020-03-12T00:00:00+00:00","2020-03-13T00:00:00+00:00","2020-03-16T00:00:00+00:00","2020-03-17T00:00:00+00:00","2020-03-18T00:00:00+00:00","2020-03-19T00:00:00+00:00","2020-03-20T00:00:00+00:00","2020-03-23T00:00:00+00:00","2020-03-24T00:00:00+00:00","2020-03-25T00:00:00+00:00","2020-03-26T00:00:00+00:00","2020-03-27T00:00:00+00:00","2020-03-30T00:00:00+00:00","2020-03-31T00:00:00+00:00","2020-04-01T00:00:00+00:00","2020-04-02T00:00:00+00:00","2020-04-03T00:00:00+00:00","2020-04-06T00:00:00+00:00","2020-04-07T00:00:00+00:00","2020-04-08T00:00:00+00:00","2020-04-09T00:00:00+00:00","2020-04-13T00:00:00+00:00","2020-04-14T00:00:00+00:00","2020-04-15T00:00:00+00:00","2020-04-16T00:00:00+00:00","2020-04-17T00:00:00+00:00","2020-04-20T00:00:00+00:00","2020-04-21T00:00:00+00:00","2020-04-22T00:00:00+00:00","2020-04-23T00:00:00+00:00","2020-04-24T00:00:00+00:00","2020-04-27T00:00:00+00:00","2020-04-28T00:00:00+00:00","2020-04-29T00:00:00+00:00","2020-04-30T00:00:00+00:00","2020-05-01T00:00:00+00:00","2020-05-04T00:00:00+00:00","2020-05-05T00:00:00+00:00","2020-05-06T00:00:00+00:00","2020-05-07T00:00:00+00:00","2020-05-08T00:00:00+00:00","2020-05-11T00:00:00+00:00","2020-05-12T00:00:00+00:00","2020-05-13T00:00:00+00:00","2020-05-14T00:00:00+00:00","2020-05-15T00:00:00+00:00","2020-05-18T00:00:00+00:00","2020-05-19T00:00:00+00:00","2020-05-20T00:00:00+00:00","2020-05-21T00:00:00+00:00","2020-05-22T00:00:00+00:00","2020-05-26T00:00:00+00:00","2020-05-27T00:00:00+00:00","2020-05-28T00:00:00+00:00","2020-05-29T00:00:00+00:00","2020-06-01T00:00:00+00:00","2020-06-02T00:00:00+00:00","2020-06-03T00:00:00+00:00","2020-06-04T00:00:00+00:00","2020-06-05T00:00:00+00:00","2020-06-08T00:00:00+00:00","2020-06-09T00:00:00+00:00","2020-06-10T00:00:00+00:00","2020-06-11T00:00:00+00:00","2020-06-12T00:00:00+00:00","2020-06-15T00:00:00+00:00","2020-06-16T00:00:00+00:00","2020-06-17T00:00:00+00:00","2020-06-18T00:00:00+00:00","2020-06-19T00:00:00+00:00","2020-06-22T00:00:00+00:00","2020-06-23T00:00:00+00:00","2020-06-24T00:00:00+00:00","2020-06-25T00:00:00+00:00","2020-06-26T00:00:00+00:00","2020-06-29T00:00:00+00:00","2020-06-30T00:00:00+00:00","2020-07-01T00:00:00+00:00","2020-07-02T00:00:00+00:00","2020-07-06T00:00:00+00:00","2020-07-07T00:00:00+00:00","2020-07-08T00:00:00+00:00","2020-07-09T00:00:00+00:00","2020-07-10T00:00:00+00:00","2020-07-13T00:00:00+00:00","2020-07-14T00:00:00+00:00","2020-07-15T00:00:00+00:00","2020-07-16T00:00:00+00:00","2020-07-17T00:00:00+00:00","2020-07-20T00:00:00+00:00","2020-07-21T00:00:00+00:00","2020-07-22T00:00:00+00:00","2020-07-23T00:00:00+00:00","2020-07-24T00:00:00+00:00","2020-07-27T00:00:00+00:00","2020-07-28T00:00:00+00:00","2020-07-29T00:00:00+00:00","2020-07-30T00:00:00+00:00","2020-07-31T00:00:00+00:00","2020-08-03T00:00:00+00:00","2020-08-04T00:00:00+00:00","2020-08-05T00:00:00+00:00","2020-08-06T00:00:00+00:00","2020-08-07T00:00:00+00:00","2020-08-10T00:00:00+00:00","2020-08-11T00:00:00+00:00","2020-08-12T00:00:00+00:00","2020-08-13T00:00:00+00:00","2020-08-14T00:00:00+00:00","2020-08-17T00:00:00+00:00","2020-08-18T00:00:00+00:00","2020-08-19T00:00:00+00:00","2020-08-20T00:00:00+00:00","2020-08-21T00:00:00+00:00","2020-08-24T00:00:00+00:00","2020-08-25T00:00:00+00:00","2020-08-26T00:00:00+00:00","2020-08-27T00:00:00+00:00","2020-08-28T00:00:00+00:00","2020-08-31T00:00:00+00:00","2020-09-01T00:00:00+00:00","2020-09-02T00:00:00+00:00","2020-09-03T00:00:00+00:00","2020-09-04T00:00:00+00:00","2020-09-08T00:00:00+00:00","2020-09-09T00:00:00+00:00","2020-09-10T00:00:00+00:00","2020-09-11T00:00:00+00:00","2020-09-14T00:00:00+00:00","2020-09-15T00:00:00+00:00","2020-09-16T00:00:00+00:00","2020-09-17T00:00:00+00:00","2020-09-18T00:00:00+00:00","2020-09-21T00:00:00+00:00","2020-09-22T00:00:00+00:00","2020-09-23T00:00:00+00:00","2020-09-24T00:00:00+00:00","2020-09-25T00:00:00+00:00","2020-09-28T00:00:00+00:00","2020-09-29T00:00:00+00:00","2020-09-30T00:00:00+00:00","2020-10-01T00:00:00+00:00","2020-10-02T00:00:00+00:00","2020-10-05T00:00:00+00:00","2020-10-06T00:00:00+00:00","2020-10-07T00:00:00+00:00","2020-10-08T00:00:00+00:00","2020-10-09T00:00:00+00:00","2020-10-12T00:00:00+00:00","2020-10-13T00:00:00+00:00","2020-10-14T00:00:00+00:00","2020-10-15T00:00:00+00:00","2020-10-16T00:00:00+00:00","2020-10-19T00:00:00+00:00","2020-10-20T00:00:00+00:00","2020-10-21T00:00:00+00:00","2020-10-22T00:00:00+00:00","2020-10-23T00:00:00+00:00","2020-10-26T00:00:00+00:00","2020-10-27T00:00:00+00:00","2020-10-28T00:00:00+00:00","2020-10-29T00:00:00+00:00","2020-10-30T00:00:00+00:00","2020-11-02T00:00:00+00:00","2020-11-03T00:00:00+00:00","2020-11-04T00:00:00+00:00","2020-11-05T00:00:00+00:00","2020-11-06T00:00:00+00:00","2020-11-09T00:00:00+00:00","2020-11-10T00:00:00+00:00","2020-11-11T00:00:00+00:00","2020-11-12T00:00:00+00:00","2020-11-13T00:00:00+00:00","2020-11-16T00:00:00+00:00","2020-11-17T00:00:00+00:00","2020-11-18T00:00:00+00:00","2020-11-19T00:00:00+00:00","2020-11-20T00:00:00+00:00","2020-11-23T00:00:00+00:00","2020-11-24T00:00:00+00:00","2020-11-25T00:00:00+00:00","2020-11-27T00:00:00+00:00","2020-11-30T00:00:00+00:00","2020-12-01T00:00:00+00:00","2020-12-02T00:00:00+00:00","2020-12-03T00:00:00+00:00","2020-12-04T00:00:00+00:00","2020-12-07T00:00:00+00:00","2020-12-08T00:00:00+00:00","2020-12-09T00:00:00+00:00","2020-12-10T00:00:00+00:00","2020-12-11T00:00:00+00:00","2020-12-14T00:00:00+00:00","2020-12-15T00:00:00+00:00","2020-12-16T00:00:00+00:00","2020-12-17T00:00:00+00:00","2020-12-18T00:00:00+00:00","2020-12-21T00:00:00+00:00","2020-12-22T00:00:00+00:00","2020-12-23T00:00:00+00:00","2020-12-24T00:00:00+00:00","2020-12-28T00:00:00+00:00","2020-12-29T00:00:00+00:00","2020-12-30T00:00:00+00:00","2020-12-31T00:00:00+00:00","2021-01-04T00:00:00+00:00","2021-01-05T00:00:00+00:00","2021-01-06T00:00:00+00:00","2021-01-07T00:00:00+00:00","2021-01-08T00:00:00+00:00","2021-01-11T00:00:00+00:00","2021-01-12T00:00:00+00:00","2021-01-13T00:00:00+00:00","2021-01-14T00:00:00+00:00","2021-01-15T00:00:00+00:00","2021-01-19T00:00:00+00:00","2021-01-20T00:00:00+00:00","2021-01-21T00:00:00+00:00","2021-01-22T00:00:00+00:00","2021-01-25T00:00:00+00:00","2021-01-26T00:00:00+00:00","2021-01-27T00:00:00+00:00","2021-01-28T00:00:00+00:00","2021-01-29T00:00:00+00:00","2021-02-01T00:00:00+00:00","2021-02-02T00:00:00+00:00","2021-02-03T00:00:00+00:00","2021-02-04T00:00:00+00:00","2021-02-05T00:00:00+00:00","2021-02-08T00:00:00+00:00","2021-02-09T00:00:00+00:00","2021-02-10T00:00:00+00:00","2021-02-11T00:00:00+00:00","2021-02-12T00:00:00+00:00","2021-02-16T00:00:00+00:00","2021-02-17T00:00:00+00:00","2021-02-18T00:00:00+00:00","2021-02-19T00:00:00+00:00","2021-02-22T00:00:00+00:00","2021-02-23T00:00:00+00:00","2021-02-24T00:00:00+00:00","2021-02-25T00:00:00+00:00","2021-02-26T00:00:00+00:00","2021-03-01T00:00:00+00:00","2021-03-02T00:00:00+00:00","2021-03-03T00:00:00+00:00","2021-03-04T00:00:00+00:00","2021-03-05T00:00:00+00:00","2021-03-08T00:00:00+00:00","2021-03-09T00:00:00+00:00","2021-03-10T00:00:00+00:00","2021-03-11T00:00:00+00:00","2021-03-12T00:00:00+00:00","2021-03-15T00:00:00+00:00","2021-03-16T00:00:00+00:00","2021-03-17T00:00:00+00:00","2021-03-18T00:00:00+00:00","2021-03-19T00:00:00+00:00","2021-03-22T00:00:00+00:00","2021-03-23T00:00:00+00:00","2021-03-24T00:00:00+00:00","2021-03-25T00:00:00+00:00","2021-03-26T00:00:00+00:00","2021-03-29T00:00:00+00:00","2021-03-30T00:00:00+00:00","2021-03-31T00:00:00+00:00","2021-04-01T00:00:00+00:00","2021-04-05T00:00:00+00:00","2021-04-06T00:00:00+00:00","2021-04-07T00:00:00+00:00","2021-04-08T00:00:00+00:00","2021-04-09T00:00:00+00:00","2021-04-12T00:00:00+00:00","2021-04-13T00:00:00+00:00","2021-04-14T00:00:00+00:00","2021-04-15T00:00:00+00:00","2021-04-16T00:00:00+00:00","2021-04-19T00:00:00+00:00","2021-04-20T00:00:00+00:00","2021-04-21T00:00:00+00:00","2021-04-22T00:00:00+00:00","2021-04-23T00:00:00+00:00","2021-04-26T00:00:00+00:00","2021-04-27T00:00:00+00:00","2021-04-28T00:00:00+00:00","2021-04-29T00:00:00+00:00","2021-04-30T00:00:00+00:00","2021-05-03T00:00:00+00:00","2021-05-04T00:00:00+00:00","2021-05-05T00:00:00+00:00","2021-05-06T00:00:00+00:00","2021-05-07T00:00:00+00:00","2021-05-10T00:00:00+00:00","2021-05-11T00:00:00+00:00","2021-05-12T00:00:00+00:00","2021-05-13T00:00:00+00:00","2021-05-14T00:00:00+00:00","2021-05-17T00:00:00+00:00","2021-05-18T00:00:00+00:00","2021-05-19T00:00:00+00:00","2021-05-20T00:00:00+00:00","2021-05-21T00:00:00+00:00","2021-05-24T00:00:00+00:00","2021-05-25T00:00:00+00:00","2021-05-26T00:00:00+00:00","2021-05-27T00:00:00+00:00","2021-05-28T00:00:00+00:00","2021-06-01T00:00:00+00:00","2021-06-02T00:00:00+00:00","2021-06-03T00:00:00+00:00","2021-06-04T00:00:00+00:00","2021-06-07T00:00:00+00:00","2021-06-08T00:00:00+00:00","2021-06-09T00:00:00+00:00","2021-06-10T00:00:00+00:00","2021-06-11T00:00:00+00:00","2021-06-14T00:00:00+00:00","2021-06-15T00:00:00+00:00","2021-06-16T00:00:00+00:00","2021-06-17T00:00:00+00:00","2021-06-18T00:00:00+00:00","2021-06-21T00:00:00+00:00","2021-06-22T00:00:00+00:00","2021-06-23T00:00:00+00:00","2021-06-24T00:00:00+00:00","2021-06-25T00:00:00+00:00","2021-06-28T00:00:00+00:00","2021-06-29T00:00:00+00:00","2021-06-30T00:00:00+00:00","2021-07-01T00:00:00+00:00","2021-07-02T00:00:00+00:00","2021-07-06T00:00:00+00:00","2021-07-07T00:00:00+00:00","2021-07-08T00:00:00+00:00","2021-07-09T00:00:00+00:00","2021-07-12T00:00:00+00:00","2021-07-13T00:00:00+00:00","2021-07-14T00:00:00+00:00","2021-07-15T00:00:00+00:00","2021-07-16T00:00:00+00:00","2021-07-19T00:00:00+00:00","2021-07-20T00:00:00+00:00","2021-07-21T00:00:00+00:00","2021-07-22T00:00:00+00:00","2021-07-23T00:00:00+00:00","2021-07-26T00:00:00+00:00","2021-07-27T00:00:00+00:00","2021-07-28T00:00:00+00:00","2021-07-29T00:00:00+00:00","2021-07-30T00:00:00+00:00","2021-08-02T00:00:00+00:00","2021-08-03T00:00:00+00:00","2021-08-04T00:00:00+00:00","2021-08-05T00:00:00+00:00","2021-08-06T00:00:00+00:00","2021-08-09T00:00:00+00:00","2021-08-10T00:00:00+00:00","2021-08-11T00:00:00+00:00","2021-08-12T00:00:00+00:00","2021-08-13T00:00:00+00:00","2021-08-16T00:00:00+00:00","2021-08-17T00:00:00+00:00","2021-08-18T00:00:00+00:00","2021-08-19T00:00:00+00:00","2021-08-20T00:00:00+00:00","2021-08-23T00:00:00+00:00","2021-08-24T00:00:00+00:00","2021-08-25T00:00:00+00:00","2021-08-26T00:00:00+00:00","2021-08-27T00:00:00+00:00","2021-08-30T00:00:00+00:00","2021-08-31T00:00:00+00:00","2021-09-01T00:00:00+00:00","2021-09-02T00:00:00+00:00","2021-09-03T00:00:00+00:00","2021-09-07T00:00:00+00:00","2021-09-08T00:00:00+00:00","2021-09-09T00:00:00+00:00","2021-09-10T00:00:00+00:00","2021-09-13T00:00:00+00:00","2021-09-14T00:00:00+00:00","2021-09-15T00:00:00+00:00","2021-09-16T00:00:00+00:00","2021-09-17T00:00:00+00:00","2021-09-20T00:00:00+00:00","2021-09-21T00:00:00+00:00","2021-09-22T00:00:00+00:00","2021-09-23T00:00:00+00:00","2021-09-24T00:00:00+00:00","2021-09-27T00:00:00+00:00","2021-09-28T00:00:00+00:00","2021-09-29T00:00:00+00:00","2021-09-30T00:00:00+00:00","2021-10-01T00:00:00+00:00","2021-10-04T00:00:00+00:00","2021-10-05T00:00:00+00:00","2021-10-06T00:00:00+00:00","2021-10-07T00:00:00+00:00","2021-10-08T00:00:00+00:00","2021-10-11T00:00:00+00:00","2021-10-12T00:00:00+00:00","2021-10-13T00:00:00+00:00","2021-10-14T00:00:00+00:00","2021-10-15T00:00:00+00:00","2021-10-18T00:00:00+00:00","2021-10-19T00:00:00+00:00","2021-10-20T00:00:00+00:00","2021-10-21T00:00:00+00:00","2021-10-22T00:00:00+00:00","2021-10-25T00:00:00+00:00","2021-10-26T00:00:00+00:00","2021-10-27T00:00:00+00:00","2021-10-28T00:00:00+00:00","2021-10-29T00:00:00+00:00","2021-11-01T00:00:00+00:00","2021-11-02T00:00:00+00:00","2021-11-03T00:00:00+00:00","2021-11-04T00:00:00+00:00","2021-11-05T00:00:00+00:00","2021-11-08T00:00:00+00:00","2021-11-09T00:00:00+00:00","2021-11-10T00:00:00+00:00","2021-11-11T00:00:00+00:00","2021-11-12T00:00:00+00:00","2021-11-15T00:00:00+00:00","2021-11-16T00:00:00+00:00","2021-11-17T00:00:00+00:00","2021-11-18T00:00:00+00:00","2021-11-19T00:00:00+00:00","2021-11-22T00:00:00+00:00","2021-11-23T00:00:00+00:00","2021-11-24T00:00:00+00:00","2021-11-26T00:00:00+00:00","2021-11-29T00:00:00+00:00","2021-11-30T00:00:00+00:00","2021-12-01T00:00:00+00:00","2021-12-02T00:00:00+00:00","2021-12-03T00:00:00+00:00","2021-12-06T00:00:00+00:00","2021-12-07T00:00:00+00:00","2021-12-08T00:00:00+00:00","2021-12-09T00:00:00+00:00","2021-12-10T00:00:00+00:00","2021-12-13T00:00:00+00:00","2021-12-14T00:00:00+00:00","2021-12-15T00:00:00+00:00","2021-12-16T00:00:00+00:00","2021-12-17T00:00:00+00:00","2021-12-20T00:00:00+00:00","2021-12-21T00:00:00+00:00","2021-12-22T00:00:00+00:00","2021-12-23T00:00:00+00:00","2021-12-27T00:00:00+00:00","2021-12-28T00:00:00+00:00","2021-12-29T00:00:00+00:00","2021-12-30T00:00:00+00:00","2021-12-31T00:00:00+00:00","2022-01-03T00:00:00+00:00","2022-01-04T00:00:00+00:00","2022-01-05T00:00:00+00:00","2022-01-06T00:00:00+00:00","2022-01-07T00:00:00+00:00","2022-01-10T00:00:00+00:00","2022-01-11T00:00:00+00:00","2022-01-12T00:00:00+00:00","2022-01-13T00:00:00+00:00","2022-01-14T00:00:00+00:00","2022-01-18T00:00:00+00:00","2022-01-19T00:00:00+00:00","2022-01-20T00:00:00+00:00","2022-01-21T00:00:00+00:00","2022-01-24T00:00:00+00:00","2022-01-25T00:00:00+00:00","2022-01-26T00:00:00+00:00","2022-01-27T00:00:00+00:00","2022-01-28T00:00:00+00:00","2022-01-31T00:00:00+00:00","2022-02-01T00:00:00+00:00","2022-02-02T00:00:00+00:00","2022-02-03T00:00:00+00:00","2022-02-04T00:00:00+00:00","2022-02-07T00:00:00+00:00","2022-02-08T00:00:00+00:00","2022-02-09T00:00:00+00:00","2022-02-10T00:00:00+00:00","2022-02-11T00:00:00+00:00","2022-02-14T00:00:00+00:00","2022-02-15T00:00:00+00:00","2022-02-16T00:00:00+00:00","2022-02-17T00:00:00+00:00","2022-02-18T00:00:00+00:00","2022-02-22T00:00:00+00:00","2022-02-23T00:00:00+00:00","2022-02-24T00:00:00+00:00","2022-02-25T00:00:00+00:00","2022-02-28T00:00:00+00:00"],"y":[45.772934,45.948635,46.012543,46.204227,46.355965,46.419868,45.94067,46.371952,46.451817,45.924671,46.531696,46.100395,45.253784,44.255421,44.447109,44.247437,44.463078,43.935944,43.416801,44.0797,44.64677,44.423138,43.041412,43.113274,42.777843,43.34491,43.281017,43.744274,43.744274,44.479053,44.726646,44.982231,45.141968,45.070091,44.526974,44.926323,44.910347,44.910347,45.445477,45.58923,45.645142,45.764954,46.491741,46.499737,46.595596,46.843185,47.018875,47.018875,47.002922,47.386284,47.689793,47.633892,47.322395,47.657841,47.993301,47.713753,47.630363,47.622334,47.902985,47.927059,47.782715,48.127506,48.592587,48.728897,48.472305,48.584576,48.913326,49.025593,49.0737,49.675091,49.739246,48.961449,49.089745,49.554813,49.514725,49.755283,50.09206,49.915653,48.712887,49.033615,49.675091,48.825127,49.482651,48.279861,47.886955,46.275219,45.401196,47.486019,47.445919,48.207695,47.630363,46.772377,46.804447,46.138889,45.810135,44.030014,44.671509,44.110203,44.110203,43.925785,45.465336,44.976215,44.046055,45.192711,45.43327,43.837559,43.12392,43.492771,43.292305,44.62339,44.89603,44.904045,45.914387,45.866276,45.946472,45.978535,45.80212,45.016289,44.839901,44.130051,44.355652,44.210617,42.816685,42.349373,42.16404,41.962631,42.091537,43.461281,43.888321,44.22673,44.242836,44.96801,44.959949,45.000237,43.694946,43.944729,44.452332,43.896385,44.871326,45.330597,45.886536,45.830143,45.507832,45.274189,45.322521,46.297462,46.047695,46.410263,46.313576,46.144382,46.386089,46.096031,44.734356,44.484562,44.315357,44.275074,44.903557,45.008289,44.218681,44.065578,43.831924,43.203453,43.340424,43.074528,43.815807,43.179279,43.187328,44.452332,44.919647,45.515911,45.024406,45.258072,45.483685,45.677048,46.257175,46.249119,46.402218,46.386089,46.410263,47.175713,47.06292,46.813133,46.410263,47.395935,47.193447,47.460724,47.379723,47.274441,47.48502,47.063869,48.04385,47.995262,47.930477,48.25444,48.327328,48.497402,48.878075,48.748474,48.788979,49.056248,48.343529,48.829475,48.91045,49.056248,49.218231,49.210133,49.112942,49.112942,49.177734,49.185829,49.574577,49.777058,50.748966,50.975739,50.91095,50.514084,50.73275,50.595081,49.955246,49.882347,49.088634,49.169643,49.858078,50.060535,50.068645,49.355915,50.149612,49.809467,49.736568,49.428802,50.489784,51.137722,51.299683,51.283489,51.340195,51.485977,51.672241,52.004333,52.020523,52.036705,51.809944,52.117706,52.214897,52.352577,52.668449,52.868759,52.795464,52.828033,52.876892,52.958313,52.893181,52.868759,53.414276,53.308437,53.601555,53.479427,53.357277,53.398026,53.609707,54.090115,54.008678,54.358807,54.497234,53.84584,53.68298,53.772564,54.073826,54.1064,54.41581,54.578655,53.601555,53.951691,54.83923,54.774071,54.896198,55.050934,55.417316,55.69418,55.547596,55.612743,55.97913,56.125698,55.946575,56.312988,56.516544,56.62241,55.409172,54.945076,54.953201,55.645317,55.946575,55.002079,55.116077,56.117577,55.743027,55.238194,55.734867,55.653473,54.586796,54.953201,54.619377,54.041271,53.104874,53.715569,53.967968,54.814789,54.635658,54.765938,55.287025,55.485725,55.354916,55.763725,56.180729,56.180729,56.450569,56.499626,56.53233,56.679523,56.515991,56.278881,56.074451,55.600208,55.632908,55.665615,55.935444,55.264957,55.583847,56.393333,56.671345,56.61409,57.112858,57.488956,57.660652,57.807865,57.709732,57.390858,56.96566,56.507801,56.753098,57.055607,57.562565,56.973877,57.243683,56.720398,56.327927,56.278881,56.818497,56.957512,56.53233,55.812805,55.755547,55.98452,56.319748,56.581379,57.194626,55.894558,55.788258,55.215908,54.578156,54.553604,54.267445,54.651737,53.874977,53.866783,54.610844,53.637852,53.727802,53.89949,54.152973,54.978779,54.67627,54.586323,53.964722,54.424561,55.179993,55.664478,56.165375,56.970074,56.961864,57.076805,57.692669,57.282101,56.222858,55.976501,56.189991,55.713741,56.066822,55.549511,56.485596,56.452747,57.142502,57.241047,56.871525,56.633411,55.385284,55.196423,54.892609,54.646263,53.20108,53.496696,50.811581,50.598095,47.091854,49.448509,47.346401,49.514198,49.842651,50.926544,50.442074,50.450294,48.20039,47.338196,47.346401,48.972256,49.670219,48.840866,49.65379,51.205723,51.345318,51.550598,50.967602,49.571663,49.259663,50.729473,50.121826,48.816242,49.161106,49.65379,50.359962,51.238575,51.484898,50.942959,50.762325,49.111839,47.600971,47.878319,49.009247,49.628357,48.497433,48.852398,47.647186,46.070507,47.226192,48.192013,49.124809,48.67905,50.354782,50.396057,50.874844,50.808811,51.733356,50.684978,51.708591,51.023445,51.254574,52.220387,53.095409,52.014011,52.575359,54.391438,54.440964,53.062397,51.650803,52.501064,53.524651,53.235733,53.508144,54.152039,52.088314,52.550591,53.590706,53.062397,53.384331,52.492802,51.634293,51.560009,50.602436,50.363041,49.240383,49.083542,50.561165,50.718006,52.864285,52.864285,52.8395,53.417343,53.442112,53.582462,52.368984,53.301777,52.484558,51.923222,51.328861,51.518734,51.692081,51.089481,52.690933,52.79137,53.339298,53.746086,53.754391,53.098541,53.621555,53.380814,54.136272,54.161175,54.401928,54.27739,54.401928,54.941547,55.041176,55.174,54.916637,55.041176,55.705326,56.045692,56.062294,56.078896,56.07061,56.585312,56.294746,56.336254,56.112091,56.087204,56.734753,56.834354,57.68116,57.647972,57.780773,57.905308,57.996593,57.540028,57.980026,57.897011,57.647972,58.337009,58.420029,58.395119,58.212463,58.536243,58.619278,58.669075,58.835114,58.453236,58.90152,58.61097,58.345303,57.407188,57.905308,58.50304,58.793625,58.752094,59.83963,59.715115,60.063797,60.155106,60.379246,60.171715,60.105301,59.623783,59.86454,60.696617,60.496532,60.196358,60.08799,60.246403,60.754978,60.554878,59.879536,59.837852,59.137527,58.028633,58.512211,59.395992,58.670624,58.612274,59.504353,59.295948,58.954094,59.045792,58.537209,58.728989,59.596085,60.029602,60.171368,59.862873,60.246403,60.112991,59.587738,58.595577,58.637276,58.420494,58.070328,58.220398,58.061977,57.403328,57.094845,56.828041,55.844238,55.377323,56.402832,56.419491,56.519562,56.62793,56.494564,57.178204,56.27776,56.13604,54.693653,54.643646,55.060513,56.344448,56.27776,56.769684,55.977623,56.586254,56.169384,56.75301,57.336613,57.503391,58.112003,58.003639,56.694645,57.153214,56.219692,56.48785,57.032532,56.915211,58.406799,58.566021,59.043667,58.800659,58.247608,58.105152,57.585606,57.543682,57.317444,58.214077,58.071621,58.457096,58.876099,58.976635,58.440327,57.79509,57.275543,57.283913,58.155411,59.303425,59.244793,58.917973,58.68335,58.281113,59.462654,59.613495,59.973827,60.049232,60.099522,60.216827,60.166557,60.149784,60.283875,60.744751,60.862064,60.85371,60.728008,60.652569,60.200085,60.543636,60.535259,60.526894,60.602295,60.149784,60.510113,60.468227,60.468227,61.674923,61.976578,61.607857,61.741962,62.001717,62.931873,63.267048,63.007294,62.898357,62.94865,62.864834,62.881599,62.741802,62.101845,61.689255,62.303936,62.017628,62.12711,62.21973,62.447086,62.918644,62.901802,62.657608,62.026058,61.663994,61.756588,61.487148,61.992401,62.632339,62.927074,62.800758,61.714489,61.714489,60.931366,60.76297,60.931366,60.86401,60.897694,61.663994,61.024014,61.217686,61.680832,60.299824,59.533566,59.592491,59.626202,59.398842,58.565205,58.413628,58.733616,59.946182,60.013546,60.165108,60.948215,60.872437,60.552444,61.032417,61.386105,61.369225,61.099796,61.015587,61.133484,61.327152,61.503963,61.579754,61.967102,61.992401,61.638695,61.377659,62.143951,62.86813,62.480774,62.824295,62.289906,62.077839,61.780922,61.73003,61.059898,62.162651,63.740471,63.647163,63.969521,63.799843,63.630226,63.825294,64.266457,64.274879,64.207047,64.325813,64.274879,64.716026,64.894135,65.233482,65.309807,65.386154,65.750916,65.691559,65.912094,65.649139,65.530365,66.166573,65.470985,66.090248,66.20903,66.090248,66.488945,66.455002,66.565269,66.6586,66.743431,66.667076,67.133644,66.24292,65.793358,66.429573,65.208023,65.623672,66.429573,66.370171,66.522865,66.82827,67.498413,67.600204,67.761391,68.092209,68.287323,68.168564,68.295792,68.677521,68.601173,68.227928,68.041298,68.550293,67.998909,68.419868,68.189781,68.701096,68.752213,68.990807,68.624367,68.828918,68.078995,68.377228,68.138657,68.556236,68.752213,69.638474,69.902634,69.65551,67.959709,68.965256,67.934135,67.508072,68.155693,68.445442,69.186783,69.289078,69.570251,69.442451,69.88559,70.09861,69.382797,70.10714,70.763298,71.035988,71.427994,71.760338,71.521729,71.837036,71.854065,72.595444,72.910744,72.612495,73.353874,73.345367,73.46463,72.740334,72.603958,72.510231,72.978912,72.48465,72.782928,71.81147,72.101181,71.751801,70.737732,71.402405,72.288658,72.314201,71.581375,70.976334,72.07563,71.66658,72.16938,72.714737,71.785896,70.004906,70.183823,69.281418,69.974915,70.625626,71.147888,70.779732,71.370476,71.293434,71.396149,72.158173,72.543449,73.057129,73.091393,74.153046,74.22155,74.504066,74.170197,74.409904,74.829414,74.94072,75.103394,74.966423,74.666763,74.94072,74.94931,74.692413,74.769501,74.846527,75.79689,75.831161,75.839714,75.343132,75.043457,75.351677,75.16333,75.111961,75.274635,74.880783,73.767761,73.528038,73.031464,73.510925,73.08284,73.733536,74.041756,73.853378,72.543449,72.808868,73.005783,72.577675,72.945839,73.596542,73.682137,73.742096,74.572556,75.154762,75.351677,75.120514,75.274635,75.711273,76.113686,77.004097,76.884232,76.370529,76.025543,75.905121,75.767456,76.059952,75.75885,75.40612,76.059952,75.930916,75.285683,75.836273,75.156624,74.158669,74.107056,75.776062,76.309456,76.644974,76.059952,77.12674,77.694542,78.245148,78.202141,78.649506,78.279564,78.580666,78.864571,78.924782,79.320534,78.855965,78.62368,78.726906,79.088234,78.855965,79.062469,78.012863,79.053818,79.105453,78.985031,79.656052,80.034607,80.387314,80.008797,79.776512,79.527,80.241081,80.636803,80.550781,80.628197,80.860451,80.748657,80.576576,80.266884,80.155045,79.922737,80.748657,80.929314,80.662605,79.664665,79.518394,79.544205,80.103409,79.862526,81.152992,81.041161,81.580818,82.065132,82.289986,82.679176,82.627274,82.653214,82.955887,82.229431,82.212151,81.995926,82.523476,82.584023,82.653214,82.886711,81.840271,82.739708,83.154831,83.094284,82.7743,83.051048,83.206711,82.488907,80.707344,80.188416,80.750565,79.920326,80.854332,80.326759,78.441414,79.038185,78.917091,79.87706,80.906242,81.00135,81.92675,82.039192,82.54081,82.938629,83.180779,82.627274,83.180779,83.111588,83.587234,83.552658,83.639145,84.080208,84.235893,83.621864,84.936401,84.953728,85.109375,85.08342,85.022896,84.547234,84.607788,83.630478,83.526711,84.19265,84.910454,84.42617,84.849915,84.538567,84.045631,84.343567,83.639977,83.466255,83.839752,84.664955,85.325119,85.594391,85.359848,84.187202,83.179588,83.544418,84.543327,82.684494,81.867966,82.423897,82.988525,83.857132,84.022171,84.360909,84.795258,84.578079,84.69101,83.865814,83.98745,84.404366,84.717079,84.743118,84.67363,84.812637,84.03083,84.421745,84.195885,84.404366,85.359848,85.307724,84.838654,84.0569,84.413048,84.803932,84.169853,84.821304,85.107941,85.550957,86.115547,86.019997,86.480362,86.497757,86.602013,86.567238,86.749641,87.453232,87.904915,88.052605,88.03521,87.739883,87.114494,87.401115,87.470619,87.783318,88.434807,88.539017,88.74749,88.721428,88.102173,88.546989,88.494637,88.730179,88.765038,89.401741,89.349426,89.820419,89.349426,88.66909,89.052879,88.660393,88.738884,89.166283,88.939499,89.166283,88.137047,89.087769,88.878433,89.331993,89.541313,89.558762,89.087769,89.061623,88.738884,88.799919,86.994453,86.776421,87.369514,86.60199,86.619377,86.16584,87.160164,87.47419,87.317192,87.93644,88.320236,88.328918,89.122658,89.567482,89.776825,89.977417,89.846581,90.317596,90.483292,90.430969,90.335022,90.605423,90.675186,90.579254,90.387375,90.80603,90.622856,89.986145,90.326279,90.492012,89.907639,89.672157,90.308853,90.4048,90.867081,90.692635,89.855316,89.279663,89.976212,88.565544,89.284019,89.12632,88.70575,87.540398,87.584221,88.565544,88.39032,87.02346,88.469162,86.64669,85.56897,84.202118,84.464943,83.991829,84.175835,85.09584,85.945732,87.645538,86.962112,88.013535,88.626877,88.504196,89.652008,89.520607,90.072601,91.141541,91.194107,90.861183,91.290512,91.737373,91.807487,92.087845,92.175461,92.157928,92.140411,92.149185,92.140411,92.68364,92.447105,92.70993,93.200615,93.542343,93.524811,93.7351,93.402138,92.613564,93.191856,93.612411,93.498528,93.708817,93.042885,93.095459,91.570892,91.991447,90.545738,89.888611,89.222687,91.132805,93.288239,93.673767,94.044586,94.255974,94.28241,94.564262,94.766823,94.300011,93.366341,93.295883,91.692795,90.794357,91.877769,93.498482,92.723343,92.062721,91.851349,91.349266,90.424461,91.622314,91.754448,92.168442,93.568947,93.146133,93.533714,92.441498,91.190735,92.062721,90.811974,91.833725,93.243042,92.91713,93.938873,93.657013,93.190193,94.141472,94.185501,95.092735,95.541946,95.691711,95.744553,95.718109,96.255424,96.24662,96.46682,96.431564,96.369926,96.026428,96.61657,96.2202,95.832657,95.982399,94.652321,95.004669,93.56012,93.489662,94.661148,94.132652,95.312965,95.12796,96.2202,95.876671,96.678223,96.510864,95.999985,94.50267,94.316841,94.573441,95.776985,94.909737,94.73275,95.033638,95.688499,95.369911,95.776985,96.113251,96.564552,96.184044,96.325653,96.812347,96.768105,95.644241,96.422997,96.396446,96.80352,97.104385,97.210556,96.706161,96.962799,96.538025,95.493805,96.369881,96.706161,95.55574,95.290276,95.653084,96.856613,96.449532,96.166359,96.219437,97.175186,97.290222,97.653061,97.60881,97.546867,97.794647,97.60881,96.591125,97.476059,97.387573,96.732712,96.971642,96.900826,97.237129,96.458374,96.378754,95.78582,95.706169,96.874313,97.148643,96.51149,96.069008,96.599968,96.75927,97.715012,97.290222,97.883141,97.980476,97.237129,96.936249,96.881157,94.854828,95.112556,95.734688,95.610237,95.361412,95.885757,94.286041,94.481567,95.628029,96.712288,97.147766,97.023346,97.698769,97.725441,97.734337,97.307755,97.165535,96.605644,95.592476,94.961479,96.090164,96.747818,96.82782,96.730057,96.383453,96.223473,96.543427,95.752457,95.476952,96.667854,95.796898,95.894646,95.761353,96.125732,96.738968,96.490105,95.628029,93.548378,90.855515,87.202812,86.162987,89.291351,91.450989,91.522087,90.837738,88.171539,89.833481,89.940109,88.660332,90.899948,89.655716,90.117867,90.526688,90.180061,91.317665,92.090858,91.966461,90.508911,90.873291,89.655716,89.477959,89.158028,88.977577,86.592575,86.574715,88.182579,88.361259,89.64753,91.300064,91.005295,91.82708,92.622086,92.747147,92.76503,92.086128,91.612686,92.997253,93.354576,93.461746,93.354576,92.622086,93.979843,94.962425,94.739113,94.426468,95.668098,95.525192,95.158951,96.409515,96.650681,96.418434,96.311249,96.329109,95.36441,95.578773,95.167877,93.747589,92.756073,94.104889,93.988777,95.489464,95.36441,95.775291,95.677025,95.927147,95.998627,96.141525,95.730614,96.632828,95.623428,94.194214,95.927147,95.176811,94.605141,93.792252,94.06913,92.220116,92.550644,93.542137,94.899895,93.506424,91.91642,92.646774,93.482231,94.650093,94.533295,94.326691,95.332817,94.614159,93.697853,92.296417,92.503021,91.263329,89.026421,88.011276,87.957382,88.55928,86.277481,87.598061,85.756432,85.666618,84.822151,85.154541,86.969215,85.540825,86.843452,85.855247,86.196594,88.334702,88.352646,86.645798,87.166824,87.373466,85.576759,84.2472,84.139412,84.193306,83.151215,84.768257,86.313416,87.741768,87.391426,87.409393,88.685051,87.61602,88.065216,89.107254,89.035423,88.325722,90.427864,90.930916,91.380096,91.694527,91.883171,90.742264,91.182472,91.155495,92.700668,92.592865,92.27697,92.854614,93.567604,93.982803,94.073059,94.045967,93.278801,93.251732,93.314896,94.352867,94.713867,94.60556,95.201233,94.804131,93.883514,94.912422,93.775215,94.045967,93.784256,94.695824,95.805946,95.769867,95.724731,96.365547,96.636314,96.735596,96.257248,96.392616,96.157951,96.419701,96.654366,95.787895,95.228333,95.968414,95.002708,94.443085,94.407005,94.749977,94.849243,95.977448,95.101982,95.065849,94.244545,95.183197,94.325783,94.316734,93.946686,94.668747,94.551414,95.81498,96.509949,96.49192,96.925133,96.879974,97.141731,97.466644,97.195892,97.701324,97.890869,98.23381,98.035255,97.024429,96.221138,96.047981,95.948265,96.229286,95.91201,96.564735,96.773209,96.555649,97.888268,94.325539,92.44902,94.144264,95.830421,97.144913,97.335289,96.610046,97.199295,97.153954,98.667877,99.075829,99.900787,99.846367,100.326881,100.218079,100.499107,100.29966,100.834518,100.435638,100.916107,100.653206,100.761986,100.64415,100.771072,101.006744,100.870789,100.109291,100.48098,100.598824,101.432838,101.369385,101.46003,101.160873,101.668533,101.56881,101.940491,101.360321,101.487228,101.804512,101.659477,101.686668,101.958633,101.387505,101.324059,101.115555,101.668533,101.505363,101.215279,101.269653,101.786385,102.076485,102.212456,101.94957,99.41124,100.798256,99.22139,99.230507,100.250778,99.849945,99.995712,99.950172,101.097969,101.835846,101.252823,100.451157,101.025093,101.617203,100.624268,101.416809,101.107071,100.651604,101.043304,101.079765,100.697166,101.243729,99.895515,100.013908,99.658653,99.63134,99.348915,99.931946,100.250778,100.059471,100.023064,100.524063,100.095909,99.795296,99.458244,99.130295,99.194077,98.456192,97.772972,97.34481,97.281067,99.430908,99.859077,101.097969,101.453232,101.489662,101.753853,102.54641,102.400642,102.919861,102.728584,103.502899,103.803513,103.949257,104.350098,103.748856,103.894615,103.648636,103.220497,103.220497,103.940155,104.468513,105.762062,106.144661,106.718582,106.445267,107.06472,106.099121,106.536385,106.372406,106.673012,107.128586,106.826088,106.551109,106.71611,106.981934,106.092773,106.101952,105.707779,106.51445,107.330261,107.119438,107.458595,107.046089,107.128586,107.467751,107.183609,107.449417,106.972771,107.220268,106.798607,107.165268,106.881088,107.71524,108.641052,108.485214,108.311058,107.577751,107.669418,107.660255,107.733574,108.549377,108.292732,108.283562,108.40274,109.117737,109.576042,110.126022,110.529358,111.134346,111.033516,111.198517,111.858505,111.71183,111.629326,111.81266,112.060158,111.647682,113.150986,112.426819,112.518463,112.078491,111.721001,111.409332,111.46434,111.867668,111.950172,111.546844,112.555153,112.417656,112.353477,112.087646,110.520187,110.666855,110.630188,110.641251,110.512321,111.285789,111.488373,111.902695,111.709358,111.43309,111.488373,111.037163,111.387085,111.304184,111.442322,111.405487,110.834602,110.079567,111.037163,110.797768,110.705688,111.617256,111.258163,112.436775,113.136566,113.164177,113.274681,112.897148,113.191818,113.191818,112.915581,112.998436,113.56012,113.385155,113.348343,113.596947,113.265488,113.044472,113.652199,113.587746,111.543602,111.875092,112.676186,113.274681,113.46804,113.799515,114.278343,114.259918,114.075752,114.03891,115.097809,115.475349,115.328011,115.033371,115.16227,115.364838,115.272797,115.254364,115.825233,115.659508,115.401688,115.328011,116.26722,115.475349,115.341217,115.387474,115.67424,115.757477,114.80471,115.886986,114.961952,115.119202,115.396729,115.52623,114.388451,115.183968,115.267204,115.239471,116.062729,116.247742,116.765762,116.765762,116.839767,117.515022,117.589035,117.431778,117.441025,117.829529,117.746277,117.542786,117.348526,117.283768,117.524292,117.413277,117.14502,117.394783,117.626038,117.302261,117.172752,115.452225,115.66497,116.849022,116.737984,116.950768,115.091461,114.897217,115.063705,116.220009,115.868477,115.683479,115.960999,116.007256,116.053482,116.664009,117.459541,117.727776,116.821259,117.14502,117.089523,117.006287,118.264275,118.717567,118.773056,118.736046,118.958076,119.244812,119.300316,119.494583,119.161575,119.25724,119.062157,119.090027,119.740341,119.935448,120.32563,120.957344,121.198891,121.319672,121.960678,121.877052,121.635521,121.923538,122.090729,121.942085,122.034996,122.118599,122.174355,122.332253,122.360138,123.019745,122.499519,122.713196,122.072151,122.230095,123.224121,122.68531,122.936127,123.038338,123.066208,123.391357,123.623596,123.409935,123.614319,123.112656,123.13121,123.196251,122.973305,122.341583,123.456375,123.224121,123.539993,124.394676,124.301788,124.589767,124.459709,125.695313,125.695313,126.661507,126.410637,126.243408,125.704597,125.630295,126.131935,126.791504,127.088837,127.228134,127.246735,126.652184,127.785553,128.668152,128.147919,128.138596,128.430847,128.384125,128.281433,128.32811,128.608215,128.13205,129.037613,129.78447,130.279266,131.044754,131.343552,131.586273,131.380859,132.491867,133.266693,132.678513,133.901505,133.668121,134.358932,135.423218,135.759338,135.628616,135.684647,137.103653,136.207413,134.788391,134.835098,134.779083,131.91301,126.722404,128.794907,128.225403,123.613617,125.312691,126.98378,127.385208,129.168335,130.746078,130.802063,130.017868,129.411041,129.495071,131.502274,132.883926,131.21286,129.765808,128.225403,129.056305,130.51265,130.98877,131.100815,131.595627,133.780136,133.70546,132.874573,132.202408,132.025055,132.2491,130.550018,130.746078,130.634048,127.433899,124.762428,128.052536,125.784149,125.493553,127.218307,124.321861,125.868523,127.283897,128.174408,125.4842,125.905998,127.94944,127.358917,128.389969,128.024429,129.093002,130.452179,130.630264,129.917862,128.886795,128.839951,127.293304,127.433899,128.643082,128.727463,127.790077,127.986908,127.246414,126.89962,128.540024,129.102402,129.149216,130.367813,131.502014,131.764496,131.830093,131.033356,131.670746,131.708252,131.417633,132.401901,131.914444,132.261292,132.04567,131.792618,130.4897,132.176895,131.26767,132.636246,133.254883,133.451721,134.632797,134.435989,134.895279,135.092087,135.373306,134.867157,135.25145,135.120224,134.970261,134.520325,134.829636,133.901657,134.092743,132.210098,132.511322,131.278137,132.097122,132.191238,132.511322,132.17244,133.292603,134.412781,135.636536,135.984863,135.015259,136.126007,136.21077,135.994247,136.615524,136.916763,136.587296,136.342529,136.559036,136.86026,138.027527,137.80159,136.700226,135.806,136.577881,136.417831,137.227371,137.688629,138.309952,138.761749,138.629959,138.535858,137.669815,137.142685,138.046356,136.954391,138.027527,138.535858,138.799423,139.317139,139.364227,139.091217,139.957245,140.964508,141.049194,141.792862,141.209198,141.265717,141.068024,140.606735,140.183197,139.863129,140.164307,140.635025,140.644394,141.350464,141.463409,140.569138,141.322189,141.341019,142.432922,142.291763,141.774048,141.698746,141.15274,141.482208,141.54274,141.7603,141.552231,141.741364,140.540146,139.632172,139.528152,139.282196,134.846298,131.971008,133.711304,133.115448,136.047485,136.009674,134.042358,133.692429,133.162704,132.377716,128.263351,130.675201,128.414688,127.70533,129.701019,131.053528,132.538483,131.999374,132.642532,133.3992,136.123169,135.858322,134.467957,131.88588,131.696701,130.731964,132.207443,132.491196,130.192825,127.875549,128.499786,127.809349,129.795593,129.956375,132.983017,132.737106,133.692429,135.252991,130.835999,130.675201,127.591797,127.733696,127.70533,128.433594,128.12149,125.908249,123.222107,123.18428,121.292625,119.334778,116.79995,113.880165,119.360123,120.435173,120.282951,121.424599,121.51976,118.703667,122.632866,123.869667,125.144501,125.734367,126.352745,126.381332,125.582153,126.95211,127.313637,128.34111,130.006058,128.226944,128.360184,128.626556,129.834824,128.883438,128.702652,130.643478,131.794647,131.98494,132.926788,133.478577,133.345398,132.080093,132.213257,132.489166,134.144577,134.563187,134.325302,135.799957,136.066315,136.351761,135.88559,136.779892,136.97966,136.722809,136.77037,136.484955,137.398254,136.760849,136.503967,135.466995,134.410965,134.135056,136.104401,136.561066,137.436371,137.322174,137.959595,138.473328,138.444794,137.873962,139.491318,136.580124,136.486359,137.538528,136.907272,137.509827,138.418564,140.025482,140.0159,140.350693,140.647232,141.355057,141.479416,140.628098,141.278503,141.268997,142.215912,142.101151,142.139389,141.613297,141.82373,141.862015,143.287216,143.095947,142.856781,143.631592,143.842026,143.851593,142.780289,142.541153,144.081131,143.621979,141.144623,140.915054,140.465485,141.115936,137.548096,138.79155,139.614182,140.876785,139.881989,138.887207,140.245514,139.738495,137.959396,138.294205,137.079391,136.141998,136.409836,134.573288,134.305466,137.318542,138.370712,139.145477,140.408127,141.192459,141.058563,140.867218,141.517639,141.211578,141.314316,142.860031,143.349655,144.664993,144.684174,143.877716,142.514404,142.24559,143.100052,144.098557,145.116241,145.452255,146.546738,146.460342,145.673065,145.932312,146.527573,146.729156,147.478027,147.382019,147.007584,146.037903,146.575562,145.740295,146.085907,147.06517,147.967667,147.180405,148.246109,147.890854,147.746857,146.133942,144.780182,143.608917,139.346161,141.16069,141.314316,144.002518,142.975235,141.208679,143.224869,139.086929,139.346161,141.458298,143.138428,142.043945,143.24408,143.119263,139.509354,140.920685,140.267822,141.266312,143.138428,143.090424,142.15918,143.676102,145.596252,145.625076,145.711487,145.855515,147.046005,147.487625,147.343613,147.044632,147.401459,147.382187,147.285736,146.639526,146.71669,145.356812,146.27301,145.829361,144.961349,145.63649,143.73645,141.402405,142.550156,144.41156,143.84256,141.518158,142.820206,143.73645,145.279633,145.144592,146.572037,146.301941,146.78421,146.195892,147.150742,146.707047,147.006042,147.411118,147.999466,148.896423,148.867493,149.224365,148.703537,150.294937,150.84465,150.709625,150.64212,151.15332,151.577682,151.278717,151.539093,151.606613,151.760925,152.918304,153.005096,153.053329,152.571075,152.185287,152.522873,153.969604,154.297516,155.030533,154.33609,152.937592,152.030975,153.014755,153.236603,154.596497,154.104645,153.969604,154.33609,155.599548,155.599548,156.737656,156.795517,156.863037,157.547821,158.358002,158.454407,158.489334,159.255402,159.109955,158.246902,158.663864,159.982681,158.964478,159.507523,159.129333,159.91481,160.913589,160.448151,161.543915,161.456635,161.854202,163.240906,163.648193,163.299072,163.396057,163.580322,162.028763,159.594803,161.136642,161.029953,161.417847,158.566925,159.963272,162.484528,164.084579,164.540314,163.599716,164.802139,165.257919,166.295502,166.2276,166.499115,166.111237,166.954895,166.450638,164.743958,159.323273,154.455322,153.601974,146.872208,145.883118,151.924408,148.006775,153.921982,148.792252,146.028549,134.304779,140.96666,133.82962,120.816116,131.812592,116.811218,122.668251,115.599083,116.452415,111.700844,108.520195,118.81852,120.457314,127.77018,123.696655,127.448563,125.626205,119.827774,122.322556,120.237083,128.647232,128.793411,133.315247,135.810028,134.192291,138.265839,135.02066,135.654083,139.493729,137.232819,132.974121,135.975677,135.995163,138.022186,140.468246,140.029724,143.996017,142.124939,138.197601,138.723862,140.010208,139.240341,141.062698,143.713409,143.732925,140.672928,137.846771,139.328079,140.166138,144.814621,143.274872,145.925598,145.048523,145.457825,147.484833,149.813965,149.29744,149.794479,150.807968,151.948166,154.316269,153.82901,157.775833,159.9198,158.457977,157.278809,148.001328,150.047836,151.655807,154.569656,153.819275,153.945953,153.078613,154.140854,154.745056,150.54483,152.365707,148.821899,151.004959,153.237,154.127838,154.744598,157.182205,155.586487,156.859161,155.889999,157.563995,155.860611,157.906647,159.864563,159.149918,159.756882,160.980606,161.440704,162.282608,160.510696,159.267395,160.637939,159.463211,161.577759,161.21553,162.037857,163.447571,164.093689,165.288055,166.09079,166.227844,166.687958,165.346771,167.559235,167.431946,167.382996,168.097641,168.352188,167.696243,168.175964,168.596924,170.212189,170.779999,172.326752,172.737915,173.942032,173.550461,175.087433,177.42717,171.259689,169.801041,165.033493,168.264084,165.503387,165.562134,168.146606,169.105988,168.547958,167.157852,165.532776,163.53569,165.160782,161.176376,161.518997,164.118027,167.096436,166.162598,167.410995,168.806824,167.489624,170.507385,168.403824,171.342911,172.846863,174.301682,176.886887,175.953064,174.783325,174.8423,174.527756,172.030991,172.630615,172.040833,173.259705,173.829834,170.674484,169.986389,164.353943,165.907059,164.147507,165.857895,169.04274,172.728897,176.297119,176.208649,178.302383,178.007477,179.452454,177.683105,180.199554,182.401398,181.988571,180.06192,181.064545,180.111069,181.585526,184.396851,184.328064,184.976807,183.522003,185.851669,186.008926,186.382492,188.250122,188.043716,188.859573,187.00177,187.276993,186.942764,186.411972,188.967697,189.233093,190.717392,190.176773,189.626297,189.665619,189.990005,190.307816,191.511902,190.712463,191.225693,192.104095,189.370193,190.899979,192.61734,195.706528,196.654022,195.479538,196.17041,196.387543,196.200027,194.492569,196.269119,198.746399,198.598373,198.272659,198.835251,198.114746,193.357544,194.877487,191.462555,194.729446,197.522568,197.818649,200.325562,201.450729,203.256882,203.335846,203.246994,203.730621,204.786682,204.421478,203.997101,202.822601,203.286499,201.35199,201.322388,203.582581,198.262802,197.473236,202.546249,200.641388,197.749557,194.62088,198.134491,197.167267,200.226852,201.65799,204.293213,204.69783,206.128967,205.299896,206.07962,202.329132,202.585724,203.681259,201.361877,199.693909,201.171371,204.45903,203.498474,203.439056,204.67688,207.241669,209.657913,209.737137,209.558868,210.776886,212.054321,212.113739,212.707901,212.232574,214.490372,215.035004,213.62886,211.668137,214.193298,212.51976,215.094421,215.87674,215.77771,215.748001,216.569916,214.985504,215.361786,213.747681,213.708069,214.797363,216.619431,214.074448,212.35141,207.568451,209.855957,213.440674,212.985184,211.321548,210.707581,212.995071,212.876236,215.025116,214.430954,215.203369,215.599457,215.965866,216.054977,216.342163,215.381607,217.253189,217.431442,217.877075,217.263092,218.223648,218.867325,219.20401,218.629654,217.540359,217.332413,214.638916,217.599792,218.837616,218.748474,220.233459,221.057907,221.306229,221.316147,221.326096,222.527985,223.759674,223.272949,223.570953,221.68367,224.36557,224.991348,223.739807,223.431885,222.647171,220.898987,217.760162,221.52475,223.511353,223.700073,225.934998,226.391891,225.170151,225.517792,226.511093,225.170151,224.852295,226.461441,225.448257,227.057404,227.335526,227.24614,227.395126,227.842117,228.477829,228.606949,228.696365,226.997818,224.733093,224.56424,226.580627,228.775818,229.590317,230.245895,228.706284,231.169647,232.043747,231.606705,232.013947,232.798645,232.719177,231.715958,231.169647,230.365082,228.537415,229.05394,227.673248,229.550583,229.34198,227.504379,223.56102,223.541153,225.795944,228.547348,228.820374,228.322159,223.539291,223.688766,221.267426,224.316498,220.938599,223.001221,223.927887,226.080185,225.58197,223.957779,223.79837,224.814713,228.551331,229.906479,230.743484,232.427444,233.334198,233.971924,233.523529,234.958389,234.868713,233.264465,235.755539,236.064423,237.240204,237.97757,239.661545,240.568283,241.49498,241.893539,241.196045,238.824524,239.11351,240.887146,240.887146,241.923431,240.927002,241.305649,240.618103,239.462265,239.541962,240.349091,235.14772,237.359787,232.616776,229.338516,233.065155,230.364838,233.324249,238.31636,239.342682,237.060852,238.675079,236.502853,234.729218,238.346252,235.675827,234.59967,231.301483,235.835236,238.166901,239.811005,242.960007,242.460007,242.600006,242.210007,241.440002,242.970001,242.509995,237.25,237.190002,236.110001,235.699997,238.050003,238.419998,234.929993,235.020004,230.449997,228.029999,225.369995,220.910004,222.330002,219.220001,218.369995,216.75,222.089996,226.809998,228.539993,230.050003,224.580002,226.070007,225.5,227.570007,231.270004,227.350006,222.940002,222.119995,226.070007,226.279999,221.139999,219.369995,216.850006,212.860001,216.589996,221.410004,219.460007],"marker":{},"line":{}}];
            var layout = {"width":600,"height":600,"template":{"layout":{"title":{"x":0.05},"font":{"color":"rgba(42, 63, 95, 1.0)"},"paper_bgcolor":"rgba(255, 255, 255, 1.0)","plot_bgcolor":"rgba(229, 236, 246, 1.0)","autotypenumbers":"strict","colorscale":{"diverging":[[0.0,"#8e0152"],[0.1,"#c51b7d"],[0.2,"#de77ae"],[0.3,"#f1b6da"],[0.4,"#fde0ef"],[0.5,"#f7f7f7"],[0.6,"#e6f5d0"],[0.7,"#b8e186"],[0.8,"#7fbc41"],[0.9,"#4d9221"],[1.0,"#276419"]],"sequential":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"sequentialminus":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]},"hovermode":"closest","hoverlabel":{"align":"left"},"coloraxis":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"geo":{"showland":true,"landcolor":"rgba(229, 236, 246, 1.0)","showlakes":true,"lakecolor":"rgba(255, 255, 255, 1.0)","subunitcolor":"rgba(255, 255, 255, 1.0)","bgcolor":"rgba(255, 255, 255, 1.0)"},"mapbox":{"style":"light"},"polar":{"bgcolor":"rgba(229, 236, 246, 1.0)","radialaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""},"angularaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""}},"scene":{"xaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"yaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"zaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true}},"ternary":{"aaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"baxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"caxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"bgcolor":"rgba(229, 236, 246, 1.0)"},"xaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"yaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"annotationdefaults":{"arrowcolor":"#2a3f5f","arrowhead":0,"arrowwidth":1},"shapedefaults":{"line":{"color":"rgba(42, 63, 95, 1.0)"}},"colorway":["rgba(99, 110, 250, 1.0)","rgba(239, 85, 59, 1.0)","rgba(0, 204, 150, 1.0)","rgba(171, 99, 250, 1.0)","rgba(255, 161, 90, 1.0)","rgba(25, 211, 243, 1.0)","rgba(255, 102, 146, 1.0)","rgba(182, 232, 128, 1.0)","rgba(255, 151, 255, 1.0)","rgba(254, 203, 82, 1.0)"]},"data":{"bar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}},"error_x":{"color":"rgba(42, 63, 95, 1.0)"},"error_y":{"color":"rgba(42, 63, 95, 1.0)"}}],"barpolar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"carpet":[{"aaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"},"baxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"}}],"choropleth":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"contourcarpet":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"heatmap":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"heatmapgl":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram":[{"marker":{"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"histogram2d":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram2dcontour":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"mesh3d":[{"colorbar":{"outlinewidth":0.0,"ticks":""}}],"parcoords":[{"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"pie":[{"automargin":true}],"scatter":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatter3d":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}},"line":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattercarpet":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergeo":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattergl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scattermapbox":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolar":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterpolargl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"scatterternary":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":""}}}],"surface":[{"colorbar":{"outlinewidth":0.0,"ticks":""},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"table":[{"cells":{"fill":{"color":"rgba(235, 240, 248, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}},"header":{"fill":{"color":"rgba(200, 212, 227, 1.0)"},"line":{"color":"rgba(255, 255, 255, 1.0)"}}}]}}};
            var config = {"responsive":true};
            Plotly.newPlot('2556851b-7e13-433e-8b2b-da75f16bfd02', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_2556851b7e13433e8b2bda75f16bfd02();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_2556851b7e13433e8b2bda75f16bfd02();
            }
</script>
*)
(**
Ok, back to the main objective. We need to calculate returns.
We calculate returns from sequential days,
so we need to make sure that our data is sorted correctly
from the oldest to the newest data. We can do this with `List.Sort`.

*)
[1; 7; 10; 2; -1] |> List.sort(* output: 
[-1; 1; 2; 7; 10]*)
(**
Sort it by the date field.

*)
let sortedBnd = bnd |> List.sortBy (fun x -> x.Date)
(**
The first three observations.

*)
sortedBnd[0..2](* output: 
[{ Symbol = "BND"
   Date = 1/4/2010 12:00:00 AM
   Open = 78.599998
   High = 78.730003
   Low = 78.540001
   Close = 78.68
   AdjustedClose = 56.119133
   Volume = 1098100.0 }; { Symbol = "BND"
                           Date = 1/5/2010 12:00:00 AM
                           Open = 78.889999
                           High = 79.0
                           Low = 78.790001
                           Close = 78.910004
                           AdjustedClose = 56.283173
                           Volume = 814600.0 }; { Symbol = "BND"
                                                  Date = 1/6/2010 12:00:00 AM
                                                  Open = 78.970001
                                                  High = 78.980003
                                                  Low = 78.699997
                                                  Close = 78.879997
                                                  AdjustedClose = 56.261776
                                                  Volume = 981300.0 }]*)
(**
The last 3 observations.

*)
sortedBnd[(sortedBnd.Length-3)..](* output: 
[{ Symbol = "BND"
   Date = 2/24/2022 12:00:00 AM
   Open = 81.389999
   High = 81.480003
   Low = 81.150002
   Close = 81.25
   AdjustedClose = 81.25
   Volume = 8883500.0 }; { Symbol = "BND"
                           Date = 2/25/2022 12:00:00 AM
                           Open = 81.199997
                           High = 81.330002
                           Low = 81.139999
                           Close = 81.32
                           AdjustedClose = 81.32
                           Volume = 5923200.0 }; { Symbol = "BND"
                                                   Date = 2/28/2022 12:00:00 AM
                                                   Open = 81.660004
                                                   High = 81.760002
                                                   Low = 81.639999
                                                   Close = 81.739998
                                                   AdjustedClose = 81.739998
                                                   Volume = 507460.0 }]*)
(**
Great, they are properly sorted. Now I want sequential pairs of data.
`List.pairwise` is good for this.

*)
[1 .. 5] |> List.pairwise(* output: 
[(1, 2); (2, 3); (3, 4); (4, 5)]*)
let sequentialBnd = bnd |> List.pairwise
sequentialBnd[0](* output: 
({ Symbol = "BND"
   Date = 1/4/2010 12:00:00 AM
   Open = 78.599998
   High = 78.730003
   Low = 78.540001
   Close = 78.68
   AdjustedClose = 56.119133
   Volume = 1098100.0 }, { Symbol = "BND"
                           Date = 1/5/2010 12:00:00 AM
                           Open = 78.889999
                           High = 79.0
                           Low = 78.790001
                           Close = 78.910004
                           AdjustedClose = 56.283173
                           Volume = 814600.0 })*)
sequentialBnd[1](* output: 
({ Symbol = "BND"
   Date = 1/5/2010 12:00:00 AM
   Open = 78.889999
   High = 79.0
   Low = 78.790001
   Close = 78.910004
   AdjustedClose = 56.283173
   Volume = 814600.0 }, { Symbol = "BND"
                          Date = 1/6/2010 12:00:00 AM
                          Open = 78.970001
                          High = 78.980003
                          Low = 78.699997
                          Close = 78.879997
                          AdjustedClose = 56.261776
                          Volume = 981300.0 })*)
(**
Take the first pair to see how to calculate returns.

Extract the first and second elements of the tuple using pattern matching.

*)
let (bndA, bndB) = sequentialBnd[0]
bndA(* output: 
{ Symbol = "BND"
  Date = 1/4/2010 12:00:00 AM
  Open = 78.599998
  High = 78.730003
  Low = 78.540001
  Close = 78.68
  AdjustedClose = 56.119133
  Volume = 1098100.0 }*)
bndB(* output: 
{ Symbol = "BND"
  Date = 1/5/2010 12:00:00 AM
  Open = 78.889999
  High = 79.0
  Low = 78.790001
  Close = 78.910004
  AdjustedClose = 56.283173
  Volume = 814600.0 }*)
(**
Remember that with continuous compounding, $FV_T = PV_t \times e^{r}$
where $FV$ is the future value, $PV$ is the present value, $r$ is return
between period $t$ and $T$.

If we take the log of both sides of the equation, we get

\begin{equation}
 log(FV) = log(PV) + r \rightarrow log(FV) - log (PV) = r
\end{equation}

This $r$ is known as the log return.
So to find the log return between two periods we can take the
difference of the log prices (where the prices are adjusted for dividends).

*)
(log bndB.AdjustedClose) - (log bndA.AdjustedClose)(* output: 
No value returned by any evaluator*)
(**
Putting it all together.

*)
let bndReturn = 
    bnd
    |> List.sortBy (fun x -> x.Date)
    |> List.pairwise
    |> List.map (fun (a, b) -> (log b.AdjustedClose) - (log a.AdjustedClose))

let vtiReturn =
    vti
    |> List.sortBy (fun x -> x.Date)
    |> List.pairwise
    |> List.map (fun (a, b) -> (log b.AdjustedClose) - (log a.AdjustedClose))


let bndAvgReturn = bndReturn |> List.average
bndAvgReturn(* output: 
0.000122937773*)
let vtiAvgReturn = vtiReturn |> List.average
vtiAvgReturn(* output: 
0.000512414861*)
(**
* Portfolio returns for different weights.

*)
let differentReturns =
  [ for w in [0.0 .. 0.2 .. 1.0] -> w, w*bndAvgReturn + (1.0-w)*vtiAvgReturn ]

differentReturns(* output: 
[(0.0, 0.000512414861); (0.2, 0.0004345194434); (0.4, 0.0003566240258);
 (0.6, 0.0002787286082); (0.8, 0.0002008331906); (1.0, 0.000122937773)]*)
Chart.Line(differentReturns)
(**
## Portfolio Variance

For a portfolio of $N$ assets, the portfolio variance $\sigma_p^2$ is

\begin{equation}
 \sigma_p^2 = \sum^N_{i=1} \sum^N_{j=1} w_i w_j cov(r_i,r_j) = \sum^N_{i=1} w^2_i\sigma^2_i + 2\sum_{j<i} w_i w_j cov(r_i,r_j)
\end{equation}

where $i$ and $j$ index assets, $r_i$ is the return of asset $i$, $\sigma^2_i$ is the variance of $r_i$,
and $cov(r_i,r_j)$ is the covariance between $r_i$ and $r_j$.

For a portfolio of two assets $x$ and $y$ this simplifies to:

\begin{equation}
 \sigma^2_p = w_x^2 \sigma^2_x + w_y^2 \sigma^2_y + 2 w_x w_y cov(r_x,r_y).
\end{equation}

this.
For next time: Portfolio Variance and Leverage

## Leverage

*)
#r "nuget: FSharp.Stats"

open FSharp.Stats
open FSharp.Stats.Correlation




Seq.pearson bndReturn vtiReturn

