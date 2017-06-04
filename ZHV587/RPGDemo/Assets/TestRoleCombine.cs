using UnityEngine;
using System.Collections;
using Air2000;

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

    private int count;

    private void OnGUI()
    {
        PostLoadedCharacterDelegate callback = delegate (CharacterProvider.Request request)
        {
            Character Character = request.Character;
            Player player = new Player(PlayerType.PT_Robot);
            player.Data = new PBMessage.go_login_playerInfo();
            Helper.SetLayer(request.Character.gameObject, LAYER.Player.ToString());
            request.Character.Player = player;
            player.Character = request.Character;
            request.Character.SetChActive(true);
            Character = request.Character;
            Character.Initialize(player, request.Task.LoadConfigRequest.Asset as TextAsset);
            player.EnableAI = false;
            Character.gameObject.AddComponent<CharacterCommandTest>();
        };
        if (GUI.Button(new Rect(300, 10, 150, 30), "Create Archer"))
        {
            Player player = new Player(PlayerType.PT_Hero);
            player.Data = new PBMessage.go_login_playerInfo();
            player.Data.name = "Archer";
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = "Archer";
            fashion.Animation = "Archer";
            fashion.Head = "Head01";
            fashion.Body = "Body01";
            fashion.Weapon = "Weapon01";
            fashion.Config = "Archer";

            CharacterProvider.Execute(fashion, callback);
        }
        if (GUI.Button(new Rect(300, 50, 150, 30), "Create Pastor"))
        {
            Player player = new Player(PlayerType.PT_Hero);
            player.Data = new PBMessage.go_login_playerInfo();
            player.Data.name = "Pastor";
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = "Pastor";
            fashion.Animation = "Pastor";
            fashion.Head = "Head01";
            fashion.Body = "Body01";
            fashion.Weapon = "Weapon01";
            fashion.Config = "Pastor";

            CharacterProvider.Execute(fashion, callback);
        }
        if (GUI.Button(new Rect(300, 90, 150, 30), "Create Warrior"))
        {
            Player player = new Player(PlayerType.PT_Hero);
            player.Data = new PBMessage.go_login_playerInfo();
            player.Data.name = "Warrior";
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = "Warrior";
            fashion.Animation = "Warrior";
            fashion.Head = "Head2";
            fashion.Body = "Body2";
            fashion.Weapon = "Weapon2";
            fashion.Config = "Warrior";

            CharacterProvider.Execute(fashion, callback);
        }
    }
}