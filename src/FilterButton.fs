module FilterButton

open Fable.React
open Fable.React.Props
open State

let FilterButton =
    FunctionComponent.Of<{| CurrentFilter: Filter
                            FilterBy: Filter
                            Href: string
                            OnClick: Filter -> unit |}>(fun props ->

    let filterText =
        match props.FilterBy with
        | All -> "All"
        | Active -> "Active"
        | Completed -> "Completed"

    a [ classList [ "selected", props.FilterBy = props.CurrentFilter ]
        Href props.Href
        OnClick (fun _ -> props.OnClick props.FilterBy) ] [
        str filterText
    ])