namespace saper.DataStructures
{
    public class Vector2D
    {
        public int x { get; set; }
        public int y { get; set; }

        public Vector2D(int x=0, int y=0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D(Vector2D vector2D)
        {
            this.x = vector2D.x;
            this.y = vector2D.y;
        }
    }
}
