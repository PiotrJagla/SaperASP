namespace saper.DataStructures
{
    public class Cell
    {
        public enum Type
        {
            empty = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            bomb,
            flagged, 
            notOpened,
            detonatedBomb,
            missedBomb
        }
        public Type cellType { get; set; }
        public bool IsOpened { get; set; }

        public bool IsFlagged { get; set; }


        public Cell(Type cellType)
        {
            this.cellType = cellType;
            IsOpened = false;
            IsFlagged = false;
        }

    }
}
