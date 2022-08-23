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

            while(isThereABomb(randomBombCoordinates) == true)
            {
                randomBombCoordinates.x = ProjectScopeResources.randomInt.Next(this.gameBoardSize.x);
                randomBombCoordinates.y = ProjectScopeResources.randomInt.Next(this.gameBoardSize.y);
            }

            this.insertBomb(randomBombCoordinates);
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
                    if(Validation.isOutsize2DArray(new Vector2D(iii,kkk), gameBoardSize) == false &&
                        this.isThereABomb(new Vector2D(iii,kkk)) == true)
                    {
                        ++adjacentBombsNumber;
                    }
                        
                }
            }

            return adjacentBombsNumber;
        }

        private bool isThereABomb(Vector2D gameBoardCoordinates)
        {
            return gameBoard[gameBoardCoordinates.x][gameBoardCoordinates.y].cellType == Cell.Type.bomb;
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

        private void initVariables()
        {
            bombsNumber = 10;
            gameBoardSize = new Vector2D(9, 9);
        }

        public void openButton(Vector2D buttonCoordinates)
        {
            if(gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged == false)
                gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsOpened = true;
        }

        public void flagButton(Vector2D buttonCoordinates)
        {
            if(gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsOpened == false)
            {
                gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged = 
                    !gameBoard[buttonCoordinates.x][buttonCoordinates.y].IsFlagged;
            }
        }

        public Cell.Type getButtonState(Vector2D cellPos)
        {
            Cell.Type cellType = gameBoard[cellPos.x][cellPos.y].cellType;

            if (gameBoard[cellPos.x][cellPos.y].IsOpened == false)
            {
                cellType = Cell.Type.notOpened;
            }

            if(gameBoard[cellPos.x][cellPos.y].IsFlagged == true)
            {
                cellType = Cell.Type.flagged;
            }

            return cellType;
        }

        public int getXsize()
        {
            return gameBoard[0].Count();
        }

        public int getYsize()
        {
            return gameBoard.Count();
        }

        public bool isBombClicked()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (gameBoard[iii][kkk].IsOpened == true &&
                        isThereABomb(new Vector2D(iii,kkk)) == true)
                    {
                        gameBoard[iii][kkk].cellType = Cell.Type.detonatedBomb;
                        this.revealAllBombs();
                        return true;
                    }
                }
            }
            return false;
        }

        private void revealAllBombs()
        {
            for (int iii = 0; iii < gameBoardSize.x; ++iii)
            {
                for (int kkk = 0; kkk < gameBoardSize.y; ++kkk)
                {
                    if (isThereABomb(new Vector2D(iii, kkk)) == true)
                    {
                        gameBoard[iii][kkk].IsOpened = true;
                    }
                }
            }
        }

        
    }
}
