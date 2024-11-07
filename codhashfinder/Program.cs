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

                91 - Asset Logs

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
                        case "91":
                        {
                            AssetLogs();

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

    static string CalcHashLegacy(string data)
    {
        ulong result = 0xCBF29CE484222325;

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
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt");

        var options = new ParallelOptions{MaxDegreeOfParallelism = 8};

        string[] foundHashes = [];

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

                    Parallel.ForEach(JUPAnimAssetLog, hash =>
                    {
                        if(generatedHash == hash)
                        {
                            string fullHash = generatedHash + "," + stringedName;
                            foundHashes = foundHashes.Append(fullHash).ToArray();
                            Console.WriteLine(fullHash);
                        }
                    });
                }
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAnims.WriteLine(foundHash);
        }
    }

    static void T10Anims()
    {
        Console.WriteLine("Generating T10 Anim Hashes:\n");

        string[] T10VMTypes = File.ReadAllLines(@"Files\Anims\VMTypes.txt");
        string[] T10AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10AnimAssetLog.txt");
        string[] SharedWeaponNamesAnims = File.ReadAllLines(@"Files\Shared\WeaponNamesAnims.txt");

        if(!File.Exists(@"Files\GeneratedHashesAnims.txt"))
        {
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt");

        var options = new ParallelOptions{MaxDegreeOfParallelism = 8};

        string[] foundHashes = [];

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

                    Parallel.ForEach(T10AnimAssetLog, hash =>
                    {
                        if(generatedHash == hash)
                        {
                            string fullHash = generatedHash + "," + stringedName;
                            foundHashes = foundHashes.Append(fullHash).ToArray();
                            Console.WriteLine(fullHash);
                        }
                    });
                }
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAnims.WriteLine(foundHash);
        }
    }

    static void JUPAnimPackages()
    {

    }

    static void T10AnimPackages()
    {
        
    }

    static void AssetLogs()
    {
        Console.WriteLine("Generate JUP or T10 Asset Logs?\n\n");

        string? userResponse = Console.ReadLine();
        string game = "";

        switch(userResponse)
        {
            case "t10":
            case "T10":
            {
                game = "T10";

                break;   
            }
            case "jup":
            case "Jup":
            case "JUP":
            {
                game = "JUP";

                break;
            }
            default:
            {
                return;
            }
        }

        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "AnimAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "AnimAssetLogForUnhashing.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "BoneAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "BoneAssetLogForUnhashing.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "ImageAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "ImageAssetLogForUnhashing.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "MaterialAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "MaterialAssetLogForUnhashing.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "ModelAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "ModelAssetLogForUnhashing.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "SoundAssetLog.txt",string.Empty);
        System.IO.File.WriteAllText(@"Files\Asset Logs\" + game + "SoundAssetLogForUnhashing.txt",string.Empty);

        string[] AssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "AssetLog.txt");

        using StreamWriter AnimAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "AnimAssetLog.txt");
        using StreamWriter AnimAssetLogForUnhashing = new StreamWriter(@"Files\Asset Logs\" + game + "AnimAssetLogForUnhashing.txt");

        using StreamWriter ImageAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "ImageAssetLog.txt");
        using StreamWriter ImageAssetLogForUnhashing = new StreamWriter(@"Files\Asset Logs\" + game + "ImageAssetLogForUnhashing.txt");

        using StreamWriter MaterialAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "MaterialAssetLog.txt");
        using StreamWriter MaterialAssetLogForUnhashing = new StreamWriter(@"Files\Asset Logs\" + game + "MaterialAssetLogForUnhashing.txt");

        using StreamWriter ModelAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "ModelAssetLog.txt");
        using StreamWriter ModelAssetLogForUnhashing = new StreamWriter(@"Files\Asset Logs\" + game + "ModelAssetLogForUnhashing.txt");

        using StreamWriter SoundAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "SoundAssetLog.txt");
        using StreamWriter SoundAssetLogForUnhashing = new StreamWriter(@"Files\Asset Logs\" + game + "SoundAssetLogForUnhashing.txt");


        foreach(string hash in AssetLog)
        {
            string assetType = hash.Substring(0, hash.IndexOf(','));
            string result = hash.Substring(hash.IndexOf(',') + 1);

            switch(assetType)
            {
                case "Anim":
                {
                    if(hash.Contains("xanim_"))
                    {
                        //Hashed Anims
                        result = result.Replace("xanim_","");
                        AnimAssetLog.WriteLine(result);
                    }
                    else
                    {
                        //Unhashed Anims
                        AnimAssetLogForUnhashing.WriteLine(result);
                    }

                    break;
                }
                case "Image":
                {
                    if(hash.Contains("ximage_"))
                    {
                        //Hashed Images
                        result = result.Replace("ximage_","");
                        ImageAssetLog.WriteLine(result);
                    }
                    else
                    {
                        //Unhashed Images
                        ImageAssetLogForUnhashing.WriteLine(result);
                    }

                    break;
                }
                case "Material":
                {
                    if(hash.Contains("xmaterial_"))
                    {
                        //Hashed Materials
                        result = result.Replace("xmaterial_","");
                        MaterialAssetLog.WriteLine(result);
                    }
                    else
                    {
                        //Unhashed Materials
                        MaterialAssetLogForUnhashing.WriteLine(result);
                    }

                    break;
                }
                case "Model":
                {
                    if(hash.Contains("xmodel_"))
                    {
                        //Hashed Models
                        result = result.Replace("xmodel_","");
                        ModelAssetLog.WriteLine(result);
                    }
                    else
                    {
                        //Unhashed Models
                        ModelAssetLogForUnhashing.WriteLine(result);
                    }

                    break;
                }
                case "Sound":
                {
                    if(hash.Contains("xsound_"))
                    {
                        //Hashed Sounds
                        result = result.Replace("xsound_","");
                        SoundAssetLog.WriteLine(result);
                    }
                    else
                    {
                        //Unhashed Sounds
                        SoundAssetLogForUnhashing.WriteLine(result);
                    }

                    break;
                }
            }
        }
    }

    static void ToggleDebug()
    {
        Globals.DebugToggle = Globals.DebugToggle ? Globals.DebugToggle = false : Globals.DebugToggle = true;
    }
}