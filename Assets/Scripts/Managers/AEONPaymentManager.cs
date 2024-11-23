using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AEONPaymentManager : MonoBehaviour
{

    private string secretKey;
    private string aeonAPI;
    private string apiKey;
    private string redirectURL;

    //payement data values for the dictionary...
    private string appId;
    private string merchantOrderNo;
    private string userId;
    private string orderAmount;
    private string payCurrency;
    private string paymentTokens;
    private string paymentExchange;


    private string GenerateSignature(Dictionary<string, string> parameters)
    {
        // Sort parameters alphabetically by key 
        var sortedParams = new SortedDictionary<string, string>(parameters);
        StringBuilder concatenatedParams = new StringBuilder();

        // Build the signature string in the format: key1=value1&key2=value2... like a JSON
        foreach (var param in sortedParams)
        {
            concatenatedParams.Append($"{param.Key}={param.Value}&");
        }

        // Append the secret key at the end 
        concatenatedParams.Append($"key={secretKey}");

        // Generate the SHA-512 hash of the concatenated string 
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(concatenatedParams.ToString()));
            StringBuilder hash = new StringBuilder();
            foreach (byte b in bytes)
            {
                hash.Append(b.ToString("X2")); // Convert to uppercase hex string 
            }
            return hash.ToString();
        }
    }

    public IEnumerator MakePayment(float amount)
    {
        // Prepare data to send in the request 
        Dictionary<string, string> paymentData = new Dictionary<string, string>
        {
            { "appId", appId },
            { "merchantOrderNo", merchantOrderNo },
            { "userId", userId },
            { "orderAmount", amount.ToString() },
            { "payCurrency", payCurrency },
            { "paymentTokens", paymentTokens },
            { "paymentExchange", paymentExchange }
        };

        paymentData["sign"] = GenerateSignature(paymentData);
        paymentData["redirectURL"] = redirectURL;

        string jsonData = JsonUtility.ToJson(paymentData);

        using (UnityWebRequest request = new UnityWebRequest(aeonAPI, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            // Send the request and wait for a response 
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Payment initiated successfully!");

                // Parse the response to get the redirect URL 
                PaymentResponse response = JsonUtility.FromJson<PaymentResponse>(request.downloadHandler.text);
                if (response != null && !string.IsNullOrEmpty(response.redirect_url))
                {
                    Application.OpenURL(response.redirect_url);
                }
                else
                {
                    Debug.LogError("Invalid response or missing redirect URL.");
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

}

/*
        Dictionary<string, string> paymentData = new Dictionary<string, string> 
        { 
            { "appId", appId }, 
            { "merchantOrderNo", merchantOrderNo }, 
            { "userId", userId }, 
            { "orderAmount", orderAmount }, 
            { "payCurrency", payCurrency }, 
            { "paymentTokens", paymentTokens }, 
            { "paymentExchange", paymentExchange } 
        }; 

        // Step 2: Generate the signature 
        paymentData["sign"] = GenerateSignature(paymentData); 
        paymentData["redirectURL"] = redirectURL; 

        // Convert the dictionary to JSON using the custom method 
        string jsonData = ConvertToJson(paymentData);
    */