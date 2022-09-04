using saper.DataStructures;

namespace saper.Game
{
    public class GameBoard
    {
        
        private List<List<Cell>> gameBoard;
        private int bombsNumber;
        private Vector2D gameBoardSize;
        private bool isCustomModeEnabled;


        public GameBoard()
        {
            this.initVariables();
            this.allocateGameBoardMemory();
            this.initGameBoard();
        }

        private void writeInConsoleBombsCoordinates()
        {
            //TEST FUNCTION
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (this.isThereABomb(new Vector2D(iii,kkk)) == true)
                    {
                        Console.WriteLine("X: " + iii + " Y: " + kkk);
                    }
                }
            }
        }

        private void initVariables()
        {
            bombsNumber = 10;
            gameBoardSize = new Vector2D(9, 9);
            isCustomModeEnabled = false;
        }


        private void allocateGameBoardMemory()
        {
            gameBoard = new List<List<Cell>>();
            for (int iii = 0; iii < gameBoardSize.y; ++iii)
            {
                gameBoard.Add(new List<Cell>());
                for (int kkk = 0; kkk < gameBoardSize.x; ++kkk)
                {
                    gameBoard[iii].Add(new Cell(Cell.Type.one));
                }
            }
        }

        private void initGameBoard()
        {
            this.distributeGameBoardBombs();
            this.insertCorrectNumbersInCells();
        }

        private void distributeGameBoardBombs()
        {
            for(int iii = 0; iii<this.bombsNumber; ++iii)
            {
                this.pickRandomBombCoordinates();
            }
        }

        private void pickRandomBombCoordinates()
        {
            Vector2D randomBombCoordinates = new Vector2D();
            randomBombCoordinates.x = ProjectScopeResources.randomInt.Next(this.gameBoardSize.x);
            randomBombCoordinates.y = ProjectScopeResources.randomInt.Next(this.gameBoardSize.y);

            while (isThereABomb(randomBombCoordinates) == true)
            {
                randomBombCoordinates.x = ProjectScopeResources.randomInt.Next(this.gameBoardSize.x);
                randomBombCoordinates.y = ProjectScopeResources.randomInt.Next(this.gameBoardSize.y);
            }

            this.insertBomb(randomBombCoordinates);
        }

        private bool isThereABomb(Vector2D gameBoardCoordinates)
        {
            return gameBoard[gameBoardCoordinates.y][gameBoardCoordinates.x].cellType == Cell.Type.bomb;
        }

        private void insertBomb(Vector2D coordinates)
        {
            gameBoard[coordinates.y][coordinates.x].cellType = Cell.Type.bomb;
        }

        private void insertCorrectNumbersInCells()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (this.isThereABomb(new Vector2D(iii, kkk)) == false)
                    {
                        this.insertNumberInCell(new Vector2D(iii, kkk));
                    }
                }
            }
        }

        private void insertNumberInCell(Vector2D cellCoordinates)
        {
            gameBoard[cellCoordinates.y][cellCoordinates.x].cellType = 
                (Cell.Type)this.getAdjancentBombsNumber(cellCoordinates);
        }

        private int getAdjancentBombsNumber(Vector2D cellCoordinates)
        {
            int adjacentBombsNumber = 0;
            for (int iii = cellCoordinates.x - 1; iii <= cellCoordinates.x + 1; ++iii)
            {
                for (int kkk = cellCoordinates.y - 1; kkk <= cellCoordinates.y + 1; ++kkk)
                {
                    if (Validation.isOutsize2DArray(new Vector2D(iii, kkk), gameBoardSize) == false &&
                        this.isThereABomb(new Vector2D(iii, kkk)) == true)
                    {
                        ++adjacentBombsNumber;
                    }
                }
            }
            return adjacentBombsNumber;
        }

        public void flagButton(Vector2D buttonCoordinates)
        {
            if (gameBoard[buttonCoordinates.y][buttonCoordinates.x].IsOpened == false &&
                this.isGameBlocked() == false)
            {
                gameBoard[buttonCoordinates.y][buttonCoordinates.x].IsFlagged =
                    !gameBoard[buttonCoordinates.y][buttonCoordinates.x].IsFlagged;
            }
        }

        public void openButton(Vector2D buttonCoordinates)
        {
            if (gameBoard[buttonCoordinates.y][buttonCoordinates.x].IsFlagged == false &&
                this.isGameBlocked() == false)
            {
                gameBoard[buttonCoordinates.y][buttonCoordinates.x].IsOpened = true;
                this.specjalOpenedButtonCases(buttonCoordinates);
            }
        }

        private bool isGameBlocked()
        {
            return this.didLost() == true || this.didWon() == true;
        }

        public bool didLost()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[kkk][iii].cellType == Cell.Type.detonatedBomb)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool didWon()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[kkk][iii].cellType == Cell.Type.bomb &&
                        gameBoard[kkk][iii].IsFlagged == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void specjalOpenedButtonCases(Vector2D buttonCoordinates)
        {
            if (this.isItFirstRound() == true && this.isThereABomb(buttonCoordinates) == true)
                this.changeBombPosition(buttonCoordinates);

            this.isEmptyCellClicked(buttonCoordinates);
            this.isBombClicked(buttonCoordinates);
        }

        private bool isItFirstRound()
        {
            return this.openedCellsNumber() == 1;
        }

        private int openedCellsNumber()
        {
            int openedCells = 0;
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[kkk][iii].IsOpened == true)
                        ++openedCells;
                }
            }
            return openedCells;
        }

        private void changeBombPosition(Vector2D bombCoordinates)
        {
            gameBoard[bombCoordinates.y][bombCoordinates.x].cellType = Cell.Type.empty;
            this.pickRandomBombCoordinates();
            this.insertCorrectNumbersInCells();
        }

        private void isEmptyCellClicked(Vector2D cellCoordinates)
        {
            if (gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.empty)
                this.revealAdjancedEmptyCells(cellCoordinates);
        }

        private void revealAdjancedEmptyCells(Vector2D cellCoordinates)
        {
            List<Vector2D> adjancedEmptyCellsCoords = this.getAdjancedEmptyCellsCoords(cellCoordinates);

            while (adjancedEmptyCellsCoords.Count != 0)
            {
                for (int iii = 0; iii < adjancedEmptyCellsCoords.Count; ++iii)
                    gameBoard[adjancedEmptyCellsCoords[iii].y][adjancedEmptyCellsCoords[iii].x].IsOpened = true;
                
                List<Vector2D> revealedAdjancedEmptyCellsCoords = adjancedEmptyCellsCoords.ToList<Vector2D>();
                adjancedEmptyCellsCoords.Clear();

                for (int iii = 0; iii < revealedAdjancedEmptyCellsCoords.Count; ++iii)
                    adjancedEmptyCellsCoords.AddRange(this.getAdjancedEmptyCellsCoords(revealedAdjancedEmptyCellsCoords[iii]));
            }
        }

        private List<Vector2D> getAdjancedEmptyCellsCoords(Vector2D cellCoordinates)
        {
            
            List<Vector2D> adjancedEmptyCellsCoords = new List<Vector2D>();

            List<Vector2D> LeftRightUpAndDownCellCoords = new List<Vector2D>();
            LeftRightUpAndDownCellCoords.Add(new Vector2D(cellCoordinates.x, cellCoordinates.y - 1));
            LeftRightUpAndDownCellCoords.Add(new Vector2D(cellCoordinates.x, cellCoordinates.y + 1));
            LeftRightUpAndDownCellCoords.Add(new Vector2D(cellCoordinates.x - 1, cellCoordinates.y));
            LeftRightUpAndDownCellCoords.Add(new Vector2D(cellCoordinates.x + 1, cellCoordinates.y));
          
            for (int iii = 0; iii < LeftRightUpAndDownCellCoords.Count; ++iii)
            {
                if (Validation.isOutsize2DArray(LeftRightUpAndDownCellCoords[iii], gameBoardSize) == false)
                {
                    if (this.isThereANumber(LeftRightUpAndDownCellCoords[iii]) == true)
                    {
                        gameBoard[LeftRightUpAndDownCellCoords[iii].y][LeftRightUpAndDownCellCoords[iii].x].IsOpened = true;
                    }
                    else if (gameBoard[LeftRightUpAndDownCellCoords[iii].y][LeftRightUpAndDownCellCoords[iii].x].IsOpened == false)
                    {
                        adjancedEmptyCellsCoords.Add(LeftRightUpAndDownCellCoords[iii]);
                    }
                }
            }

            return adjancedEmptyCellsCoords.ToList<Vector2D>();
        }

        private bool isThereANumber(Vector2D cellCoordinates)
        {
            return gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.one ||
                   gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.two ||
                   gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.three ||
                   gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.four ||
                   gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.five ||
                   gameBoard[cellCoordinates.y][cellCoordinates.x].cellType == Cell.Type.six;
        }

        public bool isBombClicked(Vector2D clickedButtonCoordinates)
        {
            if (gameBoard[clickedButtonCoordinates.y][clickedButtonCoordinates.x].IsOpened == true &&
                this.isThereABomb(clickedButtonCoordinates) == true &&
                this.didLost() == false)
            {
                gameBoard[clickedButtonCoordinates.y][clickedButtonCoordinates.x].cellType = Cell.Type.detonatedBomb;
                this.revealAllBombs();
                return true;
            }
            return false;
        }

        private void revealAllBombs()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[kkk][iii].IsFlagged == true &&
                         this.isThereABomb(new Vector2D(iii, kkk)) == false)
                    {
                        gameBoard[kkk][iii].cellType = Cell.Type.missedBomb;
                        gameBoard[kkk][iii].IsFlagged = false;
                        gameBoard[kkk][iii].IsOpened = true;
                    }
                    else if (isThereABomb(new Vector2D(iii, kkk)) == true)
                        gameBoard[kkk][iii].IsOpened = true;
                }
            }
        }


        public void resetGameBoard()
        {
            this.setDefaultCellsParameters();
            this.initGameBoard();
        }

        private void setDefaultCellsParameters()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    gameBoard[kkk][iii].IsOpened = false;
                    gameBoard[kkk][iii].IsFlagged = false;
                    gameBoard[kkk][iii].cellType = Cell.Type.empty;

                }
            }
        }

        public int getXsize()
        {
            return gameBoard[0].Count();
        }

        public int getYsize()
        {
            return gameBoard.Count();
        }

        public Cell.Type getButtonState(Vector2D cellPos)
        {
            if (gameBoard[cellPos.y][cellPos.x].IsFlagged == true)
                return Cell.Type.flagged;
            else if (gameBoard[cellPos.y][cellPos.x].IsOpened == false)
                return Cell.Type.notOpened;
            else
                return gameBoard[cellPos.y][cellPos.x].cellType;
        }

        public int bombsLeft()
        {
            return this.bombsNumber - this.getFlaggedButtonsNumber();
        }

        private int getFlaggedButtonsNumber()
        {
            int flagButtons = 0;
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[kkk][iii].IsFlagged == true)
                        ++flagButtons;
                }
            }
            return flagButtons;
        }

        public void changeDifficultyLevel(string difficultyLevel)
        {
            if (difficultyLevel.ToLower() == "easy")
            {
                this.bombsNumber = 10;
                this.gameBoardSize.x = 9;
                this.gameBoardSize.y = 9;
            }
            else if (difficultyLevel.ToLower() == "medium")
            {
                this.bombsNumber = 40;
                this.gameBoardSize.x = 16;
                this.gameBoardSize.y = 16;
            }
            else if (difficultyLevel.ToLower() == "hard")
            {
                this.bombsNumber = 99;
                this.gameBoardSize.x = 25;
                this.gameBoardSize.y = 25;
            }
            else 
            {
                isCustomModeEnabled = true;
                return;
            }

            this.allocateGameBoardMemory();
            this.resetGameBoard();
        }

        public bool isCustomInputConfigurationShown()
        {
            return isCustomModeEnabled;
        }

        public void changeGameBoardToCustom(Vector2D newGameBoardSize, int bombsNumber)
        {
            isCustomModeEnabled = false;
            this.bombsNumber = bombsNumber;
            this.gameBoardSize = new Vector2D(newGameBoardSize.x, newGameBoardSize.y);
            this.allocateGameBoardMemory();
            this.resetGameBoard();
        }
    }
}
