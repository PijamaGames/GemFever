using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        User user = new User();
        user.id = "Pedro";
        user.password = "fjdskla";
        user.gems = 10;
        user.avatar_bodyType = 2;
        user.avatar_bodyType = 3;
        user.avatar_color = 1;
        user.avatar_face = 4;
        user.avatar_hat = 30;
        user.avatar_frame = 20;
        user.items_faces = new int[] { 4, 2, 1 };
        user.items_hats = new int[] { 8, 2, 1, 7, 5, 34, 1 };
        user.items_frames = new int[] { 2, 1 };
        user.friendRequests = new string[] { "Ale, Dani" };
        user.friends = new string[] { "Juanma, Mariam, Martín" };

        string json = JsonUtility.ToJson(user);
        Debug.Log("json: " + json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
