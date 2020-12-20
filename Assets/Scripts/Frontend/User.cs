using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string id = "";
    public string password = "";
    public int gems = 0;
    public string[] friendRequests;
    public string[] friends;

    public int avatar_bodyType = 0;
    public int avatar_skinTone = 0;
    public int avatar_color = 0;
    public int avatar_face = 0;
    public int avatar_hat = 0;
    public int avatar_frame = 0;

    public int[] items_faces;
    public int[] items_hats;
    public int[] items_frames;

    public static User FromJson(string json)
    {
        return JsonUtility.FromJson<User>(json);
    }
}
