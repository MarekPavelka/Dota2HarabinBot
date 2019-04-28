namespace Dota2HarabinBot
{
    using System.Runtime.InteropServices;
    using System;
    using System.IO;
    using System.Timers;
    using WindowsInput;
    using WindowsInput.Native;
    using Dota2GSI;
    using Newtonsoft.Json;

    class Program
    {
        private static readonly InputSimulator inputSim = new InputSimulator();
        private const string CursorLogPath = @"C:\Users\LoisLane\Desktop\cursor.log";
        private static Timer cursorLogTimer = new Timer(3000);

        static void Main(string[] args)
        {
            cursorLogTimer.Elapsed += OnTimerElapsed;
            cursorLogTimer.Start();

            var gsl = new GameStateListener(32866);
            gsl.NewGameState += OnNewState;
            var isSuccess = gsl.Start();
            if (!isSuccess) throw new InvalidOperationException("Listening for game state updates failed, most likely due to insufficient privileges");

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            LogCursorPosition();
        }

        private static void LogCursorPosition()
        {
            var cursorPos = GetCursorPosition();
            File.AppendAllText(CursorLogPath, cursorPos.ToString() + Environment.NewLine);
        }

        private static int lastActionSecond = 0;
        private static void OnNewState(GameState gameState)
        {
            if (gameState.Abilities.Count == 0) return;
            // decision logic

            var secondNow = DateTime.Now.Second;
            if (secondNow % 5 == 0)
            {
                if (lastActionSecond == secondNow) return;
                lastActionSecond = secondNow;
                //WriteText("Hi from?");
                //DoSomeClicks();
            }
        }

        private static void WriteText(string textToWrite)
        {
            inputSim.Keyboard
                .KeyPress(VirtualKeyCode.RETURN)
                .Sleep(100)
                .TextEntry(textToWrite)
                .KeyPress(VirtualKeyCode.RETURN);
        }

        private static void DoSomeClicks()
        {
            inputSim.Mouse
                .MoveMouseBy(100, 0)
                .RightButtonClick();
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return $"X: {X} Y: {Y}";
            }
        }

        #region Serialization
        private void SerializeDeserialize()
        {
            Lolo lolo = new Lolo();
            lolo.Foo = "Jozef";
            lolo.Lionel = 42;
            string serialized = JsonConvert.SerializeObject(lolo);
            Lolo deserialized = JsonConvert.DeserializeObject<Lolo>(serialized);
        }

        class Lolo
        {
            public string Foo;
            public int Lionel;
        }
        #endregion
    }
}
