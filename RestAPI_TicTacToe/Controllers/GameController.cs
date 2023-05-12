using Microsoft.AspNetCore.Mvc;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Services;
using RestAPI_TicTacToe.StaticInfo;
using System.Reflection.Metadata.Ecma335;

namespace RestAPI_TicTacToe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService) 
        {
            _gameService = gameService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllGamesAsync()
        {
            var games = await _gameService.GetAllGamesAsync();
            if (games.Count < 1) { return NotFound(); }
            return Ok(games);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> GetGameByIdAsync(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if(game == null) { return NotFound(); }
            return Ok(game);
        }

        [HttpGet("{code:guid}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGameByCodeAsync([FromRoute] Guid code)
        {
            var game = await _gameService.GetGameByCodeAsync(code);
            if(game == null) { return NotFound(); }
            return Ok(game);
        }

        [HttpGet("{date}")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGamesByDateAsync(DateTime date)
        {
            var games = await _gameService.GetGamesByDateAsync(date);
            if (games.Count < 1) { return NotFound(); }
            return Ok(games);
        }

        [HttpGet("{names}")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGamesByPlayersNamesAsync(string FirstName, string SecondName)
        {
            var games = await _gameService.GetGamesByPlayersNames(FirstName, SecondName);
            if (games.Count < 1) { return NotFound(); }
            return Ok(games);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateGameAsync(int firstPlayerId,int secondPlayerId)
        {
            var game = await _gameService.CreateGameAsync(firstPlayerId,secondPlayerId);
            return Ok(game);
        }

        [HttpPost("{code:guid}/joinGame")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGameToJoinAsync(Guid code)
        {
            var game = await _gameService.GetGameByCodeAsync(code);
            if(game == null) { return NotFound(); }

            var updatedGame = await _gameService.EntryGameWithSecondPlayerAsync(code);
            return Ok(updatedGame);
        }


        [HttpPost("{gameId}/createMove")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMoveAsync(int playerId, int gameId, int cell)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);
       
            if (game == null) { return NotFound($"Game with ID {gameId} wasn't found"); }

            if(game.Status == GameStatus.GameOver) { return BadRequest
                                                     ($"Game {game} is already finished"); }

            try
            {
                var check = await _gameService.CreateMoveAsync(playerId, gameId, cell);
                return Ok(check);
            }
            catch(ApplicationException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Game), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) { return NotFound(); }

            await _gameService.DeleteGameAsync(id);
            return NoContent();
        }
    }
}
