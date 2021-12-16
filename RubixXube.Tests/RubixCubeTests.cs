using Core;
using FluentAssertions;
using FsCheck.Xunit;
using System.Linq;
using Xunit;

namespace RubixXube.Tests
{
    public class RubixCubeTests
    {
        public class A_solved_rubix_cube
        {
            private RubixCube Solved { get; } = RubixCube.Solved;

            [Fact]
            public void Has_a_front_face_that_is_entirely_blue()
            {
                Solved[Face.Front].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.Blue, Colour.Blue, Colour.Blue},
                        { Colour.Blue, Colour.Blue, Colour.Blue },
                        { Colour.Blue, Colour.Blue, Colour.Blue }
                    });
            }

            [Fact]
            public void Has_a_right_face_that_is_entirely_green()
            {
                Solved[Face.Right].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.Green, Colour.Green, Colour.Green},
                        { Colour.Green, Colour.Green, Colour.Green },
                        { Colour.Green, Colour.Green, Colour.Green }
                    });
            }

            [Fact]
            public void Has_a_back_face_that_is_entirely_white()
            {
                Solved[Face.Back].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.White, Colour.White, Colour.White},
                        { Colour.White, Colour.White, Colour.White },
                        { Colour.White, Colour.White, Colour.White }
                    });
            }

            [Fact]
            public void Has_a_left_face_that_is_entirely_yellow()
            {
                Solved[Face.Left].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow},
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow },
                        { Colour.Yellow, Colour.Yellow, Colour.Yellow }
                    });
            }

            [Fact]
            public void Has_a_top_face_that_is_entirely_orange()
            {
                Solved[Face.Top].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.Orange, Colour.Orange, Colour.Orange},
                        { Colour.Orange, Colour.Orange, Colour.Orange },
                        { Colour.Orange, Colour.Orange, Colour.Orange }
                    });
            }

            [Fact]
            public void Has_a_bottom_face_that_is_entirely_red()
            {
                Solved[Face.Bottom].Should().BeEquivalentTo(
                    new Colour[3, 3]
                    {
                        { Colour.Red, Colour.Red, Colour.Red},
                        { Colour.Red, Colour.Red, Colour.Red },
                        { Colour.Red, Colour.Red, Colour.Red }
                    });
            }
        }

        public class A_scrambled_cube
        {
            private RubixCube Scrambled { get; } = RubixCube.Scrambled();

            [Fact]
            public void Has_3_columns_and_3_rows_on_each_face()
            {
                Scrambled[Face.Front].GetLength(0).Should().Be(3);
                Scrambled[Face.Front].GetLength(1).Should().Be(3);

                Scrambled[Face.Right].GetLength(0).Should().Be(3);
                Scrambled[Face.Right].GetLength(1).Should().Be(3);

                Scrambled[Face.Back].GetLength(0).Should().Be(3);
                Scrambled[Face.Back].GetLength(1).Should().Be(3);

                Scrambled[Face.Left].GetLength(0).Should().Be(3);
                Scrambled[Face.Left].GetLength(1).Should().Be(3);

                Scrambled[Face.Top].GetLength(0).Should().Be(3);
                Scrambled[Face.Top].GetLength(1).Should().Be(3);

                Scrambled[Face.Bottom].GetLength(0).Should().Be(3);
                Scrambled[Face.Bottom].GetLength(1).Should().Be(3);
            }

            [Fact]
            public void Has_9_squares_of_each_colour()
            {
                var all = new[]
                {
                    Scrambled[Face.Front],
                    Scrambled[Face.Right],
                    Scrambled[Face.Back],
                    Scrambled[Face.Left],
                    Scrambled[Face.Top],
                    Scrambled[Face.Bottom]
                }
                .SelectMany(x => Flatten(x));

                all.Where(c => c == Colour.Green).Should().HaveCount(9);
                all.Where(c => c == Colour.Blue).Should().HaveCount(9);
                all.Where(c => c == Colour.White).Should().HaveCount(9);
                all.Where(c => c == Colour.Yellow).Should().HaveCount(9);
                all.Where(c => c == Colour.Orange).Should().HaveCount(9);
                all.Where(c => c == Colour.Red).Should().HaveCount(9);
            }
        }

        [Property]
        public void Rotating_1_time_changes_the_face(Face face)
        {
            var cube = RubixCube.Scrambled();
            var rotated = cube.Rotate(face);

            rotated[face].Should().BeEquivalentTo(new Colour[3, 3]
            {
                { cube[face][0,2], cube[face][1,2], cube[face][2,2] },
                { cube[face][0,1], cube[face][1,1], cube[face][2,1] },
                { cube[face][0,0], cube[face][1,0], cube[face][2,0] },
            });
        }

        [Property]
        public void Rotating_1_time_moves_the_adjacent_blocks(Face face)
        {
            var cube = RubixCube.Scrambled();
            var rotated = cube.Rotate(face);
        }

        [Property]
        public void Rotating_1_time_does_not_change_the_opposite_face(Face face)
        {
            var cube = RubixCube.Scrambled();
            var rotated = cube.Rotate(face);
            rotated[face.Opposite()].Should().BeEquivalentTo(cube[face.Opposite()]);
        }

        [Property]
        public void Rotate_4_times_changes_nothing(Face face)
        {
            var cube = RubixCube.Scrambled();

            cube.Rotate(face).Rotate(face).Rotate(face).Rotate(face).Should().BeEquivalentTo(cube);
        }

        private static T[] Flatten<T>(T[,] array)
        {
            var result = new T[array.Length];

            var write = 0;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int z = 0; z <= array.GetUpperBound(1); z++)
                {
                    result[write++] = array[i, z];
                }
            }
            // Step 3: return the new array.
            return result;
        }
    }
}