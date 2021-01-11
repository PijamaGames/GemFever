using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptsManager : MonoBehaviour
{
    [SerializeField] RectTransform _canvasRect;
    public static RectTransform canvasRect;
    [SerializeField] GameObject promptPrefab;
    [SerializeField] GameObject promptsContainer;
    [SerializeField] int maxPrompts;

    //List<Button> allPrompts = new List<Button>();
    static Stack<Button> avaiblePrompts = new Stack<Button>();
    static HashSet<Button> inUsePrompts = new HashSet<Button>();


    // Start is called before the first frame update
    void Awake()
    {
        avaiblePrompts.Clear();
        inUsePrompts.Clear();
        canvasRect = _canvasRect;
        GameObject obj;
        Button btn;
        for(int i = 0; i < maxPrompts; i++)
        {
            obj = Instantiate(promptPrefab, promptsContainer.transform);
            
            //obj.transform.SetParent(promptsContainer.transform);
            btn = obj.GetComponent<Button>();
            //btn.interactable = GameManager.isHandheld;
            btn.interactable = true;
            //allPrompts.Add(btn);
            avaiblePrompts.Push(btn);
            obj.SetActive(false);
            
        }
    }

    public static Button RequestPrompt()
    {
        if(avaiblePrompts.Count > 0)
        {
            Button btn = avaiblePrompts.Pop();
            btn.gameObject.SetActive(true);
            inUsePrompts.Add(btn);
            return btn;
        }
        return null;
    }

    public static void ReleasePrompt(Button prompt)
    {
        inUsePrompts.Remove(prompt);
        prompt.gameObject.SetActive(false);
        avaiblePrompts.Push(prompt);
    }
}
