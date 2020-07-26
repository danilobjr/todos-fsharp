module App

open Browser.Dom
open Browser.Types
open Fable.React
open Fable.React.Props
open Todo

let data = [
    { Id = System.Guid.NewGuid (); Text = "active 1"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 2"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 3"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 4"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "completed 5"; Completed = true }
    { Id = System.Guid.NewGuid (); Text = "completed 6"; Completed = true }
    { Id = System.Guid.NewGuid (); Text = "completed 7"; Completed = true }
]

type Filter =
    | All
    | Active
    | Completed

let filterTodos filter (todos: Todo list) =
    match filter with
    | Active ->
        todos |> List.filter (fun t -> not t.Completed)
    | Completed ->
        todos |> List.filter (fun t -> t.Completed)
    | All ->
        todos

type State = {
    Todos: Todo list
    Filter: Filter
}

type Action =
    | ClearCompleted
    | Filter of Filter

let initialState = {
    Todos = data
    Filter = All
}

let reducer state action =
    match action with
    | ClearCompleted ->
        let activeTodos =
            state.Todos
            |> filterTodos Active
        { state with Todos = activeTodos }
    | Filter Active ->
        { state with Filter = Active }
    | Filter Completed ->
        { state with Filter = Completed }
    | Filter All ->
        { state with Filter = All }

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

let App =
    FunctionComponent.Of(fun () ->

    let newTodoText = Hooks.useState ""
    let state = Hooks.useReducer(reducer, initialState)

    let filteredItems = 
        state.current.Todos
        |> filterTodos state.current.Filter
        |> List.map TodoItem

    fragment [] [
        header [] [
            h1 [] [ str "todos" ]

            form [ OnSubmit (fun e -> e.preventDefault ()) ] [
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

            ul [ Class "todos" ] filteredItems

            footer [] [
                span [ Class "count" ] []

                ul [ Class "filters" ] [
                    li [] [
                        a [ classList [ "selected", state.current.Filter = All ]
                            Href "#/"
                            OnClick (fun _ -> state.update (Filter All)) ]
                          [ str "All" ]
                    ]
                    li [] [
                        a [ classList [ "selected", state.current.Filter = Active ]
                            Href "#/active"
                            OnClick (fun _ -> state.update (Filter Active)) ]
                          [ str "Active" ]
                    ]
                    li [] [
                        a [ classList [ "selected", state.current.Filter = Completed ]
                            Href "#/completed"
                            OnClick (fun _ -> state.update (Filter Completed)) ]
                          [ str "Completed" ]
                    ]
                ]

                button [ Class "clear"; OnClick (fun _ -> state.update ClearCompleted) ] [ str "Clear completed" ]
            ]
        ]
    ])

ReactDom.render (App(), document.getElementById "app")
