using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoleCombine : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(300, 10, 150, 30), "Create Warrior"))
        {
            CharacterProvider.fashionInfo fashion = new CharacterProvider.fashionInfo();
            fashion.Directory = "Warrior";
            fashion.Animation = "Warrior";
            fashion.Head = "Head2";
            fashion.Body = "Body2";
            fashion.Weapon = "Weapon2";
            GameObject Animation = Resources.Load<GameObject>("Role/Warrior/" + fashion.Animation);
            GameObject Head = Resources.Load<GameObject>("Role/Warrior/Head/" + fashion.Head);
            GameObject Body = Resources.Load<GameObject>("Role/Warrior/Body/" + fashion.Body);
            GameObject Weapon = Resources.Load<GameObject>("Role/Warrior/Weapon/" + fashion.Weapon);

            GameObject animation = GameObject.Instantiate(Animation);
            GameObject head = GameObject.Instantiate(Head);
            GameObject body = GameObject.Instantiate(Body);
            GameObject weapon = GameObject.Instantiate(Weapon);

            GameObject go = CharacterProvider.CombineCharacter(animation, head, body, weapon);
        }
    }
}