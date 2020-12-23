using System;
using System.Collections.Generic;

namespace Snake
{
    public partial class MainWindow
    {
        public class Apple : PositionedEntity
        {
            List<PositionedEntity> m_snake;
            public Apple(List<PositionedEntity> s)
                : base(0, 0, 40, 40, "pack://application:,,,/Resources/fruit.png")
            {
                m_snake = s;
                move();
            }

            public override void move()
            {
                Random rand = new Random();
                do
                {
                    x = rand.Next(13) * 40 + 40;
                    y = rand.Next(13) * 40 + 40;
                    bool overlap = false;
                    foreach (var p in m_snake)
                    {
                        if (p.x == x && p.y == y)
                        {
                            overlap = true;
                            break;
                        }
                    }
                    if (!overlap)
                        break;
                } while (true);
            }
        }

        public class PoisonedApple : PositionedEntity
        {
            List<PositionedEntity> m_snake;
            Apple r_apple;
            public PoisonedApple(List<PositionedEntity> s, Apple apple)
                : base(0, 0, 40, 40, "pack://application:,,,/Resources/fruit.png")
            {
                m_snake = s;
                r_apple = apple;
                move();
            }

            public override void move()
            {
                Random rand = new Random();
                do
                {
                    x = rand.Next(13) * 40 + 40;
                    y = rand.Next(13) * 40 + 40;
                    bool overlap = false;
                    if (r_apple.x == x && r_apple.y == y)
                        overlap = true;
                    else foreach (var p in m_snake)
                        if (p.x == x && p.y == y)
                        {
                            overlap = true;
                            break;
                        }
                    if (!overlap)
                        break;
                } while (true);

            }
        }
    }
}
