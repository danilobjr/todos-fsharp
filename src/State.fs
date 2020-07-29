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
      Todos: (Todo list) option }

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

let filterTodos filter (todos: (Todo list) option) =
    match todos with
    | Some todos' -> 
        match filter with
        | Active ->
            todos'
            |> List.filter (fun t -> not t.Completed)
            |> Some
        | Completed ->
            todos'
            |> List.filter (fun t -> t.Completed)
            |> Some
        | All ->
            todos
    | None -> todos

let reducer state action =
    match action with
    | Add text ->
        match createTodo text with
        | Some newTodo ->
            let todos =
                match state.Todos with
                | Some todos ->
                    Some (newTodo::todos)
                | None -> 
                    state.Todos
            { state with Todos = todos }
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
            match state.Todos with
            | Some todos ->
                todos
                |> List.filter (fun t -> t.Id <> id)
                |> Some
            | None -> state.Todos
        { state with Todos = todos }

    | Save (id, updatedText) ->
        match updatedText with
        | Some updatedText' ->
            let todos =
                match state.Todos with
                | Some todos ->
                    todos
                    |> List.map (fun t ->
                        match t.Id = id with
                        | true -> { t with Text = updatedText' }
                        | false -> t)
                    |> Some
                | None -> state.Todos
            { state with Todos = todos }
        | None -> state

    | SetAllAsCompleted completed ->
        let todos =
            match state.Todos with
            | Some todos ->
                todos
                |> List.map (fun t -> { t with Completed = completed })
                |> Some
            | None -> state.Todos
        { state with
            AllCompleted = completed
            Todos = todos }
            
    | ToggleCompleted id ->
        let todos =
            match state.Todos with
            | Some todos ->
                todos
                |> List.map (fun t ->
                    match t.Id = id with
                        | true -> { t with Completed = not t.Completed }
                        | false -> t)
                |> Some
            | None -> state.Todos
        { state with Todos = todos }