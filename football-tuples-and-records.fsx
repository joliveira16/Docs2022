(**
[![Binder](img/badge-binder.svg)](https://mybinder.org/v2/gh/nhirschey/teaching/gh-pages?filepath=football-tuples-and-records.ipynb)&emsp;
[![Script](img/badge-script.svg)](/Teaching//football-tuples-and-records.fsx)&emsp;
[![Notebook](img/badge-notebook.svg)](/Teaching//football-tuples-and-records.ipynb)

# Working with Tuples and Records.

> Developed with [Davide Costa](https://github.com/DavideGCosta)
> 

This set of exercises covers creating and manipulating tuples, records, and anonymous records.
Before you start it is a good idea to review the relevant sections of
the F# language reference (
[tuples](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/tuples),
[records](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records),
and [anonymous records](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/anonymous-records)
) and
F# for fun and profit (
[tuples](https://fsharpforfunandprofit.com/posts/tuples/) and
[records](https://fsharpforfunandprofit.com/posts/records/))
before you start.

## Import the Football Players Data from the Csv File

*)
#r "nuget:FSharp.Data"
open FSharp.Data
(**
In order to import the data correctly we need to create the sample, define the type from the sample and then load the csv file.
We'll use [FSharp.Data CsvProvider](https://fsprojects.github.io/FSharp.Data/library/CsvProvider.html).

### Load the Csv file.

We define the type from the csv sample file.

*)
let [<Literal>] CsvPath = __SOURCE_DIRECTORY__ + "/FootballPlayers.csv"
type FootballPlayersCsv = CsvProvider<CsvPath>
(**
This will load the sample csv file.

*)
let playerStatsTable = 
    FootballPlayersCsv.GetSample().Rows
    |> Seq.toList
(**
Let's see the first 5 rows from the loaded Csv data, stored in `playerStatsTable`.
Again, we do this by using the List `List.truncate` property.

*)
playerStatsTable
|> List.truncate 5(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition*)
(**
## EXERCISES - PART 1

* [Transforming collection elements into new types.](#Transforming-collections)
  

  0 [Creating tuples.](#Creating-tuples)
    
  
  1 [Creating records.](#Creating-records)
    
  
  2 [Creating anonymous records.](#Creating-anonymous-records)
    
  

* [Simple data transformations.](#Simple-transformations)
  

  0 [Transformations using tuples.](#Transformations-using-tuples)
    
  
  1 [Transformations using records.](#Transformations-using-records)
    
  
  2 [Transformations using anonymous records.](#Transformations-using-anonymous-records)
    
  

* [Creating and transforming TeamRecord.](#Creating-and-transforming-TeamRecord)
  

<h2 class=numbered><a name=Transforming-collections class=anchor href=#Transforming-collections>Transforming collections</a></h2>

<h3 class=numbered><a name=Creating-tuples class=anchor href=#Creating-tuples>Creating tuples</a></h3>

Example: Transform each element of the `playerStatsTable` List into a tuple with the player and nation ( `Player`, `Nation`)

*)
playerStatsTable
|> List.map(fun x -> x.Player, x.Nation)
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition*)
(**
* Transform each element of the `playerStatsTable` List into a tuple with the player and team ( `Player`, `Team`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,38) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Team'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into a tuple with the player and league/competiton ( `Player`, `League`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,40) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'League'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into a tuple with the player and age ( `Player`, `Age`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,37) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Age'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into a tuple with the player and matches played ( `Player`, `MatchesPlayed`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,47) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'MatchesPlayed'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into a tuple with the player and goals scored ( `Player`, `GoalsScored`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,45) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

<h3 class=numbered><a name=Creating-records class=anchor href=#Creating-records>Creating records</a></h3>

Example: Define a record named `PlayerAndNation` with a field named `Player` that is a `string` and `Nation` that is a `string`.
Then transform each element of the `playerStatsTable` List into a `PlayerAndNation` record.

*)
type PlayerAndNation =
    { Player : string 
      Nation : string }
(**
The above code creates a record type called `PlayerAndNation`.
This record contains two fields: `Player` of `string` type and `Nation` of `string` type.
Remember, if the types from the csv file are different an error will occur when creating an instance of the record.

Common types:

* `string`, example: `"hello world"`

* `int`, example: `2`

* `float`, example: `2.0`

* `decimal`, example: `2.0m`

Check [basic types documentation](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/basic-types)
to learn about F# basic types.

Now by having the record type created we can `map` the `playerStatsTable` rows to the record `PlayerAndNation`.

*)
playerStatsTable
|> List.map(fun x -> 
    { Player = x.Player
      Nation = x.Nation })
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition*)
(**
Note that you choose the name of the fields in the record. Instead of `Player` it could be anything.
The following code block for example would have also worked,
but the field name for the player is `PlayerName` instead of `Player` and `Nationality` instead of `Nation`:

*)
type PlayerAndNation2 =
    { PlayerName : string 
      Nationality : string }

playerStatsTable
|> List.map(fun x -> 
    { PlayerName = x.Player
      Nationality = x.Nation })
(**
* Define a record named `PlayerAndTeam` with a field named `Player` that is a `string` and `Team` that is a `string`. 
Then transform each element of the `playerStatsTable` List into a `PlayerAndTeam` record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   PlayerAndTeam
   playersByPosition
```

</details>
</span>
</p>
</div>

* Define a record named `PlayerAndLeague` with a field named `Player` that is a `string` and `League` that is a `string`. 
Then transform each element of the `playerStatsTable` List into a `PlayerAndLeague` record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
```

</details>
</span>
</p>
</div>

* Define a record named `PlayerAndAge` with a field named `Player` that is a `string` and `Age` that is a integer(`int`). 
Then transform each element of the `playerStatsTable` List into a `PlayerAndAge` record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   PlayerAndAge
   playersByPosition
```

</details>
</span>
</p>
</div>

* Define a record named `PlayerAndMatchesPlayed` with a field named `Player` that is a `string` and `MatchesPlayed` that is a integer(`int`). 
Then transform each element of the `playerStatsTable` List into a `PlayerAndMatchesPlayed` record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   PlayerAndMatchesPlayed
   playersByPosition
```

</details>
</span>
</p>
</div>

* Define a record named `PlayerAndGoalsScored` with a field named `Player` that is a `string` and `GoalsScored` that is a integer(`int`). 
Then transform each element of the `playerStatsTable` List into a `PlayerAndGoalsScored` record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
```

</details>
</span>
</p>
</div>

<h3 class=numbered><a name=Creating-anonymous-records class=anchor href=#Creating-anonymous-records>Creating anonymous records</a></h3>

Example: Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `Nation` field that is a `string`.

With `Anonymous records` we don't need to define the record type beforehand and we don't need to specify the type of each field.

*)
playerStatsTable
|> List.map(fun x -> 
    {| Player = x.Player
       Nation = x.Nation |})
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition*)
(**
* Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `Team` field that is a `string`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,17)-(4,21) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Team'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `League` field that is a `string`.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,19)-(4,25) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'League'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `Age` field that is a integer(`int`).

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,17)-(4,20) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Age'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `MatchesPlayed` field that is a integer(`int`).

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,26)-(4,39) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'MatchesPlayed'.
```

</details>
</span>
</p>
</div>

* Transform each element of the `playerStatsTable` List into an anonymous record with a `Player` field that is a `string` and a `GoalsScored` field that is a integer(`int`).

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,24)-(4,35) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

<h2 class=numbered><a name=Simple-transformations class=anchor href=#Simple-transformations>Simple transformations</a></h2>

Now that you are used to work with `List.map` to organize the data into tuples, records and anonymous records.
Let's try to do it while applying some simple transformations as sum, multiplication, type transformations and so on.

<h3 class=numbered><a name=Transformations-using-tuples class=anchor href=#Transformations-using-tuples>Transformations using tuples</a></h3>

Example: map the `playerStatsTable` to a tuple of player and age, but add 1 to age. ( `Player`, `Age + 1`)

*)
playerStatsTable
|> List.map(fun x -> x.Age + 1)
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,22)-(2,27) typecheck error Lookup on object of indeterminate type based on information prior to this program point. A type annotation may be needed prior to this program point to constrain the type of the object. This may allow the lookup to be resolved.*)
(**
When to use integers or floats/decimals:

0 Use integers if the results of the calculations should be integers (1, 2, 3, 4, ...).

1 Use floats or decimals if the results of the calculations may be floats or decimals (1.1, 2.1324, ...).

* map the `playerStatsTable` to a tuple of player and goals scored, but multiply goals scored by 10. ( `Player`, `GoalsScored * 10`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,34)-(2,45) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

* map the `playerStatsTable` to a tuple of player and goals scored, but divide GoalsScored by 2. ( `Player`, `GoalsScored / 2`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,35)-(2,46) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

In this case, look how dividing using integers rounds the results to the nearest integers.
If the results are decimals you might prefer to get exact results.
For that you can use floats or decimals types.
In order to convert a variable to float you have to use the syntax: `float variable`.

Example: map the `playerStatsTable` to a tuple of player and age, but convert age to float. ( `Player`, `float Age`)

*)
playerStatsTable
|> List.map(fun x -> x.Player, float x.Age) 
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,40)-(2,43) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Age'.*)
(**
* map the `playerStatsTable` to a tuple of player and goals scored, but convert goalsScored to float. ( `Player`, `float GoalsScored`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,40)-(2,51) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

* map the `playerStatsTable` to a tuple of player and goals scored, but divide goalsScored by 2.0. ( `Player`, `GoalsScored / 2.0`)
Hint: convert goals scored to float and divide by 2.0 (you can't divide by 2 because if you perform math operations with different types, you'll get an error).

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (2,35)-(2,46) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

<h3 class=numbered><a name=Transformations-using-records class=anchor href=#Transformations-using-records>Transformations using records</a></h3>

Example: map the `playerStatsTable` to a record of player and age, but add 1 to age. ( `Player`, `Age + 1`)

*)
type PlayerAndAgePlus1Int =
    { Player : string
      AgePlus1Int : int }

playerStatsTable
|> List.map(fun x ->
    { Player = x.Player 
      AgePlus1Int = x.Age + 1})
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (8,23)-(8,26) typecheck error The type 'PlayerAndAgePlus1Int' does not define the field, constructor or member 'Age'.*)
(**
* map the `playerStatsTable` to a record of player and goals scored, but multiply goals scored by 10. ( `Player`, `GoalsScored * 10`)
Hint: You have to create a new type record.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
   PlayerAndGls
```

</details>
</span>
</p>
</div>

* map the `playerStatsTable` to a record of player and goals scored, but divide goals scored by 2.0. ( `Player`, `float GoalsScored  / 2.0`)
Hint: You have to create a new type record, because previous type has goals scored as integers but you want floats.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (5,1)-(5,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (8,35)-(8,46) typecheck error The type 'PlayerAndGlsFloat' does not define the field, constructor or member 'GoalsScored'. Maybe you want one of the following:
   GoalsScoredFloat
```

</details>
</span>
</p>
</div>

<h3 class=numbered><a name=Transformations-using-anonymous-records class=anchor href=#Transformations-using-anonymous-records>Transformations using anonymous records</a></h3>

Example: map the `playerStatsTable` to an anonymoys record of player and age, but add 1 to age. ( `Player`, `Age + 1`)

*)
playerStatsTable
|> List.map(fun x -> 
    {| Player = x.Player
       AgePlus1 = x.Age + 1 |})
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,21)-(4,24) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Age'.*)
// or 

playerStatsTable
|> List.map(fun x ->
    {| Player = x.Player 
       AgePlus1Float = (float x.Age) + 1.0 |})
|> List.truncate 5 //just to observe the first 5 rows, not a part of the exercise.(* output: 
input.fsx (3,1)-(3,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (6,33)-(6,36) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'Age'.*)
(**
* map the `playerStatsTable` to an anonymous record of player and goals scored, but multiply goals scored by 10. ( `Player`, `GoalsScored * 10`)

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,29)-(4,40) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
input.fsx (9,1)-(9,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (12,33)-(12,44) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

* map the `playerStatsTable` to an anonymous record of player and goals scored, but divide goals scored by 2.0. ( `Player`, `float GoalsScored  / 2.0`)
Hint: Remember that you have to transform GoalsScored to float.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (1,1)-(1,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (4,36)-(4,47) typecheck error The type 'PlayerAndNation' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

<h2 class=numbered><a name=Creating-and-transforming-TeamRecord class=anchor href=#Creating-and-transforming-TeamRecord>Creating and transforming TeamRecord</a></h2>

Now that you are used to work with records and perform simple Transformations, map `playerStatsTable` to a record type that includes:

* Player (`Player`) - type `string`

* Nation (`Nation`) - type `string`

* League (`League`) - type `string`

* AgeNextYear (`Age + 1`) - type `int`

* HalfGoalsScored (`GoalsScored / 2.0`) - type `float`

Hint: Create a new type.

<div style="padding-left: 40px;">
<p> 
<span>
<details>
<summary><p style="display:inline">answer</p></summary>

```
input.fsx (8,1)-(8,17) typecheck error The value or constructor 'playerStatsTable' is not defined. Maybe you want one of the following:
   playersByPosition
input.fsx (13,23)-(13,26) typecheck error The type 'TeamRecord' does not define the field, constructor or member 'Age'.
input.fsx (14,33)-(14,44) typecheck error The type 'TeamRecord' does not define the field, constructor or member 'GoalsScored'.
```

</details>
</span>
</p>
</div>

*)

