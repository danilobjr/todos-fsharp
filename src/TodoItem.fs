module TodoItem

open Browser.Types
open Fable.React
open Fable.React.Props
open Todo

let TodoItem = 
    FunctionComponent.Of<{| Todo: Todo
                            OnCheckClick: MouseEvent -> unit
                            OnRemoveClick: MouseEvent -> unit |}>(fun props ->

    li [ classList [ "completed", props.Todo.Completed ]] [
        div [ Class "view" ] [
            input [ Class "toggle"
                    Type "checkbox"
                    Checked props.Todo.Completed
                    OnClick props.OnCheckClick ]
            label [] [ str props.Todo.Text ]
            button [ Class "destroy"; OnClick props.OnRemoveClick ] []
        ]
        // input [ Class "edit"; Value "todo" ]
    ])