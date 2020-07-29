module TodoItem

open Browser.Types
open Fable.React
open Fable.React.Props
open Todo


let [<Literal>] ESCAPE_KEY = 27. 

let TodoItem = 
    FunctionComponent.Of<{| Editing: bool
                            Todo: Todo
                            OnCancelEdition: unit -> unit
                            OnCheckClick: Event -> unit
                            OnLabelDoubleClick: MouseEvent -> unit
                            OnRemoveClick: MouseEvent -> unit
                            OnSave: TodoText.Text option -> unit |}>(fun props ->

    let text = Hooks.useState (Some props.Todo.Text)

    let currentTextValue =
        match text.current with
        | Some t -> t |> TodoText.value
        | None -> ""

    let handleBlur _ =
        text.update text.current
        props.OnCancelEdition ()

    let handleKeyPress (e: KeyboardEvent) =
        match e.which with
        | ESCAPE_KEY -> props.OnCancelEdition ()
        | _ -> ()

    let handleDoubleClick e =
        text.update (Some props.Todo.Text)
        props.OnLabelDoubleClick e

    let handleSubmit (e: Event) =
        e.preventDefault ()
        match text.current with
        | Some _ ->
            props.OnSave text.current
            props.OnCancelEdition ()
        | None -> ()

    li [ classList [ "completed", props.Todo.Completed
                     "editing", props.Editing ]] [

        if (not props.Editing) then
            div [ Class "view" ] [
                input [
                    Class "toggle"
                    Type "checkbox"
                    Checked props.Todo.Completed
                    OnChange props.OnCheckClick ]

                label [ OnDoubleClick handleDoubleClick ] [ str (props.Todo.Text |> TodoText.value) ]

                button [ Class "destroy"; OnClick props.OnRemoveClick ] []
            ]

        if (props.Editing) then
            form [ OnSubmit handleSubmit ] [
                input [
                    Class "edit"
                    AutoFocus true
                    Value currentTextValue
                    OnBlur handleBlur
                    OnChange (fun e -> text.update (e.Value |> TodoText.create))
                    OnKeyDown handleKeyPress ]
            ]
    ])