module Data

open Todo

let data = [
    { Id = System.Guid.NewGuid (); Text = "active 1"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 2"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 3"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "active 4"; Completed = false }
    { Id = System.Guid.NewGuid (); Text = "completed 5"; Completed = true }
    { Id = System.Guid.NewGuid (); Text = "completed 6"; Completed = true }
    { Id = System.Guid.NewGuid (); Text = "completed 7"; Completed = true }
]

