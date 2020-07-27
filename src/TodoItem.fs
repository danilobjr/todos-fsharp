module TodoItem

open Browser.Types
open Fable.React
open Fable.React.Props
open Todo
open Browser.Dom

[<Literal>]
let ESCAPE_KEY = 27.0 

let TodoItem = 
    FunctionComponent.Of<{| Editing: bool
                            Todo: Todo
                            OnCancelEdition: unit -> unit
                            OnCheckClick: Event -> unit
                            OnLabelDoubleClick: MouseEvent -> unit
                            OnRemoveClick: MouseEvent -> unit
                            OnSave: string -> unit |}>(fun props ->

    let text = Hooks.useState(props.Todo.Text)

    let handleBlur _ =
        text.update props.Todo.Text
        props.OnCancelEdition ()

    let handleKeyPress (e: KeyboardEvent) =
        match e.which with
        | ESCAPE_KEY -> props.OnCancelEdition ()
        | _ -> ()

    let handleDoubleClick e =
        text.update props.Todo.Text
        props.OnLabelDoubleClick e

    let handleSubmit (e: Event) =
        e.preventDefault ()
        props.OnSave text.current
        props.OnCancelEdition ()

    li [ classList [ "completed", props.Todo.Completed
                     "editing", props.Editing ]] [

        if (not props.Editing) then
            div [ Class "view" ] [
                input [ Class "toggle"
                        Type "checkbox"
                        Checked props.Todo.Completed
                        OnChange props.OnCheckClick ]
                label [ OnDoubleClick handleDoubleClick ] [ str props.Todo.Text ]
                button [ Class "destroy"; OnClick props.OnRemoveClick ] [] ]

        if (props.Editing) then
            form [ OnSubmit handleSubmit ] [
                input [ Class "edit"
                        AutoFocus true
                        Value text.current
                        OnBlur handleBlur
                        OnChange (fun e -> text.update e.Value)
                        OnKeyDown handleKeyPress ]
            ]
    ])