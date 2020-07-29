module Todo

open System
open TodoText

type Todo =
    { Id: Guid
      Text: Text
      Completed: bool }

let create =
    function
    | Some text ->
        Some { Id = Guid.NewGuid()
               Text = text
               Completed = false }
    | None -> None
