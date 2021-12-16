namespace Core
{
    public class RubixCube
    {
        private RubixCube(Dictionary<Face, Colour[,]> faces)
        {
            EnsureIsCube(faces);
            EnsureIs3x3(faces);

            _faces = faces;
        }

        private void EnsureIsCube(Dictionary<Face, Colour[,]> faces)
        {
            if (faces.Count != 6)
            {
                throw new ArgumentOutOfRangeException("A cube must have 6 faces.");
            }
        }

        private void EnsureIs3x3(Dictionary<Face, Colour[,]> faces)
        {
            if (faces.Any(f => f.Value.GetLength(0) != 3 || f.Value.GetLength(1) != 3))
            {
                throw new ArgumentOutOfRangeException("A cube must have 3 columns and 3 rows per face.");
            }
        }

        private readonly Dictionary<Face, Colour[,]> _faces;

        public Colour[,] this[Face face] => _faces[face];

        public RubixCube Rotate(Face face)
        {
            var rotated = Copy();
            rotated._faces[face] = rotated._faces[face].Rotate();
            return rotated;
        }

        private RubixCube Copy()
        {
            var copy = new Dictionary<Face, Colour[,]>();

            foreach(var face in _faces.Keys)
            {
                copy[face] = Copy(_faces[face]);
            }

            return new RubixCube(copy);
        }

        private Colour[,] Copy(Colour[,] face)
        {
            var copy = new Colour[face.GetLength(0), face.GetLength(1)];

            Array.Copy(face, copy, face.Length);

            return copy;
        }


        public static RubixCube Scrambled()
        {
            var totals = new Dictionary<Colour, int>()
            {
                { Colour.Blue, 0 },
                { Colour.Green, 0 },
                { Colour.Yellow, 0 },
                { Colour.White, 0 },
                { Colour.Orange, 0 },
                { Colour.Red, 0 }
            };

            var rnd = new Random();

            var cube = new Dictionary<Face, Colour[,]>();

            foreach (var face in Enum.GetValues(typeof(Face)).Cast<Face>())
            {
                cube.Add(face, new Colour[3, 3]
                {
                    { RandomColour(), RandomColour(), RandomColour() },
                    { RandomColour(), RandomColour(), RandomColour() },
                    { RandomColour(), RandomColour(), RandomColour() }
                });
            }

            return new RubixCube(cube);

            Colour RandomColour()
            {
                var colour = (Colour)rnd.Next(0, 5);

                while (totals[colour] >= 9)
                {
                    colour = Next(colour);
                }

                totals[colour]++;

                return colour;
            }

            Colour Next(Colour colour)
            {
                if (colour == Colour.Red)
                {
                    return Colour.Blue;
                }
                else
                {
                    return (Colour)((int)colour + 1);
                }

            }
        }

        public static RubixCube Solved => new(
            new Dictionary<Face, Colour[,]>
            {
                {
                    Face.Front,
                    new Colour[3, 3]
                    {
                        { Colour.Blue, Colour.Blue, Colour.Blue },
                        { Colour.Blue, Colour.Blue, Colour.Blue },
                        { Colour.Blue, Colour.Blue, Colour.Blue }
                    }
                },

                {
                    Face.Right,
                    new Colour[3, 3]
                    {
                        { Colour.Green, Colour.Green, Colour.Green },
                        { Colour.Green, Colour.Green, Colour.Green },
                        { Colour.Green, Colour.Green, Colour.Green }
                    }
                },

                {
                    Face.Back,
                    new Colour[3, 3]
                    {
                        { Colour.White, Colour.White, Colour.White },
                        { Colour.White, Colour.White, Colour.White },
                        { Colour.White, Colour.White, Colour.White }
                    }
                },

                {
                    Face.Left,
                    new Colour[3, 3]
                    {
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow },
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow },
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow }
                    }
                },

                {
                    Face.Top,
                    new Colour[3, 3]
                    {
                        { Colour.Orange, Colour.Orange, Colour.Orange },
                        { Colour.Orange, Colour.Orange, Colour.Orange },
                        { Colour.Orange, Colour.Orange, Colour.Orange }
                    }
                },

                {
                    Face.Bottom,
                    new Colour[3, 3]
                    {
                        { Colour.Red, Colour.Red, Colour.Red },
                        { Colour.Red, Colour.Red, Colour.Red },
                        { Colour.Red, Colour.Red, Colour.Red }
                    }
                },
            });
    }

    internal static class ArrayHelpers
    {
        public static T[,] Rotate<T>(this T[,] array)
        {
            var width = array.GetUpperBound(0) + 1;
            var height = array.GetUpperBound(1) + 1;
            var rotated = new T[height, width];

            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var newRow = col;
                    var newCol = height - (row + 1);

                    rotated[newCol, newRow] = array[col, row];
                }
            }

            return rotated;
        }
    }

    public static class FaceHelper
    {
        public static Face Opposite(this Face face)
        {
            return face switch
            {
                Face.Front => Face.Back,
                Face.Left => Face.Right,
                Face.Back => Face.Front,
                Face.Right => Face.Left,
                Face.Top => Face.Bottom,
                Face.Bottom => Face.Top,
                _ => throw new InvalidOperationException()
            };
        }

        public static Face[] Adjacent(this Face face)
        {
            return Enum.GetValues(typeof(Face))
                .Cast<Face>()
                .Where(i => i != face && i != face.Opposite())
                .ToArray();
        }
    }

    public enum Face
    {
        Front,
        Right,
        Back,
        Left,
        Top,
        Bottom
    }

    public enum Colour
    {
        Blue,
        Green,
        White,
        Yellow,
        Orange,
        Red
    }
}