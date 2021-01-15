using System.Windows;
using System.Windows.Media;

namespace Snake
{
    public class Head : PositionedEntity
    {
        public enum Direction
        {
            RIGHT, DOWN, LEFT, UP, NONE
        };

        protected Direction m_direction;

        public Direction direction
        {
            set
            {
                m_direction = value;
                RotateTransform rotateTransform = new RotateTransform(90 * (int)value);
                image.RenderTransform = rotateTransform;
            }
        }

        public Head()
            : base(280, 280, 40, 40, "pack://application:,,,/Resources/head.png")
        {
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            m_direction = Direction.NONE;
        }

        public override void move()
        {
            switch (m_direction)
            {
                case Direction.DOWN:
                    y += 40;
                    break;
                case Direction.UP:
                    y -= 40;
                    break;
                case Direction.LEFT:
                    x -= 40;
                    break;
                case Direction.RIGHT:
                    x += 40;
                    break;
            }
        }
    }
    public class BodyPart : PositionedEntity
    {
        PositionedEntity m_next;
        public BodyPart(PositionedEntity next)
            : base(next.x, next.y, 40, 40, "pack://application:,,,/Resources/body.png")
        {
            m_next = next;
        }

        public override void move()
        {
            x = m_next.x;
            y = m_next.y;
        }
    }

}
