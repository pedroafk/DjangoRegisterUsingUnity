using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

//Script para avançar nos inputs de login usando tab e voltar usando shift tab e Método Get e Post via Rest Framework
public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;

    public InputField username;

    public InputField score;

    [Serializable]
    public class Gamer
    {
        public string username;
        public int score;
    }

    public void PostData()
    {
        string userNameFromInput = username.text;
        int scoreFromInput = Int32.Parse(score.text);

        Gamer gamer = new Gamer();
        gamer.username = userNameFromInput;
        gamer.score = scoreFromInput;

        string json = JsonUtility.ToJson(gamer);
        //Debug.Log(json);
        StartCoroutine(PostRequest(url, json));
        //StartCoroutine(PostDjangoWebRequest());
    }


    //Método global para Post
    IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("Error While Sending: " + request.error);
        }
        else
        {
            Debug.Log($"{"Tudo Okay"} Received: " + request.responseCode);
        }
    }

    //Metodo para Get
    [ContextMenu("GetJson")]
    public async void getJsonFromSite() {
        var url = "url";
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        var operation = www.SendWebRequest();

        while(!operation.isDone)
            await Task.Yield();

        if(www.result == UnityWebRequest.Result.Success)
            Debug.Log($"Success: {www.downloadHandler.text}");
        else
            Debug.Log($"Failed: {www.error}");
    }

    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)){
            Selectable previus = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if(previus != null) {
                previus.Select();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Tab)){
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if(next != null){
                next.Select();
            }
        }
    }
}
