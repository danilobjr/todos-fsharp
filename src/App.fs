module App

open Browser.Dom
open Fable.React
open Fable.React.Props
open ActiveTodosCount
open AddTodoForm
open FilterButton
open ReactUtils
open State
open TodoItem

let App =
    FunctionComponent.Of(fun () ->

    let (state, dispatch) = useReducer reducer initialState

    let todoItems = 
        let filteredTodos = 
            state.Todos
            |> filterTodos state.Filter

        match filteredTodos with
        | Some todos ->
            todos
            |> List.map (fun t ->
                let editing =
                    match state.Editing with
                    | Some id -> t.Id = id
                    | None -> false

                TodoItem
                    {| Editing = editing
                       Todo = t
                       OnCancelEdition = (fun _ -> dispatch CancelEdition)
                       OnCheckClick = (fun _ -> dispatch (ToggleCompleted t.Id))
                       OnLabelDoubleClick = (fun _ -> dispatch (Edit t.Id))
                       OnRemoveClick = (fun _ -> dispatch (Remove t.Id))
                       OnSave = (fun updatedText -> dispatch (Save (t.Id, updatedText))) |})
        | None -> []

    let activeTodosCount =
        let filteredTodos =
            state.Todos
            |> filterTodos Active

        match filteredTodos with
        | Some todos ->
            todos
            |> List.length
        | None -> 0

    let completedTodosCount =
        match state.Todos with
        | Some todos -> 
            (todos |> List.length) - activeTodosCount
        | None -> 0

    fragment [] [
        header [] [
            h1 [] [ str "todos" ]

            AddTodoForm {| OnPressEnter = (fun text -> dispatch (Add (TodoText.create text))) |}
        ]

        section [ Class "main" ] [
            // FIXME should be hidden when no items left
            // FIXME should be checked when all items left are completed
            input [
                Id "toggle-all"
                Type "checkbox"
                OnClick (fun e -> dispatch (SetAllAsCompleted (not state.AllCompleted))) ]

            label [ HtmlFor "toggle-all" ] [ str "Mark all as complete" ]

            ul [ Class "todos" ] todoItems

            footer [] [
                ActiveTodosCount { Count = activeTodosCount }

                ul [ Class "filters" ] [
                    li [] [
                        FilterButton
                            {| CurrentFilter = state.Filter
                               FilterBy = All
                               Href = "#/"
                               OnClick = (fun filter -> dispatch (Filter filter)) |}
                    ]

                    li [] [
                        FilterButton
                            {| CurrentFilter = state.Filter
                               FilterBy = Active
                               Href = "#/active"
                               OnClick = (fun filter -> dispatch (Filter filter)) |}
                    ]

                    li [] [
                        FilterButton
                            {| CurrentFilter = state.Filter
                               FilterBy = Completed
                               Href = "#/completed"
                               OnClick = (fun filter -> dispatch (Filter filter)) |}
                    ]
                ]

                if (completedTodosCount > 0) then
                    button [
                        Class "clear";
                        OnClick (fun _ -> dispatch ClearCompleted) ] [
                        str (sprintf "Clear completed (%i)" completedTodosCount)
                    ]
            ]
        ]
    ])

ReactDom.render (App(), document.getElementById "app")
