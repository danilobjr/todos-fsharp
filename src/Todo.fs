module Todo

open System

type Todo = {
    Id: Guid
    Text: string
    Completed: bool
}

let create text = {
    Id = Guid.NewGuid ()
    Text = text
    Completed = false
}