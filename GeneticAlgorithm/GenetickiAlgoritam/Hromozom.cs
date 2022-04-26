namespace GenetickiAlgoritam
{
    public class Hromozom
    {
        public int x { get; set; }
        public int y { get; set; }

        public Hromozom()
        {
            this.x = 0;
            this.y = 0;
        }

        public Hromozom(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public string toString()
        {
            return "x = " + Funkcije.Dekodiraj(this.x) + ", y = " + Funkcije.Dekodiraj(this.y);
        }
    }
}