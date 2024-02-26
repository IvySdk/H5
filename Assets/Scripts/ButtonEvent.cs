using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ivy.MiniGame;
using Ivy.Cloud;
using UnityEngine.UI;
using UnityEngine.Diagnostics;

public class ButtonEvent : MonoBehaviour
{


    void Awake()
    {
        RiseSdk.Instance.Init();

        RiseSdkListener.Event_CheckPreLoginState += (bool state) =>
        {
            Ivy.Utils.Log.Print($"log state::{state}");
        };
        RiseSdkListener.Event_LoginResult += (int state, string str) =>
        {
            if (state == 1)
            {
                Ivy.Utils.Log.Print("login success");
            }
            else
            {
                Ivy.Utils.Log.Print("login fail");
            }
        };
        RiseSdkListener.OnCloudFunctionResult += (string api, string data) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} data:{data}");
        };

        RiseSdkListener.OnCloudFunctionFailed += (string api) =>
        {
            Ivy.Utils.Log.Print($"cloud func:: api:{api} failed");
        };

        // RiseSdkListener.OnAdEvent += 

        RiseSdkListener.OnPaymentEvent += (RiseSdk.PaymentResult result, int i, string str) =>
        {
            Ivy.Utils.Log.Print($"pay result::{result} {i} {str}");
        };

        RiseSdkListener.OnPaymentWithPayloadEvent += (RiseSdk.PaymentResult result, int i, string str, string str1) =>
        {
            Ivy.Utils.Log.Print($"pay result::{result} {i} {str} {str1}");
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.Find("IsLogin").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            RiseSdk.Instance.IsLogin();
            // Debug.Log($"has login wechat? {MiniSdk.IsPreLogin}");
        });

        this.transform.Find("login").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            RiseSdk.Instance.Login();
        });

        this.transform.Find("Me").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            string str = RiseSdk.Instance.Me();
            Debug.Log($"me::{str}");
        });

        // this.transform.Find("logout").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        // {

        // });

        this.transform.Find("Pay").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            RiseSdk.Instance.Pay(1);
        });

        this.transform.Find("GetPay").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            string str = RiseSdk.Instance.GetPaymentData(1);
            Ivy.Utils.Log.Print($"get pay data success: {str}");
        });

        this.transform.Find("service").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            RiseSdk.Instance.ShowAIHelp("", "");
        });

        this.transform.Find("system").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            bool state = RiseSdk.Instance.IsIOSSystem();
            Ivy.Utils.Log.Print($"is ios system? {state}");
        });

        this.transform.Find("CloudFunc").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            Dictionary<string, object> _dict = new Dictionary<string, object>()
            {
                {"appid", Ivy.MiniGame.MiniGameConfig.appid},
                {"uuid", Ivy.MiniGame.MiniGameConfig.openId},
                {"login_code", Ivy.MiniGame.MiniGameConfig.LoginCode},
            };
            RiseSdk.Instance.CallTxCloudFunc("api_timestamp", _dict.ToString());
        });

    }

    // Update is called once per frame
    void Update()
    {

    }



}
