module ActiveTodosCount

open Fable.React
open Fable.React.Props

let getItemsLeftPhrase = function
    | Some count when count = 1 -> "1 item left"
    | Some count -> sprintf "%i items left" count
    | None -> "No items left"

type ActiveTodosCountProps = { Count: int }

let ActiveTodosCount =
    FunctionComponent.Of<ActiveTodosCountProps>(fun props ->

    let toOption count =
        match count with
        | 0 -> None
        | _ -> Some count

    let itemsLeftPhrase =
        props.Count
        |> toOption
        |> getItemsLeftPhrase
    
    span [ Class "count" ] [ str itemsLeftPhrase ])