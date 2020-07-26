module State

open Todo

type Filter =
    | All
    | Active
    | Completed

type State = {
    Todos: Todo list
    Filter: Filter
}

type Action =
    | ClearCompleted
    | Filter of Filter

let initialState = {
    Todos = Data.data
    Filter = All
}

let filterTodos filter (todos: Todo list) =
    match filter with
    | Active ->
        todos |> List.filter (fun t -> not t.Completed)
    | Completed ->
        todos |> List.filter (fun t -> t.Completed)
    | All ->
        todos

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
