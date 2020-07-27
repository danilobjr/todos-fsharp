module State

open System
open Todo

type Filter =
    | All
    | Active
    | Completed

type State = {
    AllCompleted: bool
    Editing: Guid option
    Filter: Filter
    Todos: Todo list }

type Action =
    | Add of string
    | CancelEdition
    | ClearCompleted
    | Edit of Guid
    | Filter of Filter
    | Remove of Guid
    | Save of Guid * string
    | SetAllAsCompleted of bool
    | ToggleCompleted of Guid

let initialState = {
    AllCompleted = false
    Editing = None
    Filter = All
    Todos = Data.data }

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
    | Add text ->
        // FIXME should only create a new todo if text is not empty
        // Put this logic inside domain model
        let newTodo = create text
        { state with Todos = newTodo::state.Todos }
    | CancelEdition ->
        { state with Editing = None}
    | ClearCompleted ->
        let activeTodos =
            state.Todos
            |> filterTodos Active
        { state with Todos = activeTodos }
    | Edit id ->
        { state with Editing = Some id }
    | Filter Active ->
        { state with Filter = Active }
    | Filter Completed ->
        { state with Filter = Completed }
    | Filter All ->
        { state with Filter = All }
    | Remove id ->
        let todos =
            state.Todos
            |> List.filter (fun t -> t.Id <> id)
        { state with Todos = todos }
    | Save (id, updatedText) ->
        let todos =
            state.Todos
            |> List.map (fun t ->
                match t.Id = id with
                | true -> { t with Text = updatedText }
                | false -> t)
        { state with Todos = todos }
    | SetAllAsCompleted completed ->
        let todos =
            state.Todos
            |> List.map (fun t -> { t with Completed = completed })
        { state with
            AllCompleted = completed
            Todos = todos }
    | ToggleCompleted id ->
        let todos =
            state.Todos
            |> List.map (fun t ->
                match t.Id = id with
                    | true -> { t with Completed = not t.Completed }
                    | false -> t)
        { state with Todos = todos }