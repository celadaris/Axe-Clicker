using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using BestProfanityDetector;

public class RegisterScript : MonoBehaviour
{
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_Text responseTxt;
    string salt = "d3@gh";

    public void Register()
    {
        StartCoroutine(RegisterName());
    }

    IEnumerator RegisterName()
    {
        ProfanityFilter filter = new ProfanityFilter();

        //check for bad words in username
        if (!filter.HasProfanity(userName.text))
        {
            //salt password
            string saltedPwrd = password.text + salt;

            //get hash
            byte[] data = Encoding.ASCII.GetBytes(saltedPwrd);
            data = SHA256.Create().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);

            LoginDTO loginTable = new LoginDTO()
            {
                playerName = userName.text,
                pwrdHash = hash
            };

            string json = JsonConvert.SerializeObject(loginTable);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

            UnityWebRequest request;
            using (request = UnityWebRequest.Post("https://localhost:7295/register", json))
            {
                request.disposeUploadHandlerOnDispose = true;
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.SetRequestHeader("Content-Type", "application/json");

                responseTxt.text = "registering...";
                yield return request.SendWebRequest();

                if (request.responseCode == 201)
                {
                    responseTxt.text = request.downloadHandler.text;
                }
                else
                {
                    responseTxt.text = "registration failed.";
                }

            }
        }
        else
        {
            responseTxt.text = "name was unavailable";
        }
            
    }
}
