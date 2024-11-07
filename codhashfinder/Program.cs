using System.Net;

public static class Globals
{
    public static bool DebugToggle = false;
}

class CODHashFinder
{
    static void Main(string[] args)
    {
        for(;;)
        {
            Console.WriteLine(@"Welcome to the COD Hash Finder! Please select an operation:
        
            1 - Animations

                11 - MW2/3 Animations (IW9/JUP)

                12 - BO6 Animations (T10)

            2 - Anim Packages

                21 - MW2/3 Anim Packages (IW9/JUP)

                22 - BO6 Anim Packages (T10)
            
            3 - Images

            4 - Materials

            5 - Models

            6 - Sounds

            7 - Soundbanks

            9 - Miscellanious

                99 - Toggle Debug

            "
            );

            Console.WriteLine("Debug Activated = " + Globals.DebugToggle.ToString() + @"
            ");

            string? userResponse = Console.ReadLine();
            string[] responses = userResponse.Split(',');
            
            foreach(string operation in responses)
            {
                switch(operation)
                {
                    case "11":
                    {
                        JUPAnims();

                        break;
                    }
                    case "12":
                    {
                        T10Anims();

                        break;
                    }
                    case "21":
                    {
                        JUPAnimPackages();

                        break;
                    }
                    case "22":
                    {
                        T10AnimPackages();

                        break;
                    }
                    case "Debug":
                    case "99":
                    {
                        ToggleDebug();

                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }
    }

    static void JUPAnims()
    {

    }

    static void T10Anims()
    {

    }

    static void JUPAnimPackages()
    {

    }

    static void T10AnimPackages()
    {
        
    }

    static void ToggleDebug()
    {
        Globals.DebugToggle = Globals.DebugToggle ? Globals.DebugToggle = false : Globals.DebugToggle = true;
    }
}