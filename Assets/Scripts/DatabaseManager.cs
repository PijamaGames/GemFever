using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private static MongoClient client = new MongoClient("mongodb+srv://lavidaesdura:5pqzOR78P&@ranking.zndxy.mongodb.net/<dbname>?retryWrites=true&w=majority");
    private static IMongoDatabase database;
    private static IMongoCollection<BsonDocument> collection;

    // Start is called before the first frame update
    public static void Connect()
    {
        database = client.GetDatabase("BallDB");
        collection = database.GetCollection<BsonDocument>("Users");
    }

    private static BsonDocument UserToBSON()
    {
        BsonArray levelTimes = new BsonArray();
        Debug.Log("count: " + User.bestTimes.Count);
        for (int i = 0; i < User.bestTimes.Count; i++)
        {
            levelTimes.Add(new BsonDocument { { "time" , User.bestTimes[i] } });
        }
        return new BsonDocument { {"_id", User.username }, { "times", levelTimes} };
    }

    private static void JsonToUser(string json)
    {
        Debug.Log(json);
        BsonDocument document = BsonDocument.Parse(json);
        var times = document.GetValue("times").AsBsonArray;
        int index = 1;
        foreach(var t in times)
        {
            User.bestTimes[index - 1] = (float)t.AsBsonDocument.GetValue("time").AsDouble;
            index++;
        }
    }

    public async static void GetUser()
    {
        var task = collection.FindAsync(new BsonDocument { { "_id", User.username} });
        var results = await task;
        var list = results.ToList();
        if(list.Count > 0)
        {
            var userJson = list[0].ToJson();
            JsonToUser(userJson);
        }
    }

    public async static void SaveUser()
    {
        Debug.Log("Saving User");
        var document = UserToBSON();
        await collection.ReplaceOneAsync(new BsonDocument { { "_id", User.username } }, document, new UpdateOptions() { IsUpsert = true});
    }

    public async static void TryCreateUser()
    {
        User.Init();
        var task = collection.FindAsync(new BsonDocument { { "_id", User.username } });
        var results = await task;
        var list = results.ToList();
        if (list.Count > 0)
        {
            Debug.Log("User already existed, getting data...");
            var userJson = list[0].ToJson();
            JsonToUser(userJson);
        } else
        {
            Debug.Log("User did not exist, creating data...");
            SaveUser();
        }
    }

    public async static Task GetRankingForCurrentLevel(List<string> rankingResult, int maxSize = 5)
    {
        var task = collection.FindAsync(new BsonDocument ());
        var results = await task;
        var list = results.ToList();
        string name;
        float time;
        BsonArray times;
        List<KeyValuePair<string, float>> candidates = new List<KeyValuePair<string, float>>();

        foreach(var rawUser in list)
        {
            name = rawUser.GetValue("_id").AsString;
            times = rawUser.GetValue("times").AsBsonArray;
            time = (float)times.ToList()[Level.levelSelected-1].AsBsonDocument.GetValue("time").AsDouble;
            if(time < 998f)
            {
                candidates.Add(new KeyValuePair<string, float>(name, time));
                Debug.Log("candidate: " + name + " time: " + time);
            }
        }

        candidates.Sort((v1,v2)=>
        {
            return Mathf.RoundToInt(v1.Value*1000f) - Mathf.RoundToInt(v2.Value*1000f);
        });

        int i = 0;
        int size = candidates.Count;
        if (size > maxSize) size = maxSize;
        while (i < size)
        {
            rankingResult.Add("" + candidates[i].Key + ": " + UsefulFuncs.FormatTime(candidates[i].Value));
            i++;
        }
    }
}
