module App

open Browser.Dom
open Fable.React
open Fable.React.Props
open ActiveTodosCount
open FilterButton
open ReactUtils
open State
open TodoItem

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

    let completedTodosCount =
        (state.Todos |> List.length) - activeTodosCount

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
            input [ Id "toggle-all"; Type "checkbox"; OnClick (fun e -> dispatch (ToggleAllCompleted (not state.AllCompleted))) ]

            label [ HtmlFor "toggle-all" ] [ str "Mark all as complete" ]

            ul [ Class "todos" ] filteredItems

            footer [] [
                ActiveTodosCount { Count = activeTodosCount }

                ul [ Class "filters" ] [
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = All
                            Href = "#/"
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = Active
                            Href = "#/active"
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                    li [] [
                        FilterButton {
                            CurrentFilter = state.Filter
                            FilterBy = Completed
                            Href = "#/completed"
                            OnClick = (fun filter -> dispatch (Filter filter))
                        }
                    ]
                ]

                if (completedTodosCount > 0) then
                    button [ Class "clear";
                             OnClick (fun _ -> dispatch ClearCompleted) ]
                           [ str (sprintf "Clear completed (%i)" completedTodosCount) ]
            ]
        ]
    ])

ReactDom.render (App(), document.getElementById "app")
