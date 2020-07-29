module State

open System
open Todo
open TodoText

type Filter =
    | All
    | Active
    | Completed

type State =
    { AllCompleted: bool
      Editing: Guid option
      Filter: Filter
      Todos: Todo list }

type Action =
    | Add of Text option
    | CancelEdition
    | ClearCompleted
    | Edit of Guid
    | Filter of Filter
    | Remove of Guid
    | Save of Guid * Text option
    | SetAllAsCompleted of bool
    | ToggleCompleted of Guid

let initialState =
    { AllCompleted = false
      Editing = None
      Filter = All
      Todos = DataSource.todos }

let filterTodos filter todos =
    match filter with
    | Active ->
        todos
        |> List.filter (fun t -> not t.Completed)
    | Completed ->
        todos
        |> List.filter (fun t -> t.Completed)
    | All ->
        todos

let reducer state action =
    match action with
    | Add text ->
        match Todo.create text with
        | Some newTodo -> { state with Todos = newTodo::state.Todos }
        | None -> state

    | CancelEdition ->
        { state with Editing = None }

    | ClearCompleted ->
        { state with Todos = state.Todos |> filterTodos Active }

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
        match updatedText with
        | Some updatedText' ->
            let todos =
                state.Todos 
                |> List.map (fun t ->
                    match t.Id = id with
                    | true -> { t with Text = updatedText' }
                    | false -> t)
            { state with Todos = todos }
        | None -> state

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