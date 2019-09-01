using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* *************************************************************************************
 *      For:                        
 *       - "casting" phase
 *              - animations showing player they can say a spell appears
 *              - spells that they can say appears on left side of screen
 *              - magic circle appears/disappears below Vivi's feet
 *       - when casting a spell
 *              - magic circle appears/disappears below Vivi's feet
 *              - animations showing player they can say a spell disappears
 *              - spells that they can say disappears from left side of screen
 *              
**************************************************************************************** */

public class UseSpell : MonoBehaviour {

    //For Keyword: Cast ("casting" phase)
   
    //Black gradient covers screen
    public GameObject castDarkness;
    //Moon appears above
    public GameObject castMoon;

    //For Keyword: Vivi ("moving" phase)

    //Speech Bubble appears above Vivi
    public GameObject viviKeyword;
    //Black gradient covers screen
    public GameObject gradient;
    //Hints of available words appear above Vivi
    public GameObject viviKeywordUi;
    
    //Animation during the phases, and timer that counts the amount of time the player has to say the next keyword
    private Animator anim;
    private float clock;
    private float moveClock;

    //Checks if Vivi can cast a spell
    public bool Cast; //Animator
    //Checks if Vivi can move
    public bool Move;
    public bool Active;
    //Checks for animations
    private bool animFlag;

    //For spells that the player can say that shows on the left side
    bool rolling;
    bool rolledOut;
    bool rollingIn;
    //List of spell gameobjects used
    public List<GameObject> spells;

    //grants access to the speech recognition software
    SpeechRecognition01 speech;
    //used to access Vivi, the water/ice animations and particle systems
    GameObject vivi;
    //used to access the position of the UI
    Vector3 uiSize;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        animFlag = false;
        vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        viviKeyword.SetActive(false);
        gradient.SetActive(false);
        uiSize = viviKeywordUi.transform.localScale;
        viviKeywordUi.transform.localScale = new Vector3(0f,0f,0f);
        Active = true;
        rolling = false;
        rolledOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        vivi.GetComponent<Animator>().SetBool("Cast", Cast);
        Cast = false;
        //Controlling movement state feedback
        Move = speech.moveOn;

        //When player says "cast", start animation for having all available spells appear on the left side on screen
        if (speech.castOn)
        {
            //print("castOn");
            if (!rolledOut)
            {
                rolledOut = true;
                StartCoroutine("RollOutAllSpells");
            }
 
        }
        //When player says "cast", start animation for having all available spells disappear from the left side on screen
        else if (!speech.castOn)
        {
            //print("!castOn");
            if (rolledOut)
            {
                rolledOut = false;
                StartCoroutine("RollInAllSpells");
            }
        }

        // If Vivi is moving 
        if (Move)
        {
            moveClock = 0;
            animFlag = true;
        }
        else
        {
            moveClock = 0f;
            animFlag = false;
            viviKeyword.SetActive(false);
            gradient.SetActive(false);
            viviKeywordUi.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        // When the player says "cast", make magic circle appear below Vivi
        if (speech.castOn && !Cast && Active)
        {
            Cast = true;
            //Has magic circle appear below Vivi's feet
            gameObject.transform.position = new Vector3(vivi.transform.position.x,
                        vivi.GetComponent<BoxCollider2D>().bounds.min.y + 1.2f, vivi.transform.position.z);
            
            // Make magical circle appear below Vivi no matter what size she is
            anim.speed = 1.4f;
            anim.Play("UseSpellAnim", 0, 0);
            clock = 0f;
            animFlag = true; //Keep the animation going as long until they say a spell
        }

        //If player says "cast", and Vivi is not moving, start animation to tell player to say a spell.
        if (animFlag) {

            if (!Move)
            {
                clock += Time.deltaTime;
                castDarkness.SetActive(true);
                castMoon.SetActive(true);
            }
            else
            {
                moveClock += Time.deltaTime;
                viviKeyword.SetActive(true);
                gradient.SetActive(true);
                viviKeywordUi.transform.localScale = uiSize;
            }

        }

        // The time allowing a player to say a spell is now over, so all animations for the "casting" phase is now off.
        if (clock > .5f || !Cast)
        {
            gameObject.transform.position = new Vector3(0, -10000f, 0);
            anim.speed = 0;
            clock = 0f;
            animFlag = false;
            castDarkness.SetActive(false);
            castMoon.SetActive(false);
        }
        
        // Makes the magic circle and all animations "disappear" once the animation is done, measured by time.
        if (moveClock > 5f)
        {
            moveClock = 0f;
            animFlag = false;
            viviKeyword.SetActive(false);
            gradient.SetActive(false);
            viviKeywordUi.transform.localScale = new Vector3(0f, 0f, 0f);
        }
   }


    /*  ************************************************************************
     *   Images of available spells are shown on the left side of the screen   
     *  ************************************************************************ */

    // Has all spell gameobjects disappear from the left side of the screen after "casting" phase
    public IEnumerator RollOutAllSpells()
    {
        print("Rolling Out");
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollOutSpell(spells[i]));
            yield return new WaitForSecondsRealtime(.25f);
        }
    }

    // Rotates the gameobject of each spell to align them when it is leaving the left side of the screen.
    IEnumerator RollOutSpell(GameObject spell)
    {

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -2));
            yield return null;
        }
    }

    // Has all spell gameobjects appear on left side of the screen during "casting" phase
    public IEnumerator RollInAllSpells()
    {
        print("Rolling In");
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollInSpell(spells[i]));
            yield return new WaitForSecondsRealtime(.25f);
        }
    }

    // Rotates the gameobject of each spell to align them when it is shown on the left side of the screen.
    IEnumerator RollInSpell(GameObject spell)
    {

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 2));
            yield return null;
        }
    }
}
