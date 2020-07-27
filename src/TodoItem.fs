module TodoItem

open System
open Fable.React
open Fable.React.Props
open Todo

let TodoItem = 
    FunctionComponent.Of<{| Todo: Todo
                            OnRemoveClick: Guid -> unit |}>(fun props ->

    li [ classList [ "completed", props.Todo.Completed ]] [
        div [ Class "view" ] [
            input [ Class "toggle"; Type "checkbox"; Checked props.Todo.Completed ]
            label [] [ str props.Todo.Text ]
            button [ Class "destroy"; OnClick (fun _ -> props.OnRemoveClick props.Todo.Id) ] []
        ]
        // input [ Class "edit"; Value "todo" ]
    ])