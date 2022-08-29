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
                if (this.isItFirstRound() == true && this.isThereABomb(buttonCoordinates) == true)
                    this.changeBombPosition(buttonCoordinates);
                
                gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsOpened = true;
            }
            this.isBombClicked(buttonCoordinates);
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

        private bool isItFirstRound()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[iii][kkk].IsOpened == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void changeBombPosition(Vector2D bombCoordinates)
        {
            this.pickRandomBombCoordinates();
            gameBoard[bombCoordinates.x][bombCoordinates.y].cellType =
                (Cell.Type)(this.getAdjancentBombsNumber(bombCoordinates) - 1);
            //Minus jeden ponieważ w tej komórce znajdowała się bomba, którą policzyła funkcja
            //getAdjancentBombsNumber
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
            this.resetGameBoard();
        }
    }
}
