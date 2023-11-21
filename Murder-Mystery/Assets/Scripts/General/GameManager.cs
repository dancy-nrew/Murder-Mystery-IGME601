using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Lists to hold characters and clues
    public List<Character> characters = new List<Character>();
    public List<Clue> clues = new List<Clue>();
    public List<CharacterSO> characterList = new List<CharacterSO>();
    public int flashMessageDuration;
    private Dictionary<CharacterSO.ECharacter, CharacterSO> characterDict = new Dictionary<CharacterSO.ECharacter, CharacterSO>();
    public TextMeshProUGUI FlashMessageObject;
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
    }

    private void Start()
    {
       foreach(CharacterSO character in characterList)
        {
            characterDict.Add(character.character, character);
        }
    }
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

    public CharacterSO GetCharacterSOFromKey(CharacterSO.ECharacter key)
    {
        return characterDict[key];
    }

    public void FlashMessage(string message)
    {
        if (FlashMessageObject == null)
        {
            // Container for message not found. Post to Debug Log for now. This means there's a bug in this scene.
            Debug.Log(message);
            return;
        }
        FlashMessageObject.text = message;
        FlashMessageObject.gameObject.SetActive(true);
        StartCoroutine(ShowMessage(FlashMessageObject.gameObject));

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
    
}
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