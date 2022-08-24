using Microsoft.AspNetCore.Mvc;
using saper.DataStructures;
using saper.Game;

namespace saper.Controllers
{
    public class SaperController : Controller
    {
        private static GameBoard saperBoard = new GameBoard();

        public SaperController()
        {

        }


        public IActionResult Index()
        {
            return View("~/Views/Game.cshtml", saperBoard);
        }

        public IActionResult gameBoardLeftMouseClickEvent(string buttonPosition)
        {
            Vector2D buttonCoordinates = new Vector2D(StringManipulation.getButtonCoordsFromString(buttonPosition));
            saperBoard.openButton(buttonCoordinates);
            return View("~/Views/Game.cshtml", saperBoard);
        }

        public IActionResult gameBoardRightMouseClickEvent(string buttonPosition)
        {
            Vector2D buttonCoordinates = new Vector2D(StringManipulation.getButtonCoordsFromString(buttonPosition));
            saperBoard.flagButton(buttonCoordinates);
            return View("~/Views/Game.cshtml", saperBoard);
        }
    }
}
