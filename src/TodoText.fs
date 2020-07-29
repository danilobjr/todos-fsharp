module TodoText

open System

type Text = private Text of string

let create text =
    if (String.IsNullOrEmpty text) then
        None
    else if (String.length text > 240) then
        None
    else
        Some (Text text)

let value (Text t) = t