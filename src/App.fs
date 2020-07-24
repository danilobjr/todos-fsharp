module App

open Browser.Dom
open Fable.React
open Fable.React.Props

let App =
    fun () ->
        div [] [
            header [] [
                h1 [] [ str "todos" ]
                input [ Class "new"
                        Placeholder "What needs to be done?"
                        AutoFocus true ]
            ]

            section [ Class "main" ] [
                input [ Id "toggle-all"; Type "checkbox" ]
                label [ HtmlFor "toggle-all" ] [ str "Mark all as complete" ]

                ul [ Class "todos" ] []

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
        ]

ReactDom.render (App(), document.getElementById "app")
