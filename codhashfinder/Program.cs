using System;
using System.IO;
using System.Net;
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

                21 - Anim Packages
            
            3 - Images

                31 - Textures from Materials

            4 - Materials

                41 - MW2/3 Materials (IW9/JUP)

                42 - BO6 Materials (T10)

                43 - BO6 Model Materials (T10)

                44 - BO6 Blueprint Materials (T10)

            5 - Models

            6 - Sounds

                61 - Unhash Sounds

                62 - Sound Guesser

            7 - Soundbanks

                71 - Unhash Soundbanks

                72 - Soundbank Name Guesser

            8 - Old Hashes

                81 - Old Animations

                82 - Old Images

                83 - Old Materials

                84 - Old Models

                85 - Old Sounds

                86 - Old Soundbanks

            9 - Miscellanious

                91 - Asset Logs

                98 - Hasher

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
                            AnimPackages();

                            break;
                        }
                        case "31":
                        {
                            MaterialsToImages();

                            break;
                        }
                        case "41":
                        {
                            JUPMaterials();
                            
                            break;
                        }
                        case "42":
                        {
                            T10Materials();
                            
                            break;
                        }
                        case "43":
                        {
                            T10ModelMaterials();
                            
                            break;
                        }
                        case "44":
                        {
                            T10BPMaterials();
                            
                            break;
                        }
                        case "61":
                        {
                            SoundUnhashing();

                            break;
                        }
                        case "62":
                        {
                            SoundGuesser();

                            break;
                        }
                        case "71":
                        {
                            Soundbanks();

                            break;
                        }
                        case "72":
                        {
                            SoundbankGuesser();

                            break;
                        }
                        case "81":
                        {
                            OldAnims();

                            break;
                        }
                        case "82":
                        {
                            OldImages();

                            break;
                        }
                        case "83":
                        {
                            OldMaterials();

                            break;
                        }
                        case "84":
                        {
                            OldModels();

                            break;
                        }
                        case "85":
                        {
                            OldSounds();

                            break;
                        }
                        case "86":
                        {
                            OldSoundbanks();

                            break;
                        }
                        case "91":
                        {
                            AssetLogs();

                            break;
                        }
                        case "98":
                        {
                            Hasher();

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

    static string PickGame()
    {
        for(;;)
        {
            Console.WriteLine("JUP or T10?\n");

            string? userResponse = Console.ReadLine();

            switch(userResponse)
            {
                case "t10":
                case "T10":
                {
                    return "T10";
                }
                case "jup":
                case "Jup":
                case "JUP":
                {
                    return "JUP";
                }
            }
        }
    }

    static void JUPAnims()
    {
        Console.WriteLine("Generating JUP Anim Hashes:\n");

        string[] JUPVMTypes = File.ReadAllLines(@"Files\Anims\VMTypes.txt");
        string[] JUPAnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPAnimAssetLog.txt");
        string[] SharedWeaponNamesAnims = File.ReadAllLines(@"Files\Shared\WeaponNamesAnims.txt");

        if(File.Exists(@"Files\GeneratedHashesAnims.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt", true);

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

        if(File.Exists(@"Files\GeneratedHashesAnims.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt", true);

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

    static void AnimPackages()
    {
        string game = PickGame();
        game = game.ToLower();

        string game1 = "bo6";
        if(game == "JUP")
        {
            game1 = "mw6";
        }

        Console.WriteLine("Unhashing Animpackage Names:\n");

        string[] Weapons = File.ReadAllLines(@"Files\Shared\WeaponNames.txt");
        string[] AnimPackages = Directory.GetFiles(@"Files\Anim Packages\anim_pkgs_" + game1);
        string[] AnimPackageVariantNames = File.ReadAllLines(@"Files\Anim Packages\AnimPackageVariantNames.txt");

        Parallel.ForEach(AnimPackages, animpackage =>
        {
            if(animpackage.Contains(game1 + "\\soundbank_"))
            {
                foreach(string weapon in Weapons)
                {
                    Parallel.ForEach(AnimPackageVariantNames, animpackageSuffix =>
                    {
                        string animpackageHashed =  animpackage.Substring(animpackage.LastIndexOf('\\') + 1);
                        animpackageHashed = animpackageHashed.Replace("soundbank_","");
                        animpackageHashed = animpackageHashed.Replace(".csv","");
                        string stringedName = "weapon_" + game + "_" + weapon + animpackageSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + animpackageHashed);
                        }

                        if(CalcHash(stringedName) == animpackageHashed)
                        {
                            if(File.Exists(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\animpkg_" + animpackageHashed + ".csv"))
                            {
                                Console.WriteLine(animpackageHashed + " | " + stringedName);
                                File.Move(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\animpkg_" + animpackageHashed + ".csv", @"Files\Anim Packages\anim_pkgs_" + game1 + "\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            }
        });
    }

    static void MaterialsToImages()
    {
        string game = PickGame();

        Console.WriteLine("Generating Image names from Materials:\n");

        string[] ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "ImageAssetLog.txt");
        string[] MaterialNames = File.ReadAllLines(@"Files\Images\MaterialNames.txt");
        string[] TextureTypes = File.ReadAllLines(@"Files\Images\TextureNames.txt");

        using StreamWriter GeneratedHashesImages = new StreamWriter(@"Files\GeneratedHashesImages.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialNames, materialName =>
        {
            Parallel.ForEach(TextureTypes, textureType =>
            {
                string stringedName = materialName + "_" + textureType;
                string generatedHash = CalcHash(stringedName);

                if(Globals.DebugToggle)
                {
                    Console.WriteLine(stringedName);
                }

                Parallel.ForEach(ImageAssetLog, hash =>
                {
                    if(generatedHash == hash)
                    {
                        string fullHash = generatedHash + "," + stringedName;
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                        Console.WriteLine(fullHash);
                    }
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesImages.WriteLine(foundHash);
        }
    }

    static void JUPMaterials()
    {
        Console.WriteLine("Generating JUP Material Names:\n");

        string[] JUPMaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPMaterialAssetLog.txt");
        string[] MaterialKeywords = File.ReadAllLines(@"Files\Materials\Keywords.txt");
        string[] JUPNumbers = File.ReadAllLines(@"Files\Materials\JUPNumbers.txt");
        string[] JUPWeaponNames = File.ReadAllLines(@"Files\Shared\JupWeaponNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(JUPNumbers, JUPNumber =>
        {
            Parallel.ForEach(MaterialKeywords, Keyword =>
            {
                Parallel.ForEach(JUPWeaponNames, JUPWeaponName =>
                {
                    string stringedName = "m/mtl_jup_" + JUPWeaponName + "_" + Keyword + JUPNumber;
                    string generatedHash = CalcHash(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(JUPMaterialAssetLog, hashedAsset => 
                    {
                        if(generatedHash == hashedAsset)
                        {
                            string fullHash = generatedHash + "," + stringedName;
                            foundHashes = foundHashes.Append(fullHash).ToArray();
                            Console.WriteLine(fullHash);
                        }
                    });
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesMaterials.WriteLine(foundHash);
        }
    }

    static void T10Materials()
    {
        Console.WriteLine("Generating T10 Material Names:\n");
        
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] MaterialKeywords = File.ReadAllLines(@"Files\Materials\Keywords.txt");
        string[] T10WeaponNames = File.ReadAllLines(@"Files\Shared\T10WeaponNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialKeywords, Keyword =>
        {
            Parallel.ForEach(T10WeaponNames, T10WeaponName =>
            {
                string stringedName = "m/mtl_t10_" + T10WeaponName + "_" + Keyword;
                string generatedHash = CalcHash(stringedName);

                if(Globals.DebugToggle)
                {
                    Console.WriteLine(stringedName);
                }

                Parallel.ForEach(T10MaterialAssetLog, hashedAsset => 
                {
                    if(generatedHash == hashedAsset)
                    {
                        string fullHash = generatedHash + "," + stringedName;
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                        Console.WriteLine(fullHash);
                    }
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesMaterials.WriteLine(foundHash);
        }
    }

    static void T10ModelMaterials()
    {
        Console.WriteLine("Generating T10 Material Names From Models:\n");
        
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] T10ModelNames = File.ReadAllLines(@"Files\Materials\T10WeaponModelNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(T10ModelNames, T10ModelName =>
        {
            string stringedName = "m/mtl_" + T10ModelName;
            string generatedHash = CalcHash(stringedName);

            if(Globals.DebugToggle)
            {
                Console.WriteLine(stringedName);
            }

            Parallel.ForEach(T10MaterialAssetLog, hashedAsset => 
            {
                if(generatedHash == hashedAsset)
                {
                    string fullHash = generatedHash + "," + stringedName;
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                    Console.WriteLine(fullHash);
                }
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesMaterials.WriteLine(foundHash);
        }
    }

    static void T10BPMaterials()
    {
        string game = PickGame();

        Console.WriteLine("Generating Blueprint Material Names:\n");

        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] T10BPNames = File.ReadAllLines(@"Files\Materials\T10BlueprintNames.txt");
        string[] MaterialNames = File.ReadAllLines(@"Files\Images\MaterialNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialNames, MaterialName =>
        {
            Parallel.ForEach(T10BPNames, BPName =>
            {
                string stringedName = "m/" + MaterialName + BPName;
                string generatedHash = CalcHash(stringedName);

                if(Globals.DebugToggle)
                {
                    Console.WriteLine(stringedName);
                }

                Parallel.ForEach(T10MaterialAssetLog, hashedAsset => 
                {
                    if(generatedHash == hashedAsset)
                    {
                        string fullHash = generatedHash + "," + stringedName;
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                        Console.WriteLine(fullHash);
                    }
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesMaterials.WriteLine(foundHash);
        }
    }

    static void SoundUnhashing()
    {
    }

    static void SoundGuesser()
    {
        string game = PickGame();

        Console.WriteLine("Enter a sound path up to the suffix to guess a sound name, type 'Quit' to exit:\n");

        if(File.Exists(@"Files\GeneratedHashesSounds.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesSounds.txt");
            file.Close();
        }

        for(;;)
        {
            string[] SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "SoundAssetLog.txt");
            string[] SoundSuffixes = File.ReadAllLines(@"Files\Sounds\SoundSuffixes.txt");

            string? userResponse = Console.ReadLine();

            using StreamWriter GeneratedHashesSounds = new StreamWriter(@"Files\GeneratedHashesSounds.txt", true);

            if(userResponse != null && userResponse.ToLower() == "quit")
            {
                break;
            }
            else if(userResponse != null)
            {
                Parallel.ForEach(SoundSuffixes, suffix =>
                {
                    string stringedName = userResponse + suffix;
                    string generatedHash = CalcHash(stringedName);

                    Parallel.ForEach(SoundAssetLog, hashedSound =>
                    {
                        if(hashedSound == generatedHash)
                        {
                            string swappedSybmols = userResponse.Replace('.','\\');
                            string fullHash = generatedHash + "," + swappedSybmols + suffix;
                            GeneratedHashesSounds.WriteLine(fullHash);
                            Console.WriteLine(fullHash);
                        }
                    });
                });
            }

            GeneratedHashesSounds.Close();
        }
    }

    static void Soundbanks()
    {
        string game = PickGame();
        game = game.ToLower();

        string game1 = "bo6";
        if(game == "JUP")
        {
            game1 = "mw6";
        }

        Console.WriteLine("Unhashing Soundbank Names:\n");

        string[] NoPlatformWeapons = File.ReadAllLines(@"Files\Shared\NoPlatformWeaponNames.txt");
        string[] Soundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_" + game1);
        string[] SoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_" + game1);
        string[] SoundbankSuffixes = File.ReadAllLines(@"Files\Sound Banks\SoundbankSuffixes.txt");

        Parallel.ForEach(Soundbanks, soundbank =>
        {
            if(soundbank.Contains(game1 + "\\soundbank_"))
            {
                foreach(string weapon in NoPlatformWeapons)
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankHashed =  soundbank.Substring(soundbank.LastIndexOf('\\') + 1);
                        soundbankHashed = soundbankHashed.Replace("soundbank_","");
                        soundbankHashed = soundbankHashed.Replace(".csv","");
                        string stringedName = "weapon_" + game + "_" + weapon + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankHashed);
                        }

                        if(CalcHash(stringedName) == soundbankHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbank_" + game1 + "\\soundbank_" + soundbankHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbank_" + game1 + "\\soundbank_" + soundbankHashed + ".csv", @"Files\Sound Banks\soundbank_" + game1 + "\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            }
        });

        Parallel.ForEach(SoundbanksTR, soundbankTR =>
        {
            if(soundbankTR.Contains(game1 + "\\soundbanktr_"))
            {
                foreach(string weapon in NoPlatformWeapons)
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankTRHashed =  soundbankTR.Substring(soundbankTR.LastIndexOf('\\') + 1);
                        soundbankTRHashed = soundbankTRHashed.Replace("soundbanktr_","");
                        soundbankTRHashed = soundbankTRHashed.Replace(".csv","");
                        string stringedName = "weapon_" + game + "_" + weapon + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankTRHashed);
                        }

                        if(CalcHash(stringedName) == soundbankTRHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbanktr_" + game1 + "\\soundbanktr_" + soundbankTRHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankTRHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbanktr_" + game1 + "\\soundbanktr_" + soundbankTRHashed + ".csv", @"Files\Sound Banks\soundbanktr_" + game1 + "\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            }
        });
    }

    static void SoundbankGuesser()
    {
        string game = PickGame();
        game = game.ToLower();

        string game1 = "bo6";
        if(game == "JUP")
        {
            game1 = "mw6";
        }

        Console.WriteLine("Enter name to guess, type 'Quit' to exit:\n");

        for(;;)
        {
            
            string? userResponse = Console.ReadLine();

            if(userResponse != null && userResponse.ToLower() == "quit")
            {
                break;
            }

            string[] Soundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_" + game1);
            string[] SoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_" + game1);
            string[] SoundbankSuffixes = File.ReadAllLines(@"Files\Sound Banks\SoundbankSuffixes.txt");

            Parallel.ForEach(Soundbanks, soundbank =>
            {
                if(soundbank.Contains(game1 + "\\soundbank_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankHashed =  soundbank.Substring(soundbank.LastIndexOf('\\') + 1);
                        soundbankHashed = soundbankHashed.Replace("soundbank_","");
                        soundbankHashed = soundbankHashed.Replace(".csv","");
                        string stringedName = userResponse + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankHashed);
                        }

                        if(CalcHash(stringedName) == soundbankHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbank_" + game1 + "\\soundbank_" + soundbankHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbank_" + game1 + "\\soundbank_" + soundbankHashed + ".csv", @"Files\Sound Banks\soundbank_" + game1 + "\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });

            Parallel.ForEach(SoundbanksTR, soundbankTR =>
            {
                if(soundbankTR.Contains(game1 + "\\soundbanktr_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankTRHashed =  soundbankTR.Substring(soundbankTR.LastIndexOf('\\') + 1);
                        soundbankTRHashed = soundbankTRHashed.Replace("soundbanktr_","");
                        soundbankTRHashed = soundbankTRHashed.Replace(".csv","");
                        string stringedName = userResponse + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankTRHashed);
                        }

                        if(CalcHash(stringedName) == soundbankTRHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbanktr_" + game1 + "\\soundbanktr_" + soundbankTRHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankTRHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbanktr_" + game1 + "\\soundbanktr_" + soundbankTRHashed + ".csv", @"Files\Sound Banks\soundbanktr_" + game1 + "\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });
        }
    }


    static void OldAnims()
    {
        Console.WriteLine("Checking old Anim Names:\n");

        if(File.Exists(@"Files\GeneratedHashesAnims.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt", true);

        string[] JupAnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPAnimAssetLog.txt");
        string[] T10AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10AnimAssetLog.txt");
        string[] OldAnims = File.ReadAllLines(@"Files\Old Hashes\OldAnims.txt");

        string[] foundHashes = [];

        foreach(string animName in OldAnims)
        {
            string generatedHash = CalcHash(animName);

            Parallel.ForEach(JupAnimAssetLog, hashedAnim =>
            {
                if(generatedHash == hashedAnim)
                {
                    string fullHash = hashedAnim + "," + animName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10AnimAssetLog, hashedAnim =>
            {
                if(generatedHash == hashedAnim)
                {
                    string fullHash = hashedAnim + "," + animName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAnims.WriteLine(foundHash);
        }
    }

    static void OldImages()
    {
        Console.WriteLine("Checking old Image names:\n");

        if(File.Exists(@"Files\GeneratedHashesImages.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesImages.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesImages = new StreamWriter(@"Files\GeneratedHashesImages.txt", true);

        string[] JupImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPImageAssetLog.txt");
        string[] T10ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10ImageAssetLog.txt");
        string[] OldImages = File.ReadAllLines(@"Files\Old Hashes\OldImages.txt");

        string[] foundHashes = [];

        foreach(string imageName in OldImages)
        {
            string generatedHash = CalcHash(imageName);

            Parallel.ForEach(JupImageAssetLog, hashedImage =>
            {
                if(generatedHash == hashedImage)
                {
                    string fullHash = hashedImage + "," + imageName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10ImageAssetLog, hashedImage =>
            {
                if(generatedHash == hashedImage)
                {
                    string fullHash = hashedImage + "," + imageName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesImages.WriteLine(foundHash);
        }
    }

    static void OldMaterials()
    {
        Console.WriteLine("Checking old Material names:\n");

        if(File.Exists(@"Files\GeneratedHashesMaterials.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesMaterials.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] JupMaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPMaterialAssetLog.txt");
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] OldMaterials = File.ReadAllLines(@"Files\Old Hashes\OldMaterials.txt");

        string[] foundHashes = [];

        foreach(string materialName in OldMaterials)
        {
            string generatedHash = CalcHash(materialName);

            Parallel.ForEach(JupMaterialAssetLog, hashedMaterial =>
            {
                if(generatedHash == hashedMaterial)
                {
                    string fullHash = hashedMaterial + "," + materialName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10MaterialAssetLog, hashedMaterial =>
            {
                if(generatedHash == hashedMaterial)
                {
                    string fullHash = hashedMaterial + "," + materialName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesMaterials.WriteLine(foundHash);
        }
    }

    static void OldModels()
    {
        Console.WriteLine("Checking old Model names:\n");

        if(File.Exists(@"Files\GeneratedHashesModels.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesModels.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesModels = new StreamWriter(@"Files\GeneratedHashesModel.txt", true);

        string[] JupModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\JUPModelAssetLog.txt");
        string[] T10ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10ModelAssetLog.txt");
        string[] OldModels = File.ReadAllLines(@"Files\Old Hashes\OldModels.txt");

        string[] foundHashes = [];

        foreach(string modelName in OldModels)
        {
            string generatedHash = CalcHash(modelName);

            Parallel.ForEach(JupModelAssetLog, hashedModel =>
            {
                if(generatedHash == hashedModel)
                {
                    string fullHash = hashedModel + "," + modelName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10ModelAssetLog, hashedModel =>
            {
                if(generatedHash == hashedModel)
                {
                    string fullHash = hashedModel + "," + modelName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesModels.WriteLine(foundHash);
        }
    }

    static void OldSounds()
    {

    }

    static void OldSoundbanks()
    {
        Console.WriteLine("Unhashing old Soundbank Names:\n");
        
        string[] JUPSoundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_mw6");
        string[] JUPSoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_mw6");
        string[] T10Soundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_bo6");
        string[] T10SoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_bo6");
        string[] OldSoundbankNames = File.ReadAllLines(@"Files\Old Hashes\OldSoundbankNames.txt");
        string[] SoundbankSuffixes = File.ReadAllLines(@"Files\Sound Banks\SoundbankSuffixes.txt");

        Parallel.ForEach(OldSoundbankNames, oldSoundbank =>
        {
            Parallel.ForEach(JUPSoundbanks, soundbank =>
            {
                if(soundbank.Contains("mw6\\soundbank_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankHashed =  soundbank.Substring(soundbank.LastIndexOf('\\') + 1);
                        soundbankHashed = soundbankHashed.Replace("soundbank_","");
                        soundbankHashed = soundbankHashed.Replace(".csv","");
                        string stringedName = oldSoundbank + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankHashed);
                        }

                        if(CalcHash(stringedName) == soundbankHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbank_mw6\\soundbank_" + soundbankHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbank_mw6\\soundbank_" + soundbankHashed + ".csv", @"Files\Sound Banks\soundbank_mw6\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });

            Parallel.ForEach(JUPSoundbanksTR, soundbankTR =>
            {
                if(soundbankTR.Contains("mw6\\soundbanktr_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankTRHashed =  soundbankTR.Substring(soundbankTR.LastIndexOf('\\') + 1);
                        soundbankTRHashed = soundbankTRHashed.Replace("soundbanktr_","");
                        soundbankTRHashed = soundbankTRHashed.Replace(".csv","");
                        string stringedName = oldSoundbank + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankTRHashed);
                        }

                        if(CalcHash(stringedName) == soundbankTRHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbanktr_mw6\\soundbanktr_" + soundbankTRHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankTRHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbanktr_mw6\\soundbanktr_" + soundbankTRHashed + ".csv", @"Files\Sound Banks\soundbanktr_mw6\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });

            Parallel.ForEach(T10Soundbanks, soundbank =>
            {
                if(soundbank.Contains("bo6\\soundbank_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankHashed =  soundbank.Substring(soundbank.LastIndexOf('\\') + 1);
                        soundbankHashed = soundbankHashed.Replace("soundbank_","");
                        soundbankHashed = soundbankHashed.Replace(".csv","");
                        string stringedName = oldSoundbank + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankHashed);
                        }

                        if(CalcHash(stringedName) == soundbankHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbank_bo6\\soundbank_" + soundbankHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbank_bo6\\soundbank_" + soundbankHashed + ".csv", @"Files\Sound Banks\soundbank_bo6\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });

            Parallel.ForEach(T10SoundbanksTR, soundbankTR =>
            {
                if(soundbankTR.Contains("bo6\\soundbanktr_"))
                {
                    Parallel.ForEach(SoundbankSuffixes, soundBankSuffix =>
                    {
                        string soundbankTRHashed =  soundbankTR.Substring(soundbankTR.LastIndexOf('\\') + 1);
                        soundbankTRHashed = soundbankTRHashed.Replace("soundbanktr_","");
                        soundbankTRHashed = soundbankTRHashed.Replace(".csv","");
                        string stringedName = oldSoundbank + soundBankSuffix;

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName + " | " + soundbankTRHashed);
                        }

                        if(CalcHash(stringedName) == soundbankTRHashed)
                        {
                            if(File.Exists(@"Files\Sound Banks\soundbanktr_bo6\\soundbanktr_" + soundbankTRHashed + ".csv"))
                            {
                                Console.WriteLine(soundbankTRHashed + " | " + stringedName);
                                File.Move(@"Files\Sound Banks\soundbanktr_bo6\\soundbanktr_" + soundbankTRHashed + ".csv", @"Files\Sound Banks\soundbanktr_bo6\\" + stringedName + ".csv");
                            }
                        }
                    });
                }
            });
        });
    }

    static void AssetLogs()
    {
        string game = PickGame();

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

    static void Hasher()
    {
        for(;;)
        {
            Console.WriteLine("Enter an asset name to hash, type 'Quit' to cancel:\n");

            string? userResponse = Console.ReadLine();

            if(userResponse != null && userResponse.ToLower() == "quit")
            {
                break;
            }
            else if(userResponse != null)
            {
                Console.WriteLine(userResponse + " | " + CalcHash(userResponse));
            }
        }
    }

    static void ToggleDebug()
    {
        Globals.DebugToggle = Globals.DebugToggle ? Globals.DebugToggle = false : Globals.DebugToggle = true;
    }
}