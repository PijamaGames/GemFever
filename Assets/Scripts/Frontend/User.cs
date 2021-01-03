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
    public string[] eventsAttended = new string[0];

    public int avatar_bodyType = 0;
    public int avatar_skinTone = 0;
    public int avatar_color = 0;
    public string avatar_face = "";
    public string avatar_hat = "";
    public string avatar_frame = "";

    public string[] items_faces = new string[0];
    public string[] items_hats = new string[0];
    public string[] items_frames = new string[0];

    public bool allowRequests = true;
    public bool allowInvitations = true;

    public static User FromJson(string json)
    {
        return JsonUtility.FromJson<User>(json);
    }

    public static string ToJson(User user)
    {
        return JsonUtility.ToJson(user);
    }
}
