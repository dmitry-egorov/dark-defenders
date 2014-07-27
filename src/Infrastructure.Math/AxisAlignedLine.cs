namespace Infrastructure.Math
{
    public struct AxisAlignedLine
    {
        private readonly Axis   _orientation;
        private readonly Vector _origin;
        private readonly double _length;

        public AxisAlignedLine(Axis orientation, Vector origin, double length)
        {
            _orientation = orientation;
            _origin = origin;
            _length = length;
        }

        public DiscreteAxisAlignedLine DiscreteExpanded(AxisDirection expandedTo)
        {
            //Note: making a line a bit narrower to avoid rounding errors
            const double compensation = 0.0000000000001;

            var start = (MainCoordinateStart() + compensation).ToInt();
            var end   = (MainCoordinateEnd()   - compensation).ToInt();

            var length = end - start + 1;

            var level = Level().ToIntTolerantIn(expandedTo.Other());

            return new DiscreteAxisAlignedLine(_orientation, _orientation.PointAlong(start, level), length);
        }

        private double MainCoordinateEnd()
        {
            return (MainCoordinateStart() + _length);
        }

        private double MainCoordinateStart()
        {
            return _origin.CoordinateFor(_orientation);
        }

        private double Level()
        {
            return _origin.CoordinateFor(_orientation.Other());
        }

        public AxisAlignedLine MovedTo(Vector snapPosition)
        {
            return new AxisAlignedLine(_orientation, snapPosition, _length);
        }

        public AxisAlignedLine SnapToNextDiscreteLevel(AxisDirection direction, int levelSteps, double slope)
        {
            var main  = MainCoordinateStart();
            var level = Level();
            
            var snappedLevel = level.RoundTo(direction) + levelSteps * direction.GetIncrement();
            var snappedMain  = slope * (snappedLevel - level) + main;

            var snappedOrigin = Vector.ByAxis(_orientation, snappedMain, snappedLevel);

            return MovedTo(snappedOrigin);
        }

        public Vector GetOrigin()
        {
            return _origin;
        }
    }
}