module App

open Browser.Dom
open Fable.React
open Fable.React.Props
open Todo
open ReactUtils

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

// Components

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

type FilterButtonProps = {
    CurrentFilter: Filter
    FilterBy: Filter
    OnClick: Filter -> unit
}

let FilterButton =
    FunctionComponent.Of<FilterButtonProps>(fun props ->

    let filterText =
        match props.FilterBy with
        | All -> "All"
        | Active -> "Active"
        | Completed -> "Completed"

    a [ classList [ "selected", props.FilterBy = props.CurrentFilter ]
        Href "#/"
        OnClick (fun _ -> props.OnClick props.FilterBy) ]
      [ str filterText ])

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
    let (state, dispatch) = useReducer reducer initialState

    let filteredItems = 
        state.Todos
        |> filterTodos state.Filter
        |> List.map TodoItem

    let activeTodosCount =
        state.Todos
        |> filterTodos Active
        |> List.length

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
                ActiveTodosCount { Count = activeTodosCount }

                ul [ Class "filters" ] [
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = All
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = Active
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = Completed
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                ]

                button [ Class "clear"; OnClick (fun _ -> dispatch ClearCompleted) ] [ str "Clear completed" ]
            ]
        ]
    ])

ReactDom.render (App(), document.getElementById "app")
