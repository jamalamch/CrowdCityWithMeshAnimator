
namespace MatrixAlgebra
{
    [System.Serializable]
    public class Bool2dArray : Array2D<bool>
    {
        public Bool2dArray(int width, int height) : base(width, height)
        {
        }

        public Bool2dArray(int width, int height, bool[] elemnts) : base(width, height, elemnts)
        {
        }

        public Bool2dArray(Bool2dArray bool2DArray) : base(bool2DArray)
        {
        }
    }
}