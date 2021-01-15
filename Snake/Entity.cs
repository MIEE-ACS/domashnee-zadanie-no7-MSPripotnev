using System.Windows.Controls;
using System.Windows.Media;

namespace Snake
{
    public class Entity
    {
        protected int m_width;
        protected int m_height;

        Image m_image;
        public Entity(int w, int h, string image)
        {
            m_width = w;
            m_height = h;
            m_image = new Image();
            m_image.Source = (new ImageSourceConverter()).ConvertFromString(image) as ImageSource;
            m_image.Width = w;
            m_image.Height = h;

        }

        public Image image
        {
            get
            {
                return m_image;
            }
        }

        public void ChangeImage(string value)
        {
            m_image = new Image
            {
                Source = (new ImageSourceConverter()).ConvertFromString(value) as ImageSource,
                Width = m_width,
                Height = m_height
            };
        }
    }
    public class PositionedEntity : Entity
    {
        protected int m_x;
        protected int m_y;
        public PositionedEntity(int x, int y, int w, int h, string image)
            : base(w, h, image)
        {
            m_x = x;
            m_y = y;
        }

        public virtual void move() { }

        public int x
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public int y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }
    }
}
