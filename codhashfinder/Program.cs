using System.Net;
using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections;

public static class Globals
{
    public static bool WritingHash = false;

    public static bool DebugToggle = false;
}

class CODHashFinder
{
    static void Main(string[] args)
    {
        for(;;)
        {
            Console.WriteLine(@"Welcome to the COD Hash Finder! Please select an operation.
            To enter multiple in sequence, seperate your option with a ',':
        
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

            if(userResponse != null)
            {
                string[] responses = userResponse.Split(',');

                Console.Clear();
            
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
    }

    static string CalcHash(string data)
    {
        ulong result = 0x47F5817A5EF961BA;

        for(int i = 0; i < Encoding.UTF8.GetByteCount( data ); i++)
        {
            ulong value = data[i];

            if(value == '\\')
            {
                value = '/';
            }

            result = 0x100000001B3 * (value ^ result);
        }

        return String.Format("{0:x}", result & 0xFFFFFFFFFFFFFFF);
    }

    static void JUPAnims()
    {
        Console.WriteLine("Generating JUP Anim Hashes:\n");

        string[] JUPVMTypes = File.ReadAllLines(@"Files\Anims\VMTypes.txt");
        string[] JUPAnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPAnimAssetLog.txt");
        string[] SharedWeaponNamesAnims = File.ReadAllLines(@"Files\Shared\WeaponNamesAnims.txt");

        if(!File.Exists(@"Files\GeneratedHashesAnims.txt"))
        {
            File.Create(@"Files\GeneratedHashesAnims.txt");
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt");

        var options = new ParallelOptions{MaxDegreeOfParallelism = 8};

        Parallel.ForEach(JUPVMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnims, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "jup_vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(JUPAnimAssetLog, options, async (hash, ct) =>
                    {
                        if(generatedHash == hash)
                        {
                            await GeneratedHashesAnims.WriteLineAsync(generatedHash + "," + stringedName);
                            Console.WriteLine(generatedHash + "," + stringedName);
                        }
                    });
                }
            });
        });
    }

    static void T10Anims()
    {
        Console.WriteLine("Generating T10 Anim Hashes:\n");

        string[] T10VMTypes = File.ReadAllLines(@"Files\Anims\VMTypes.txt");
        string[] T10AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10AnimAssetLog.txt");
        string[] SharedWeaponNamesAnims = File.ReadAllLines(@"Files\Shared\WeaponNamesAnims.txt");

        if(!File.Exists(@"Files\GeneratedHashesAnims.txt"))
        {
            File.Create(@"Files\GeneratedHashesAnims.txt");
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt");

        var options = new ParallelOptions{MaxDegreeOfParallelism = 8};

        Parallel.ForEach(T10VMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnims, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(T10AnimAssetLog, options, async (hash, ct) =>
                    {
                        if(generatedHash == hash)
                        {
                            await GeneratedHashesAnims.WriteLineAsync(generatedHash + "," + stringedName);
                            Console.WriteLine(generatedHash + "," + stringedName);
                        }
                    });
                }
            });
        });
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