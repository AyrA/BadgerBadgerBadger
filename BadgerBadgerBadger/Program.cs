using BadgerBadgerBadger.Properties;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace BadgerBadgerBadger
{
    class Program
    {
        private static IntPtr LastBadgerWindow = IntPtr.Zero;
        private static Process BadgerPad;
        private static Thread BadgerThread = null;

        static void Main(string[] args)
        {
            Console.Error.WriteLine("It's all Badgers from here on. Maybe");
            var BadgerFile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(BadgerFile, Resources.Badger);
            BadgerPlayerInterface.ExtractLib();
            using (var VI = new BadgerPlayerInterface(BadgerFile))
            {
                VI.OnBadgerEnded += delegate
                {
                    BadgerThread = null;
                };

                using (BadgerPad = Process.Start(new ProcessStartInfo("notepad.exe", "") { WindowStyle = ProcessWindowStyle.Maximized }))
                {
                    BadgerPad.EnableRaisingEvents = true;
                    BadgerPad.Exited += delegate
                    {
                        //No need to continue if the user exits early
                        Environment.Exit(1);
                    };
                    BadgerPad.WaitForInputIdle(2000);
                    var EditHandle = NativeMethods.GetAllChildren(BadgerPad.MainWindowHandle, true).FirstOrDefault(m => NativeMethods.GetHandleClassName(m).ToLower() == "edit");
                    if (EditHandle != IntPtr.Zero)
                    {
                        Badger(EditHandle, VI);
                    }
                    while (BadgerThread != null) { Thread.Sleep(50); }
                    try
                    {
                        System.IO.File.Delete(BadgerFile);
                    }
                    catch
                    {
                    }
                }
            }
            Environment.Exit(0);
        }

        private static void EraseBadgerText(IntPtr BadgerEditHandle, string BadgerText, int BadgerTimeout = 20)
        {
            do
            {
                BadgerText = BadgerText.Substring(0, BadgerText.Length - 1);
                NativeMethods.SetControlText(BadgerEditHandle, BadgerText);
                Thread.Sleep(BadgerTimeout);
            } while (!string.IsNullOrEmpty(BadgerText));
        }

        private static void Badger(IntPtr BadgerHandle, BadgerPlayerInterface BadgerPlayer)
        {
            if (BadgerThread == null)
            {
                BadgerThread = new Thread(delegate ()
                {
                    var BadgerEditor = NativeMethods.GetControlOwner(BadgerHandle);
                    NativeMethods.SetControlText(BadgerEditor, "Badger Badger Badger - /u/AyrA_ch");

                    var BadgerGrid = new StringGrid(80, 25);
                    var BadgerRandom = new Random();

                    #region strings

                    var BadgerMsg = new string[] {
                        "╔════════╗",
                        "║ Badger ║",
                        "╚════════╝"
                    };
                    var BadgerASCII = new string[] {
                        @"               ___,,___",
                        @"          _,-='=- =-  -`'--.__,,.._",
                        @"       ,-;// /  - -       -   -= - '=.",
                        @"     ,'///    -     -   -   =  - ==-=\`.",
                        @"    |/// /  =    `. - =   == - =.=_,,._ `=/|",
                        @"   ///    -   -    \  - - = ,ndDMHHMM/\b  \\",
                        @" ,' - / /        / /\ =  - /MM(,,._`YQMML  `|",
                        @"<_,=^Kkm / / / / ///H|wnWWdMKKK#''-;. `'0\  |",
                        @"       `''QkmmmmmnWMMM\''WHMKKMM\   `--. \> \",
                        @"             `'''  `->>>    ``WHMb,.    `-_<@)",
                        @"                               `'QMM`.",
                        @"                                  `>>>"
                    };

                    var BadgerFont = new string[] {
                        @"#_###############_",
                        @"| |#############| |",
                        @"| |__###__#_##__| |#__#_##___#_#__",
                        @"| '_ \#/ _` |/ _` |/ _` |/ _ \ '__|",
                        @"| |_) | (_| | (_| | (_| |  __/ |",
                        @"|_.__/#\__,_|\__,_|\__, |\___|_|",
                        @"####################__/ |",
                        @"###################|___/"
                    };

                    var MushroomFont1 = new string[] {
                        @"##*   *##   ##*   *##     *#####*    ##*   *##",
                        @"###   ###   ##*   *##    *#######*   ##*   *##",
                        @"###* *###   ##*   *##    ###***###   ##*   *##",
                        @"####*####   ##*   *##    ##*   *##   ##*   *##",
                        @"#########   ##*   *##    ##*   *##   ##*   *##",
                        @"#########   ##*   *##    ###*        ##*   *##",
                        @"##*###*##   ##*   *##    *######*    #########",
                        @"##*###*##   ##*   *##     *######*   #########",
                        @"##*   *##   ##*   *##      ****###   ###***###",
                        @"##*   *##   ##*   *##    ##*   *##   ##*   *##",
                        @"##*   *##   ##*   *##    ##*   *##   ##*   *##",
                        @"##*   *##   ###***###    ###***###   ##*   *##",
                        @"##*   *##   *#######*    *#######*   ##*   *##",
                        @"##*   *##    *#####*      *#####*    ##*   *##"
                    };

                    var MushroomFont2 = new string[]
                    {
                        @"#######*     *#####*     *#####*    ##*   *##",
                        @"########*   *#######*   *#######*   ###   ###",
                        @"###***###   ###***###   ###***###   ###* *###",
                        @"##*   *##   ##*   *##   ##*   *##   ####*####",
                        @"##*   *##   ##*   *##   ##*   *##   #########",
                        @"##*  *###   ##*   *##   ##*   *##   #########",
                        @"########*   ##*   *##   ##*   *##   ##*###*##",
                        @"########*   ##*   *##   ##*   *##   ##*###*##",
                        @"###***###   ##*   *##   ##*   *##   ##*   *##",
                        @"##*   *##   ##*   *##   ##*   *##   ##*   *##",
                        @"##*   *##   ##*   *##   ##*   *##   ##*   *##",
                        @"##*   *##   ###***###   ###***###   ##*   *##",
                        @"##*   *##   *#######*   *#######*   ##*   *##",
                        @"##*   *##    *#####*     *#####*    ##*   *##"
                    };

                    var MushroomASCII1 = new string[] {
                        @"         ___..._",
                        @"    _,--'       '`-.",
                        @"  ,'.  .            \",
                        @",/:. .     .       .'",
                        @"|;..  .      _..--'",
                        @"`--:...-,-'''\",
                        @"        |:.  `.",
                        @"        l;.   l",
                        @"        `|:.   |",
                        @"         |:.   `.,",
                        @"        .l;.    j, ,",
                        @"     `. \`;:.   //,/",
                        @"      .\\)`;,|\'/(",
                        @"       ` `    `(,"
                    };

                    var MushroomASCII2 = new string[] {
                        @"       ------------",
                        @"     /  (_)_   _    \",
                        @"  /)      (_) (_)      \",
                        @" |       _          _   |",
                        @"| _     (_)   _    (_) _ |",
                        @"|(_)  _      (_)  _   (_)|",
                        @"|____(_)_________(_)_____|",
                        @" \\\\\\\\\||||||||///////",
                        @"          |      |",
                        @"          |      |",
                        @"          |      |",
                        @"           \____/"
                    };

                    var BigA = new string[] {
                        @"########################################_####_#_####_#_####_",
                        @"####/\########/\########/\########/\###| |##| | |##| | |##| |",
                        @"###/  \######/  \######/  \######/  \##| |__| | |__| | |__| |",
                        @"##/ /\ \####/ /\ \####/ /\ \####/ /\ \#|  __  |  __  |  __  |",
                        @"#/ ____ \##/ ____ \##/ ____ \##/ ____ \| |##| | |##| | |##| |",
                        @"/_/####\_\/_/####\_\/_/####\_\/_/####\_\_|##|_|_|##|_|_|##|_|"
                    };

                    var SnakeASCII = new string[] {
                    @"           /^\/^\",
                    @"         _|__|  O|",
                    @"\/     /~     \_/ \",
                    @" \____|__________/  \",
                    @"        \_______      \",
                    @"                `\     \                 \",
                    @"                  |     |                  \",
                    @"                 /      /                    \",
                    @"                /     /                       \\",
                    @"              /      /                         \ \",
                    @"             /     /                            \  \",
                    @"           /     /             _----_            \   \",
                    @"          /     /           _-~      ~-_         |   |",
                    @"         (      (        _-~    _--_    ~-_     _/   |",
                    @"          \      ~-____-~    _-~    ~-_    ~-_-~    /",
                    @"            ~-_           _-~          ~-_       _-~",
                    @"               ~--______-~                ~-___-~"
                    };

                    #endregion

#if !DEBUG
                    const string Clippy = @" ___________________________________
< It looks like you try to be funny >
<     Let me help you with that     >
 -----------------------------------
 \
  \
     __
    /  \
    |  |
    @  @
    |  |
    || |/
    || ||
    |\_/|
    \___/
";
                    NativeMethods.SetControlText(BadgerHandle, Clippy);
                    Thread.Sleep(5000);
                    EraseBadgerText(BadgerHandle, Clippy);
#endif
                    BadgerPlayer.BadgerPlay();
                    Thread.Sleep(500);

                    //Badger
                    for (var BadgerCounter = 0; BadgerCounter < 12; BadgerCounter++)
                    {
                        NativeMethods.SetControlText(BadgerHandle, string.Join(" ", Enumerable.Repeat("Badger", BadgerCounter + 1)));
                        //4.5 second sequence for 12 badgers. Too lazy to calculate.
                        Thread.Sleep((int)(4500.0 / 12.0));
                    }

                    //Mushrooms
                    NativeMethods.SetControlText(BadgerHandle, "Mushroom");
                    Thread.Sleep(700);
                    NativeMethods.SetControlText(BadgerHandle, "MUSHROOM MUSHROOM");
                    Thread.Sleep(1100);

                    //Badger boxes
                    for (var BadgerCounter = 0; BadgerCounter < 11; BadgerCounter++)
                    {
                        BadgerGrid.Set(BadgerMsg, BadgerRandom.Next(0, 70), BadgerRandom.Next(0, 22), false);
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep((int)(4100.0 / 11.0));
                    }

                    //Mushrooms
                    for (var BadgerCounter = 0; BadgerCounter < 2; BadgerCounter++)
                    {
                        BadgerGrid.Reset();
                        BadgerGrid.Set(MushroomFont1, 0, 0, false);
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep(350);
                        BadgerGrid.Reset();
                        BadgerGrid.Set(MushroomFont2, 0, 0, false);
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep(400);
                    }
                    Thread.Sleep(400);


                    //Actual badgers
                    for (var BadgerCounter = 0; BadgerCounter < 11; BadgerCounter++)
                    {
                        BadgerGrid.Reset();
                        BadgerGrid.Set(BadgerASCII, BadgerRandom.Next(0, 80 - 46), BadgerRandom.Next(0, 24 - BadgerASCII.Length), false);
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep((int)(4100.0 / 11.0));
                    }

                    //Mushroom ASCII
                    //This one comes with a delay
                    BadgerGrid.Reset();
                    Thread.Sleep(200);
                    BadgerGrid.Set(MushroomASCII1, BadgerRandom.Next(0, 40 - 24), BadgerRandom.Next(0, 24 - MushroomASCII1.Length), false);
                    NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                    Thread.Sleep(500);
                    BadgerGrid.Set(MushroomASCII2, BadgerRandom.Next(40, 80 - 26), BadgerRandom.Next(0, 24 - MushroomASCII2.Length), false);
                    NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                    Thread.Sleep(1200);

                    //Stacked badger title
                    BadgerGrid.Reset();
                    for (var BadgerCounter = 0; BadgerCounter < 11; BadgerCounter++)
                    {
                        BadgerGrid.Set(BadgerFont, BadgerCounter * 4, BadgerCounter * 4, true, '#');
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep((int)(4100.0 / 11.0));
                    }

                    //AAAAAAA
                    BadgerGrid.Reset();
                    for (var BadgerCounter = 0; BadgerCounter < 9; BadgerCounter++)
                    {
                        BadgerGrid.Set(BigA, BadgerCounter * 2, BadgerCounter * 2, true, '#');
                        NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                        Thread.Sleep(50);
                    }

                    BadgerGrid.Reset();
                    BadgerGrid.Set(SnakeASCII, 0, 0, false);
                    NativeMethods.SetControlText(BadgerHandle, BadgerGrid.FullGrid);
                    Thread.Sleep(1000);

                    BadgerSnakeGame(BadgerHandle);

                    BadgerPad.EnableRaisingEvents = false;

                    BadgerPlayer.BadgerStop();

                    NativeMethods.SetControlText(BadgerHandle, string.Join("\r\n", new string[]{
                        "Code:   /u/AyrA_ch",
                        "Music:  Badgers : animated music video : MrWeebl",
                        "Link:   https://www.youtube.com/watch?v=EIyixC9NsLI",
                        "Canvas: notepad.exe"
                    }));

                    BadgerThread = null;
                });
                BadgerThread.IsBackground = true;
                BadgerThread.Start();
            }
        }

        private static void BadgerSnakeGame(IntPtr BadgerEditorHandle)
        {
            var BadgerGrid = new StringGrid(80, 25);
            var BadgerSnakeGameDosStyleAsciiDrawingBorder = new string[] {
                "╔═╗",
                "║ ║",
                "╚═╝"
            };
            BadgerSnakeGameDosStyleAsciiDrawingBorder[0] = BadgerSnakeGameDosStyleAsciiDrawingBorder[0][0] + string.Empty.PadRight(78, BadgerSnakeGameDosStyleAsciiDrawingBorder[0][1]) + BadgerSnakeGameDosStyleAsciiDrawingBorder[0][2];
            BadgerSnakeGameDosStyleAsciiDrawingBorder[1] = BadgerSnakeGameDosStyleAsciiDrawingBorder[1][0] + string.Empty.PadRight(78, BadgerSnakeGameDosStyleAsciiDrawingBorder[1][1]) + BadgerSnakeGameDosStyleAsciiDrawingBorder[1][2];
            BadgerSnakeGameDosStyleAsciiDrawingBorder[2] = BadgerSnakeGameDosStyleAsciiDrawingBorder[2][0] + string.Empty.PadRight(78, BadgerSnakeGameDosStyleAsciiDrawingBorder[2][1]) + BadgerSnakeGameDosStyleAsciiDrawingBorder[2][2];

            BadgerGrid.Set(BadgerSnakeGameDosStyleAsciiDrawingBorder[0], 0, 0, false);
            for (var BadgerSnakeGameDosStyleAsciiDrawingBorderCounter = 0; BadgerSnakeGameDosStyleAsciiDrawingBorderCounter < 23; BadgerSnakeGameDosStyleAsciiDrawingBorderCounter++)
            {
                BadgerGrid.Set(BadgerSnakeGameDosStyleAsciiDrawingBorder[1], 0, BadgerSnakeGameDosStyleAsciiDrawingBorderCounter + 1, false);
            }
            BadgerGrid.Set(BadgerSnakeGameDosStyleAsciiDrawingBorder[2], 0, 24, false);

            //Food item
            BadgerGrid.Set("#", 72, 5, false);

            for (var BadgerSnakeMoveCounter = 0; BadgerSnakeMoveCounter < 52; BadgerSnakeMoveCounter++)
            {
                BadgerGrid.Set(" ─══════", 15 + BadgerSnakeMoveCounter, 5, false);
                NativeMethods.SetControlText(BadgerEditorHandle, BadgerGrid.FullGrid);
                Thread.Sleep(95);
            }
            //New food
            BadgerGrid.Set("#", 42, 18, false);
            //Simulate a turn
            BadgerGrid.Set(" ─═════╗", 67, 5, false);

            //Pause Menu
            BadgerGrid.Set(new string[] {
                "╔═══════╗",
                "║ PAUSE ║",
                "╚═══════╝"
            }, 35, 12, false);
            NativeMethods.SetControlText(BadgerEditorHandle, BadgerGrid.FullGrid);
            Thread.Sleep(500);
        }
    }
}
