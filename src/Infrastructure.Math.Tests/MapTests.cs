using NUnit.Framework;

namespace Infrastructure.Math.Tests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void Should_return_false_when_no_values_are_on_a_horizontal_line()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);
            
            map.Fill(1);

            var actual = map.IsAnyAtLine(Axis.X, 0, 2, 0, 0);

            Assert.IsFalse(actual);
        }

        [Test]
        public void Should_return_false_when_no_values_are_on_a_vertical_line()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);

            map.Fill(1);

            var actual = map.IsAnyAtLine(Axis.Y, 0, 2, 0, 0);

            Assert.IsFalse(actual);
        }

        [Test]
        public void Should_return_true_when_single_value_is_on_a_horizontal_line()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);

            map.Fill(1);

            map[1, 1] = 0;

            var actual = map.IsAnyAtLine(Axis.X, 0, 2, 1, 0);

            Assert.IsTrue(actual);
        }

        [Test]
        public void Should_return_true_when_single_value_is_on_a_vertical_line()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);

            map.Fill(1);
            map[1, 1] = 0;

            var actual = map.IsAnyAtLine(Axis.Y, 0, 2, 1, 0);

            Assert.IsTrue(actual);
        }

        [Test]
        public void Should_return_true_when_requesting_default_value_on_a_horizontal_line_out_of_range()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);

            map.Fill(1);

            var actual = map.IsAnyAtLine(Axis.X, 1, 3, 0, 0);

            Assert.IsTrue(actual);
        }

        [Test]
        public void Should_return_true_when_requesting_default_value_on_a_vertical_line_out_of_range()
        {
            var map = new Map<int>(new Dimensions(3, 3), 0);

            map.Fill(1);

            var actual = map.IsAnyAtLine(Axis.Y, 1, 3, 0, 0);

            Assert.IsTrue(actual);
        }
    }
}
