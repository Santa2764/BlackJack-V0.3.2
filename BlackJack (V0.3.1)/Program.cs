using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading;
using System.Runtime.CompilerServices;

namespace BlackJack__V0._3_
{
    public class Card
    {
        public string Value { get; }
        public string Suit { get; }

        public Card(string value, string suit)
        {
            Value = value;
            Suit = suit;
        }

        public string GetName()
        {
            return Value + " " + Suit;
        }
    }  //step 2


    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
        }

        public void CreateDeck()
        {
            string[] values = { "Туз", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Валет", "Дама", "Батя" };
            string[] suits = { "♣", "♦", "♥", "♠" };

            foreach (string suit in suits)
            {
                foreach (string value in values)
                {
                    cards.Add(new Card(value, suit));
                }
            }
        }

        public void Shuffle()
        {
            Random random = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }

        public Card Deal()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }  //step 3


    public class Player
    {
        public string Name { get; }
        public int Score { get; set; }
        public List<Card> Hand { get; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public int GetCardValue(Card card)
        {
            int value = 0;
            int aceCount = 0;

            if (card.Value == "Туз")
            {

                if (value < 11) value += 11;
                else value += 11;
                aceCount++;
            }
            else if (card.Value == "Валет" || card.Value == "Дама" || card.Value == "Батя")
            {
                value += 10;
            }
            else
            {
                value += int.Parse(card.Value);
            }

            //while (value > 21 && aceCount > 0)
            //{
            //    value -= 10;
            //    aceCount--;
            //}
            this.Score += value;

            return value;
        }

        public string HandAsString()
        {
            string result = "";
            foreach (Card card in Hand)
            {
                result += card.Value;
                result += " ";
                result += card.Suit;
                result += ", ";
            }
            return result;
        }

        public void HandAsCards()
        {
            Console.ForegroundColor = ConsoleColor.White;
            var left1 = Console.CursorLeft;
            var top1 = Console.CursorTop;
            foreach (Card card in Hand)
            {
                Console.SetCursorPosition(left1, top1);
                string space2 = "";
                string space3 = "";
                if (card.Suit == "♣") Console.BackgroundColor = ConsoleColor.Green;
                else if (card.Suit == "♦") Console.BackgroundColor = ConsoleColor.Blue;
                else if (card.Suit == "♥") Console.BackgroundColor = ConsoleColor.Red;
                else Console.BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i <= card.Value.Length; i++)
                {
                    space2 += card.Suit;
                    space3 += "-";
                }
                string[] result = new string[5] { "|-" + space3 + "-|", "|" + card.Value + "   |", "| " + space2 + " |", "|   " + card.Value + "|", "|-" + space3 + "-|" };


                left1 = Console.CursorLeft;
                top1 = Console.CursorTop;
                for (int i = 0; i < result.Length; i++)
                {
                    Console.SetCursorPosition(left1, top1 + i);
                    Console.Write(result[i]);
                }


                left1 = Console.CursorLeft + 2;
                top1 = Console.CursorTop - 4;

            }
            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
            this.GetCardValue(card);
        }
    }  //step 4


    interface IGameStrategy
    {
        bool WantCard(Player player);
    }

    class HumanStrategy : IGameStrategy
    {
        public bool WantCard(Player player)
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Хош карту? "); Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("(Y"); Console.ResetColor(); Console.Write(" / ");
            Console.ForegroundColor = ConsoleColor.Red; Console.Write("N "); Console.ResetColor(); Console.Write(") ? ");
            ConsoleKeyInfo k = Console.ReadKey(true);
            return (k.Key == ConsoleKey.Y);





            //string answer = Console.ReadLine().ToLower();
            //return (answer == "y");

        }
    }

    class Game
    {
        private Deck deck;
        private List<Player> players;
        private IGameStrategy gameStrategy;

        public Game(string playerName)
        {
            deck = new Deck();
            deck.CreateDeck();
            deck.Shuffle();
            players = new List<Player>();
            players.Add(new Player(playerName));
            players.Add(new Player("Дилер"));
            gameStrategy = new HumanStrategy();
        }

        public void ShowPlayer(Player player, int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(player.Name + " была выдана карта.");
            Console.WriteLine(player.Name + " на руках:\n");
            player.HandAsCards();



            Console.WriteLine(player.Name + " имеет " + player.Score + " очков");
            Console.ResetColor();
            Console.WriteLine();
        }

        public void Start()
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Добро пожаловать в "); Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Толератный Нигер Джек!"); Console.ResetColor();

            int dealerScore = 0, playerScore = 0;
            var leftStart = Console.CursorLeft;    //позиция курсора перед выводом инфы о игроке для "обновления" консоли
            var topStart = Console.CursorTop;
            var leftFin = Console.CursorLeft;    //позиция курсора после вывода всех игроков для вывода итога игры
            var topFin = Console.CursorTop;

            Console.WriteLine();

            foreach (Player player in players)
            {
                player.AddCard(deck.Deal());
                player.AddCard(deck.Deal());
                leftStart = Console.CursorLeft;
                topStart = Console.CursorTop;
                ShowPlayer(player, leftStart, topStart);

                while (true)
                {

                    if (player == players[players.Count - 1])
                    {
                        Console.SetCursorPosition(leftStart, topStart);
                        dealerScore = player.Score;

                        if (player.Score >= 17)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Дилеру больше карты не даём.");
                            Console.ResetColor();
                            break;
                        }
                        else
                        {
                            player.AddCard(deck.Deal());
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Дилер берёт карту.");
                            Console.ResetColor();
                            Console.WriteLine();
                        }

                        if (player.Score > 21)
                        {
                            ShowPlayer(player, leftStart, topStart);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Дилер проиграл! Он лох,ты победил");
                            Console.ResetColor();

                            return;
                        }
                        else if (player.Score == 21)
                        {
                            ShowPlayer(player, leftStart, topStart);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("У Дилера очко. Ты проиграл! Лох");
                            Console.ResetColor();

                            return;
                        }
                    }

                    else
                    {


                        playerScore = player.Score;

                        if (gameStrategy.WantCard(player))
                        {
                            player.AddCard(deck.Deal());
                        }
                        else break;

                        if (player.Score > 21)
                        {
                            ShowPlayer(player, leftStart, topStart);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(player.Name + " проиграл! Дилер-мудак!");
                            Console.ResetColor();

                            return;
                        }
                        else if (player.Score == 21)
                        {
                            ShowPlayer(player, leftStart, topStart);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Это же очко, я тебя поздравляю");
                            Console.ResetColor();

                            return;
                        }
                    }
                    ShowPlayer(player, leftStart, topStart);
                    leftFin = Console.CursorLeft;
                    topFin = Console.CursorTop;
                }
            }

            Console.SetCursorPosition(leftFin, topFin);
            if (playerScore == dealerScore)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Ничья походу");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }
            else if (playerScore > dealerScore)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Ну походу ты выиграл. Молодец");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Ты лох. Ты проиграл");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Game game = new Game("Я");
            game.Start();

        }
    }
}
