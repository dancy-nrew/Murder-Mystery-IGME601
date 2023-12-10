using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Lists to hold characters and clues
    public List<Character> characters = new List<Character>();
    public List<Clue> clues = new List<Clue>();
    public List<CharacterSO> characterList = new List<CharacterSO>();
    public int flashMessageDuration;
    public List<string> trueOnStart = new List<string>();
    private Dictionary<CharacterSO.ECharacter, CharacterSO> characterDict = new Dictionary<CharacterSO.ECharacter, CharacterSO>();
    public TextMeshProUGUI FlashMessageObject;

    public bool _shouldLoadFromSavedLocation = false;
    //Default this to the origin
    private Vector3 _lastPlayerLocation = new Vector3(0,0,0);
    
    //Default this to Ace;
    public CharacterSO.ECharacter _lastTalkedToCharacter;
    private int _lastVisitedScene;

    public bool _justWalkedThroughDoor = false;
    public string _doorKey;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /*
        The following are fabricated suspects for the purpose of testing:
        ----
        ----
        
        
        Character Columbo = new Character("[REDACTED] Culombo", 
        "Lieutenant Columbo is as an unassuming yet brilliant homicide detective working with the Los Angeles Police Department. Known for his disheveled appearance, often seen in a rumpled raincoat, he possesses an unorthodox approach to solving crimes. His seemingly absent-minded demeanor hides a keen analytical mind and a talent for observing minute details that others often overlook. Columbo is polite and often comes across as naive, frequently saying, \"Just one more thing...\" before catching suspects off-guard with a critical question or observation. Despite his casual, almost chaotic methods, he has a razor-sharp intellect and an unwavering dedication to justice. He's adept at lulling suspects into a false sense of security, leading them to underestimate him—a tactic that inevitably leads to their undoing.");
        Texture2D image = Resources.Load<Texture2D>("CharacterPortraits/Portrait_columbo");
        Columbo.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

        Character Sherlock = new Character("Sherlock Holmes", 
        "Sherlock Holmes, a master detective, resides at the iconic 221B Baker Street in London. Renowned for his extraordinary deductive reasoning and sharp observational skills, he is a tall, lean figure, often seen in tweed suits and a deerstalker hat. Holmes boasts a broad knowledge across various fields, with a particular mastery in the art of deduction. Although he can be enigmatic and introspective, his partnership with Dr. John H. Watson is a testament to his deeper layers. His unorthodox methods and unwavering commitment to justice have cemented his legendary status in the world of criminal investigation.");
        image = Resources.Load<Texture2D>("CharacterPortraits/portrait_sherlock");
        Sherlock.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

        Character Phoenix = new Character("Phoenix Wright",
        "Phoenix Wright, a defense attorney based in Los Angeles, is known for his tenacity in court and his iconic spiky black hair and blue suit. A graduate of Ivy University, Wright is recognized for his dramatic courtroom tactics and his famous catchphrase, \"Objection!\" Despite facing career challenges, his unwavering commitment to justice and strong sense of morality make him a respected figure in the legal community.");
        image = Resources.Load<Texture2D>("CharacterPortraits/portrait_phoenix");
        Phoenix.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

        Character Nancy = new Character("Nancy Drew",
        "Nancy Drew is a distinguished figure in the realm of amateur sleuthing, having garnered widespread recognition for her remarkable investigative skills. Born and raised in the fictional town of River Heights, Nancy's curiosity and intelligence were evident from a young age. She possesses an innate knack for unraveling complex mysteries, a trait she likely inherited from her father, Carson Drew, a prominent attorney."
        +"\nStanding tall with an athletic build, Nancy exudes confidence and poise. Her striking strawberry-blonde hair and expressive blue eyes often leave a lasting impression on those she encounters. Beyond her physical attributes, it's her sharp intellect, unwavering determination, and remarkable bravery that truly set her apart."
        +"\nNancy's educational background includes a comprehensive study in diverse subjects, though specifics about her schooling are not prominently highlighted. However, her extensive knowledge in various fields suggests a well-rounded and thorough education."
        +"\nDespite the potential dangers associated with her investigative endeavors, Nancy approaches each case with a level head and a compassionate heart. Her strong sense of justice drives her to seek the truth and assist those in need, making her not only an exceptional detective but also a cherished friend and community member."); 
        image = Resources.Load<Texture2D>("CharacterPortraits/portrait_nancy");
        Nancy.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

        characters.Add(Sherlock);
        characters.Add(Columbo);
        
        //characters.Add(Phoenix);
        //characters.Add(Nancy);
        */

        /*
        Manually adding the charcaters into the Journal data. This could defintely be done in a much more efficient/modular manner
        but given our time constraints and small amount of characters this should suffice.
        */

        Texture2D image;

        Character Quincy = new Character("Dr. Quincy Reeves", "A blunt and poor-tempered mathematician that cares deeply about the Math Department.");
        image = Resources.Load<Texture2D>("CharacterPortraits/ReevesSprite");
        Quincy.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        Quincy.motive.Add("bMathDepartmentLeadership");
        Quincy.motive.Add("Has a grudge against Dr Yates for being promoted to Department Chair. Some people just can’t let things go.");
        Quincy.witness.Add("bPaineRevelation");
        Quincy.witness.Add("Miss Paine heard Dr Reeves and Dr Yates fighting on Saturday. It sounded violent.");
        Quincy.location.Add("bHasPickedUpVase");
        Quincy.location.Add("The broken vase puts him at the scene of the crime.");

        Character Guillermo = new Character("Dr. Guillermo Stone", "A nervous mathematician who worked closely with Dr Yates in the past. He claims he was home alone all weekend.");
        image = Resources.Load<Texture2D>("CharacterPortraits/StoneSprite");
        Guillermo.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        Guillermo.motive.Add("bStolenResearch");
        Guillermo.motive.Add("Believes that Dr Yates stole research from him.");
        Guillermo.witness.Add("bReevesRevelation");
        Guillermo.witness.Add("Dr. Reeves overheard Dr Stone and Dr Yates arguing on Friday.");
        Guillermo.location.Add("bHasPickedUpAward");
        Guillermo.location.Add("The stolen award means he was at the scene of the crime.");

        Character Julietta = new Character("Dr. Julietta Morse","A physicist working in the Math Department who was friends with Dr. Yates.");
        image = Resources.Load<Texture2D>("CharacterPortraits/MorseSprite");
        Julietta.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        Julietta.motive.Add("bBrokenHeart");
        Julietta.motive.Add("She and Dr Yates were in a relationship that imploded.");
        Julietta.witness.Add("bConnorRevelation");
        Julietta.witness.Add("Dr Morse asked him for the key to the maintenance closet on Saturday. Seems weird that she wanted cleaning supplies the day after she cleaned her office.");
        Julietta.location.Add("bHasPickedUpModel");
        Julietta.location.Add("The mangled model shows that she was at the scene of the crime.");

        Character Wynona = new Character("Wynona Paine","One of Dr. Yates\' most promising PhD students in mathematics.");
        image = Resources.Load<Texture2D>("CharacterPortraits/PaineSprite");
        Wynona.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        
        Character Leo = new Character("Leo Connor", "Dr. Yates\'  former teaching assistant");
        image = Resources.Load<Texture2D>("CharacterPortraits/ConnorSprite");
        Leo.Portrait = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        Leo.motive.Add("bAlwaysTrue");
        Leo.motive.Add("He hated Dr Yates for firing him from his TA position.");
        Leo.witness.Add("bAlwaysTrue");
        Leo.witness.Add("Miss Paine saw him in Cantor hall on Sunday");
        Leo.location.Add("bAlwaysTrue");
        Leo.location.Add("The unfinished homework puts him at the scene of the crime.");

        characters.Add(Quincy);
        characters.Add(Guillermo);
        characters.Add(Julietta);
        characters.Add(Wynona);
        characters.Add(Leo);

    }

    private void Start()
    {
       foreach(CharacterSO character in characterList)
        {
            characterDict.Add(character.character, character);
        }
    }

    #region Journal Related Functions
    public string GetCharacterInfo(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            return characters[pageIndex].getInfo();
        }
        return "...";
    }

    public string GetCharacterName(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            return characters[pageIndex].getName();
        }
        return "???";
    }

    public Sprite GetCharacterSprite(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            return characters[pageIndex].getPortrait();
        }
        return null;
    }

    public string GetCharacterMotive(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            if (characters[pageIndex].motive.Count > 1)
            {
                if (DialogueDataWriter.Instance.CheckCondition(characters[pageIndex].motive[0], true))
                {
                    return "Motive: " + characters[pageIndex].motive[1];
                }
                else
                {
                    return "Motive: __________";
                }
            }
            else
            {
                return "";
            }
        }
        return null;
    }

    public string GetCharacterWitness(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            if (characters[pageIndex].witness.Count > 1)
            {
                if (DialogueDataWriter.Instance.CheckCondition(characters[pageIndex].witness[0], true))
                {
                    return "Witnesses: " + characters[pageIndex].witness[1];
                }
                else
                {
                    return "Witnesses: __________";
                }
            }
            else
            {
                return "";
            }
        }
        return null;
    }

    public string GetCharacterLocation(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < characters.Count)
        {
            if (characters[pageIndex].location.Count > 1)
            {
                if (DialogueDataWriter.Instance.CheckCondition(characters[pageIndex].location[0], true))
                {
                    return "Location: " + characters[pageIndex].location[1];
                }
                else
                {
                    return "Location: __________";
                }
            }
            else
            {
                return "";
            }
        }
        return null;
    }

    public string GetClueName(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < clues.Count)
        {
            return clues[pageIndex].getName();
        }
        return "???";
    }

    public string GetClueInfo(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < clues.Count)
        {
            return clues[pageIndex].getInfo();
        }
        return "...";
    }

    public Sprite GetClueSketch(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < clues.Count)
        {
            return clues[pageIndex].getSketch();
        }
        return null;
    }

    public void AddClue(Clue newClue)
    {
        Debug.Log(newClue.Name);
        // Avoid adding duplicates
        if (!clues.Exists(c => c.Name == newClue.Name))
        {
            clues.Add(newClue);
        }
    }

    public void AppendToCharacterDescription(String name, String append)
    {
        foreach(Character Suspect in characters)
        {
            if(Suspect.Name.Equals(name))
            {
                if(!Suspect.Description.Contains(append))
                {
                    Suspect.Description += "\n" + append;   // upon testing, the Collect() function seemed to be called multiple times. this is to prevent duplicate adding
                }
            }
        }
    }
    #endregion

    public CharacterSO GetCharacterSOFromKey(CharacterSO.ECharacter key)
    {
        return characterDict[key];
    }


    public void FlashMessage(string message)
    {
        GameObject FlashMessageGameObject = GameObject.FindGameObjectWithTag("MsgBox");
        if (FlashMessageGameObject)
        {
            TextMeshProUGUI FlashMessageText = FlashMessageGameObject.GetComponent<TextMeshProUGUI>();
            if (FlashMessageText)
            {
                FlashMessageGameObject.SetActive(true);
                FlashMessageText.text = message;
                StartCoroutine(ShowMessage(FlashMessageGameObject));
            }
        }
    }

    IEnumerator ShowMessage(GameObject go)
    {
        while (true)
        {
            yield return new WaitForSeconds(flashMessageDuration);
            go.SetActive(false);
            yield break;
        }
    }

    public void StorePlayerLastLocation()
    {
        // Store the player's location as Vector3 in case we want to load in the future
        _lastPlayerLocation = GameObject.Find("Player").transform.position;
    }

    public Vector3 LoadPlayerPosition()
    {
        //This should only happen when the card battler ends, so we should unset the boolean here
        _shouldLoadFromSavedLocation = false;
        return _lastPlayerLocation;
    }

    public void SetLastTalkedTo(CharacterSO.ECharacter chr)
    {
        // Keep a record of who we just talked to. We'll need it for the card battler stuff
        _lastTalkedToCharacter = chr;
    }

    public CharacterSO.ECharacter GetLastTalkedTo()
    {
        return _lastTalkedToCharacter;
    }

    public void SetLastVisitedScene(int index)
    {
        // Set last visited scene as index. Useful for loading back into the exploration
        // after card battler ends
        _lastVisitedScene = index;
    }

    public int LoadLastVisitedScene()
    {
        int sceneToVisit = _lastVisitedScene;

        // Flush out the last saved value
        _lastVisitedScene = 0;

        return sceneToVisit;
    }

    public void SaveDoorInfo(string doorKey)
    {
        _doorKey = doorKey;
        _justWalkedThroughDoor = true;
    }

    public string GetDoorInfo()
    {
        _justWalkedThroughDoor = false;
        return _doorKey;
    }

    public void ResetGameState()
    {
        foreach(var parameter in DialogueDataWriter.Instance.GetParameters())
        {
            foreach(string setToTrue in trueOnStart)
            {
                if (parameter.parameterKey.Equals(setToTrue))
                {
                    Debug.Log("Setting " + setToTrue);
                    DialogueDataWriter.Instance.UpdateDialogueData(parameter.parameterKey, true);
                }
                else
                {
                    DialogueDataWriter.Instance.UpdateDialogueData(parameter.parameterKey, false);
                }
            }
        }
    }
    
}
#region Journal related classes
public class Character
{
    public Character(String name, String desc)
    {
        Name = name;
        Description = desc;
    }
    public string Name;
    public string Description;
    public Sprite Portrait;
    public List<string> motive = new List<string>();
    public List<string> witness = new List<string>();
    public List<string> location = new List<string>();

    public override string ToString()
    {
        return "Name: " + Name + "\nDescription: " + Description;
    }
    public string getName()
    {
        return Name;
    }
    public string getInfo()
    {
        return Description;
    }
    
    public Sprite getPortrait()
    {
        return Portrait;
    }
}

public class Clue
{
    public string Name;
    public string Description;
    public Sprite Sketch;
    
    public string getName()
    {
        return Name;
    }
    public string getInfo()
    {
        return Description;
    }
    
    public Sprite getSketch()
    {
        return Sketch;
    }

    public override string ToString()
    {
        return "Name: " + Name;
    }
}
#endregion