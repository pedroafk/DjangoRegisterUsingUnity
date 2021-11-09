using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

//Script para avançar nos inputs de login usando tab e voltar usando shift tab
public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    //Gerenciamento de input para tela de login

    public InputField username, first_name, last_name, email;

    public class RegisterUser{
        public string username, first_name, last_name, email; 
    }

    public void PostRegister(){
        string usernameFromInput = username.text;
        string myNameFromInput = first_name.text;
        string lNameFromInput = last_name.text;
        string emailFromInput = email.text;

        RegisterUser registerUser = new RegisterUser();
        registerUser.username = usernameFromInput;
        registerUser.first_name = myNameFromInput;
        registerUser.last_name = lNameFromInput;
        registerUser.email = emailFromInput;
        string json = JsonUtility.ToJson(registerUser);
        Debug.Log($"registerJson: {json}");
        StartCoroutine(PostRequest("",json));
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

    //Método para get do django
    [ContextMenu("")]
    public async void getJsonFromDjango() {
        var url = "";
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

    // Seleciona primeiro elemento para poder avançar usando tab
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    // Update da sequência de campos usando tab
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
