using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestProfanityDetector
{
    public class ProfanityFilter
    {
        //blacklisted words
        string[] badWords = new string[]
        {
            "abbo",
            "abortion",
            "abuse",
            "addict",
            "addicts",
            "africa",
            "african",
            "alla",
            "allah",
            "alligatorbait",
            "amateur",
            "american",
            "anal",
            "analannie",
            "analsex",
            "angie",
            "angry",
            "anus",
            "arab",
            "arabs",
            "areola",
            "argie",
            "aroused",
            "arse",
            "arsehole",
            "asian",
            "assassin",
            "assassinate",
            "assassination",
            "assault",
            "assbagger",
            "assblaster",
            "assclown",
            "asscowboy",
            "asses",
            "assfuck",
            "assfucker",
            "asshat",
            "asshole",
            "assholes",
            "asshore",
            "assjockey",
            "asskiss",
            "asskisser",
            "assklown",
            "asslick",
            "asslicker",
            "asslover",
            "assman",
            "assmonkey",
            "assmunch",
            "assmuncher",
            "asspacker",
            "asspirate",
            "asspuppies",
            "assranger",
            "asswhore",
            "asswipe",
            "athletesfoot",
            "attack",
            "australian",
            "babe",
            "babies",
            "backdoor",
            "backdoorman",
            "backseat",
            "badfuck",
            "balllicker",
            "balls",
            "ballsack",
            "banging",
            "baptist",
            "barelylegal",
            "barf",
            "barface",
            "barfface",
            "bast",
            "bastard ",
            "bazongas",
            "bazooms",
            "beaner",
            "beast",
            "beastality",
            "beastial",
            "beastiality",
            "beatoff",
            "beat-off",
            "beatyourmeat",
            "beaver",
            "bestial",
            "bestiality",
            "biatch",
            "bible",
            "bicurious",
            "bigass",
            "bigbastard",
            "bigbutt",
            "bigger",
            "bisexual",
            "bi-sexual",
            "bitch",
            "bitcher",
            "bitches",
            "bitchez",
            "bitchin",
            "bitching",
            "bitchslap",
            "bitchy",
            "biteme",
            "black",
            "blackman",
            "blackout",
            "blacks",
            "blind",
            "blow",
            "blowjob",
            "boang",
            "bogan",
            "bohunk",
            "bollick",
            "bollock",
            "bomb",
            "bombers",
            "bombing",
            "bombs",
            "bomd",
            "bondage",
            "boner",
            "bong",
            "boob",
            "boobies",
            "boobs",
            "booby",
            "boody",
            "boom",
            "boong",
            "boonga",
            "boonie",
            "booty",
            "bootycall",
            "bountybar",
            "brea5t",
            "breast",
            "breastjob",
            "breastlover",
            "breastman",
            "brothel",
            "bugger",
            "buggered",
            "buggery",
            "bullcrap",
            "bulldike",
            "bulldyke",
            "bullshit",
            "bumblefuck",
            "bumfuck",
            "bunga",
            "bunghole",
            "buried",
            "burn",
            "butchbabes",
            "butchdike",
            "butchdyke",
            "butt",
            "buttbang",
            "butt-bang",
            "buttface",
            "buttfuck",
            "butt-fuck",
            "buttfucker",
            "butt-fucker",
            "buttfuckers",
            "butt-fuckers",
            "butthead",
            "buttman",
            "buttmunch",
            "buttmuncher",
            "buttpirate",
            "buttplug",
            "buttstain",
            "byatch",
            "cacker",
            "cameljockey",
            "cameltoe",
            "canadian",
            "cancer",
            "carpetmuncher",
            "carruth",
            "catholic",
            "catholics",
            "cemetery",
            "chav",
            "cherrypopper",
            "chickslick",
            "children's",
            "chin",
            "chinaman",
            "chinamen",
            "chinese",
            "chink",
            "chinky",
            "choad",
            "chode",
            "christ",
            "christian",
            "church",
            "cigarette",
            "cigs",
            "clamdigger",
            "clamdiver",
            "clit",
            "clitoris",
            "clogwog",
            "cocaine",
            "cock",
            "cockblock",
            "cockblocker",
            "cockcowboy",
            "cockfight",
            "cockhead",
            "cockknob",
            "cocklicker",
            "cocklover",
            "cocknob",
            "cockqueen",
            "cockrider",
            "cocksman",
            "cocksmith",
            "cocksmoker",
            "cocksucer",
            "cocksuck ",
            "cocksucked ",
            "cocksucker",
            "cocksucking",
            "cocktail",
            "cocktease",
            "cocky",
            "cohee",
            "coitus",
            "color",
            "colored",
            "coloured",
            "commie",
            "communist",
            "condom",
            "coolie",
            "cooly",
            "coon",
            "coondog",
            "copulate",
            "cornhole",
            "corruption",
            "cra5h",
            "crabs",
            "crack",
            "crackpipe",
            "crackwhore",
            "crack-whore",
            "crap",
            "crapola",
            "crapper",
            "crappy",
            "crotch",
            "crotchjockey",
            "crotchmonkey",
            "crotchrot",
            "cumbubble",
            "cumfest",
            "cumjockey",
            "cumm",
            "cummer",
            "cumming",
            "cumquat",
            "cumqueen",
            "cumshot",
            "cunilingus",
            "cunillingus",
            "cunn",
            "cunnilingus",
            "cunntt",
            "cunt",
            "cunteyed",
            "cuntfuck",
            "cuntfucker",
            "cuntlick ",
            "cuntlicker ",
            "cuntlicking ",
            "cuntsucker",
            "cybersex",
            "cyberslimer",
            "dago",
            "dahmer",
            "dammit",
            "damn",
            "damnation",
            "damnit",
            "darkie",
            "darky",
            "datnigga",
            "dead",
            "deapthroat",
            "death",
            "deepthroat",
            "defecate",
            "dego",
            "demon",
            "deth",
            "devil",
            "devilworshipper",
            "dick",
            "dickbrain",
            "dickforbrains",
            "dickhead",
            "dickless",
            "dicklick",
            "dicklicker",
            "dickman",
            "dickwad",
            "dickweed",
            "diddle",
            "died",
            "dies",
            "dike",
            "dildo",
            "dingleberry",
            "dink",
            "dipshit",
            "dipstick",
            "dirty",
            "disease",
            "diseases",
            "disturbed",
            "dive",
            "dixiedike",
            "dixiedyke",
            "doggiestyle",
            "doggystyle",
            "dong",
            "doodoo",
            "doo-doo",
            "doom",
            "dope",
            "dragqueen",
            "dragqween",
            "dripdick",
            "drug",
            "drunk",
            "drunken",
            "dumb",
            "dumbass",
            "dumbbitch",
            "dumbfuck",
            "dyefly",
            "dyke",
            "easyslut",
            "eatballs",
            "eatme",
            "eatpussy",
            "ecstacy",
            "ejaculate",
            "ejaculated",
            "ejaculating ",
            "ejaculation",
            "enema",
            "enemy",
            "erect",
            "erection",
            "escort",
            "ethiopian",
            "ethnic",
            "european",
            "excrement",
            "execute",
            "executed",
            "execution",
            "executioner",
            "explosion",
            "facefucker",
            "faeces",
            "fagging",
            "faggot",
            "fagot",
            "failed",
            "failure",
            "fairies",
            "fairy",
            "faith",
            "fannyfucker",
            "fart",
            "farted ",
            "farting ",
            "farty ",
            "fastfuck",
            "fatah",
            "fatass",
            "fatfuck",
            "fatfucker",
            "fatso",
            "fckcum",
            "fear",
            "feces",
            "felatio ",
            "felch",
            "felcher",
            "felching",
            "fellatio",
            "feltch",
            "feltcher",
            "feltching",
            "fetish",
            "fingerfuck ",
            "fingerfucked ",
            "fingerfucker ",
            "fingerfuckers",
            "fingerfucking ",
            "fister",
            "fistfuck",
            "fistfucked ",
            "fistfucker ",
            "fistfucking ",
            "fisting",
            "flange",
            "flasher",
            "flatulence",
            "floo",
            "flydie",
            "flydye",
            "fock",
            "fondle",
            "footaction",
            "footfuck",
            "footfucker",
            "footlicker",
            "footstar",
            "fore",
            "foreskin",
            "forni",
            "fornicate",
            "foursome",
            "fourtwenty",
            "fraud",
            "freakfuck",
            "freakyfucker",
            "freefuck",
            "fubar",
            "fucck",
            "fuck",
            "fucka",
            "fuckable",
            "fuckbag",
            "fuckbuddy",
            "fucked",
            "fuckedup",
            "fucker",
            "fuckers",
            "fuckface",
            "fuckfest",
            "fuckfreak",
            "fuckfriend",
            "fuckhead",
            "fuckher",
            "fuckin",
            "fuckina",
            "fucking",
            "fuckingbitch",
            "fuckinnuts",
            "fuckinright",
            "fuckit",
            "fuckknob",
            "fuckme ",
            "fuckmehard",
            "fuckmonkey",
            "fuckoff",
            "fuckpig",
            "fucks",
            "fucktard",
            "fuckwhore",
            "fuckyou",
            "fudgepacker",
            "fugly",
            "fuks",
            "funeral",
            "funfuck",
            "fungus",
            "fuuck",
            "gangbang",
            "gangbanged ",
            "gangbanger",
            "gangsta",
            "gatorbait",
            "gaymuthafuckinwhore",
            "gaysex ",
            "geez",
            "geezer",
            "geni",
            "genital",
            "german",
            "getiton",
            "ginzo",
            "gipp",
            "girls",
            "givehead",
            "glazeddonut",
            "godammit",
            "goddamit",
            "goddammit",
            "goddamn",
            "goddamned",
            "goddamnes",
            "goddamnit",
            "goddamnmuthafucker",
            "goldenshower",
            "gonorrehea",
            "gonzagas",
            "gook",
            "gotohell",
            "goyim",
            "greaseball",
            "gringo",
            "groe",
            "gross",
            "grostulation",
            "gubba",
            "gummer",
            "gypo",
            "gypp",
            "gyppie",
            "gyppo",
            "gyppy",
            "hamas",
            "handjob",
            "hapa",
            "harder",
            "hardon",
            "harem",
            "headfuck",
            "headlights",
            "hebe",
            "heeb",
            "hell",
            "henhouse",
            "heroin",
            "herpes",
            "heterosexual",
            "hijack",
            "hijacker",
            "hijacking",
            "hillbillies",
            "hindoo",
            "hiscock",
            "hitler",
            "hitlerism",
            "hitlerist",
            "hobo",
            "hodgie",
            "hoes",
            "hole",
            "holestuffer",
            "homicide",
            "homo",
            "homobangers",
            "homosexual",
            "honger",
            "honk",
            "honkers",
            "honkey",
            "honky",
            "hook",
            "hooker",
            "hookers",
            "hooters",
            "hore",
            "hork",
            "horn",
            "horney",
            "horniest",
            "horny",
            "horseshit",
            "hosejob",
            "hoser",
            "hostage",
            "hotdamn",
            "hotpussy",
            "hottotrot",
            "hummer",
            "husky",
            "hussy",
            "hustler",
            "hymen",
            "hymie",
            "iblowu",
            "idiot",
            "ikey",
            "illegal",
            "incest",
            "insest",
            "intercourse",
            "interracial",
            "intheass",
            "inthebuff",
            "israel",
            "israeli",
            "israel's",
            "italiano",
            "itch",
            "jackass",
            "jackoff",
            "jackshit",
            "jacktheripper",
            "jade",
            "japanese",
            "japcrap",
            "jebus",
            "jeez",
            "jerkoff",
            "jesus",
            "jesuschrist",
            "jewish",
            "jiga",
            "jigaboo",
            "jigg",
            "jigga",
            "jiggabo",
            "jigger ",
            "jiggy",
            "jihad",
            "jijjiboo",
            "jimfish",
            "jism",
            "jiz ",
            "jizim",
            "jizjuice",
            "jizm ",
            "jizz",
            "jizzim",
            "jizzum",
            "joint",
            "juggalo",
            "jugs",
            "junglebunny",
            "kaffer",
            "kaffir",
            "kaffre",
            "kafir",
            "kanake",
            "kigger",
            "kike",
            "kill",
            "killed",
            "killer",
            "killing",
            "kills",
            "kink",
            "kinky",
            "kissass",
            "knife",
            "knockers",
            "kock",
            "kondum",
            "koon",
            "kotex",
            "krap",
            "krappy",
            "kraut",
            "kumbubble",
            "kumbullbe",
            "kummer",
            "kumming",
            "kumquat",
            "kums",
            "kunilingus",
            "kunnilingus",
            "kunt",
            "kyke",
            "lactate",
            "laid",
            "lapdance",
            "latin",
            "lesbain",
            "lesbayn",
            "lesbian",
            "lesbin",
            "lesbo",
            "lezbe",
            "lezbefriends",
            "lezbo",
            "lezz",
            "lezzo",
            "liberal",
            "libido",
            "licker",
            "lickme",
            "lies",
            "limey",
            "limpdick",
            "limy",
            "lingerie",
            "liquor",
            "livesex",
            "loadedgun",
            "lolita",
            "looser",
            "loser",
            "lotion",
            "lovebone",
            "lovegoo",
            "lovegun",
            "lovejuice",
            "lovemuscle",
            "lovepistol",
            "loverocket",
            "lowlife",
            "lubejob",
            "lucifer",
            "luckycammeltoe",
            "lugan",
            "lynch",
            "macaca",
            "mafia",
            "magicwand",
            "mams",
            "manhater",
            "manpaste",
            "marijuana",
            "mastabate",
            "mastabater",
            "masterbate",
            "masterblaster",
            "mastrabator",
            "masturbate",
            "masturbating",
            "mattressprincess",
            "meatbeatter",
            "meatrack",
            "meth",
            "mexican",
            "mgger",
            "mggor",
            "mickeyfinn",
            "mideast",
            "milf",
            "minority",
            "mockey",
            "mockie",
            "mocky",
            "mofo",
            "moky",
            "moles",
            "molest",
            "molestation",
            "molester",
            "molestor",
            "moneyshot",
            "mooncricket",
            "mormon",
            "moron",
            "moslem",
            "mosshead",
            "mothafuck",
            "mothafucka",
            "mothafuckaz",
            "mothafucked ",
            "mothafucker",
            "mothafuckin",
            "mothafucking ",
            "mothafuckings",
            "motherfuck",
            "motherfucked",
            "motherfucker",
            "motherfuckin",
            "motherfucking",
            "motherfuckings",
            "motherlovebone",
            "muff",
            "muffdive",
            "muffdiver",
            "muffindiver",
            "mufflikcer",
            "mulatto",
            "muncher",
            "munt",
            "murder",
            "murderer",
            "muslim",
            "naked",
            "narcotic",
            "nasty",
            "nastybitch",
            "nastyho",
            "nastyslut",
            "nastywhore",
            "nazi",
            "necro",
            "negro",
            "negroes",
            "negroid",
            "negro's",
            "niger",
            "nigerian",
            "nigerians",
            "nigg",
            "nigga",
            "niggah",
            "niggaracci",
            "niggard",
            "niggarded",
            "niggarding",
            "niggardliness",
            "niggardliness's",
            "niggardly",
            "niggards",
            "niggard's",
            "niggaz",
            "nigger",
            "niggerhead",
            "niggerhole",
            "niggers",
            "nigger's",
            "niggle",
            "niggled",
            "niggles",
            "niggling",
            "nigglings",
            "niggor",
            "niggur",
            "niglet",
            "nignog",
            "nigr",
            "nigra",
            "nigre",
            "nipple",
            "nipplering",
            "nittit",
            "nlgger",
            "nlggor",
            "nofuckingway",
            "nook",
            "nookey",
            "nookie",
            "noonan",
            "nooner",
            "nude",
            "nudger",
            "nuke",
            "nutfucker",
            "nymph",
            "ontherag",
            "oral",
            "orga",
            "orgasim ",
            "orgasm",
            "orgies",
            "orgy",
            "osama",
            "paki",
            "palesimian",
            "palestinian",
            "pansies",
            "pansy",
            "panti",
            "panties",
            "payo",
            "pearlnecklace",
            "peck",
            "pecker",
            "peckerwood",
            "peehole",
            "peen",
            "pee-pee",
            "peepshow",
            "peepshpw",
            "pendy",
            "penetration",
            "peni5",
            "penile",
            "penis",
            "penises",
            "penthouse",
            "period",
            "perv",
            "phonesex",
            "phuk",
            "phuked",
            "phuking",
            "phukked",
            "phukking",
            "phungky",
            "phuq",
            "pi55",
            "picaninny",
            "piccaninny",
            "pickaninny",
            "piker",
            "pikey",
            "piky",
            "pimp",
            "pimped",
            "pimper",
            "pimpjuic",
            "pimpjuice",
            "pimpsimp",
            "pindick",
            "piss",
            "pissed",
            "pisser",
            "pisses ",
            "pisshead",
            "pissin ",
            "pissing",
            "pissoff ",
            "pistol",
            "pixie",
            "pixy",
            "playboy",
            "playgirl",
            "pocha",
            "pocho",
            "pocketpool",
            "pohm",
            "polack",
            "pommie",
            "pommy",
            "poon",
            "poontang",
            "poop",
            "pooper",
            "pooperscooper",
            "pooping",
            "poorwhitetrash",
            "popimp",
            "porchmonkey",
            "porn",
            "pornflick",
            "pornking",
            "porno",
            "pornography",
            "pornprincess",
            "poverty",
            "premature",
            "pric",
            "prick",
            "prickhead",
            "primetime",
            "propaganda",
            "pros",
            "prostitute",
            "protestant",
            "pu55i",
            "pu55y",
            "pube",
            "pubic",
            "pubiclice",
            "pudboy",
            "pudd",
            "puddboy",
            "puke",
            "puntang",
            "purinapricness",
            "puss",
            "pussie",
            "pussies",
            "pussy",
            "pussycat",
            "pussyeater",
            "pussyfucker",
            "pussylicker",
            "pussylips",
            "pussylover",
            "pussypounder",
            "pusy",
            "quashie",
            "queef",
            "queer",
            "quickie",
            "quim",
            "ra8s",
            "rabbi",
            "racial",
            "racist",
            "radical",
            "radicals",
            "raghead",
            "randy",
            "rape",
            "raped",
            "raper",
            "rapist",
            "rearend",
            "rearentry",
            "rectum",
            "redlight",
            "redneck",
            "reefer",
            "reestie",
            "refugee",
            "reject",
            "remains",
            "rentafuck",
            "republican",
            "rere",
            "retard",
            "retarded",
            "ribbed",
            "rigger",
            "rimjob",
            "rimming",
            "roach",
            "robber",
            "roundeye",
            "rump",
            "russki",
            "russkie",
            "sadis",
            "sadom",
            "samckdaddy",
            "sandm",
            "sandnigger",
            "satan",
            "scag",
            "scallywag",
            "scat",
            "schlong",
            "screw",
            "screwyou",
            "scrotum",
            "scum",
            "semen",
            "seppo",
            "servant",
            "sexed",
            "sexfarm",
            "sexhound",
            "sexhouse",
            "sexing",
            "sexkitten",
            "sexpot",
            "sexslave",
            "sextogo",
            "sextoy",
            "sextoys",
            "sexual",
            "sexually",
            "sexwhore",
            "sexy",
            "sexymoma",
            "sexy-slim",
            "shag",
            "shaggin",
            "shagging",
            "shat",
            "shav",
            "shawtypimp",
            "sheeney",
            "shhit",
            "shinola",
            "shit",
            "shitcan",
            "shitdick",
            "shite",
            "shiteater",
            "shited",
            "shitface",
            "shitfaced",
            "shitfit",
            "shitforbrains",
            "shitfuck",
            "shitfucker",
            "shitfull",
            "shithapens",
            "shithappens",
            "shithead",
            "shithouse",
            "shiting",
            "shitlist",
            "shitola",
            "shitoutofluck",
            "shits",
            "shitstain",
            "shitted",
            "shitter",
            "shitting",
            "shitty ",
            "shoot",
            "shooting",
            "shortfuck",
            "showtime",
            "sick",
            "sissy",
            "sixsixsix",
            "sixtynine",
            "sixtyniner",
            "skank",
            "skankbitch",
            "skankfuck",
            "skankwhore",
            "skanky",
            "skankybitch",
            "skankywhore",
            "skinflute",
            "skum",
            "skumbag",
            "slant",
            "slanteye",
            "slapper",
            "slaughter",
            "slav",
            "slave",
            "slavedriver",
            "sleezebag",
            "sleezeball",
            "slideitin",
            "slime",
            "slimeball",
            "slimebucket",
            "slopehead",
            "slopey",
            "slopy",
            "slut",
            "sluts",
            "slutt",
            "slutting",
            "slutty",
            "slutwear",
            "slutwhore",
            "smack",
            "smackthemonkey",
            "smut",
            "snatch",
            "snatchpatch",
            "snigger",
            "sniggered",
            "sniggering",
            "sniggers",
            "snigger's",
            "sniper",
            "snot",
            "snowback",
            "snownigger",
            "sodom",
            "sodomise",
            "sodomite",
            "sodomize",
            "sodomy",
            "sonofabitch",
            "sonofbitch",
            "sooty",
            "soviet",
            "spaghettibender",
            "spaghettinigger",
            "spank",
            "spankthemonkey",
            "sperm",
            "spermacide",
            "spermbag",
            "spermhearder",
            "spermherder",
            "spic",
            "spick",
            "spig",
            "spigotty",
            "spik",
            "spit",
            "spitter",
            "splittail",
            "spooge",
            "spreadeagle",
            "spunk",
            "spunky",
            "squaw",
            "stagg",
            "stiffy",
            "strapon",
            "stringer",
            "stripclub",
            "stroke",
            "stroking",
            "stupid",
            "stupidfuck",
            "stupidfucker",
            "suck",
            "suckdick",
            "sucker",
            "suckme",
            "suckmyass",
            "suckmydick",
            "suckmytit",
            "suckoff",
            "suicide",
            "swallow",
            "swallower",
            "swalow",
            "swastika",
            "sweetness",
            "syphilis",
            "taboo",
            "taff",
            "tampon",
            "tang",
            "tantra",
            "tarbaby",
            "tard",
            "teat",
            "terror",
            "terrorist",
            "teste",
            "testicle",
            "testicles",
            "thicklips",
            "thirdeye",
            "thirdleg",
            "threesome",
            "threeway",
            "timbernigger",
            "tinkle",
            "titbitnipply",
            "titfuck",
            "titfucker",
            "titfuckin",
            "titjob",
            "titlicker",
            "titlover",
            "tits",
            "tittie",
            "titties",
            "titty",
            "toilet",
            "tongethruster",
            "tongue",
            "tonguethrust",
            "tonguetramp",
            "tortur",
            "torture",
            "tosser",
            "towelhead",
            "trailertrash",
            "tramp",
            "trannie",
            "tranny",
            "transexual",
            "transsexual",
            "transvestite",
            "triplex",
            "trisexual",
            "trojan",
            "trots",
            "tuckahoe",
            "tunneloflove",
            "turd",
            "turnon",
            "twat",
            "twink",
            "twinkie",
            "twobitwhore",
            "unfuckable",
            "upskirt",
            "uptheass",
            "upthebutt",
            "urinary",
            "urinate",
            "urine",
            "usama",
            "uterus",
            "vagina",
            "vaginal",
            "vatican",
            "vibr",
            "vibrater",
            "vibrator",
            "vietcong",
            "violence",
            "virgin",
            "virginbreaker",
            "vomit",
            "vulva",
            "wank",
            "wanker",
            "wanking",
            "waysted",
            "weapon",
            "weenie",
            "weewee",
            "welcher",
            "welfare",
            "wetb",
            "wetback",
            "wetspot",
            "whacker",
            "whash",
            "whigger",
            "whiskey",
            "whiskeydick",
            "whiskydick",
            "whit",
            "whitenigger",
            "whites",
            "whitetrash",
            "whitey",
            "whiz",
            "whop",
            "whore",
            "whorefucker",
            "whorehouse",
            "wigger",
            "willie",
            "williewanker",
            "willy",
            "women's",
            "wuss",
            "wuzzie",
            "yankee",
            "yellowman",
            "zigabo",
            "zipperhead",
        };

        string[] smallBadWords = new string[]
        {
            "abo",
            "ass",
            "bi",
            "bra",
            "cum",
            "die",
            "dix",
            "ero",
            "evl",
            "fag",
            "fat",
            "fok",
            "fu",
            "fuc",
            "fuk",
            "gay",
            "gin",
            "gob",
            "god",
            "goy",
            "gun",
            "gyp",
            "hiv",
            "ho",
            "jap",
            "jew",
            "kid",
            "kkk",
            "kum",
            "ky",
            "lez",
            "lsd",
            "mad",
            "nig",
            "nip",
            "pee",
            "pom",
            "poo",
            "pot",
            "pud",
            "sex",
            "sob",
            "sos",
            "tit",
            "tnt",
            "uck",
            "uk",
            "wab",
            "wn",
            "wog",
            "wop",
            "wtf",
            "xtc",
            "xxx",
        };

        //whitelist
        string[] whiteList = new string[]
        {
            "something", "somethings", "somethang", "somethange", "zomething", "methane", "aholehole", "aktashite", "assapanick", "assart", "bastinado", "boobyalla", "bum-bailiff", "bumfiddler", "bummalo", "clatterfart", "cockapert", "cock-bell", "cockchafer", "dik-dik", "dreamhole", "fanny-blower", "fartlek", "fuksheet", "gullgroper", "haboob", "humpenscrump", "invagination", "jaculate", "jerkinhead", "knobstick", "kumbang", "lobcocked", "nestle-cock", "nicker-pecker", "nobber", "nodgecock", "pakapoo", "peniaphobia", "penistone", "pershittie", "pissaladière", "pissasphalt", "poonga", "sack-butt", "sexagesm", "sexangle", "sexfoiled", "shittah", "skiddy-cock", "slagger", "tease-hole", "tetheradick", "tit-bore", "tit-tyrant", "wankapin"
        };

        //index[0] is the leet text, index[1] is the translated character
        string[,] lettersReplacedWithLetters = new string[,]
        {
            { "aye", "a" }, { "cl", "d" } , { "ph", "f" } , { "gee", "g" } , { "eye", "i" } , { "nn", "m" } , { "iti", "m" } , { "jti", "m" } , { "oh", "o" } , { "p", "o" } , { "z", "s" } , { "ehs", "s" } , { "es", "s" } , { "uu", "w" } , { "ecks", "x" } , { "j", "y" } , { "s" , "z" } , {"q", "o"}
        };

        string[,] numbersAndWithLetters = new string[,]
        {
            { "4", "a" } , { "8", "b" } , { "13", "b" } , { "3", "e" } , { "0", "o" } , { "12", "r" } , { "5", "s" } , { "i3", "b" } , { "j3", "b" } , { "i7", "d" } , { "3y3", "i" } , { "i2", "r" } , { "2u", "w" }
        };

        string[,] symbolsAndWithLetters = new string[,]
        {
            { @"/\", "a" } , { @"@", "a" } , { @"(l", "a" } , { @"Д", "a" } , { @"ß", "b" } , { @"|-]", "b" } , { @"[", "c" } , { @"¢", "c" } , { @"{", "c" } , { @"<", "c" } , { @"(", "c" } , { @"©", "c" } , { @")", "d" } , { @"|)", "d" } , { @"(|", "d" } , { @"[)", "d" } , { @"i>", "d" } , { @"t)", "d" } , { @"|}", "d" } , { @">", "d" } , { @"|]", "d" } , { @"€", "e" } , { @"ë", "e" } , { @"[-", "e" } , { @"|=-", "e" } , { @"|=", "f" } , { @"ƒ", "f" } , { @"|#", "f" } , { @"/=", "f" } , { @"(_+", "g" } , { @"c-", "g" } , { @"(?,", "g" } , { @"[,", "g" } , { @"{,", "g" } , { @"<-", "g" } , { @"(.", "g" } , { @"#", "h" } , { @"/-/", "h" } , { @"[-]", "h" } , { @"]-[", "h" } , { @")-(", "h" } , { @"(-)", "h" } , { @":-:", "h" } , { @"|~|", "h" } , { @"|-|", "h" } , { @"]~[", "h" } , { @"!-!", "h" } , { @"\-/", "h" } , { @"i+i", "h" } , { @"!", "i" } , { @",_|", "j" } , { @"_|", "j" } , { @"._|", "j" } , { @"._]", "j" } , { @"_]", "j" } , { @",_]", "j" } , { @"]", "j" } , { @";", "j" } , { @">|", "k" } , { @"|<", "k" } , { @"/<", "k" } , { @"1<", "k" } , { @"|c", "k" } , { @"|(", "k" } , { @"|{", "k" } , { @"|_", "l" } , { @"/\/\", "m" } , { @"/V\", "m" } , { @"[v]", "m" } , { @"[]v[]", "m" } , { @"|\/|", "m" } , { @"^^", "m" } , { @"<\/>", "m" } , { @"{v}", "m" } , { @"(v)", "m" } , { @"|v|", "m" } , { @"|\|\", "m" } , { @"]\/[", "m" } , { @"^/", "n" } , { @"|\|", "n" } , { @"/\/", "n" } , { @"[\]", "n" } , { @"<\>", "n" } , { @"{\}", "n" } , { @"|v", "n" } , { @"/v", "n" } , { @"И", "n" } , { @"ท", "n" } , { @"()", "o" } , { @"<>", "o" } , { @"Ø", "o" } , { @"|*", "p" } , { @"|o", "p" } , { @"|º", "p" } , { @"|" + '\u0022', "p" } , { @"[]d", "p" } , { @"|°", "p" } , { @"(_,)", "q" } , { @"()_", "q" } , { @"0_", "q" } , { @"<|", "q" } , { @"|`", "r" } , { @"|~", "r" } , { @"|?", "r" } , { @"®", "r" } , { @"[z", "r" } , { @"Я", "r" } , { @".-", "r" } , { @"|-", "r" } , { @"$", "s" } , { @"§", "s" } , { @"+", "t" } , { @"-|-", "t" } , { @"']['", "t" } , { @"†", "t" } , { '\u0022' + @"|" + '\u0022', "t" } , { @"~|~", "t" } , { @"(_)", "u" } , { @"|_|", "u" } , { @"l|", "u" } , { @"µ", "u" } , { @"บ", "u" } , { @"\/", "v" } , { @"|/", "v" } , { @"\|", "v" } , { @"\/\/", "w" } , { @"\n", "w" } , { @"'//", "w" } , { @"\\'", "w" } , { @"\^/", "w" } , { @"(n)", "w" } , { @"\v/", "w" } , { @"\x/", "w" } , { @"\_|_/", "w" } , { @"\_:_/", "w" } , { @"Ш", "w" } , { @"Щ", "w" } , { @"\\//\\//", "w" } , { @"พ", "w" } , { @"v²", "w" } , { @"><", "x" } , { @"Ж", "x" } , { @"×", "x" } , { @")(", "x" } , { @"`/", "y" } , { @"Ч", "y" } , { @"¥", "y" } , { @"\//", "y" } , { @"-/_", "z" } , { @"%", "z" } , { @">_", "z" } , { @"~/_", "z" } , { @"-\_", "z" } , { @"-|_", "z" }
        };

        string[,] symbolsWithNumbers = new string[,]
        {
            { @"|3", "b" } , { @"!3", "b" } , { @"(3", "b" } , { @"/3", "b" } , { @")3", "b" } , { @"1-1", "h" } , { @"1<", "k" } , { @"1^1", "m" } , { @"|7", "p" } , { @"0_", "q" } , { @"/2", "r" } , { @"|9", "r" } , { @"|2", "r" } , { @"7_", "z" }
        };


        //some leet text has multiple meanings
        string[,] symbolTriplets = new string[2, 4]
        {
            { @"?", "d", "p", "x" } , { @"&", "e", "g", "q" }
        };

        string[,] symbolDoubles = new string[10, 3]
        {
             {@"[]", "i", "o"} , { @"][", "i", "t"} , {@"}{", "x", "h" } , {@"/-\", "h", "a" } , {@"\|/", "w", "y" } , {@"^" , "n", "a" } , {@"|", "i", "l" } , {@"|^", "p", "r"} , {@"|>", "p", "d"} , {"£", "l", "e" }
        };

        string[,] numberQuadruplets = new string[1, 5]
        {
            {"2", "q", "r", "s", "z"}
        };

        string[,] numberTriplets = new string[3, 4]
        {
            { "1", "i", "j", "l" } , {"7", "l", "t", "y"} , {"9", "g", "p", "q"}
        };

        string[,] numberDoubles = new string[1, 3]
        {
            { "6", "b", "g" }
        };

        string[,] letterDoubles = new string[1, 3]
        {
            { "v", "f", "u" }
        };

        public bool HasProfanity(string rawInputTxt)
        {
            //make input text lower case
            rawInputTxt = rawInputTxt.ToLower();
            List<string> inputTxt = rawInputTxt.Split(' ').ToList();
            bool hasBadWord = false;


            //add all the character variations to check against the inputTxt
            List<string[,]> questionableCharsList = new List<string[,]>()
            {
                lettersReplacedWithLetters,
                numbersAndWithLetters,
                symbolsAndWithLetters,
                symbolsWithNumbers,
                symbolTriplets,
                symbolDoubles,
                numberQuadruplets,
                numberTriplets,
                numberDoubles,
                letterDoubles
            };


            //remove any whitelisted words from being checked
            for (int i = 0; i < inputTxt.Count; i++)
            {
                foreach (string goodWord in whiteList)
                {
                    if (inputTxt[i] == goodWord)
                    {
                        inputTxt.Remove(inputTxt[i]);
                    }
                }
            }

            //do a quick initial check
            if (CheckForBadWord(inputTxt))
            {
                hasBadWord = true;
            }

            //get all the possible "leet" characters that could have been used
            List<string[]> lettersReplacedList = QuestionableWords(inputTxt, questionableCharsList);

            //reverse the possible leet text against every combination that it could be used
            List<string> allCombinationList = ReverseLeetText(lettersReplacedList);

            //check newely made words against known blacklist
            if (CheckForBadWord(allCombinationList))
            {
                hasBadWord = true;
            }

            return hasBadWord;
        }

        List<string[]> QuestionableWords(List<string> inputTxt, List<string[,]> questionableChars)
        {
            //split text into words which are separated by a space
            List<string[]> questionableWordsList = new List<string[]>();
            questionableWordsList.Clear();

            //for every word get all the possible leet characters and get there indexes
            for (int w = 0; w < inputTxt.Count; w++)
            {
                for (int qCharsIndex = 0; qCharsIndex < questionableChars.Count; qCharsIndex++)
                {
                    //loop through each potentially bad char
                    for (int i = 0; i < questionableChars[qCharsIndex].GetLength(0); i++)
                    {
                        AddSusCharArray(ref questionableWordsList, inputTxt[w], inputTxt[w], questionableChars, qCharsIndex, i);
                    }
                }
            }
            return questionableWordsList;
        }

        List<string> ReverseLeetText(List<string[]> questionableWords)
        {
            List<string> wordList = new List<string>();
            List<string> allWordCombinations = new List<string>();

            //get all words
            for (int i = 0; i < questionableWords.Count; i++)
            {
                if (!wordList.Contains(questionableWords[i][0]))
                {
                    wordList.Add(questionableWords[i][0]);
                }
            }

            //iterate through words
            for (int w = 0; w < wordList.Count; w++)
            {
                //create a list specific for current word
                List<string[]> currentWord = new List<string[]>();
                questionableWords.ForEach(x =>
                {
                    if (x[0] == wordList[w])
                    {
                        currentWord.Add(x);
                    }
                });

                //create a dictionary to make a list from 0 - combinationQty to lookup the currentWord's char to work with
                ConcurrentDictionary<int, int[]> dic = new ConcurrentDictionary<int, int[]>();
                int charCount = 0;
                for (int i = 0; i < currentWord.Count; i++)
                {
                    for (int j = 3; j < currentWord[i].GetLength(0); j++)
                    {
                        dic.TryAdd(charCount, new int[] { i, j });
                        charCount++;
                    }
                }
                //need to join multiple words and return that
                allWordCombinations.AddRange(GetCombination(currentWord, dic));
            }

            return allWordCombinations;
        }

        List<string> GetCombination(List<string[]> currentWord, ConcurrentDictionary<int, int[]> dict)
        {
            //use a concurrentbag for thread safety instead of a List
            ConcurrentBag<string> reversedCombinations = new ConcurrentBag<string>();
            ConcurrentBag<string> combinations = new ConcurrentBag<string>();

            //iterate through all combinations https://stackoverflow.com/questions/7802822/all-possible-combinations-of-a-list-of-values
            //count needs to be an int to work with Parallel For loop
            int count = (int)Math.Pow(2, dict.Count);
            Parallel.For(1, count, i =>
            {
                combinations.Add(Convert.ToString(i, 2).PadLeft(dict.Count, '0'));
            });

            //go through all combinations and replace words in a multi-threaded fasion
            Parallel.ForEach(combinations, c =>
            {
                List<int[]> changes = new List<int[]>();
                List<char> newWord = new List<char>();
                newWord.AddRange(currentWord[0][0]);

                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == '1')
                    {
                        //replace a possible leet character with a translation
                        ReplaceCharacters(currentWord, ref newWord, dict, ref changes, i);
                    }

                    //once the character changes completes a word add to a list
                    if (i >= c.Length - 1)
                    {
                        reversedCombinations.Add(new string(newWord.ToArray()));
                    }
                }
            });

            //return all translated words
            return reversedCombinations.ToList();
        }

        void ReplaceCharacters(List<string[]> currentWord, ref List<char> newWord, ConcurrentDictionary<int, int[]> dict, ref List<int[]> changes, int j)
        {
            int charIndex = int.Parse(currentWord[dict[j][0]][1]);
            string currentCharacters = currentWord[dict[j][0]][2].ToString();
            string newCharacters = currentWord[dict[j][0]][dict[j][1]].ToString();

            //find all prior changes to the word which may affect the index of the change we are about to make
            List<int[]> changesBeforeChar = changes.FindAll(x => x[0] < charIndex);

            //if we found something apply those changes to the character we are about to place
            if (changesBeforeChar.Any())
            {
                changesBeforeChar.ForEach(x =>
                {
                    charIndex += x[1];
                });
            }


            //check if word contains the characters that it wants to remove
            if (new string(newWord.ToArray()).Contains(currentCharacters))
            {
                //remove old character and place new one
                newWord.RemoveRange(charIndex, currentCharacters.Length);
                newWord.InsertRange(charIndex, newCharacters);
            }


            //after changes, record the change for new characters in this combination
            int wordChangeAmount = newCharacters.Length - currentCharacters.Length;
            if (wordChangeAmount != 0)
            {
                int[] change = { charIndex, wordChangeAmount };
                changes.Add(change);
            }

        }

        void AddSusCharArray(ref List<string[]> questionableWordsList, string originalWord, string modifiedWord, List<string[,]> questionableChars, int qCharsIndex, int index)
        {
            string subString = questionableChars[qCharsIndex][index, 0];
            int subStringIndex = modifiedWord.IndexOf(subString);
            if (subStringIndex != -1)
            {
                //get index of possible bad character(s)
                int ActualSubStringIndex = (originalWord.Length - modifiedWord.Length) + modifiedWord.IndexOf(subString);

                //front = the word and the index of bad char
                string[] front = { originalWord, ActualSubStringIndex.ToString() };
                //back = the leet char and the char it translates to
                string[] back = Enumerable.Range(0, questionableChars[qCharsIndex].GetLength(1)).Select(x => questionableChars[qCharsIndex][index, x]).ToArray();
                //result combines the front/back
                string[] result = front.Concat(back).ToArray();
                questionableWordsList.Add(result);

                //trim the word and see if bad char appears again using recursion, if so then repeat the above process
                string shortenedWord = modifiedWord.Remove(0, subStringIndex + subString.Length);
                AddSusCharArray(ref questionableWordsList, originalWord, shortenedWord, questionableChars, qCharsIndex, index);
            }
        }

        bool CheckForBadWord(List<string> inputTxt)
        {
            //remove any duplicate words
            inputTxt = inputTxt.Distinct().ToList();
            bool hasBadWord = false;

            //check big strings for bigger bad words
            for (int i = 0; i < badWords.Length; i++)
            {
                //use single or multithreading depending on the work load
                //testing shows that single threaded is better for smaller loads
                if (inputTxt.Count < 8200)
                {
                    foreach (string x in inputTxt)
                    {
                        if (x.Length > 3)
                        {
                            if (x.Contains(badWords[i]))
                            {
                                hasBadWord = true;
                            }
                        }
                    };
                }
                else
                {
                    Parallel.ForEach(inputTxt, x =>
                    {
                        if (x.Contains(badWords[i]))
                        {
                            hasBadWord = true;
                        }
                    });
                }
            }

            //check small strings for small bad words
            for (int i = 0; i < smallBadWords.Length; i++)
            {
                foreach (string x in inputTxt)
                {
                    if (x.Length < 4)
                    {
                        foreach (string smallBadWord in smallBadWords)
                        {
                            if (x == smallBadWord)
                            {
                                hasBadWord = true;
                            }
                        }
                    }
                };
            }
            return hasBadWord;
        }
    }
}