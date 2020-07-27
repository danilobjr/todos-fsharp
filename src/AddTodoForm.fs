module AddTodoForm

open Browser.Types
open Fable.React
open Fable.React.Props

let AddTodoForm =
    FunctionComponent.Of<{| OnPressEnter: string -> unit |}>(fun props -> 

    let text = Hooks.useState("");

    let handleSubmit (e: Event) =
        e.preventDefault ()
        props.OnPressEnter text.current
        text.update ""

    form [ OnSubmit handleSubmit ] [
        input [ Class "new"
                Placeholder "What needs to be done?"
                AutoFocus true
                Value text.current
                OnChange (fun e -> text.update e.Value) ]
    ])