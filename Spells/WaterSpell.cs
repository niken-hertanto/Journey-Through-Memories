using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : MonoBehaviour {

    /* ********************************************
     *      When the player casts "Water"
     *********************************************** */

    //helps control how long the animation last
    private Animator anim;
    private float clock;
    private bool timeFlagWater, timeFlagIce, animFlag;

    //This was used where flowers where the player casts "water", but in the end this was not used.
    private bool flowerFlag;
    private GameObject[] flowers; //different kinds of flowers would appear
    private GameObject[] waters;
    private bool flowerCheck;
    public Transform yellowFlower; //Prefab yellow flower for testing

    //grants access to the speech recognition software
    SpeechRecognition01 speech;
    //used to access Vivi, the water/ice animations and particle systems
    GameObject vivi, earthing, iceDrops, rainDrops, sparkles, mist;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        flowerFlag = true;
        flowers = GameObject.FindGameObjectsWithTag("flower");
        animFlag = false;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        earthing = GameObject.Find("earth");
        iceDrops = GameObject.Find("IceDrops");
        rainDrops = GameObject.Find("RainDrops");
        sparkles = GameObject.Find("Sparkles");
        mist = GameObject.Find("mist");
        flowerCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        // When the player casts water, the water animation starts
        if (speech.word == "water" || speech.word == "ice")
        {
            // We wanted for flowers to "grow" wherever the player casts "water." But due to uneven terrain, watery areas (aka, flowers shouldn't grow here),
            //  deadlines (this would've just been a nice "touch"), etc, we decided to opt out.

            //if (flowerFlag && speech.word == "water" && gameObject.tag == "WaterAnim")
            //{
            //    Debug.Log("ARe you working?");
            //    Instantiate(yellowFlower, new Vector3(vivi.transform.position.x + (3.4f * vivi.GetComponent<Movement>().facingRight), vivi.transform.position.y + 3f, -1), Quaternion.identity);
            //    flowerFlag = false;
            //    flowers = GameObject.FindGameObjectsWithTag("flower");
            //    waters = GameObject.FindGameObjectsWithTag("Water");
            //    foreach (GameObject flower in flowers)
            //    {
            //        Physics2D.IgnoreCollision(vivi.GetComponent<Collider2D>(), flower.GetComponent<Collider2D>());
            //        Physics2D.IgnoreCollision(earthing.GetComponent<Collider2D>(), flower.GetComponent<Collider2D>());
            //        foreach (GameObject flowerz in flowers)
            //        {
            //            Physics2D.IgnoreCollision(flowerz.GetComponent<Collider2D>(), flower.GetComponent<Collider2D>());
            //        }
            //        foreach (GameObject water in waters)
            //        {
            //            Physics2D.IgnoreCollision(water.GetComponent<Collider2D>(), flower.GetComponent<Collider2D>());
            //        }
            //    }
            //}

            anim.speed = 1.2f;
            anim.Play("CloudAnim", 0, 0);
            clock = 0f;
            animFlag = true;
            
            //Checks to see whether the player casts "water" or "ice"
            if (speech.word == "water") { timeFlagWater = true; }
            else { timeFlagIce = true; }
        }

        // Checks if the animation is on, and starts the timer to check when the animation will be done
        if (animFlag) { clock += Time.deltaTime; }

        // Makes the water spell "disappear" once the animation is done
        if (clock > 2f && timeFlagWater)
        {
            rainDrops.GetComponent<ParticleSystem>().Stop();
            //sparkles.GetComponent<ParticleSystem>().Stop();
            //mist.GetComponent<ParticleSystem>().Stop();
            gameObject.transform.position = new Vector3(0, -100f, 0);
            timeFlagWater = false;
            flowerFlag = true;
            anim.speed = 0;
            animFlag = false;
            clock = 0f;
            flowerCheck = true;
        }

        // Makes the ice spell "disappear" once the animation is done
        if (clock > 2f && timeFlagIce)
        {
            iceDrops.GetComponent<ParticleSystem>().Stop();
            gameObject.transform.position = new Vector3(0, -100f, 0);
            timeFlagIce = false;
            anim.speed = 0;
            animFlag = false;
            clock = 0f;
        }

        //if (flowerCheck && flowers.Length != 0)
        //{
        //    foreach (GameObject flower in flowers)
        //    {
        //        if ((Mathf.Abs(flower.GetComponent<Rigidbody2D>().velocity.y) <= .1f))
        //        {
        //            flower.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
        //        }
        //        else { Destroy(flower); }
        //    }
        //    flowerCheck = false;
        //}
    }
}
