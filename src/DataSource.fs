module DataSource

open Todo

let createText text =
    text
    |> TodoText.create
    |> Option.get

let todos =
    [ { Id = System.Guid.NewGuid (); Text = createText "active 1"; Completed = false }
      { Id = System.Guid.NewGuid (); Text = createText "active 2"; Completed = false }
      { Id = System.Guid.NewGuid (); Text = createText "active 3"; Completed = false }
      { Id = System.Guid.NewGuid (); Text = createText "active 4"; Completed = false }
      { Id = System.Guid.NewGuid (); Text = createText "completed 5"; Completed = true }
      { Id = System.Guid.NewGuid (); Text = createText "completed 6"; Completed = true }
      { Id = System.Guid.NewGuid (); Text = createText "completed 7"; Completed = true } ]

