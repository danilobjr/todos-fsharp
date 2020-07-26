module TodoItem

open Fable.React
open Fable.React.Props
open Todo

let TodoItem = 
    FunctionComponent.Of<Todo>(fun props ->

    li [ classList [ "completed", props.Completed ]] [
        div [ Class "view" ] [
            input [ Class "toggle"; Type "checkbox"; Checked props.Completed ]
            label [] [ str props.Text ]
            button [ Class "destroy" ] []
        ]
        // input [ Class "edit"; Value "todo" ]
    ])