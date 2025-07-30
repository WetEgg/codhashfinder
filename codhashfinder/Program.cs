using System.Text;
using System.Globalization;
using System.Net;

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

                11 - MW2/3 Animations (MW5/MW6)

                12 - BO6 Animations (T10)

            2 - Anim Packages

                21 - Anim Packages
            
            3 - Images

                31 - Textures from Materials

            4 - Materials

                41 - MW2/3 Materials (MW5/MW6)

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

                73 - Unhash Aliases

            8 - Old Hashes

                81 - Old Animations

                82 - Old Images

                83 - Old Materials

                84 - Old Models

                85 - Old Sounds

                86 - Old Soundbanks

                87 - Old Animpackages

            9 - Miscellanious

                91 - Asset Logs

                97 - Word Combiner

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
                            MW6Anims();

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
                        case "22":
                        {
                            AnimPackageGuesser();

                            break;
                        }
                        case "31":
                        {
                            MaterialsToImages();

                            break;
                        }
                        case "41":
                        {
                            MW6Materials();
                            
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
                        case "73":
                        {
                            AliasHashes();

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
                        case "87":
                        {
                            OldAnimpackages();

                            break;
                        }
                        case "88":
                        {
                            OldBones();

                            break;
                        }
                        case "91":
                        {
                            AssetLogs();

                            break;
                        }
                        case "97":
                        {
                            WordCombiner();

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

        return String.Format("{0:x}", result & 0x7FFFFFFFFFFFFFFF);
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

        return String.Format("{0:x}", result & 0x7FFFFFFFFFFFFFFF);
    }
    
    static string CalcHash32(string data)
    {
        uint result = 0x811C9DC5;

        for(int i = 0; i < Encoding.UTF8.GetByteCount( data ); i++)
        {
            uint value = data[i];

            if(value == '\\')
            {
                value = '/';
            }

            result = 0x01000193 * (value ^ result);
        }

        return String.Format("{0:x}", result);
    }


    static string PickGame()
    {
        for(;;)
        {
            Console.WriteLine("MW5, MW6 or T10?\n");

            string? userResponse = Console.ReadLine();

            switch(userResponse)
            {
                case "t10":
                case "T10":
                {
                    return "T10";
                }
                case "mw6":
                case "MW6":
                {
                    return "MW6";
                }
                case "mw5":
                case "MW5":
                {
                    return "MW5";
                }
            }
        }
    }

    static string PickAssetType()
    {
        for(;;)
        {
            string? assetType = Console.ReadLine();

            if(assetType != null)
            {
                assetType = assetType.ToLower();

                if(assetType == "anim" || assetType == "image" || assetType == "material" || assetType == "sound")
                {   
                    var textinfo = new CultureInfo("en-US", false).TextInfo;
                    assetType = textinfo.ToTitleCase(assetType);

                    return assetType;
                }
            }
        }
    }

    static int PickLength()
    {
        for(;;)
        {
            string? userResponse = Console.ReadLine();

            if(userResponse != null)
            {
                int length = Int32.Parse(userResponse);

                if(length > 0)
                {
                    return length;
                }
            }
        }
    }

    static void MW6Anims()
    {
        Console.WriteLine("Generating MW6 Anim Hashes:\n");

        string[] MW6VMTypes = File.ReadAllLines(@"Files\Anims\VMTypes.txt");
        string[] MW6AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6AnimAssetLog.txt");
        string[] SharedWeaponNamesAnims = File.ReadAllLines(@"Files\Shared\WeaponNamesAnims.txt");

        if(File.Exists(@"Files\GeneratedHashesAnims.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnims.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnims = new StreamWriter(@"Files\GeneratedHashesAnims.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MW6VMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnims, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "mw6_vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(MW6AnimAssetLog, hash =>
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

        Parallel.ForEach(T10VMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnims, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "t10_vm_" + weaponName + "_" + animType;
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
        if(game == "mw6")
        {
            game1 = "mw6";
        }

        Console.WriteLine("Unhashing Animpackage Names:\n");

        string[] Weapons = File.ReadAllLines(@"Files\Shared\WeaponNames.txt");
        string[] AnimPackages = Directory.GetFiles(@"Files\Anim Packages\anim_pkgs_" + game1);
        string[] AnimPackageVariantNames = File.ReadAllLines(@"Files\Anim Packages\AnimPackageVariantNames.txt");

        if(game1 == "mw6")
        {
            Parallel.ForEach(AnimPackages, animpackage =>
            {
                if (animpackage.Contains(game1 + "\\animpkg_"))
                {
                    foreach (string weapon in Weapons)
                    {
                        Parallel.ForEach(AnimPackageVariantNames, animpackageSuffix =>
                        {
                            string animpackageHashed = animpackage.Substring(animpackage.LastIndexOf('\\') + 1);
                            animpackageHashed = animpackageHashed.Replace("animpkg_", "");
                            animpackageHashed = animpackageHashed.Replace(".csv", "");
                            string stringedName = game + "_" + weapon + animpackageSuffix;

                            if (Globals.DebugToggle)
                            {
                                Console.WriteLine(stringedName + " | " + animpackageHashed);
                            }

                            if (CalcHash(stringedName) == animpackageHashed)
                            {
                                if (File.Exists(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\animpkg_" + animpackageHashed + ".csv"))
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
        else
        {
            Parallel.ForEach(AnimPackages, animpackage =>
            {
                if (animpackage.Contains(game1 + "\\"))
                {
                    foreach (string weapon in Weapons)
                    {
                        Parallel.ForEach(AnimPackageVariantNames, animpackageSuffix =>
                        {
                            string animpackageHashed = animpackage.Substring(animpackage.LastIndexOf('\\') + 1);
                            animpackageHashed = animpackageHashed.Replace(".json", "");
                            string stringedName = game + "_" + weapon + animpackageSuffix;

                            if (Globals.DebugToggle)
                            {
                                Console.WriteLine(stringedName + " | " + animpackageHashed);
                            }

                            if (CalcHash(stringedName) == animpackageHashed)
                            {
                                if (File.Exists(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\animpkg_" + animpackageHashed + ".json"))
                                {
                                    Console.WriteLine(animpackageHashed + " | " + stringedName);
                                    File.Move(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\animpkg_" + animpackageHashed + ".json", @"Files\Anim Packages\anim_pkgs_" + game1 + "\\" + stringedName + ".json");
                                }
                            }
                        });
                    }
                }
            });
        }
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

    static void MW6Materials()
    {
        Console.WriteLine("Generating MW6 Material Names:\n");

        string[] MW6MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6MaterialAssetLog.txt");
        string[] MaterialKeywords = File.ReadAllLines(@"Files\Materials\Keywords.txt");
        string[] MW6Numbers = File.ReadAllLines(@"Files\Materials\MW6Numbers.txt");
        string[] MW6WeaponNames = File.ReadAllLines(@"Files\Shared\MW6WeaponNames.txt");
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MW6Numbers, MW6Number =>
        {
            Parallel.ForEach(MaterialKeywords, Keyword =>
            {
                Parallel.ForEach(MW6WeaponNames, MW6WeaponName =>
                {
                    Parallel.ForEach(MaterialFolders, MaterialFolder =>
                    {
                        string stringedName = MaterialFolder + "mtl_mw6_" + MW6WeaponName + "_" + Keyword + MW6Number;
                        string generatedHash = CalcHash(stringedName);

                        if(Globals.DebugToggle)
                        {
                            Console.WriteLine(stringedName);
                        }

                        Parallel.ForEach(MW6MaterialAssetLog, hashedAsset => 
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
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialKeywords, Keyword =>
        {
            Parallel.ForEach(T10WeaponNames, T10WeaponName =>
            {
                Parallel.ForEach(MaterialFolders, MaterialFolder =>
                {
                    string stringedName = MaterialFolder + "mtl_wpn_t10_" + T10WeaponName + "_" + Keyword;
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
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(T10ModelNames, T10ModelName =>
        {
            Parallel.ForEach(MaterialFolders, MaterialFolder =>
            {
                string stringedName = MaterialFolder + "mtl_" + T10ModelName;
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

    static void T10BPMaterials()
    {
        string game = PickGame();

        Console.WriteLine("Generating Blueprint Material Names:\n");

        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] T10BPNames = File.ReadAllLines(@"Files\Materials\T10BlueprintNames.txt");
        string[] T10WeaponNames = File.ReadAllLines(@"Files\Shared\T10WeaponNames.txt");
        string[] MaterialKeywords = File.ReadAllLines(@"Files\Materials\Keywords.txt");
        string[] mtlTypes = {"","_att","_wpn"};
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialKeywords, Keyword =>
        {
            Parallel.ForEach(T10BPNames, BPName =>
            {
                Parallel.ForEach(T10WeaponNames, T10WeaponName =>
                {
                    Parallel.ForEach(mtlTypes, type =>
                    {
                        Parallel.ForEach(MaterialFolders, MaterialFolder =>
                        {
                            string stringedName = MaterialFolder + "mtl" + type + "_t10_" + T10WeaponName + "_" + Keyword + BPName;
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
        string game = PickGame();

        Console.WriteLine("Unhashing Sounds:\n");

        if(File.Exists(@"Files\GeneratedHashesSounds.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesSounds.txt");
            file.Close();
        }

        string[] SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "SoundAssetLog.txt");
        string[] WeaponSoundFolderPaths = File.ReadAllLines(@"Files\Sounds\WeaponSoundFolderPaths" + game + ".txt");
        string[] WeaponSoundCategories = File.ReadAllLines(@"Files\Sounds\WeaponSoundCategories" + game + ".txt");
        string[] WeaponSoundNames = File.ReadAllLines(@"Files\Sounds\WeaponSoundNames" + game + ".txt");
        string[] WeaponSoundSuffixes = File.ReadAllLines(@"Files\Sounds\WeaponSoundSuffixes" + game + ".txt");

        using StreamWriter GeneratedHashesSounds = new StreamWriter(@"Files\GeneratedHashesSounds.txt", true);

        string[] foundHashes = [];

        foreach(string soundFolder in WeaponSoundFolderPaths)
        {
            if(soundFolder.Contains('*') == false)
            {
                Console.WriteLine("Current Folder: " + soundFolder);

                Parallel.ForEach(WeaponSoundCategories, soundCategory =>
                {
                    if(soundCategory.Contains('*') == false)
                    {
                        Parallel.ForEach(WeaponSoundNames, soundName =>
                        {
                            Parallel.ForEach(WeaponSoundSuffixes, weaponSoundSuffix =>
                            {
                                string stringedName = soundFolder + soundCategory + soundName;
                                string generatedHash = CalcHash(stringedName + weaponSoundSuffix);

                                if(Globals.DebugToggle)
                                {
                                    Console.WriteLine(stringedName + weaponSoundSuffix +  " | " + generatedHash);
                                }

                                Parallel.ForEach(SoundAssetLog, hashedSound =>
                                {
                                    if(hashedSound == generatedHash)
                                    {
                                        stringedName = stringedName.Replace('/','\\');

                                        string fullHash = generatedHash + "," + stringedName + weaponSoundSuffix;
                                        foundHashes = foundHashes.Append(fullHash).ToArray();
                                        Console.WriteLine(fullHash);
                                    }
                                });
                            });
                        });
                    }
                });
            }
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesSounds.WriteLine(foundHash);
        }

        GeneratedHashesSounds.Close();
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
        if(game == "mw6")
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
        if(game == "MW6")
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

    static void AliasHashes()
    {
        Console.WriteLine("Unhashing Aliases:\n");

        if(File.Exists(@"Files\GeneratedHashesAliases.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAliases.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAliases = new StreamWriter(@"Files\GeneratedHashesAliases.txt", true);

        string[] AliasAssetLog = File.ReadAllLines(@"Files\Sound Banks\HashedAliases.txt");
        string[] AliasNames = File.ReadAllLines(@"Files\Sound Banks\AliasNames.txt");
        string[] AliasTypes = File.ReadAllLines(@"Files\Sound Banks\AliasTypes.txt");
        string[] AliasStarts = File.ReadAllLines(@"Files\Sound Banks\AliasStarts.txt");
        string[] WeaponNames = File.ReadAllLines(@"Files\Shared\NoPlatformWeaponNames.txt");
        string[] AliasNumbers = File.ReadAllLines(@"Files\Sound Banks\AliasNumbers.txt");

        string[] foundHashes = [];

        Parallel.ForEach(AliasNames, alias =>
        {
            Parallel.ForEach(WeaponNames, weaponName =>
            {
                Parallel.ForEach(AliasTypes, type =>
                {
                    Parallel.ForEach(AliasStarts, start =>
                    {
                        Parallel.ForEach(AliasNumbers, number => 
                        {
                            string stringedName = start + "_" + type + weaponName + "_" + alias + number;
                            string generatedHash = CalcHashLegacy(stringedName);

                            if(Globals.DebugToggle)
                            {
                                Console.WriteLine(stringedName + " | " + generatedHash);
                            }

                            Parallel.ForEach(AliasAssetLog, hashedAlias =>
                            {
                                if(hashedAlias == generatedHash)
                                {
                                    string fullHash = hashedAlias + "," + stringedName;
                                    Console.WriteLine(fullHash);
                                    foundHashes = foundHashes.Append(fullHash).ToArray();
                                }
                            });
                        });
                    });
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAliases.WriteLine(foundHash);
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

        string[] MW5AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5AnimAssetLog.txt");
        string[] MW6AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6AnimAssetLog.txt");
        string[] T10AnimAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10AnimAssetLog.txt");
        string[] OldAnims = File.ReadAllLines(@"Files\Old Hashes\OldAnims.txt");

        string[] foundHashes = [];

        foreach(string animName in OldAnims)
        {
            string generatedHash = CalcHash(animName);

            Parallel.ForEach(MW5AnimAssetLog, hashedAnim =>
            {
                if(generatedHash == hashedAnim)
                {
                    string fullHash = hashedAnim + "," + animName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(MW6AnimAssetLog, hashedAnim =>
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

    static void OldBones()
    {
        Console.WriteLine("Checking old Bone Names:\n");

        if(File.Exists(@"Files\GeneratedHashesBones.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesBones.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesBones = new StreamWriter(@"Files\GeneratedHashesBones.txt", true);

        string[] MW5BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5BoneAssetLog.txt");
        string[] MW6BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6BoneAssetLog.txt");
        string[] T10BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10AnimAssetLog.txt");
        string[] OldBones = File.ReadAllLines(@"Files\Old Hashes\OldBones.txt");

        string[] foundHashes = [];

        foreach(string boneName in OldBones)
        {
            string generatedHash = CalcHash(boneName);

            Parallel.ForEach(MW5BoneAssetLog, hashedBone =>
            {
                if(generatedHash == hashedBone)
                {
                    string fullHash = hashedBone + "," + boneName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(MW6BoneAssetLog, hashedBone =>
            {
                if(generatedHash == hashedBone)
                {
                    string fullHash = hashedBone + "," + boneName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10BoneAssetLog, hashedBone =>
            {
                if(generatedHash == hashedBone)
                {
                    string fullHash = hashedBone + "," + boneName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });
        }

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesBones.WriteLine(foundHash);
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

        string[] MW5ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5ImageAssetLog.txt");
        string[] MW6ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6ImageAssetLog.txt");
        string[] T10ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10ImageAssetLog.txt");
        string[] OldImages = File.ReadAllLines(@"Files\Old Hashes\OldImages.txt");

        string[] foundHashes = [];

        foreach(string imageName in OldImages)
        {
            string generatedHash = CalcHash(imageName);

            Parallel.ForEach(MW5ImageAssetLog, hashedImage =>
            {
                if(generatedHash == hashedImage)
                {
                    string fullHash = hashedImage + "," + imageName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(MW6ImageAssetLog, hashedImage =>
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

        string[] MW5MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5MaterialAssetLog.txt");
        string[] MW6MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6MaterialAssetLog.txt");
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10MaterialAssetLog.txt");
        string[] OldMaterials = File.ReadAllLines(@"Files\Old Hashes\OldMaterials.txt");
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        string[] foundHashes = [];

        Parallel.ForEach(MaterialFolders, materialFolder =>
        {
            Parallel.ForEach(OldMaterials, materialName =>
            {
                string stringedName = materialFolder + materialName;
                string generatedHash = CalcHash(stringedName);

                Parallel.ForEach(MW5MaterialAssetLog, hashedMaterial =>
                {
                    if(generatedHash == hashedMaterial)
                    {
                        string fullHash = hashedMaterial + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });

                Parallel.ForEach(MW6MaterialAssetLog, hashedMaterial =>
                {
                    if(generatedHash == hashedMaterial)
                    {
                        string fullHash = hashedMaterial + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });

                Parallel.ForEach(T10MaterialAssetLog, hashedMaterial =>
                {
                    if(generatedHash == hashedMaterial)
                    {
                        string fullHash = hashedMaterial + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });
            });
        });
        

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

        string[] MW5ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5ModelAssetLog.txt");
        string[] MW6ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6ModelAssetLog.txt");
        string[] T10ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10ModelAssetLog.txt");
        string[] OldModels = File.ReadAllLines(@"Files\Old Hashes\OldModels.txt");

        string[] foundHashes = [];

        foreach(string modelName in OldModels)
        {
            string generatedHash = CalcHash(modelName);

            Parallel.ForEach(MW5ModelAssetLog, hashedModel =>
            {
                if(generatedHash == hashedModel)
                {
                    string fullHash = hashedModel + "," + modelName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(MW6ModelAssetLog, hashedModel =>
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
        Console.WriteLine("Checking old Sound names:\n");

        if(File.Exists(@"Files\GeneratedHashesSounds.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesSounds.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesSounds = new StreamWriter(@"Files\GeneratedHashesSounds.txt", true);

        string[] MW5SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5soundAssetLog.txt");
        string[] MW6SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6soundAssetLog.txt");
        string[] T10SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10soundAssetLog.txt");
        string[] OldSounds = File.ReadAllLines(@"Files\Old Hashes\OldSounds.txt");
        string[] SoundSuffixes = File.ReadAllLines(@"Files\Sounds\SoundSuffixes.txt");

        string[] foundHashes = [];

        Parallel.ForEach(OldSounds, soundName =>
        {
            Parallel.ForEach(SoundSuffixes, soundSuffix =>
            {
                string stringedName = soundName + soundSuffix;
                string generatedHash = CalcHash(stringedName);
                stringedName = stringedName.Replace('/','\\');

                Parallel.ForEach(MW5SoundAssetLog, hashedSound =>
                {
                    if(generatedHash == hashedSound)
                    {
                        string fullHash = hashedSound + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });

                Parallel.ForEach(MW6SoundAssetLog, hashedSound =>
                {
                    if(generatedHash == hashedSound)
                    {
                        string fullHash = hashedSound + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });

                Parallel.ForEach(T10SoundAssetLog, hashedSound =>
                {
                    if(generatedHash == hashedSound)
                    {
                        string fullHash = hashedSound + "," + stringedName;
                        Console.WriteLine(fullHash);
                        foundHashes = foundHashes.Append(fullHash).ToArray();
                    }
                });
            });
        });
        

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesSounds.WriteLine(foundHash);
        }
    }

    static void OldSoundbanks()
    {
        Console.WriteLine("Unhashing old Soundbank Names:\n");
        
        string[] MW6Soundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_mw6");
        string[] MW6SoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_mw6");
        string[] T10Soundbanks = Directory.GetFiles(@"Files\Sound Banks\soundbank_bo6");
        string[] T10SoundbanksTR = Directory.GetFiles(@"Files\Sound Banks\soundbanktr_bo6");
        string[] OldSoundbankNames = File.ReadAllLines(@"Files\Old Hashes\OldSoundbankNames.txt");
        string[] SoundbankSuffixes = File.ReadAllLines(@"Files\Sound Banks\SoundbankSuffixes.txt");

        Parallel.ForEach(OldSoundbankNames, oldSoundbank =>
        {
            Parallel.ForEach(MW6Soundbanks, soundbank =>
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

            Parallel.ForEach(MW6SoundbanksTR, soundbankTR =>
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

    static void OldAnimpackages()
    {
        Console.WriteLine("Unhashing old Anim Package Names:\n");
        
        if(File.Exists(@"Files\GeneratedHashesAnimpackageNames.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnimpackageNames.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnimpackageNames = new StreamWriter(@"Files\GeneratedHashesAnimpackageNames.txt", true);

        string[] MW6AnimPackages = Directory.GetFiles(@"Files\Anim Packages\anim_pkgs_mw6");
        string[] T10AnimPackages = Directory.GetFiles(@"Files\Anim Packages\anim_pkgs_bo6");
        string[] OldAnimpackageNames = File.ReadAllLines(@"Files\Old Hashes\OldAnimpackageNames.txt");
        string[] foundHashes = [];

        Parallel.ForEach(OldAnimpackageNames, oldAnimpackage =>
        {
            Parallel.ForEach(MW6AnimPackages, animpackage =>
            {
                if(animpackage.Contains("mw6\\animpkg_"))
                {
                    string animpackageHashed =  animpackage.Substring(animpackage.LastIndexOf('\\') + 1);
                    animpackageHashed = animpackageHashed.Replace("animpkg_","");
                    animpackageHashed = animpackageHashed.Replace(".csv","");
                    string stringedName = oldAnimpackage;

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName + " | " + animpackageHashed);
                    }

                    if(CalcHash(stringedName) == animpackageHashed)
                    {
                        if(File.Exists(@"Files\Anim Packages\anim_pkgs_mw6\\animpkg_" + animpackageHashed + ".csv"))
                        {
                            Console.WriteLine(animpackageHashed + " | " + stringedName);
                            File.Move(@"Files\Anim Packages\anim_pkgs_mw6\\animpkg_" + animpackageHashed + ".csv", @"Files\Anim Packages\anim_pkgs_mw6\\" + stringedName + ".csv");
                        }
                    }
                }
            });

            Parallel.ForEach(T10AnimPackages, animpackage =>
            {
                if(animpackage.Contains("bo6\\"))
                {
                    string animpackageHashed =  animpackage.Substring(animpackage.LastIndexOf('\\') + 1);
                    animpackageHashed = animpackageHashed.Replace(".json","");
                    string stringedName = oldAnimpackage;

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName + " | " + animpackageHashed);
                    }

                    if(CalcHash(stringedName) == animpackageHashed)
                    {
                        if(File.Exists(@"Files\Anim Packages\anim_pkgs_bo6\\" + animpackageHashed + ".json"))
                        {
                            Console.WriteLine(animpackageHashed + " | " + stringedName);
                            File.Move(@"Files\Anim Packages\anim_pkgs_bo6\\" + animpackageHashed + ".json", @"Files\Anim Packages\anim_pkgs_bo6\\" + stringedName + ".json");
                            foundHashes = foundHashes.Append(animpackageHashed + "," + stringedName).ToArray();
                        }
                    }
                }
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAnimpackageNames.WriteLine(foundHash);
        }
    }
    
    static void AnimPackageGuesser()
    {
        string game = PickGame();
        game = game.ToLower();

        string game1 = "t10";
        if(game == "MW6")
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

            string[] Animpackages = Directory.GetFiles(@"Files\Anim Packages\anim_pkgs_" + game1);

            Parallel.ForEach(Animpackages, animpackage =>
            {
                if(animpackage.Contains(game1 + "\\0x"))
                {
                    string animpackageHashed =  animpackage.Substring(animpackage.LastIndexOf("x") + 1);
                    animpackageHashed = animpackageHashed.Replace("0x","");
                    animpackageHashed = animpackageHashed.Replace(".json","");
                    string? stringedName = userResponse;

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName + " | " + animpackageHashed);
                    }

                    if(stringedName != null && CalcHash(stringedName) == animpackageHashed)
                    {
                        if(File.Exists(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\0x" + animpackageHashed + ".json"))
                        {
                            Console.WriteLine(animpackageHashed + " | " + stringedName);
                            File.Move(@"Files\Anim Packages\anim_pkgs_" + game1 + "\\soundbank_" + animpackageHashed + ".json", @"Files\Anim Packages\anim_pkgs_" + game1 + "\\" + stringedName + ".json");
                        }
                    }
                }
            });
        }
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

    static void WordCombiner()
    {
        Console.WriteLine("Which asset type?\n");

        string assetType = PickAssetType();
        
        //Console.WriteLine("Length?\n");

        //int length = PickLength();

        string[] WordsToCombine = File.ReadAllLines(@"Files\Shared\WordsToCombine.txt");
        string[] MW6AssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6" + assetType + "AssetLog.txt");
        string[] T10AssetLog = File.ReadAllLines(@"Files\Asset Logs\T10" + assetType + "AssetLog.txt");

        Console.WriteLine("Combining Words:\n");

        Parallel.ForEach(WordsToCombine , word1 =>
        {
            Parallel.ForEach(WordsToCombine , word2 =>
            {
                Parallel.ForEach(WordsToCombine , word3 =>
                {
                    Parallel.ForEach(WordsToCombine , word4 =>
                    {
                        Parallel.ForEach(WordsToCombine , word5 =>
                        {
                            Parallel.ForEach(WordsToCombine , word6 =>
                            {
                                Parallel.ForEach(WordsToCombine , word7 =>
                                {
                                    Parallel.ForEach(WordsToCombine , word8 =>
                                    {
                                        string stringedName = word1 + "_" + word2 + "_" + word3 + "_" + word4 + "_" + word5 + "_" + word6 + "_" + word7 + "_" + word8;
                                        if(assetType == "Material")
                                        {
                                            stringedName = "mo/" + stringedName;
                                        }

                                        string generatedHash = CalcHash(stringedName);

                                        if(Globals.DebugToggle)
                                        {
                                            Console.WriteLine(stringedName + " | " + generatedHash);
                                        }

                                        Parallel.ForEach(MW6AssetLog , hashedAsset =>
                                        {
                                            if(hashedAsset == generatedHash)
                                            {
                                                Console.WriteLine(stringedName);
                                            }
                                        });

                                        Parallel.ForEach(T10AssetLog , hashedAsset =>
                                        {
                                            if(hashedAsset == generatedHash)
                                            {
                                                Console.WriteLine(stringedName);
                                            }
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
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