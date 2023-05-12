using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe.Models
{
    public class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; } 
        public int Cell { get; set; }
        public Elements Element { get; set; }        
        public Player Player { get; set; }
    }
}
