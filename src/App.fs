module App

open Browser.Dom
open Browser.Types
open Fable.React
open Fable.React.Props
open Todo

let handleSubmit (newTodoText: IStateHook<string>) (todos: IStateHook<list<Todo>>) (e: Event) =
    e.preventDefault ()
    let newTodo = create newTodoText.current
    todos.update (newTodo::todos.current)
    newTodoText.update ""

let TodoItem = 
    FunctionComponent.Of(fun _ ->

    li [] [
        div [ Class "view" ] [
            input [ Class "toggle"; Type "checkbox" ]
            label [] [ str "todo" ]
            button [ Class "destroy" ] []
        ]
        // input [ Class "edit"; Value "todo" ]
    ])

let App =
    FunctionComponent.Of(fun () ->

    let newTodoText = Hooks.useState ""
    let todos = Hooks.useState []

    let items = 
        todos.current
        |> List.map TodoItem

    fragment [] [
        header [] [
            h1 [] [ str "todos" ]

            form [ OnSubmit (handleSubmit newTodoText todos) ] [
                input [ Class "new"
                        Placeholder "What needs to be done?"
                        AutoFocus true
                        Value newTodoText.current
                        OnChange (fun e -> newTodoText.update e.Value) ]
            ]
        ]

        section [ Class "main" ] [
            input [ Id "toggle-all"; Type "checkbox" ]

            label [ HtmlFor "toggle-all" ] [ str "Mark all as complete" ]

            ul [ Class "todos" ] items

            footer [] [
                span [ Class "count" ] []

                ul [ Class "filters" ] [
                    li [] [
                        a [ Class "selected"; Href "#/" ] [ str "All" ]
                    ]
                    li [] [
                        a [ Href "#/active" ] [ str "Active" ]
                    ]
                    li [] [
                        a [ Href "#/completed" ] [ str "Completed" ]
                    ]
                ]

                button [ Class "clear" ] [ str "Clear completed" ]
            ]
        ]
    ])

ReactDom.render (App(), document.getElementById "app")
