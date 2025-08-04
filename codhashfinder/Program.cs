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
                            MW6Animations();

                            break;
                        }
                        case "12":
                        {
                            T10Animations();

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
                            OldAnimations();

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

    static string CalcHash64(string data)
    {
        ulong result = 0x47F5817A5EF961BA;

        for(int i = 0; i < Encoding.UTF8.GetByteCount(data); i++)
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
        ulong result = 0xCBF29CE484222325;
        
        for (int i = 0; i < Encoding.UTF8.GetByteCount(data); i++)
        {
            ulong value = data[i];
            if (value == '\\')
            {
                value = '/';
            }

            result = 0x100000001B3 * (value ^ result);
        }
        
        return String.Format("{0:x}", result & 0x7FFFFFFFFFFFFFFF);
    }

    static string CalcHash32Bones(string data)
    {
        uint result = 0x811C9DC5;

        for (int i = 0; i < Encoding.UTF8.GetByteCount(data); i++)
        {
            uint value = data[i];

            if (value == '\\')
            {
                value = '/';
            }

            result = 0x01000193 * (value ^ result);
        }

        return String.Format("{0:x}", result);
    }

    static string PickGame()
    {
        for (; ; )
        {
            Console.WriteLine("MW5, MW6 or T10?\n");

            string? userResponse = Console.ReadLine();

            switch (userResponse)
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

    static void MW6Animations()
    {
        Console.WriteLine("Generating MW6 Anim Hashes:\n");

        string[] MW6VMTypes = File.ReadAllLines(@"Files\Animations\VMTypes.txt");
        string[] MW6AnimationAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Animations.txt");
        string[] SharedWeaponNamesAnimations = File.ReadAllLines(@"Files\Shared\WeaponNamesAnimations.txt");

        if(File.Exists(@"Files\GeneratedHashesAnimations.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnimations.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnimations = new StreamWriter(@"Files\GeneratedHashesAnimations.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MW6VMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnimations, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "mw6_vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash64(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(MW6AnimationAssetLog, hash =>
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
            GeneratedHashesAnimations.WriteLine(foundHash);
        }
    }

    static void T10Animations()
    {
        Console.WriteLine("Generating T10 Anim Hashes:\n");

        string[] T10VMTypes = File.ReadAllLines(@"Files\Animations\VMTypes.txt");
        string[] T10WMTypes = File.ReadAllLines(@"Files\Animations\WMTypes.txt");
        string[] T10WMStances = File.ReadAllLines(@"Files\Animations\WMStances.txt");
        string[] T10WMAttTypes = File.ReadAllLines(@"Files\Animations\WMAttTypes.txt");
        string[] T10WMNames = File.ReadAllLines(@"Files\Animations\T10WMNames.txt");
        string[] T10AnimationAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Animations.txt");
        string[] SharedWeaponNamesAnimations = File.ReadAllLines(@"Files\Shared\WeaponNamesAnimations.txt");
        string[] T10WMSuffixes = File.ReadAllLines(@"Files\Animations\T10WMSuffixes.txt");
        string[] T10WMDetails = File.ReadAllLines(@"Files\Animations\T10WMDetails.txt");

        if (File.Exists(@"Files\GeneratedHashesAnimations.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnimations.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnimations = new StreamWriter(@"Files\GeneratedHashesAnimations.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(T10VMTypes, animType =>
        {
            Parallel.ForEach(SharedWeaponNamesAnimations, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash64(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(T10AnimationAssetLog, hash =>
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
            Parallel.ForEach(SharedWeaponNamesAnimations, weaponName =>
            {
                if(weaponName != "")
                {
                    string stringedName = "t10_vm_" + weaponName + "_" + animType;
                    string generatedHash = CalcHash64(stringedName);

                    if(Globals.DebugToggle)
                    {
                        Console.WriteLine(stringedName);
                    }

                    Parallel.ForEach(T10AnimationAssetLog, hash =>
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

        Parallel.ForEach(T10WMTypes, animType =>
        {
            Parallel.ForEach(T10WMStances, stanceType =>
            {
                Parallel.ForEach(T10WMAttTypes, attType =>
                {
                    Parallel.ForEach(SharedWeaponNamesAnimations, weaponName =>
                    {
                        Parallel.ForEach(T10WMNames, wmName =>
                        {
                            Parallel.ForEach(T10WMSuffixes, suffixType =>
                            {
                                Parallel.ForEach(T10WMDetails, details =>
                                {
                                    if (weaponName != "")
                                    {
                                        string stringedName = wmName + weaponName + animType + stanceType + attType + details + suffixType;
                                        string generatedHash = CalcHash64(stringedName);

                                        if (Globals.DebugToggle)
                                        {
                                            Console.WriteLine(stringedName);
                                        }

                                        Parallel.ForEach(T10AnimationAssetLog, hash =>
                                        {
                                            if (generatedHash == hash)
                                            {
                                                string fullHash = generatedHash + "," + stringedName;
                                                foundHashes = foundHashes.Append(fullHash).ToArray();
                                                Console.WriteLine(fullHash);
                                            }
                                        });
                                    }
                                });
                            });
                        });
                    });
                });
            });
        });

        foreach(string foundHash in foundHashes)
        {
            GeneratedHashesAnimations.WriteLine(foundHash);
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

                            if (CalcHash64(stringedName) == animpackageHashed)
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

                            if (CalcHash64(stringedName) == animpackageHashed)
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

        string[] ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "\\" + game + "Images.txt");
        string[] MaterialNames = File.ReadAllLines(@"Files\Images\MaterialNames.txt");
        string[] TextureTypes = File.ReadAllLines(@"Files\Images\TextureNames.txt");

        using StreamWriter GeneratedHashesImages = new StreamWriter(@"Files\GeneratedHashesImages.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(MaterialNames, materialName =>
        {
            Parallel.ForEach(TextureTypes, textureType =>
            {
                string stringedName = materialName + "_" + textureType;
                string generatedHash = CalcHash64(stringedName);

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

        string[] MW6MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Materials.txt");
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
                        string generatedHash = CalcHash64(stringedName);

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
        
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Materials.txt");
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
                    string generatedHash = CalcHash64(stringedName);

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
        
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Materials.txt");
        string[] T10ModelNames = File.ReadAllLines(@"Files\Materials\T10WeaponModelNames.txt");
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        using StreamWriter GeneratedHashesMaterials = new StreamWriter(@"Files\GeneratedHashesMaterials.txt", true);

        string[] foundHashes = [];

        Parallel.ForEach(T10ModelNames, T10ModelName =>
        {
            Parallel.ForEach(MaterialFolders, MaterialFolder =>
            {
                string stringedName = MaterialFolder + "mtl_" + T10ModelName;
                string generatedHash = CalcHash64(stringedName);

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

        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Materials.txt");
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
                            string generatedHash = CalcHash64(stringedName);

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

        string[] SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "\\" + game + "Sounds.txt");
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
                                string generatedHash = CalcHash64(stringedName + weaponSoundSuffix);

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
            string[] SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "\\" + game + "Sounds.txt");
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
                    string generatedHash = CalcHash64(stringedName);

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

                        if(CalcHash64(stringedName) == soundbankHashed)
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

                        if(CalcHash64(stringedName) == soundbankTRHashed)
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

                        if(CalcHash64(stringedName) == soundbankHashed)
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

                        if(CalcHash64(stringedName) == soundbankTRHashed)
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
                            string generatedHash = CalcHash32(stringedName);

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

    static void OldAnimations()
    {
        Console.WriteLine("Checking old Anim Names:\n");

        if(File.Exists(@"Files\GeneratedHashesAnimations.txt") != true)
        {
            var file = File.Create(@"Files\GeneratedHashesAnimations.txt");
            file.Close();
        }

        using StreamWriter GeneratedHashesAnimations = new StreamWriter(@"Files\GeneratedHashesAnimations.txt", true);

        string[] MW5AnimationAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5Animations.txt");
        string[] MW6AnimationAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Animations.txt");
        string[] T10AnimationAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Animations.txt");
        string[] OldAnimations = File.ReadAllLines(@"Files\Old Hashes\OldAnimations.txt");

        string[] foundHashes = [];

        foreach(string animName in OldAnimations)
        {
            string generatedHash = CalcHash64(animName);

            Parallel.ForEach(MW5AnimationAssetLog, hashedAnim =>
            {
                if(generatedHash == hashedAnim)
                {
                    string fullHash = hashedAnim + "," + animName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(MW6AnimationAssetLog, hashedAnim =>
            {
                if(generatedHash == hashedAnim)
                {
                    string fullHash = hashedAnim + "," + animName;
                    Console.WriteLine(fullHash);
                    foundHashes = foundHashes.Append(fullHash).ToArray();
                }
            });

            Parallel.ForEach(T10AnimationAssetLog, hashedAnim =>
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
            GeneratedHashesAnimations.WriteLine(foundHash);
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

        string[] MW5BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5Bones.txt");
        string[] MW6BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Bones.txt");
        string[] T10BoneAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Bones.txt");
        string[] OldBones = File.ReadAllLines(@"Files\Old Hashes\OldBones.txt");

        string[] foundHashes = [];

        foreach(string boneName in OldBones)
        {
            string generatedHash = CalcHash64(boneName);

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

        string[] MW5ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5Images.txt");
        string[] MW6ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Images.txt");
        string[] T10ImageAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Images.txt");
        string[] OldImages = File.ReadAllLines(@"Files\Old Hashes\OldImages.txt");

        string[] foundHashes = [];

        foreach(string imageName in OldImages)
        {
            string generatedHash = CalcHash64(imageName);

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

        string[] MW5MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5Materials.txt");
        string[] MW6MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Materials.txt");
        string[] T10MaterialAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Materials.txt");
        string[] OldMaterials = File.ReadAllLines(@"Files\Old Hashes\OldMaterials.txt");
        string[] MaterialFolders = File.ReadAllLines(@"Files\Materials\MaterialFolderNames.txt");

        string[] foundHashes = [];

        Parallel.ForEach(MaterialFolders, materialFolder =>
        {
            Parallel.ForEach(OldMaterials, materialName =>
            {
                string stringedName = materialFolder + materialName;
                string generatedHash = CalcHash64(stringedName);

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

        string[] MW5ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5Models.txt");
        string[] MW6ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6Models.txt");
        string[] T10ModelAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10Models.txt");
        string[] OldModels = File.ReadAllLines(@"Files\Old Hashes\OldModels.txt");

        string[] foundHashes = [];

        foreach(string modelName in OldModels)
        {
            string generatedHash = CalcHash64(modelName);

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

        string[] MW5SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW5\MW5sounds.txt");
        string[] MW6SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\MW6\MW6sounds.txt");
        string[] T10SoundAssetLog = File.ReadAllLines(@"Files\Asset Logs\T10\T10sounds.txt");
        string[] OldSounds = File.ReadAllLines(@"Files\Old Hashes\OldSounds.txt");
        string[] SoundSuffixes = File.ReadAllLines(@"Files\Sounds\SoundSuffixes.txt");

        string[] foundHashes = [];

        Parallel.ForEach(OldSounds, soundName =>
        {
            Parallel.ForEach(SoundSuffixes, soundSuffix =>
            {
                string stringedName = soundName + soundSuffix;
                string generatedHash = CalcHash64(stringedName);
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

                        if(CalcHash64(stringedName) == soundbankHashed)
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

                        if(CalcHash64(stringedName) == soundbankTRHashed)
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

                        if(CalcHash64(stringedName) == soundbankHashed)
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

                        if(CalcHash64(stringedName) == soundbankTRHashed)
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

                    if(CalcHash64(stringedName) == animpackageHashed)
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

                    if(CalcHash64(stringedName) == animpackageHashed)
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

                    if(stringedName != null && CalcHash64(stringedName) == animpackageHashed)
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

        string[] AssetLog = File.ReadAllLines(@"Files\Asset Logs\" + game + "\\" + game + "AssetsLog.txt");

        using StreamWriter AnimationAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "Animations.txt");
        using StreamWriter AnimationAssetLogUnhashed = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "AnimationsUnhashed.txt");

        using StreamWriter ImageAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "Images.txt");
        using StreamWriter ImageAssetLogUnhashed = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "ImagesUnhashed.txt");

        using StreamWriter MaterialAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "Materials.txt");
        using StreamWriter MaterialAssetLogUnhashed = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "MaterialsUnhashed.txt");

        using StreamWriter ModelAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "Models.txt");
        using StreamWriter ModelAssetLogUnhashed = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "ModelsUnhashed.txt");

        using StreamWriter SoundAssetLog = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "Sounds.txt");
        using StreamWriter SoundAssetLogUnhashed = new StreamWriter(@"Files\Asset Logs\" + game + "\\" + game + "SoundsUnhashed.txt");

        foreach(string hash in AssetLog)
        {
            string assetType = hash.Substring(0, hash.IndexOf(','));
            //string result = hash.Substring(hash.IndexOf(',') + 1);
            string result = "";

            switch (assetType)
            {
                case "Animation":
                    {
                        if (hash.Contains("Animation,anim_"))
                        {
                            //Hashed Animations
                            result = hash.Replace("Animation,anim_", "");
                            AnimationAssetLog.WriteLine(result);
                        }
                        else
                        {
                            //Unhashed Animations
                            result = hash.Replace("Animation,", "");
                            AnimationAssetLogUnhashed.WriteLine(result);
                        }

                        break;
                    }
                case "Image":
                    {
                        if (hash.Contains("Image,image_"))
                        {
                            //Hashed Images
                            result = hash.Replace("Image,image_", "");
                            ImageAssetLog.WriteLine(result);
                        }
                        else
                        {
                            //Unhashed Images
                            result = hash.Replace("Image,", "");
                            ImageAssetLogUnhashed.WriteLine(result);
                        }

                        break;
                    }
                case "Material":
                    {
                        if (hash.Contains("Material,material_"))
                        {
                            //Hashed Materials
                            result = hash.Replace("Material,material_", "");
                            MaterialAssetLog.WriteLine(result);
                        }
                        else
                        {
                            //Unhashed Materials
                            result = hash.Replace("Material,", "");
                            MaterialAssetLogUnhashed.WriteLine(result);
                        }

                        break;
                    }
                case "Model":
                    {
                        if (hash.Contains("Model,model_"))
                        {
                            //Hashed Models
                            result = hash.Replace("Model,model_", "");
                            ModelAssetLog.WriteLine(result);
                        }
                        else
                        {
                            //Unhashed Models
                            result = hash.Replace("Model,", "");
                            ModelAssetLogUnhashed.WriteLine(result);
                        }

                        break;
                    }
                case "Sound":
                    {
                        if (hash.Contains("Sound,sound_"))
                        {
                            //Hashed Sounds
                            result = hash.Replace("Sound,sound_", "");
                            SoundAssetLog.WriteLine(result);
                        }
                        else
                        {
                            //Unhashed Sounds
                            result = hash.Replace("Sound,", "");
                            SoundAssetLogUnhashed.WriteLine(result);
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
                Console.WriteLine("64bit: " + userResponse + " | " + CalcHash64(userResponse));
                Console.WriteLine("32bit: " + userResponse + " | " + CalcHash32(userResponse));
            }
        }
    }

    static void ToggleDebug()
    {
        Globals.DebugToggle = Globals.DebugToggle ? Globals.DebugToggle = false : Globals.DebugToggle = true;
    }
}