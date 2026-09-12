using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class Bootstrapper : MonoBehaviour
{
    private AndroidJavaObject webViewObject;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        #if !UNITY_EDITOR && UNITY_ANDROID
        // Iniciar la carga del HTML desde StreamingAssets
        StartCoroutine(LoadAndShowWebView());
        #else
        // En el Editor de Unity, abrir el archivo en el navegador
        string editorPath = Path.Combine(Application.streamingAssetsPath, "index.html");
        Application.OpenURL("file://" + editorPath);
        #endif
    }

    IEnumerator LoadAndShowWebView()
    {
        // 1. Leer el archivo HTML desde StreamingAssets
        string path = Path.Combine(Application.streamingAssetsPath, "index.html");
        string htmlContent = "";

        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                htmlContent = www.downloadHandler.text;
                Debug.Log("HTML cargado correctamente desde StreamingAssets");
            }
            else
            {
                Debug.LogError("Error al cargar index.html: " + www.error);
                yield break;
            }
        }

        // 2. Crear el WebView y cargar el HTML
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            AndroidJavaObject webView = new AndroidJavaObject("android.webkit.WebView", activity);
            
            AndroidJavaObject webSettings = webView.Call<AndroidJavaObject>("getSettings");
            webSettings.Call("setJavaScriptEnabled", true);
            webSettings.Call("setDomStorageEnabled", true);
            webSettings.Call("setBuiltInZoomControls", false);
            webSettings.Call("setDisplayZoomControls", false);
            webSettings.Call("setAllowFileAccess", true);

            AndroidJavaObject client = new AndroidJavaObject("android.webkit.WebViewClient");
            webView.Call("setWebViewClient", client);

            // Usar loadDataWithBaseURL para pasar el contenido como texto
            string baseUrl = "file:///android_asset/";
            webView.Call("loadDataWithBaseURL", baseUrl, htmlContent, "text/html", "UTF-8", null);

            AndroidJavaClass layoutParamsClass = new AndroidJavaClass("android.view.ViewGroup$LayoutParams");
            int matchParent = layoutParamsClass.GetStatic<int>("MATCH_PARENT");
            AndroidJavaObject layoutParams = new AndroidJavaObject("android.view.ViewGroup$LayoutParams", matchParent, matchParent);

            activity.Call("addContentView", webView, layoutParams);
            webViewObject = webView;
        }));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if !UNITY_EDITOR && UNITY_ANDROID
            if (webViewObject != null)
            {
                if (webViewObject.Call<bool>("canGoBack"))
                    webViewObject.Call("goBack");
                else
                    Application.Quit();
            }
            #endif
        }
    }

    void OnDestroy()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        if (webViewObject != null)
        {
            webViewObject.Call("destroy");
            webViewObject = null;
        }
        #endif
    }
}
