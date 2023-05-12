using RestAPI_TicTacToe.StaticInfo;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI_TicTacToe.Models
{
    public class Game
    {
        public int Id { get; set; }
        public Guid CodeGame { get; set; }
        public DateTime Date { get; set; }
        public int FirstPlayerId { get; set; }
        public int SecondPlayerId { get; set; }
        public bool Registered { get; set; }
        public bool Draw { get; set; }
        public int WinnerId { get; set; }
        [NotMapped]
        public int[] Board { get; set; }
        public Player FirstPlayer { get; set; }        
        public Player SecondPlayer { get; set; }
        public GameStatus Status { get; set; }
        public ICollection<Move> Moves { get; set; }

    }
}
