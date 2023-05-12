# WebApi_game

## Project description:
This is a realization of the game TicTacToe in C#, made by using Repository-Service-Controller pattern.

**Made by using:**
1. *.NET 7.0*
2. *AspNetCore 7.0.1*
3. *Entity FrameworkCore 7.0.5*
4. *Entity FrameworkCore.InMemory 7.0.5*
5. *Entity FrameworkCore.SqlServer 7.0.5*
6. *Entity FrameworkCore.Tools 7.0.5*
7. *xUnit 2.4.2*
8. *Moq 4.18.4*

## Game description:
Game for two players with opportunity to create game by one player and waiting for another to join by using code. 

## Api description:
**Game**
|**HTTP method**|  **EndPoint** |**Description**| **Body request(JSON)** | **Body response(JSON)** | **Errors** |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
|      GET      |        api/Game        |          *Get list of games*           | -                                         | Massive of objects "Game"  | 404 Not Found |
|      GET      |     api/Game/{id}      |         *Get game by it's ID*          | -                                         |       Object "Game"        | 404 Not Found |
|      GET      | api/Game/{code:guid}   |      *Get game by special code*        |       { "codeGame": Guid.NewGuid() }      |       Object "Game"        | 404 Not Found |
|      GET      |    api/Game/{date}     |       *Get game(s) by date(s)*         |       { "date": new DateTime( , , )}      | Object "Game"/Massive of objects "Game" | 404 Not Found |
|      GET      |   api/Game/{names}     |    *Get game(s) by player's names*     | { "firstName":"Name", "secondName":"Name"}| Object "Game"/Massive of objects "Game" | 404 Not Found |
|      POST     |        api/Game/       |          *Create a new game*           | { "firstPlayerId":1, "secondPlayerId":2 } |       Object "Game"        |       -       |
|      POST     | ../{code:guid}/joinGame| *Join created game for second player*  |       { "codeGame": Guid.NewGuid() }      |       Object "Game"        | 404 Not Found |
|      POST     | ../{gameId}/createMove |             *Make a move*              |        { "playerId":1, "cell": 1 }        |       Object "Game"        | 404 Not Found , 400 Bad Request |
|      DELETE   |     api/Game/{id}      |             *Delete game*              | -                                         |             -              | 404 Not Found |

**Player**
|**HTTP method**|  **EndPoint** |**Description**| **Body request(JSON)** | **Body response(JSON)** | **Errors** |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
|      GET      |        api/Player        |    *Get list of players*       | -                     | Massive of objects "Player" | 404 Not Found |
|      GET      |     api/Player/{id}      |   *Get player by it's ID*      | -                     |       Object "Player"       | 404 Not Found |
|      POST     |        api/Player/       |    *Create a new player*       | { "playerName": " " } |       Object "Player"       |       -       |
|      PUT      |     api/Player/{id}      |        *Update player*         |    Object "Player"    |       Object "Player"       | 404 Not Found, 400 Bad Request |
|      DELETE   |     api/Player/{id}      |        *Delete player*         | -                     |              -              | 404 Not Found |
