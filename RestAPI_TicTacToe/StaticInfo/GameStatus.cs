namespace RestAPI_TicTacToe.StaticInfo
{
    public enum GameStatus
    {
        Registered, //one player registered the game, received a code and wait for other player to join
        Started,
        FirstPlayerTurn,
        SecondPlayerTurn,
        GameOver
    }
}
