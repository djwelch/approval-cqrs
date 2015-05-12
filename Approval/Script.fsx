// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Library.fs"
open Approval

// Define your library scripting code here


let m = [(1, false);(1, true)] |> Map.ofList

let m2 = m.Add(2, false)

m2

Map.fold (fun state key value -> state && (value || key = 2)) true m2


match Map.fold (fun state key value -> state && (value || key = 2)) true m2 with
 | true  -> [1]
 | false -> []
 ;;

let x = [1]

let y = x::2


[2] :> Set.add ([1] |> Set.ofList)
