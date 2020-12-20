using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string id = "";
    public string password = "";
    public int gems = 0;
    public string[] friendRequests = new string[0];
    public string[] friends = new string[0];

    public int avatar_bodyType = 0;
    public int avatar_skinTone = 0;
    public int avatar_color = 0;
    public int avatar_face = 0;
    public int avatar_hat = 0;
    public int avatar_frame = 0;

    public int[] items_faces = new int[0];
    public int[] items_hats = new int[0];
    public int[] items_frames = new int[0];

    public static User FromJson(string json)
    {
        return JsonUtility.FromJson<User>(json);
    }

    public static string ToJson(User user)
    {
        return JsonUtility.ToJson(user);
    }
}
