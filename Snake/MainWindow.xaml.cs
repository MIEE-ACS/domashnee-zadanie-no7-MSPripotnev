using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Поле на котором живет змея
        Entity field;
        // голова змеи
        Head head;
        // вся змея
        List<PositionedEntity> snake;
        // яблоко
        Apple apple;

        PoisonedApple papple;
        //количество очков
        int score;
        //таймер по которому 
        DispatcherTimer moveTimer;
        DispatcherTimer poisonTimer;

        //конструктор формы, выполняется при запуске программы
        public MainWindow()
        {
            InitializeComponent();
            
            snake = new List<PositionedEntity>();
            //создаем поле 300х300 пикселей
            field = new Entity(600, 600, "pack://application:,,,/Resources/snake.png");

            //создаем таймер срабатывающий раз в 300 мс
            moveTimer = new DispatcherTimer();
            moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            moveTimer.Tick += new EventHandler(moveTimer_Tick);

            poisonTimer = new DispatcherTimer();
            poisonTimer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            poisonTimer.Tick += new EventHandler(poisonTimer_Tick);
            poisonTimer.Stop();
        }

        //метод перерисовывающий экран
        private void UpdateField()
        {
            //обновляем положение элементов змеи
            foreach (var p in snake)
            {
                Canvas.SetTop(p.image, p.y);
                Canvas.SetLeft(p.image, p.x);
            }

            //обновляем положение яблока
            Canvas.SetTop(apple.image, apple.y);
            Canvas.SetLeft(apple.image, apple.x);

            //обновляем положение отравленного яблока
            if (!poisonTimer.IsEnabled && papple != null)
            {
                Canvas.SetTop(papple.image, papple.y);
                Canvas.SetLeft(papple.image, papple.x);
            }

            //обновляем количество очков
            lblScore.Content = String.Format("{0}000", score);
        }
        void poisonTimer_Tick(object sender, EventArgs e)
        {
            canvas1.Children.Remove(head.image);
            head.ChangeImage("pack://application:,,,/Resources/head.png");
            head.image.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas1.Children.Add(head.image);
            poisonTimer.Stop();
            poisonTimer.IsEnabled = false;
        }
        //обработчик тика таймера. Все движение происходит здесь
        void moveTimer_Tick(object sender, EventArgs e)
        {
            //в обратном порядке двигаем все элементы змеи
            foreach (var p in Enumerable.Reverse(snake))
                p.move();

            gameover_Check();
            //проверяем, что голова змеи врезалась в яблоко
            if (head.x == apple.x && head.y == apple.y)
                apple_occuried();
            //проверяем, что голова змеи врезалась в отравленное яблоко
            else if (papple != null && head.x == papple.x && head.y == papple.y)
                poison_apple_occuried();
            //перерисовываем экран
            UpdateField();
        }
        
        void apple_occuried()
        {
            //увеличиваем счет
            score++;
            //двигаем яблоко на новое место
            apple.move();
            // добавляем новый сегмент к змее
            snake_grow();
            if (papple != null)
            {
                canvas1.Children.Remove(papple.image);
                papple = null;
            }
            else if ((new Random()).Next(0, 100) < 5 && !poisonTimer.IsEnabled)
            {
                papple = new PoisonedApple(snake, apple);
                canvas1.Children.Add(papple.image);
            }
        }
        void poison_apple_occuried()
        {
            poisonTimer.Start();
            poisonTimer.IsEnabled = true;
            score += 25;
            apple.move();
            canvas1.Children.Remove(papple.image);

            canvas1.Children.Remove(head.image);
            head.ChangeImage("pack://application:,,,/Resources/phead.png");
            head.image.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas1.Children.Add(head.image);

            snake_grow();

            papple = null;
        }
        void gameover_Check()
        {
            //проверяем, что голова змеи не врезалась в тело
            foreach (var p in snake.Where(x => x != head))
                //если координаты головы и какой либо из частей тела совпадают
                if (p.x == head.x && p.y == head.y)
                    gameover();

            //проверяем, что голова змеи не вышла за пределы поля
            if (head.x < 40 || head.x >= 540 || head.y < 40 || head.y >= 540)
                //мы проиграли
                gameover();

            void gameover()
            {
                moveTimer.Stop();
                poisonTimer.Stop();
                tbGameOver.Visibility = Visibility.Visible;
                return;
            }
        }
        void snake_grow()
        {
            var part = new BodyPart(snake.Last());
            canvas1.Children.Add(part.image);
            snake.Add(part);
        }

        // Обработчик нажатия на кнопку клавиатуры
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    head.direction = poisonTimer.IsEnabled ? Head.Direction.DOWN : Head.Direction.UP;
                    break;
                case Key.Down:
                    head.direction = poisonTimer.IsEnabled ? Head.Direction.UP : Head.Direction.DOWN;
                    break;
                case Key.Left:
                    head.direction = poisonTimer.IsEnabled ? Head.Direction.RIGHT : Head.Direction.LEFT;
                    break;
                case Key.Right:
                    head.direction = poisonTimer.IsEnabled ? Head.Direction.LEFT : Head.Direction.RIGHT;
                    break;
            }
        }

        // Обработчик нажатия кнопки "Start"
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // обнуляем счет
            score = 0;
            // обнуляем змею
            snake.Clear();
            // очищаем канвас
            canvas1.Children.Clear();
            // скрываем надпись "Game Over"
            tbGameOver.Visibility = Visibility.Hidden;
            
            // добавляем поле на канвас
            canvas1.Children.Add(field.image);
            // создаем новое яблоко и добавлем его
            apple = new Apple(snake);
            canvas1.Children.Add(apple.image);
            // создаем голову
            head = new Head();
            snake.Add(head);
            canvas1.Children.Add(head.image);
            
            //запускаем таймер
            moveTimer.Start();
            UpdateField();

        }

    }
}
