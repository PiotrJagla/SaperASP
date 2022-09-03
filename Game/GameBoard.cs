using saper.DataStructures;

namespace saper.Game
{
    public class GameBoard
    {
        
        private List<List<Cell>> gameBoard;
        private int bombsNumber;
        private Vector2D gameBoardSize;


        public GameBoard()
        {
            this.initVariables();
            this.allocateGameBoardMemory();
            this.initGameBoard();
            this.writeInConsoleBombsCoordinates();
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
        }


        private void allocateGameBoardMemory()
        {
            gameBoard = new List<List<Cell>>();
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                gameBoard.Add(new List<Cell>());
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
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
            return gameBoard[gameBoardCoordinates.x][gameBoardCoordinates.y].cellType == Cell.Type.bomb;
        }

        private void insertBomb(Vector2D coordinates)
        {
            gameBoard[coordinates.x][coordinates.y].cellType = Cell.Type.bomb;
        }

        private void insertCorrectNumbersInCells()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if(this.isThereABomb(new Vector2D(kkk,iii)) == false)
                        this.insertNumberInCell(new Vector2D(kkk, iii));
                }
            }
        }

        private void insertNumberInCell(Vector2D cellCoordinates)
        {
            gameBoard[cellCoordinates.x][cellCoordinates.y].cellType = 
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
            if (gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsOpened == false &&
                this.isGameBlocked() == false)
            {
                gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged =
                    !gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged;
            }
        }

        public void openButton(Vector2D buttonCoordinates)
        {
            if (gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged == false &&
                this.isGameBlocked() == false)
            {
                gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsOpened = true;
                this.specjalOpenedButtonCases(buttonCoordinates);
            }
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
                    if (gameBoard[iii][kkk].IsOpened == true)
                        ++openedCells;
                }
            }
            return openedCells;
        }

        private bool isGameBlocked()
        {
            return this.didLost() == true || this.didLost() == true;
        }

        public bool didLost()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[iii][kkk].cellType == Cell.Type.detonatedBomb)
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
                    if (gameBoard[iii][kkk].cellType == Cell.Type.bomb &&
                        gameBoard[iii][kkk].IsFlagged == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void changeBombPosition(Vector2D bombCoordinates)
        {
            gameBoard[bombCoordinates.x][bombCoordinates.y].cellType = Cell.Type.empty;
            this.pickRandomBombCoordinates();
            this.insertCorrectNumbersInCells();
        }

        public bool isBombClicked(Vector2D clickedButtonCoordinates)
        {
            if (gameBoard[clickedButtonCoordinates.x][clickedButtonCoordinates.y].IsOpened == true &&
                this.isThereABomb(new Vector2D(clickedButtonCoordinates.x, clickedButtonCoordinates.y)) == true &&
                this.didLost() == false)
            {
                gameBoard[clickedButtonCoordinates.x][clickedButtonCoordinates.y].cellType = Cell.Type.detonatedBomb;
                this.revealAllBombs();
                return true;
            }
            return false;
        }

        private void isEmptyCellClicked(Vector2D cellCoordinates)
        {
            if (gameBoard[cellCoordinates.x][cellCoordinates.y].cellType == Cell.Type.empty)
                this.revealAdjancedEmptyCells(cellCoordinates);
        }

        private void revealAdjancedEmptyCells(Vector2D cellCoordinates)
        {
            List<Vector2D> adjancedEmptyCellsCoords = this.getAdjancedEmptyCellsCoords(cellCoordinates);

            while(adjancedEmptyCellsCoords.Count != 0)
            {
                for(int iii = 0; iii < adjancedEmptyCellsCoords.Count; ++iii)
                {
                    gameBoard[adjancedEmptyCellsCoords[iii].x][adjancedEmptyCellsCoords[iii].y].IsOpened = true;
                }
                List<Vector2D> temp = adjancedEmptyCellsCoords.ToList<Vector2D>();
                adjancedEmptyCellsCoords.Clear();
                for(int iii = 0; iii < temp.Count; ++iii)
                {
                    adjancedEmptyCellsCoords.AddRange(this.getAdjancedEmptyCellsCoords(temp[iii]));
                }
            }
        }

        private List<Vector2D> getAdjancedEmptyCellsCoords(Vector2D cellCoordinates)
        {
            List<Vector2D> adjancedEmptyCellsCoords = new List<Vector2D>();

            Vector2D left = new Vector2D(cellCoordinates.x, cellCoordinates.y - 1);
            Vector2D right = new Vector2D(cellCoordinates.x, cellCoordinates.y + 1);
            Vector2D up = new Vector2D(cellCoordinates.x - 1, cellCoordinates.y);
            Vector2D down = new Vector2D(cellCoordinates.x + 1, cellCoordinates.y);

            if (Validation.isOutsize2DArray(left, gameBoardSize) == false && 
                this.isThereANumber(left) == true)
                gameBoard[left.x][left.y].IsOpened = true;

            if (Validation.isOutsize2DArray(right, gameBoardSize) == false && 
                this.isThereANumber(right) == true)
                gameBoard[right.x][right.y].IsOpened = true;

            if (Validation.isOutsize2DArray(up, gameBoardSize) == false && 
                this.isThereANumber(up) == true)
                gameBoard[up.x][up.y].IsOpened = true;

            if (Validation.isOutsize2DArray(down, gameBoardSize) == false && 
                this.isThereANumber(down) == true)
                gameBoard[down.x][down.y].IsOpened = true;

            
            if (Validation.isOutsize2DArray(left, gameBoardSize) == false && 
                gameBoard[left.x][left.y].IsOpened == false)
                adjancedEmptyCellsCoords.Add(left);

            if (Validation.isOutsize2DArray(right, gameBoardSize) == false && 
                gameBoard[right.x][right.y].IsOpened == false)
                adjancedEmptyCellsCoords.Add(right);

            if (Validation.isOutsize2DArray(up, gameBoardSize) == false && 
                gameBoard[up.x][up.y].IsOpened == false)
                adjancedEmptyCellsCoords.Add(up);

            if (Validation.isOutsize2DArray(down, gameBoardSize) == false && 
                gameBoard[down.x][down.y].IsOpened == false)
                adjancedEmptyCellsCoords.Add(down);

            return adjancedEmptyCellsCoords;
        }

        private bool isThereANumber(Vector2D cellCoordinates)
        {
            return gameBoard[cellCoordinates.x][cellCoordinates.y].cellType == Cell.Type.one ||
                   gameBoard[cellCoordinates.x][cellCoordinates.y].cellType == Cell.Type.two ||
                   gameBoard[cellCoordinates.x][cellCoordinates.y].cellType == Cell.Type.three ||
                   gameBoard[cellCoordinates.x][cellCoordinates.y].cellType == Cell.Type.four;
        }

        private void revealAllBombs()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[iii][kkk].IsFlagged == true &&
                         this.isThereABomb(new Vector2D(iii, kkk)) == false)
                    {
                        gameBoard[iii][kkk].cellType = Cell.Type.missedBomb;
                        gameBoard[iii][kkk].IsFlagged = false;
                        gameBoard[iii][kkk].IsOpened = true;
                    }
                    else if (isThereABomb(new Vector2D(iii, kkk)) == true)
                        gameBoard[iii][kkk].IsOpened = true;
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
                    gameBoard[iii][kkk].IsOpened = false;
                    gameBoard[iii][kkk].IsFlagged = false;
                    gameBoard[iii][kkk].cellType = Cell.Type.empty;

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
            if (gameBoard[cellPos.x][cellPos.y].IsFlagged == true)
                return Cell.Type.flagged;
            else if (gameBoard[cellPos.x][cellPos.y].IsOpened == false)
                return Cell.Type.notOpened;
            else
                return gameBoard[cellPos.x][cellPos.y].cellType;
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
                    if (gameBoard[iii][kkk].IsFlagged == true)
                        ++flagButtons;
                }
            }
            return flagButtons;
        }

        public void changeDifficultyLevel(string difficultyLevel)
        {
            if(difficultyLevel.ToLower() == "easy")
            {
                this.bombsNumber = 10;
                this.gameBoardSize.x = 9;
                this.gameBoardSize.y = 9;
            }
            else if(difficultyLevel.ToLower() == "medium")
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
            this.allocateGameBoardMemory();
            this.resetGameBoard();
        }
    }
}
