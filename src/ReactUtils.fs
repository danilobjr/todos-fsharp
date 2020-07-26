module ReactUtils

open Fable.React

let useReducer (reducer: 'State -> 'Action -> 'State) (initialState: 'State) =
    let reducerHook = Hooks.useReducer(reducer, initialState)
    let state = reducerHook.current
    let dispatch = reducerHook.update
    (state, dispatch)
