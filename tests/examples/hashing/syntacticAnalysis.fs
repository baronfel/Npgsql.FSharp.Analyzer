module Postgres

open Npgsql.FSharp
open Npgsql.FSharp.OptionWorkflow

let connectionString = "Dummy connection string"

let findSingleUser(userId: int) = 
    connectionString
    |> Sql.connect
    |> Sql.query "SELECT * FROM users WHERE user_id = @user_id"
    |> Sql.parameters [ "@user_id", Sql.int userId ]
    |> Sql.executeAsync (fun read ->
        option {
            let username = read.text "username"
            let user_id = read.int "user_id" 
            let active = read.bool "active"
            return (username, user_id, active)
        })

let findUsers() = 
    Sql.host "localhost"
    |> Sql.connectFromConfig
    |> Sql.query "SELECT * FROM users"
    |> Sql.parameters [ "@whatever", Sql.bit false; "@another", Sql.int 12 ]
    |> Sql.executeAsync (fun read ->
        option {
            let username = read.text "username"
            let user_id = read.int "user_id" 
            let active = read.bool "active"
            return (username, user_id, active)
        })

let findNumberOfUsers() = 
    Sql.host "localhost"
    |> Sql.connectFromConfig
    |> Sql.query "SELECT COUNT(*) as count FROM users"
    |> Sql.executeSingleRow (fun read -> read.int64 "count")

let executeFunction() =
    Sql.host "localhost"
    |> Sql.connectFromConfig
    |> Sql.func "getNumberOfUsers"
    |> Sql.execute (fun read -> read.text "username")
