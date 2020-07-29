module Todo

open System
open TodoText

type Todo =
    { Id: Guid
      Text: Text
      Completed: bool }

// TODO rename this to create and use Todo.create in other files
let createTodo = function
    | Some text ->
        Some { Id = Guid.NewGuid ()
               Text = text
               Completed = false }
    | None -> None
