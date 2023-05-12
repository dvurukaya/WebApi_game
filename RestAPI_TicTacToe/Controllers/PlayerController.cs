using Microsoft.AspNetCore.Mvc;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Services;

namespace RestAPI_TicTacToe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;
        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Player), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayerByIdAsync(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if(player == null) { return NotFound();  }

            return Ok(player);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Player>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlayersAsync()
        {
            var players = await _playerService.GetAllPlayersAsync();
            if(players.Count < 1) { return NotFound(); }

            return Ok(players);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePlayerAsync(string name)
        {
            var created = await _playerService.CreatePlayerAsync(name);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Player), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlayerAsync(int playerId, Player player)
        {
            if(playerId != player.Id) { return BadRequest();  }

            var updated = await _playerService.UpdatePlayerAsync(player);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Player), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlayerAsync(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if(player == null) { return NotFound(); }

            await _playerService.DeletePlayerAsync(id);
            return NoContent();
        }
    }
}
